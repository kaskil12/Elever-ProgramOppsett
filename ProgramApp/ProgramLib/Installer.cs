using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Text.Json;
#nullable enable

namespace ProgramLib
{
    public class Installer
    {
        public string _currentDriveLetter = string.Empty;
        // #region LoadPrograms
        //load programs from programs.json
        private void LoadPrograms()
        {
            var programs = File.ReadAllText("programs.json");
            var programList = JsonSerializer.Deserialize<List<ProgramInfo>>(programs);
        }

        // #endregion
        #region InstallPrograms
        public static void InstallProgram(string programName, ProgramInfo program)
        {
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

                var process = Process.Start(processStartInfo);
                if (process != null)
                    process.WaitForExit();

                program.PostInstallAction?.Invoke();
            }
            catch (Exception ex)
            {
                File.WriteAllText(Usb.logFilePath, ex.ToString());
            }
        }
        #endregion
    }

}