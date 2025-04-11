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
                if (InstallJsonPrograms(programsPath))
                {
                    Log.LogInfo("Json Installed for PC");
                }
                else
                {
                    Log.LogInfo("Failed to install json for PC");
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
            if (InstallJsonPrograms(programsPath))
            {
                Log.LogInfo("Json updated for PC");
            }
            else
            {
                Log.LogInfo("Failed to updated json for PC");
            }
        }
        else if (Usb.isUsbDrive)
        {
            programsPath = Path.Combine($"{Usb._currentDriveLetter}", "pkgs", "programs.json");
            if (!File.Exists(programsPath) && isOnNetwork)
            {
                if (InstallJsonPrograms(programsPath))
                {
                    Log.LogInfo("Json installed for usb");
                }
                else
                {
                    Log.LogInfo("Failed to install json for usb");
                }
            }
            else if (File.Exists(programsPath) && isOnNetwork == true)
            {
                Log.LogInfo("Updating json for usb...");
                if (InstallJsonPrograms(programsPath))
                {
                    Log.LogInfo("Json updated for usb");
                }
                else
                {
                    Log.LogInfo("Failed to update json for usb");
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
        }
        else
        {
            Log.LogError("LoadPrograms", new Exception("Failed to deserialize programs.json"));
            throw new Exception("Failed to load programs.json");
        }
    }

    public bool InstallJsonPrograms(string path)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments =
                $"/C curl -u server:Trinity54 --ftp-port - ftp://10.230.64.55/IKTHub/programs.json -o \"{path}\"",
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
                    return false;
                    throw new Exception(
                        $"Failed to download programs.json. Exit code: {process.ExitCode}"
                    );
                }
                else
                {
                    Log.LogInfo($"Json installed for  {Usb._currentDriveLetter}");
                }
            }
        }
        return true;
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
