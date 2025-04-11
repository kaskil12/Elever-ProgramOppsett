using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace ProgramLib;

[JsonSerializable(typeof(Dictionary<string, ProgramInfo>))]
internal partial class ProgramJsonContexts : JsonSerializerContext { }

public class Programs
{
    string programsJson;
    public static bool isOnNetwork;

    string programsPath;
    public static string ServerIP = "10.230.64.55";

    public static readonly Dictionary<string, ProgramInfo> _programs =
        new Dictionary<string, ProgramInfo>();

    public void LoadPrograms()
    {
        CheckNetwork();
        Log.LogInfo("Network: " + isOnNetwork);
        programsPath = Path.Combine($"{Usb._currentDriveLetter}", "pkgs", "programs.json");
        // var programsJson = File.ReadAllText("./programs.json");
        if (!Usb.isUsbDrive)
        {
            try
            {
                string folderPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "IKTHub"
                );
                Directory.CreateDirectory(folderPath);
                Log.LogInfo("Created Directory");
            }
            catch (Exception e)
            {
                Log.LogError("Directory Creation Failed: ", e);
            }
            programsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "IKTHub",
                "programs.json"
            );
        }
        else
        {
            string folderPath = Path.Combine(Usb._currentDriveLetter, "pkgs", "Assets");
            Directory.CreateDirectory(folderPath);
        }

        if (!File.Exists(programsPath) && Usb.isUsbDrive == false)
        {
            Log.LogInfo("Downloading new json for PC...");
            programsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "IKTHub",
                "programs.json"
            );
            if (isOnNetwork)
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments =
                        $"/C curl -u server:Trinity54 --ftp-port - ftp://10.230.64.55/IKTHub/programs.json -o \"{programsPath}\"",
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
                                $"Failed to download programs.json. Exit code: {process.ExitCode}"
                            );
                        }
                        else
                        {
                            Log.LogInfo("Json Installed for PC");
                        }
                    }
                }
            }
            else
            {
                Log.LogInfo("No Network and no json file, Connect to network and try again");
                if (!File.Exists(programsPath))
                {
                    return;
                }
            }
        }
        else if (File.Exists(programsPath) && Usb.isUsbDrive == false && isOnNetwork)
        {
            programsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "IKTHub",
                "programs.json"
            );
            Log.LogInfo("Updating json for PC...");
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments =
                    $"/C curl -u server:Trinity54 --ftp-port - ftp://10.230.64.55/IKTHub/programs.json -o \"{programsPath}\"",
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
                            $"Failed to download programs.json. Exit code: {process.ExitCode}"
                        );
                    }
                    else
                    {
                        Log.LogInfo("Json Updated for PC");
                    }
                }
            }
        }
        else if (Usb.isUsbDrive)
        {
            programsPath = Path.Combine($"{Usb._currentDriveLetter}", "pkgs", "programs.json");
            if (isOnNetwork)
            {
                Log.LogInfo("Downloading new json for usb...");
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments =
                        $"/C curl -u server:Trinity54 --ftp-port - ftp://10.230.64.55/IKTHub/programs.json -o \"{programsPath}\"",
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
                                $"Failed to download programs.json. Exit code: {process.ExitCode}"
                            );
                        }
                        else
                        {
                            Log.LogInfo("Json installed for usb");
                        }
                    }
                }
            }
            else
            {
                Log.LogInfo("No network detected so usb not updated/Installed");
                if (!File.Exists(programsPath))
                {
                    return;
                }
            }

            Log.LogInfo("Is usb, " + programsPath.ToString());
        }

        if (!File.Exists(programsPath) || new FileInfo(programsPath).Length == 0)
        {
            Log.LogInfo($"File not found or empty: {programsPath}");
            throw new Exception("Failed to download or validate programs.json");
        }

        programsJson = File.ReadAllText(programsPath);

        Dictionary<string, ProgramInfo>? programList = JsonSerializer.Deserialize(
            programsJson,
            ProgramJsonContexts.Default.DictionaryStringProgramInfo
        );

        if (programList != null)
        {
            Task.Run(() =>
            {
                foreach (var program in programList)
                {
                    if (!_programs.ContainsKey(program.Key))
                    {
                        _programs.Add(program.Key, program.Value);
                    }
                    else
                    {
                        Log.LogInfo($"Duplicate program key detected: {program.Key}. Skipping.");
                    }
                    // Log.LogInfo($"Loaded program: {program.Key}");
                    // Log.LogInfo($"Command: {program.Value.CommandTemplate}");
                    // Log.LogInfo($"Requires Removable Drive: {program.Value.RequiresRemovableDrive}");
                    // Log.LogInfo($"Icon: {program.Value.Icon}");
                    // Log.LogInfo($"Description: {program.Value.Description}");
                    // Log.LogInfo($"Version: {program.Value.Version}");
                    // Log.LogInfo($"PostInstallAction: {program.Value.PostInstallAction}");
                }
            });
        }
        else
        {
            Log.LogError("LoadPrograms", new Exception("Failed to deserialize programs.json"));
            throw new Exception("Failed to load programs.json");
        }
    }

    public static bool CheckNetwork()
    {
        isOnNetwork = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        var sender = new Ping();
        var result = sender.Send($"{ServerIP}");
        if (result.Status == IPStatus.Success)
        {
            isOnNetwork = true;
        }
        else
        {
            isOnNetwork = false;
        }
        return isOnNetwork;
    }

    public Dictionary<string, ProgramInfo> GetProgramList()
    {
        return _programs;
    }
}

public class ProgramInfo
{
    [JsonConstructor]
    public ProgramInfo() { }

    public required string CommandTemplate { get; set; }
    public bool RequiresRemovableDrive { get; set; }
    public string Icon { get; set; }
    public string Description { get; set; }
    public string Version { get; set; }

    [JsonIgnore]
    public Action? PostInstallAction { get; set; }
}


//old code for reference
// public static readonly Dictionary<string, ProgramInfo> _programs = new Dictionary<
//     string,
//     ProgramInfo
// >
// {
//     {
//         "Office",
//         new ProgramInfo
//         {
//             CommandTemplate =
//                 "/c start /wait .\\pkgs\\OfficeOffline\\setup.exe /configure .\\pkgs\\OfficeOffline\\Elvis.xml",
//             RequiresRemovableDrive = false,
//         }
//     },
//     {
//         "Teams",
//         new ProgramInfo
//         {
//             CommandTemplate =
//                 "/c start /wait .\\pkgs/TeamsOffline\\teamsbootstrapper.exe -p -o \"{0}\\pkgs\\TeamsOffline\\MSTeams-x64.msix\"",
//             RequiresRemovableDrive = true,
//         }
//     },
//     {
//         "Ordnett",
//         new ProgramInfo
//         {
//             CommandTemplate =
//                 "/c msiexec /i \"{0}\\pkgs\\OrdnettOffline\\ordnettpluss-3.3.7-innlandet_fylkeskommune.msi\" ALLUSERS=2 /qb",
//             RequiresRemovableDrive = true,
//         }
//     },
//     {
//         "VsCode",
//         new ProgramInfo
//         {
//             CommandTemplate =
//                 "/c start /wait .\\pkgs\\VsCodeOffline\\VSCodeSetup-x64-1.96.4 /silent /mergetasks=!runcode",
//             RequiresRemovableDrive = false,
//         }
//     },
//     {
//         "Thonny",
//         new ProgramInfo
//         {
//             CommandTemplate =
//                 "/c start /wait .\\pkgs\\ThonnyOffline\\thonny-4.0.0 - 64bit.exe /S",
//             RequiresRemovableDrive = false,
//             PostInstallAction = () => Process.Start("https://micropython.org/"),
//         }
//     },
//     {
//         "Chrome",
//         new ProgramInfo
//         {
//             CommandTemplate =
//                 "/c start /wait .\\pkgs\\ChromeOffline\\ChromeStandaloneSetup64.exe",
//             RequiresRemovableDrive = false,
//         }
//     },
//     {
//         "Firefox",
//         new ProgramInfo
//         {
//             CommandTemplate = "/c start /wait .\\pkgs\\FirefoxOffline\\FireFoxInstall.exe /S",
//             RequiresRemovableDrive = false,
//         }
//     },
//     {
//         "Python",
//         new ProgramInfo
//         {
//             CommandTemplate =
//                 "/c start /wait .\\pkgs\\PythonOffline\\python-3.12.6-amd64.exe /quiet PrependPath=1",
//             RequiresRemovableDrive = false,
//         }
//     },
//     {
//         "GeoGebra",
//         new ProgramInfo
//         {
//             CommandTemplate =
//                 "/c msiexec /i \"{0}\\pkgs\\GeogebraOffline\\GeoGebra-Windows-Installer-6-0-848-0.msi\" ALLUSERS=2 /qb",
//             RequiresRemovableDrive = true,
//         }
//     },
// };
