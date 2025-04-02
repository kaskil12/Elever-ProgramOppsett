using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
#nullable enable

namespace ProgramLib
{
    public class Installer
    {
        public string _currentDriveLetter = string.Empty;

        #region InstallPrograms
        public static void InstallProgram(string programName, ProgramInfo program)
        {
            Log.LogInfo($"Installing {programName}");
            try
            {
                string cmdText = program.RequiresRemovableDrive
                    ? string.Format(program.CommandTemplate, Usb._currentDriveLetter)
                    : program.CommandTemplate;

                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = cmdText,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                };
                Log.LogInfo($"Command: {cmdText}");
                var process = Process.Start(processStartInfo);
                if (process != null)
                    process.WaitForExit();

                program.PostInstallAction?.Invoke();
            }
            catch (Exception ex)
            {
                File.WriteAllText(Log.logFilePath, ex.ToString());
            }
        }
        #endregion
    }
}
