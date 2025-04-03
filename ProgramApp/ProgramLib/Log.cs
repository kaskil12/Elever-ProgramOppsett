using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ProgramLib
{
    public class Log
    {
        public static string logFilePath = "./log.txt";

        public static void LogError(string method, Exception ex)
        {
            Console.WriteLine($"Error in {method}: {ex.Message}");
            File.AppendAllText(logFilePath, $"Error in {method}: {ex.Message}");
        }

        public static void LogInfo(string message)
        {
            Console.WriteLine($"Info: {message}");
            File.AppendAllText(logFilePath, message + Environment.NewLine);
        }
    }
}
