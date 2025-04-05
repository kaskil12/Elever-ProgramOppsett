using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProgramLib;

/// <summary>
/// This class handles the update process for the application.
/// It checks for a new version, downloads it if available,
/// and restarts the application with the new version.
/// </summary>
class Update
{
    static async Task Main(string[] args)
    {
        string currentVersion = "1.0.0";
        string versionUrl = "https://server.com/version.txt";
        string exeUrl = "https://server.com/IKTHub.exe";
        string tempPath = Path.Combine(Path.GetTempPath(), "update.exe");

        using var client = new HttpClient();
        string latestVersion = await client.GetStringAsync(versionUrl);

        if (latestVersion.Trim() != currentVersion)
        {
            Console.WriteLine("Updating to version " + latestVersion);

            byte[] newExe = await client.GetByteArrayAsync(exeUrl);
            await File.WriteAllBytesAsync(tempPath, newExe);

            Process.Start(
                new ProcessStartInfo
                {
                    FileName = tempPath,
                    Arguments = $"--update \"{Process.GetCurrentProcess().MainModule.FileName}\"",
                    UseShellExecute = false,
                }
            );

            Environment.Exit(0); // close main app
        }
        else
        {
            Console.WriteLine("Already up to date.");
        }
    }
}
