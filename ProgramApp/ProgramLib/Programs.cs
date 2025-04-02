using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProgramLib;

// Option 1: Use source generators (recommended)
[JsonSerializable(typeof(Dictionary<string, ProgramInfo>))]
internal partial class AppJsonContext : JsonSerializerContext { }

public class Programs
{
    public static readonly Dictionary<string, ProgramInfo> _programs = new Dictionary<string, ProgramInfo>();
    
    public void LoadPrograms()
    {
        // Read the JSON file
        var programsJson = File.ReadAllText("C:/Users/kasper/OneDrive - Innlandet fylkeskommune/Dokumenter/GitHub/Elever-ProgramOppsett/ProgramApp/ProgramLib/programs.json");
        
        // Option 1: Use source generators
        Dictionary<string, ProgramInfo>? programList = JsonSerializer.Deserialize(
            programsJson,
            AppJsonContext.Default.DictionaryStringProgramInfo
        );
        
        /* Option 2: Re-enable reflection (if you can't use source generators)
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            TypeInfoResolver = null // This re-enables reflection
        };
        
        Dictionary<string, ProgramInfo>? programList = JsonSerializer.Deserialize<Dictionary<string, ProgramInfo>>(
            programsJson,
            options
        );
        */

        if (programList != null)
        {
            foreach (var program in programList)
            {
                _programs.Add(program.Key, program.Value);
                Console.WriteLine($"Program: {program.Key}");
                Console.WriteLine($"CommandTemplate: {program.Value.CommandTemplate}");
                Console.WriteLine($"RequiresRemovableDrive: {program.Value.RequiresRemovableDrive}");
                Console.WriteLine($"PostInstallAction: {program.Value.PostInstallAction}");
            }
        }
        else
        {
            throw new Exception("Failed to load programs.json");
        }
    }
    public Dictionary<string, ProgramInfo> GetProgramList()
    {
        // This method returns the loaded programs as a dictionary
        return _programs;
    }
}

// Make sure ProgramInfo has JsonConstructor attribute if needed
public class ProgramInfo
{
    [JsonConstructor] // Add this if needed for parameterless constructor
    public ProgramInfo() { }
    
    public required string CommandTemplate { get; set; }
    public bool RequiresRemovableDrive { get; set; }
    public string Icon { get; set; } // Added Icon property
    
    // Action cannot be serialized directly with System.Text.Json
    // You'll need to handle PostInstallAction separately
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