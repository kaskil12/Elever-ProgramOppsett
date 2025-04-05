using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using UpdaterLib;

namespace Updater
{
    public partial class MainWindow : Window
    {
        private string _originalAppPath;
        private bool _updateComplete = false;

        public MainWindow()
        {
            InitializeComponent();

            // Hook up the close button event
            var closeButton = this.FindControl<Button>("CloseButton");
            closeButton.Click += CloseButton_Click;

            // Get command line arguments
            string[] args = Environment.GetCommandLineArgs();

            // Start the update process
            StartUpdateProcess(args);
        }

        private async void StartUpdateProcess(string[] args)
        {
            var statusText = this.FindControl<TextBlock>("StatusText");
            var progressBar = this.FindControl<ProgressBar>("UpdateProgress");
            var percentText = this.FindControl<TextBlock>("PercentText");
            var closeButton = this.FindControl<Button>("CloseButton");

            // Validate arguments
            if (args.Length < 2 || args[0] != "--update")
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    statusText.Text =
                        "Error: Invalid arguments. Expected '--update [path_to_original_exe]'";
                    progressBar.IsIndeterminate = false;
                    progressBar.Value = 0;
                    closeButton.IsEnabled = true;
                });
                return;
            }

            _originalAppPath = args[1];

            try
            {
                // Update status
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    statusText.Text = "Waiting for application to close...";
                });

                // Wait for the original application to exit
                await Task.Delay(3000);

                // Update status
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    statusText.Text = "Copying new version...";
                    progressBar.IsIndeterminate = false;
                    progressBar.Value = 50;
                    percentText.Text = "50%";
                });

                // Get the current exe path
                string updaterPath = System
                    .Diagnostics.Process.GetCurrentProcess()
                    .MainModule.FileName;

                // Copy the updater (which is the new version) to the original application path
                File.Copy(updaterPath, _originalAppPath, true);

                await Task.Delay(1000); // Give time for the file operation to complete

                // Update status
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    statusText.Text = "Update complete. Launching application...";
                    progressBar.Value = 100;
                    percentText.Text = "100%";
                });

                await Task.Delay(1500); // Show the completion message briefly

                // Mark update as complete
                _updateComplete = true;

                // Launch the updated application
                System.Diagnostics.Process.Start(_originalAppPath);

                // Enable close button
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    closeButton.IsEnabled = true;
                });

                // Optional: Close the updater or allow user to close it manually
                await Task.Delay(500);
                this.Close();
            }
            catch (Exception ex)
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    statusText.Text = $"Error during update: {ex.Message}";
                    progressBar.IsIndeterminate = false;
                    progressBar.Value = 0;
                    closeButton.IsEnabled = true;
                });
            }
        }

        private void CloseButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnClosing(WindowClosingEventArgs e)
        {
            base.OnClosing(e);

            // Clean up if we're closing after a successful update
            if (_updateComplete)
            {
                try
                {
                    string updaterPath = System
                        .Diagnostics.Process.GetCurrentProcess()
                        .MainModule.FileName;

                    // Schedule the updater for deletion after it exits
                    // This is a common way to self-delete an executable on Windows
                    var tempBatchFile = Path.Combine(
                        Path.GetTempPath(),
                        $"cleanup_{Guid.NewGuid():N}.bat"
                    );
                    string batchContent =
                        $@"
@echo off
:Repeat
tasklist | find ""{Path.GetFileName(updaterPath)}"" >nul
if not errorlevel 1 (
    timeout /t 1 >nul
    goto Repeat
)
del ""{updaterPath}""
del ""%~f0""
";
                    File.WriteAllText(tempBatchFile, batchContent);
                    System.Diagnostics.Process.Start(
                        new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = "cmd.exe",
                            Arguments = $"/c start \"\" \"{tempBatchFile}\"",
                            WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                        }
                    );
                }
                catch
                {
                    // Ignore cleanup errors
                }
            }
        }
    }
}
