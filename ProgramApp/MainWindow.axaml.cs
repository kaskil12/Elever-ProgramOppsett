using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ProgramLib;
#nullable enable
namespace ProgramApp
{
    public partial class MainWindow : Window
    {
        private readonly Dictionary<string, bool> _installOptions = new Dictionary<string, bool>();
        private string logFilePath = "./log.txt";



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
                Usb.DetectRemovableDrive();

                foreach (var program in Installer._programs)
                {
                    if (_installOptions.TryGetValue(program.Key, out bool isSelected) && isSelected)
                    {
                        Installer.InstallProgram(program.Key, program.Value);
                    }
                }

                foreach (var group in Installer._shortcutGroups)
                {
                    if (_installOptions.TryGetValue(group.Key, out bool isSelected) && isSelected)
                    {
                        Installer.CreateShortcuts(group.Value);
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


        public void FaktorKnapp(object sender, RoutedEventArgs e) =>
            Fixes.OpenUrl("https://aka.ms/mfasetup");

        public void ResetPassord(object sender, RoutedEventArgs e) =>
            Fixes.OpenUrl("https://start.innlandetfylke.no/");

        public void SkrivUt(object sender, RoutedEventArgs e) =>
            Fixes.OpenUrl("https://innlandetfylke.eu.uniflowonline.com/");
        #region System Operations

        // public void RunSystemCommand(object sender, RoutedEventArgs e)
        // {
        //     try
        //     {
        //         if (sender is Button button && button.Name != null)
        //         {
        //             string commandName = button.Name.Replace("Button", "");
        //             if (_systemCommands.TryGetValue(commandName, out var command))
        //             {
        //                 Process.Start(command.executable, command.arguments);
        //             }
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         Log.LogError("RunSystemCommand", ex);
        //     }
        // }

        public void Dism(object sender, RoutedEventArgs e) =>
            RunCommand("cmd.exe", "DISM.exe /Online /Cleanup-image /Restorehealth");

        public void SfcScan(object sender, RoutedEventArgs e) =>
            RunCommand("cmd.exe", "sfc /Scannow");

        public void DiskCleanup(object sender, RoutedEventArgs e) => RunCommand("cleanmgr.exe", "");
        public void UpdatePc(object sender, RoutedEventArgs e)
        {
            // RunCommand("powershell", "Start-Process \"ms-settings:windowsupdate\"");
            RunCommand("powershell", "Start-Process powershell.exe -Verb RunAs -ArgumentList \"-NoProfile -ExecutionPolicy Bypass -Command `\\\"Start-Process ms-settings:windowsupdate; Install-PackageProvider NuGet -Force; Install-Module PSWindowsUpdate -Force -Confirm:$false; Import-Module PSWindowsUpdate; Get-WindowsUpdate -AcceptAll -Install -AutoReboot\\\"\"");
        }

        private void RunCommand(string executable, string arguments)
        {
            try
            {
                Process.Start(executable, arguments);
            }
            catch (Exception ex)
            {
                Log.LogError($"RunCommand[{executable} {arguments}]", ex);
            }
        }
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
                "AADBrokerPlugin",
                "AADBrokerPlugin.exe",
                "Arbeids- eller skolekonto",
                "Microsoft Office Click-to-Run",
                "Microsoft Office Click-to-Run Service Monitor",
                "Arbeids- eller skolekonto (2)",
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
            Log.LogInfo("BackupUserData not implemented");

        public void RefreshSystemInfo(object sender, RoutedEventArgs e) =>
            Log.LogInfo("RefreshSystemInfo not implemented");

        public void RunAllChecks(object sender, RoutedEventArgs e)
        {
            try
            {
                Dism(sender, e);
                SfcScan(sender, e);
                DiskCleanup(sender, e);
                UpdatePc(sender, e);
                RemoveAdd(sender, e);
                BackupUserData(sender, e);
                RefreshSystemInfo(sender, e);
            }
            catch (Exception ex)
            {
                File.WriteAllText(logFilePath, ex.ToString());
            }
        }

        #endregion

    }




}
