using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace UpdaterLib
{
    public class FileUpdater
    {
        // Method signature modified to return Task for async operation
        public static async Task<bool> UpdateApplicationAsync(
            string originalPath,
            IProgress<int> progress = null
        )
        {
            try
            {
                // Report progress
                progress?.Report(10);

                // Wait for the main app to exit
                await Task.Delay(3000);

                // Report progress
                progress?.Report(30);

                // Get updater path
                string currentPath = System
                    .Diagnostics.Process.GetCurrentProcess()
                    .MainModule.FileName;

                // Report progress
                progress?.Report(50);

                // Copy the updater (which is the new version) to the original app path
                File.Copy(currentPath, originalPath, true);

                // Report progress
                progress?.Report(80);

                // Launch the updated application
                System.Diagnostics.Process.Start(originalPath);

                // Report completion
                progress?.Report(100);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Keeping the original method for backward compatibility
        public static void Update(string[] args)
        {
            if (args.Length == 2 && args[0] == "--update")
            {
                string originalPath = args[1];
                // Wait for the main app to exit
                Thread.Sleep(3000); // Give time to close (or check with loop if needed)
                string currentPath = System
                    .Diagnostics.Process.GetCurrentProcess()
                    .MainModule.FileName;
                File.Copy(currentPath, originalPath, true); // overwrite the old exe
                File.Delete(currentPath); // optional: delete updater
                System.Diagnostics.Process.Start(originalPath); // restart main app
            }
        }
    }
}
