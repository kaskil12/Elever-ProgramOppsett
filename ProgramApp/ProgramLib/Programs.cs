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