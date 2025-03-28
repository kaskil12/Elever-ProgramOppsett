using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
#nullable enable
namespace ProgramApp
{
    public partial class MainWindow : Window
    {
        private readonly Dictionary<string, bool> _installOptions = new Dictionary<string, bool>();
        private string _currentDriveLetter = "D:";
        private string logFilePath = "./log.txt";

        private readonly Dictionary<string, ProgramInfo> _programs = new Dictionary<
            string,
            ProgramInfo
        >
        {
            {
                "Office",
                new ProgramInfo
                {
                    CommandTemplate =
                        "/c start /wait .\\pkgs\\OfficeOffline\\setup.exe /configure .\\pkgs\\OfficeOffline\\Elvis.xml",
                    RequiresRemovableDrive = false,
                }
            },
            {
                "Teams",
                new ProgramInfo
                {
                    CommandTemplate =
                        "/c start /wait .\\pkgs/TeamsOffline\\teamsbootstrapper.exe -p -o \"{0}\\pkgs\\TeamsOffline\\MSTeams-x64.msix\"",
                    RequiresRemovableDrive = true,
                }
            },
            {
                "Ordnett",
                new ProgramInfo
                {
                    CommandTemplate =
                        "/c msiexec /i \"{0}\\pkgs\\OrdnettOffline\\ordnettpluss-3.3.7-innlandet_fylkeskommune.msi\" ALLUSERS=2 /qb",
                    RequiresRemovableDrive = true,
                }
            },
            {
                "VsCode",
                new ProgramInfo
                {
                    CommandTemplate =
                        "/c start /wait .\\pkgs\\VsCodeOffline\\VSCodeSetup-x64-1.96.4 /silent /mergetasks=!runcode",
                    RequiresRemovableDrive = false,
                }
            },
            {
                "Thonny",
                new ProgramInfo
                {
                    CommandTemplate =
                        "/c start /wait .\\pkgs\\ThonnyOffline\\thonny-4.0.0 - 64bit.exe /S",
                    RequiresRemovableDrive = false,
                    PostInstallAction = () => Process.Start("https://micropython.org/"),
                }
            },
            {
                "Chrome",
                new ProgramInfo
                {
                    CommandTemplate =
                        "/c start /wait .\\pkgs\\ChromeOffline\\ChromeStandaloneSetup64.exe",
                    RequiresRemovableDrive = false,
                }
            },
            {
                "Firefox",
                new ProgramInfo
                {
                    CommandTemplate =
                        "/c start /wait .\\pkgs\\FirefoxOffline\\FireFoxInstall.exe /S",
                    RequiresRemovableDrive = false,
                }
            },
            {
                "Python",
                new ProgramInfo
                {
                    CommandTemplate =
                        "/c start /wait .\\pkgs\\PythonOffline\\python-3.12.6-amd64.exe /quiet PrependPath=1",
                    RequiresRemovableDrive = false,
                }
            },
            {
                "GeoGebra",
                new ProgramInfo
                {
                    CommandTemplate =
                        "/c msiexec /i \"{0}\\pkgs\\GeogebraOffline\\GeoGebra-Windows-Installer-6-0-848-0.msi\" ALLUSERS=2 /qb",
                    RequiresRemovableDrive = true,
                }
            },
        };

        private readonly Dictionary<string, List<WebShortcut>> _shortcutGroups = new Dictionary<
            string,
            List<WebShortcut>
        >
        {
            {
                "WebShortcut",
                new List<WebShortcut>
                {
                    new WebShortcut
                    {
                        Name = "SharePoint",
                        Url = "https://innlandet.sharepoint.com",
                    },
                    new WebShortcut
                    {
                        Name = "VismaInSchool",
                        Url = "https://elverum-vgs.inschool.visma.no/Login.jsp",
                    },
                    new WebShortcut { Name = "ElverumVGS", Url = "http://elverum.vgs.no" },
                }
            },
            {
                "WebShortcutKi",
                new List<WebShortcut>
                {
                    new WebShortcut
                    {
                        Name = "KarriereInnlandet",
                        Url = "https://www.karriereinnlandet.no/",
                    },
                    new WebShortcut
                    {
                        Name = "KarriereInnsia",
                        Url = "https://innlandet.sharepoint.com/sites/Voksnedeltakere",
                    },
                    new WebShortcut
                    {
                        Name = "KarriereVisma",
                        Url = "https://karriere-innlandet.inschool.visma.no/",
                    },
                }
            },
        };

        private readonly Dictionary<string, string> _quickFixUrls = new Dictionary<string, string>
        {
            { "Factor", "https://aka.ms/mfasetup" },
            { "ResetPassword", "https://start.innlandetfylke.no/" },
            { "PrintService", "https://innlandetfylke.eu.uniflowonline.com/" },
        };

        private readonly Dictionary<string, (string executable, string arguments)> _systemCommands =
            new Dictionary<string, (string, string)>
            {
                { "Dism", ("cmd.exe", "DISM.exe /Online /Cleanup-image /Restorehealth") },
                { "SfcScan", ("cmd.exe", "sfc /Scannow") },
                { "DiskCleanup", ("cleanmgr.exe", "") },
            };

        public MainWindow()
        {
            InitializeComponent();
            CenterWindowAtTop();
        }

        private void CenterWindowAtTop()
        {
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Opened += (sender, e) =>
            {
                var screen =
                    Screens.Primary
                    ?? throw new InvalidOperationException("No primary screen detected.");
                var screenWidth = screen.Bounds.Width;
                var windowWidth = this.Width;
                this.Position = new PixelPoint((int)((screenWidth - windowWidth) / 2), 0);
            };
        }
        // #region LoadPrograms
        //load programs from programs.json
        // private void LoadPrograms()
        // {
        //     var programs = File.ReadAllText("programs.json");
        //     var programList = JsonSerializer.Deserialize<List<ProgramInfo>>(programs);
        // }
        // private void LoadShortcuts()
        // {
        //     var shortcuts = File.ReadAllText("shortcuts.json");
        //     var shortcutList = JsonSerializer.Deserialize<List<WebShortcut>>(shortcuts);
        // }

        // #endregion
        #region Installation Methods

        public void InstallButton(object sender, RoutedEventArgs e)
        {
            try
            {
                ProgressBarInstall.Value = 0;
                CollectInstallOptions();
                InstallPrograms();
            }
            catch (Exception ex)
            {
                File.WriteAllText(logFilePath, ex.ToString());

            }
        }

        private void CollectInstallOptions()
        {
            _installOptions.Clear();

            _installOptions["Office"] = OfficeCheckBox.IsChecked == true;
            _installOptions["Teams"] = TeamsCheckBox.IsChecked == true;
            _installOptions["Ordnett"] = OrdnettCheckBox.IsChecked == true;
            _installOptions["VsCode"] = VsCodeCheckBox.IsChecked == true;
            _installOptions["Thonny"] = ThonnyCheckBox.IsChecked == true;
            _installOptions["Chrome"] = ChromeCheckBox.IsChecked == true;
            _installOptions["Firefox"] = FirefoxCheckBox.IsChecked == true;
            _installOptions["Python"] = PythonCheckBox.IsChecked == true;
            _installOptions["GeoGebra"] = GeoGebraCheckBox.IsChecked == true;
            _installOptions["WebShortcut"] = WebShortcutCheckBox.IsChecked == true;
            _installOptions["WebShortcutKi"] = WebShortcutKICheckBox.IsChecked == true;
            _installOptions["EjectDisk"] = EjectDiskCheckBox.IsChecked == true;
        }

        private void InstallPrograms()
        {
            try
            {
                DetectRemovableDrive();

                foreach (var program in _programs)
                {
                    if (_installOptions.TryGetValue(program.Key, out bool isSelected) && isSelected)
                    {
                        InstallProgram(program.Key, program.Value);
                    }
                }

                foreach (var group in _shortcutGroups)
                {
                    if (_installOptions.TryGetValue(group.Key, out bool isSelected) && isSelected)
                    {
                        CreateShortcuts(group.Value);
                    }
                }


                // Finish up
                ProgressBarInstall.Value = 100;
                ResetCheckboxes();
                if (_installOptions.TryGetValue("EjectDisk", out bool shouldEject) && shouldEject)
                {
                    Console.WriteLine("Ejecting disk...");
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText(logFilePath, ex.ToString());
            }
        }

        private void DetectRemovableDrive()
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.DriveType == DriveType.Removable)
                {
                    _currentDriveLetter = drive.Name;
                    break;
                }
            }
        }

        private void InstallProgram(string programName, ProgramInfo program)
        {
            try
            {
                ProgressBarInstall.Value = 0;

                string cmdText = program.RequiresRemovableDrive
                    ? string.Format(program.CommandTemplate, _currentDriveLetter)
                    : program.CommandTemplate;

                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = cmdText,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                };

                var process = Process.Start(processStartInfo);
                if (process != null)
                    process.WaitForExit();

                program.PostInstallAction?.Invoke();
            }
            catch (Exception ex)
            {
                File.WriteAllText(logFilePath, ex.ToString());
            }
        }

        private void CreateShortcuts(List<WebShortcut> shortcuts)
        {
            try
            {
                ProgressBarInstall.Value = 0;

                foreach (var shortcut in shortcuts)
                {
                    string cmdText =
                        $"$s = (New-Object -COM WScript.Shell).CreateShortcut(\"$env:USERPROFILE\\Desktop\\{shortcut.Name}.url\"); $s.TargetPath = '{shortcut.Url}'; $s.Save()";
                    Process.Start("powershell.exe", cmdText);
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText(logFilePath, ex.ToString());
            }
        }

        private void ResetCheckboxes()
        {
            OfficeCheckBox.IsChecked = false;
            TeamsCheckBox.IsChecked = false;
            OrdnettCheckBox.IsChecked = false;
            VsCodeCheckBox.IsChecked = false;
            ThonnyCheckBox.IsChecked = false;
            ChromeCheckBox.IsChecked = false;
            FirefoxCheckBox.IsChecked = false;
            PythonCheckBox.IsChecked = false;
            GeoGebraCheckBox.IsChecked = false;
            WebShortcutCheckBox.IsChecked = false;
            // WebShortcutKICheckBox.IsChecked = false;
            EjectDiskCheckBox.IsChecked = false;
        }

        #endregion

        #region Search and Sort

        public void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            FilterSoftwareList();
        }

        public void SortButton_Click(object sender, RoutedEventArgs e)
        {
            SortSoftwareList();
        }

        private void FilterSoftwareList() { }

        private void SortSoftwareList() { }

        #endregion

        #region Quick Fix Buttons

        public void OpenWebPage(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button && button.Name != null)
                {
                    string buttonName = button.Name.Replace("Button", "");
                    if (_quickFixUrls.TryGetValue(buttonName, out string? url))
                    {
                        Process.Start(url);
                    }
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText(logFilePath, ex.ToString());
            }
        }

        public void FaktorKnapp(object sender, RoutedEventArgs e) =>
            OpenUrl("https://aka.ms/mfasetup");

        public void ResetPassord(object sender, RoutedEventArgs e) =>
            OpenUrl("https://start.innlandetfylke.no/");

        public void SkrivUt(object sender, RoutedEventArgs e) =>
            OpenUrl("https://innlandetfylke.eu.uniflowonline.com/");

        private void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch (Exception ex)
            {
                LogError($"OpenUrl[{url}]", ex);
            }
        }

        #endregion

        #region System Operations

        public void RunSystemCommand(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button && button.Name != null)
                {
                    string commandName = button.Name.Replace("Button", "");
                    if (_systemCommands.TryGetValue(commandName, out var command))
                    {
                        Process.Start(command.executable, command.arguments);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("RunSystemCommand", ex);
            }
        }

        public void Dism(object sender, RoutedEventArgs e) =>
            RunCommand("cmd.exe", "DISM.exe /Online /Cleanup-image /Restorehealth");

        public void SfcScan(object sender, RoutedEventArgs e) =>
            RunCommand("cmd.exe", "sfc /Scannow");

        public void DiskCleanup(object sender, RoutedEventArgs e) => RunCommand("cleanmgr.exe", "");

        private void RunCommand(string executable, string arguments)
        {
            try
            {
                Process.Start(executable, arguments);
            }
            catch (Exception ex)
            {
                LogError($"RunCommand[{executable} {arguments}]", ex);
            }
        }

        public void UpdatePc(object sender, RoutedEventArgs e) =>
            LogInfo("UpdatePc not implemented");

        public void RemoveAdd(object sender, RoutedEventArgs e)
        {
            string[] processesToKill = new string[]
            {
                "WINWORD",
                "EXCEL",
                "POWERPNT",
                "OneDrive",
                "Publisher",
                "Teams",
                "msedge",
                "Microsoft.AAD.BrokerPlugin",
                "Microsoft Office Click-to-Run (SxS)",
                "Office",
                "Microsoft Teams",
                "Send to OneNote Tool",
                "OneNote",
                "Microsoft Edge",
                "Microsoft.AAD.BrokerPlugin.exe",
            };

            string aadFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Packages",
                "Microsoft.AAD.BrokerPlugin_cw5n1h2txyewy"
            );

            try
            {
                foreach (var processName in processesToKill)
                {
                    var processes = Process.GetProcessesByName(processName);
                    foreach (var process in processes)
                    {
                        process.Kill();
                        process.WaitForExit();
                        Console.WriteLine($"{processName} terminated.");
                    }
                }

                if (Directory.Exists(aadFilePath))
                {
                    Directory.Delete(aadFilePath, true);
                    Console.WriteLine("✅ AAD file successfully deleted.");
                    File.WriteAllText(logFilePath, "AAD file deleted successfully.");
                }
                else
                {
                    Console.WriteLine("⚠️ AAD file not found.");
                    File.WriteAllText(logFilePath, "AAD file not found.");
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText(logFilePath, ex.ToString());
            }
        }

        public void BackupUserData(object sender, RoutedEventArgs e) =>
            LogInfo("BackupUserData not implemented");

        public void RefreshSystemInfo(object sender, RoutedEventArgs e) =>
            LogInfo("RefreshSystemInfo not implemented");

        public void RunAllChecks(object sender, RoutedEventArgs e) =>
            LogInfo("RunAllChecks not implemented");

        #endregion

        #region Utilities

        private void LogError(string method, Exception ex)
        {
            Console.WriteLine($"Error in {method}: {ex.Message}");
        }

        private void LogInfo(string message)
        {
            Console.WriteLine($"Info: {message}");
        }

        #endregion
    }

    public class ProgramInfo
    {
        public required string CommandTemplate { get; set; }
        public bool RequiresRemovableDrive { get; set; }
        public Action? PostInstallAction { get; set; }
    }

    public class WebShortcut
    {
        public required string Name { get; set; }
        public required string Url { get; set; }
    }
}
