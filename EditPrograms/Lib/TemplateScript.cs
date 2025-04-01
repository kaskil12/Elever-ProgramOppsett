using System;
using System.Collections.Generic;

namespace ProgramAppEditor
{
    public class ProgramEditor
    {
        // Add a new program or update an existing one
        public static void AddOrUpdateProgram(string programName, ProgramLib.ProgramInfo programInfo)
        {
            ProgramLib.Programs._programs[programName] = programInfo;
        }

        // Get a specific program
        public static ProgramLib.ProgramInfo GetProgram(string programName)
        {
            if (ProgramLib.Programs._programs.TryGetValue(programName, out var program))
            {
                return program;
            }
            
            throw new KeyNotFoundException($"Program '{programName}' not found.");
        }

        // Delete a program
        public static bool DeleteProgram(string programName)
        {
            return ProgramLib.Programs._programs.Remove(programName);
        }

        // Check if a program exists
        public static bool ProgramExists(string programName)
        {
            return ProgramLib.Programs._programs.ContainsKey(programName);
        }
        
        // Get all program names
        public static List<string> GetAllProgramNames()
        {
            return new List<string>(ProgramLib.Programs._programs.Keys);
        }
        
        // Get a copy of all programs
        public static Dictionary<string, ProgramLib.ProgramInfo> GetAllPrograms()
        {
            return new Dictionary<string, ProgramLib.ProgramInfo>(ProgramLib.Programs._programs);
        }
        
        // Count of programs
        public static int GetProgramCount()
        {
            return ProgramLib.Programs._programs.Count;
        }
    }
}