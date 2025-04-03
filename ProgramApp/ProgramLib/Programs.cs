using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProgramLib;

[JsonSerializable(typeof(Dictionary<string, ProgramInfo>))]
internal partial class AppJsonContext : JsonSerializerContext { }

public class Programs
{
    string programsJson = "./programs.json";
    string programsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "programs.json"
    );
    public static readonly Dictionary<string, ProgramInfo> _programs =
        new Dictionary<string, ProgramInfo>();

    public void LoadPrograms()
    {
        // var programsJson = File.ReadAllText("./programs.json");

        if (File.Exists(programsPath))
        {
            programsJson = File.ReadAllText(programsPath);
        }
        else
        {
            string localAppData = Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData
            );
            Fixes.RunCommand(
                "cmd.exe",
                $"curl -L -o \"{localAppData}/programs.json\" \"https://raw.githubusercontent.com/kaskil12/Elever-ProgramOppsett/main/ProgramApp/ProgramLib/programs.json\""
            );
            programsJson = File.ReadAllText(programsPath);
        }

        Dictionary<string, ProgramInfo>? programList = JsonSerializer.Deserialize(
            programsJson,
            AppJsonContext.Default.DictionaryStringProgramInfo
        );

        if (programList != null)
        {
            foreach (var program in programList)
            {
                _programs.Add(program.Key, program.Value);
                Log.LogInfo($"Loaded program: {program.Key}");
                Log.LogInfo($"Command: {program.Value.CommandTemplate}");
                Log.LogInfo($"Requires Removable Drive: {program.Value.RequiresRemovableDrive}");
                Log.LogInfo($"Icon: {program.Value.Icon}");
                Log.LogInfo($"Description: {program.Value.Description}");
                Log.LogInfo($"Version: {program.Value.Version}");
                Log.LogInfo($"PostInstallAction: {program.Value.PostInstallAction}");
            }
        }
        else
        {
            Log.LogError("LoadPrograms", new Exception("Failed to deserialize programs.json"));
            throw new Exception("Failed to load programs.json");
        }
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
