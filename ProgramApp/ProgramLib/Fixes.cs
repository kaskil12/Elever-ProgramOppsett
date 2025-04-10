using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Text.Json.Serialization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ProgramLib;

[JsonSerializable(typeof(Dictionary<string, FixesInfo>))]
internal partial class FixesJsonContext : JsonSerializerContext { }
public class Fixes
{
    public static string fixesPath = string.Empty;
    public static string fixesJson;

    //for loading fixes from json into a dictionary like in programs using FixesInfo
    public static Dictionary<string, FixesInfo> FixesList = new Dictionary<string, FixesInfo>();
    private readonly Dictionary<string, string> _quickFixUrls = new Dictionary<string, string>
    {
        { "Factor", "https://aka.ms/mfasetup" },
        { "ResetPassword", "https://start.innlandetfylke.no/" },
        { "PrintService", "https://innlandetfylke.eu.uniflowonline.com/" },
    };

    public static void LoadFixes()
    {
        fixesPath = Path.Combine($"{Usb._currentDriveLetter}", "pkgs", "fixes.json");
        // var fixesJson = File.ReadAllText("./fixes.json");
        if (!Usb.isUsbDrive)
        {
            try
            {
                string folderPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "IKTHub"
                );
                Directory.CreateDirectory(folderPath);
                Log.LogInfo($" Fixes : Created Directory");
            }
            catch (Exception e)
            {
                Log.LogError("Directory Creation Failed: ", e);
            }
            fixesPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "IKTHub",
                "fixes.json"
            );
        }
        else
        {
            string folderPath = Path.Combine(Usb._currentDriveLetter, "pkgs", "Assets");
            Directory.CreateDirectory(folderPath);
        }

        if (!File.Exists(fixesPath) && Usb.isUsbDrive == false)
        {
            Log.LogInfo($" Fixes : Downloading new json for PC...");
            fixesPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "IKTHub",
                "fixes.json"
            );
            if (Programs.isOnNetwork)
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments =
                        $"/C curl -u server:Trinity54 --ftp-port - ftp://10.230.64.55/IKTHub/fixes.json -o \"{fixesPath}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                };

                using (var process = Process.Start(processStartInfo))
                {
                    if (process != null)
                    {
                        process.WaitForExit();
                        if (process.ExitCode != 0)
                        {
                            throw new Exception(
                                $"Failed to download fixes.json. Exit code: {process.ExitCode}"
                            );
                        }
                        else
                        {
                            Log.LogInfo($" Fixes : Json Installed for PC");
                        }
                    }
                }
            }
            else
            {
                Log.LogInfo($" Fixes : No Network and no json file, Connect to network and try again");
                if (!File.Exists(fixesPath))
                {
                    return;
                }
            }
        }
        else if (File.Exists(fixesPath) && Usb.isUsbDrive == false && Programs.isOnNetwork)
        {
            fixesPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "IKTHub",
                "fixes.json"
            );
            Log.LogInfo($" Fixes : Updating json for PC...");
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments =
                    $"/C curl -u server:Trinity54 --ftp-port - ftp://10.230.64.55/IKTHub/fixes.json -o \"{fixesPath}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
            };

            using (var process = Process.Start(processStartInfo))
            {
                if (process != null)
                {
                    process.WaitForExit();
                    if (process.ExitCode != 0)
                    {
                        throw new Exception(
                            $"Failed to download fixes.json. Exit code: {process.ExitCode}"
                        );
                    }
                    else
                    {
                        Log.LogInfo($" Fixes : Json Updated for PC");
                    }
                }
            }
        }
        else if (Usb.isUsbDrive)
        {
            fixesPath = Path.Combine($"{Usb._currentDriveLetter}", "pkgs", "fixes.json");
            if (Programs.isOnNetwork)
            {
                Log.LogInfo($" Fixes : Downloading new json for usb...");
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments =
                        $"/C curl -u server:Trinity54 --ftp-port - ftp://10.230.64.55/IKTHub/fixes.json -o \"{fixesPath}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                };
                using (var process = Process.Start(processStartInfo))
                {
                    if (process != null)
                    {
                        process.WaitForExit();
                        if (process.ExitCode != 0)
                        {
                            throw new Exception(
                                $"Failed to download fixes.json. Exit code: {process.ExitCode}"
                            );
                        }
                        else
                        {
                            Log.LogInfo($" Fixes : Json installed for usb");
                        }
                    }
                }
            }
            else
            {
                Log.LogInfo($" Fixes : No network detected so usb not updated/Installed");
                if (!File.Exists(fixesPath))
                {
                    return;
                }
            }

            Log.LogInfo($" Fixes : Is usb, " + fixesPath.ToString());
        }

        if (!File.Exists(fixesPath) || new FileInfo(fixesPath).Length == 0)
        {
            Log.LogInfo($" Fixes : File not found or empty: {fixesPath}");
            throw new Exception("Failed to download or validate fixes.json");
        }

        fixesJson = File.ReadAllText(fixesPath);

        Dictionary<string, FixesInfo>? FixesLists = JsonSerializer.Deserialize(
            fixesJson,
            FixesJsonContext.Default.DictionaryStringFixesInfo
        );

        if (FixesLists != null)
        {
            foreach (var fixes in FixesLists)
            {
                if (!FixesList.ContainsKey(fixes.Key))
                {
                    FixesList.Add(fixes.Key, fixes.Value);
                }
                else
                {
                    Log.LogInfo($" Fixes : Duplicate program key detected: {fixes.Key}. Skipping.");
                }
                // Log.LogInfo($" Fixes : Loaded program: {program.Key}");
                // Log.LogInfo($" Fixes : Command: {program.Value.CommandTemplate}");
                // Log.LogInfo($" Fixes : Requires Removable Drive: {program.Value.RequiresRemovableDrive}");
                // Log.LogInfo($" Fixes : Icon: {program.Value.Icon}");
                // Log.LogInfo($" Fixes : Description: {program.Value.Description}");
                // Log.LogInfo($" Fixes : Version: {program.Value.Version}");
                // Log.LogInfo($" Fixes : PostInstallAction: {program.Value.PostInstallAction}");
            }
        }
        else
        {
            Log.LogError("LoadPrograms", new Exception("Failed to deserialize fixes.json"));
            throw new Exception("Failed to load fixes.json");
        }
    }

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
            Log.LogError($"OpenWebPage[{sender}]", ex);
            File.WriteAllText(Log.logFilePath, ex.ToString());
        }
    }

    public static void RemoveAdd(object sender, RoutedEventArgs e)
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
                    Log.LogInfo($" Fixes : Killed process: {processName}");
                }
            }

            if (Directory.Exists(aadFilePath))
            {
                Directory.Delete(aadFilePath, true);
                Log.LogInfo($" Fixes : Deleted directory: {aadFilePath}");
            }
            else
            {
                Console.WriteLine("⚠️ AAD file not found.");
                Log.LogInfo($" Fixes : AAD file not found: {aadFilePath}");
            }
        }
        catch (Exception ex)
        {
            Log.LogError($"RemoveAdd[{sender}]", ex);
        }
        finally
        {
            Log.LogInfo($" Fixes : Finished killing processes and deleting AAD file.");
        }
    }

    public static void OpenUrl(string url)
    {
        try
        {
            Process.Start(url);
        }
        catch (Exception ex)
        {
            Log.LogError($"OpenUrl[{url}]", ex);
        }
    }

    public static void RunCommand(string executable, string arguments)
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
}

public class FixesInfo
{
    [JsonConstructor]
    public FixesInfo() { }

    public string Name { get; set; }
    public string Description { get; set; }
    public string Command { get; set; }
}
