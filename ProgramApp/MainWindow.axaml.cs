using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using ProgramLib;
#nullable enable
namespace ProgramApp
{
    public partial class MainWindow : Window
    {
        private readonly Dictionary<string, bool> _installOptions = new Dictionary<string, bool>();

        public MainWindow()
        {
            InitializeComponent();
            CenterWindowAtTop();
            Programs program = new Programs();
            program.LoadPrograms();
            DisplayPrograms();
        }

        #region Display Programs
        //Program Boxes. This is where the function boxes are created and displayed.
        public void DisplayPrograms()
        {
            var programContainer = this.FindControl<WrapPanel>("ProgramContainer");
            if (programContainer == null)
            {
                Log.LogError(
                    "DisplayPrograms",
                    new Exception("Could not find WrapPanel container")
                );
                return;
            }

            programContainer.Children.Clear();

            foreach (var program in Programs._programs)
            {
                string programName = program.Key;
                var programInfo = program.Value;

                var border = new Border
                {
                    Width = 180,
                    Height = 120,
                    Margin = new Thickness(5),
                    Background = new SolidColorBrush(Color.Parse("#e0e0e0")),
                    CornerRadius = new CornerRadius(4),
                };

                var grid = new Grid();

                var stackPanel = new StackPanel { Margin = new Thickness(12) };

                var headerPanel = new StackPanel { Orientation = Orientation.Horizontal };

                var iconPath = $"Assets/{programInfo.Icon}";
                if (!File.Exists(iconPath))
                {
                    iconPath = "Assets/TeamsIcon.png";
                }

                var iconImage = new Image
                {
                    Width = 24,
                    Height = 24,
                    Margin = new Thickness(0, 0, 10, 0),
                    Source = new Avalonia.Media.Imaging.Bitmap(iconPath),
                };
                headerPanel.Children.Add(iconImage);

                var titleBlock = new TextBlock
                {
                    Text = programName,
                    FontWeight = FontWeight.Medium,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                headerPanel.Children.Add(titleBlock);

                stackPanel.Children.Add(headerPanel);

                var descBlock = new TextBlock
                {
                    Text = programInfo.Description,
                    Margin = new Thickness(0, 8, 0, 0),
                    FontSize = 12,
                    Opacity = 0.8,
                    TextWrapping = TextWrapping.Wrap,
                };
                stackPanel.Children.Add(descBlock);

                var versionBlock = new TextBlock
                {
                    Text = $"Version: {programInfo.Version}",
                    Margin = new Thickness(0, 8, 0, 0),
                    FontSize = 11,
                    Opacity = 0.7,
                };
                stackPanel.Children.Add(versionBlock);

                grid.Children.Add(stackPanel);

                var checkbox = new CheckBox
                {
                    Name = $"{programName}CheckBox",
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(0, 8, 8, 0),
                };

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

                border.Child = grid;

                programContainer.Children.Add(border);
            }
        }
        #endregion

        // Center the window at the top of the screen
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
                File.WriteAllText(Log.logFilePath, ex.ToString());
            }
        }

        private void CollectInstallOptions()
        {
            var programContainer = this.FindControl<WrapPanel>("ProgramContainer");
            if (programContainer == null)
            {
                Log.LogError(
                    "CollectInstallOptions",
                    new Exception("Could not find WrapPanel container")
                );
                return;
            }

            foreach (var child in programContainer.Children)
            {
                if (child is Border border && border.Child is Grid grid)
                {
                    var checkBox = grid.Children[1] as CheckBox;
                    if (checkBox != null)
                    {
                        string programName = checkBox.Name.Replace("CheckBox", "");
                        _installOptions[programName] = checkBox.IsChecked ?? false;
                    }
                }
            }
        }

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

                ProgressBarInstall.Value = 100;
                // ResetCheckboxes();
                if (_installOptions.TryGetValue("EjectDisk", out bool shouldEject) && shouldEject)
                {
                    Log.LogInfo("Ejecting disk...");
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText(Log.logFilePath, ex.ToString());
            }
        }

        #endregion

        #region Search and Sort
        // Search and Sort buttons. They are not implemented yet.

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

        // Quick Fixes in the fixes tab
        public void FaktorKnapp(object sender, RoutedEventArgs e) =>
            Fixes.OpenUrl("https://aka.ms/mfasetup");

        public void ResetPassord(object sender, RoutedEventArgs e) =>
            Fixes.OpenUrl("https://start.innlandetfylke.no/");

        public void SkrivUt(object sender, RoutedEventArgs e) =>
            Fixes.OpenUrl("https://innlandetfylke.eu.uniflowonline.com/");

        #region System Operations

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
                File.WriteAllText(Log.logFilePath, ex.ToString());
            }
        }

        #endregion
    }
}
