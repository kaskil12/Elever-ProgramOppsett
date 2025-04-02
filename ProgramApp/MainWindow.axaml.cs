using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ProgramLib;
using Avalonia.Media;
using Avalonia.Layout;
#nullable enable
namespace ProgramApp
{
    public partial class MainWindow : Window
    {
        private readonly Dictionary<string, bool> _installOptions = new Dictionary<string, bool>();

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
            // LoadPrograms();
            Programs program = new Programs();
            program.LoadPrograms();
            DisplayPrograms();
        }

        public void DisplayPrograms()
        {
            // Find the WrapPanel container
            var programContainer = this.FindControl<WrapPanel>("ProgramContainer");
            if (programContainer == null)
            {
                Log.LogError(
                    "DisplayPrograms",
                    new Exception("Could not find WrapPanel container")
                );
                return;
            }

            // Clear any existing items
            programContainer.Children.Clear();

            // Loop through each program and create a box for it
            foreach (var program in Programs._programs)
            {
                string programName = program.Key;
                var programInfo = program.Value;

                // Create main border container
                var border = new Border
                {
                    Width = 180,
                    Height = 120,
                    Margin = new Thickness(5),
                    Background = new SolidColorBrush(Color.Parse("#e0e0e0")),
                    CornerRadius = new CornerRadius(4),
                };

                // Create grid to hold content and checkbox
                var grid = new Grid();

                // Create content stack panel
                var stackPanel = new StackPanel { Margin = new Thickness(12) };

                // Create header with icon and title
                var headerPanel = new StackPanel { Orientation = Orientation.Horizontal };

                // Create and add icon
                var iconPath = $"Assets/{programInfo.Icon}";
                if (!File.Exists(iconPath))
                {
                    iconPath = "Assets/DefaultIcon.png"; // Fallback icon
                }

                var iconImage = new Image
                {
                    Width = 24,
                    Height = 24,
                    Margin = new Thickness(0, 0, 10, 0),
                    Source = new Avalonia.Media.Imaging.Bitmap(iconPath),
                };
                headerPanel.Children.Add(iconImage);

                // Create and add title
                var titleBlock = new TextBlock
                {
                    Text = programName,
                    FontWeight = FontWeight.Medium,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                headerPanel.Children.Add(titleBlock);

                // Add header to main stack panel
                stackPanel.Children.Add(headerPanel);

                // Add description block (you might need to add this to your JSON)
                string description = GetProgramDescription(programName);
                var descBlock = new TextBlock
                {
                    Text = description,
                    Margin = new Thickness(0, 8, 0, 0),
                    FontSize = 12,
                    Opacity = 0.8,
                    TextWrapping = TextWrapping.Wrap,
                };
                stackPanel.Children.Add(descBlock);

                // Add version info (you might need to add this to your JSON)
                string version = GetProgramVersion(programName);
                var versionBlock = new TextBlock
                {
                    Text = $"Version: {version}",
                    Margin = new Thickness(0, 8, 0, 0),
                    FontSize = 11,
                    Opacity = 0.7,
                };
                stackPanel.Children.Add(versionBlock);

                // Add stack panel to grid
                grid.Children.Add(stackPanel);

                // Create and add checkbox
                var checkbox = new CheckBox
                {
                    Name = $"{programName}CheckBox",
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(0, 8, 8, 0),
                };

                // Store reference to checkbox in the window
                var field = this.GetType()
                    .GetField(
                        $"{programName}CheckBox",
                        System.Reflection.BindingFlags.Instance
                            | System.Reflection.BindingFlags.NonPublic
                            | System.Reflection.BindingFlags.Public
                    );
                if (field != null)
                {
                    field.SetValue(this, checkbox);
                }

                grid.Children.Add(checkbox);

                // Add grid to border
                border.Child = grid;

                // Add the completed border to WrapPanel
                programContainer.Children.Add(border);
            }
        }

        // Helper methods to get program descriptions and versions
        // These would ideally come from your JSON data in a real implementation
        private string GetProgramDescription(string programName)
        {
            // This is a simplified implementation - you should extend your JSON to include descriptions
            switch (programName)
            {
                case "Office":
                    return "Word, Excel, PowerPoint, Outlook";
                case "Teams":
                    return "Microsoft Teams collaboration platform";
                case "Ordnett":
                    return "Dictionary and language tools";
                case "VsCode":
                    return "Code editor for development";
                case "Thonny":
                    return "Python IDE for beginners";
                case "Chrome":
                    return "Google web browser";
                case "Firefox":
                    return "Mozilla web browser";
                case "Python":
                    return "Programming language and runtime";
                case "GeoGebra":
                    return "Mathematics software for geometry";
                default:
                    return "Application software";
            }
        }

        private string GetProgramVersion(string programName)
        {
            // This is a simplified implementation - you should extend your JSON to include versions
            switch (programName)
            {
                case "Office":
                    return "2025 Enterprise";
                case "Teams":
                    return "Latest";
                case "Ordnett":
                    return "3.3.7";
                case "VsCode":
                    return "1.96.4";
                case "Thonny":
                    return "4.0.0";
                case "Chrome":
                    return "Latest";
                case "Firefox":
                    return "Latest";
                case "Python":
                    return "3.12.6";
                case "GeoGebra":
                    return "6.0.848";
                default:
                    return "Latest";
            }
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
                // CollectInstallOptions();
                InstallPrograms();
            }
            catch (Exception ex)
            {
                File.WriteAllText(Usb.logFilePath, ex.ToString());
            }
        }

        // private void CollectInstallOptions()
        // {
        //     _installOptions.Clear();

        //     _installOptions["Office"] = OfficeCheckBox.IsChecked == true;
        //     _installOptions["Teams"] = TeamsCheckBox.IsChecked == true;
        //     _installOptions["Ordnett"] = OrdnettCheckBox.IsChecked == true;
        //     _installOptions["VsCode"] = VsCodeCheckBox.IsChecked == true;
        //     _installOptions["Thonny"] = ThonnyCheckBox.IsChecked == true;
        //     _installOptions["Chrome"] = ChromeCheckBox.IsChecked == true;
        //     _installOptions["Firefox"] = FirefoxCheckBox.IsChecked == true;
        //     _installOptions["Python"] = PythonCheckBox.IsChecked == true;
        //     _installOptions["GeoGebra"] = GeoGebraCheckBox.IsChecked == true;
        //     _installOptions["EjectDisk"] = EjectDiskCheckBox.IsChecked == true;
        // }

        private void InstallPrograms()
        {
            try
            {
                Usb.DetectRemovableDrive();

                foreach (var program in Programs._programs)
                {
                    if (_installOptions.TryGetValue(program.Key, out bool isSelected) && isSelected)
                    {
                        Installer.InstallProgram(program.Key, program.Value);
                    }
                }

                // Finish up
                ProgressBarInstall.Value = 100;
                // ResetCheckboxes();
                if (_installOptions.TryGetValue("EjectDisk", out bool shouldEject) && shouldEject)
                {
                    Console.WriteLine("Ejecting disk...");
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText(Usb.logFilePath, ex.ToString());
            }
        }

        // private void ResetCheckboxes()
        // {
        //     OfficeCheckBox.IsChecked = false;
        //     TeamsCheckBox.IsChecked = false;
        //     OrdnettCheckBox.IsChecked = false;
        //     VsCodeCheckBox.IsChecked = false;
        //     ThonnyCheckBox.IsChecked = false;
        //     ChromeCheckBox.IsChecked = false;
        //     FirefoxCheckBox.IsChecked = false;
        //     PythonCheckBox.IsChecked = false;
        //     GeoGebraCheckBox.IsChecked = false;
        //     EjectDiskCheckBox.IsChecked = false;
        // }

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
            RunCommand(
                "powershell",
                "Start-Process powershell.exe -Verb RunAs -ArgumentList \"-NoProfile -ExecutionPolicy Bypass -Command `\\\"Start-Process ms-settings:windowsupdate; Install-PackageProvider NuGet -Force; Install-Module PSWindowsUpdate -Force -Confirm:$false; Import-Module PSWindowsUpdate; Get-WindowsUpdate -AcceptAll -Install -AutoReboot\\\"\""
            );
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
            Fixes.RemoveAdd(sender, e);
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
                File.WriteAllText(Usb.logFilePath, ex.ToString());
            }
        }

        #endregion
    }
}
