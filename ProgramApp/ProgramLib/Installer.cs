using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
#nullable enable

namespace ProgramLib
{
    public class Installer
    {
        public string _currentDriveLetter = string.Empty;
        // #region LoadPrograms
        //load programs from programs.json
        // private void LoadPrograms()
        // {
        //     var programs = File.ReadAllText("programs.json");
        //     var programList = JsonSerializer.Deserialize<List<ProgramInfo>>(programs);
        // }
        // private void LoadShortcuts()
        // {
        //     var shortcuts = File.ReadAllText("shortcuts.json");
        //     var shortcutList = JsonSerializer.Deserialize<List<WebShortcut>>(shortcuts);
        // }

        // #endregion
        #region ProgramInfo

        public static readonly Dictionary<string, List<WebShortcut>> _shortcutGroups = new Dictionary<
            string,
            List<WebShortcut>
        >
        {
            {
                "WebShortcut",
                new List<WebShortcut>
                {
                    new WebShortcut
                    {
                        Name = "SharePoint",
                        Url = "https://innlandet.sharepoint.com",
                    },
                    new WebShortcut
                    {
                        Name = "VismaInSchool",
                        Url = "https://elverum-vgs.inschool.visma.no/Login.jsp",
                    },
                    new WebShortcut { Name = "ElverumVGS", Url = "http://elverum.vgs.no" },
                }
            },
            {
                "WebShortcutKi",
                new List<WebShortcut>
                {
                    new WebShortcut
                    {
                        Name = "KarriereInnlandet",
                        Url = "https://www.karriereinnlandet.no/",
                    },
                    new WebShortcut
                    {
                        Name = "KarriereInnsia",
                        Url = "https://innlandet.sharepoint.com/sites/Voksnedeltakere",
                    },
                    new WebShortcut
                    {
                        Name = "KarriereVisma",
                        Url = "https://karriere-innlandet.inschool.visma.no/",
                    },
                }
            },
        };
        #endregion
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
        public static void CreateShortcuts(List<WebShortcut> shortcuts)
        {
            try
            {

                foreach (var shortcut in shortcuts)
                {
                    string cmdText =
                        $"$s = (New-Object -COM WScript.Shell).CreateShortcut(\"$env:USERPROFILE\\Desktop\\{shortcut.Name}.url\"); $s.TargetPath = '{shortcut.Url}'; $s.Save()";
                    Process.Start("powershell.exe", cmdText);
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText(Usb.logFilePath, ex.ToString());
            }
        }
    }
   
    public class WebShortcut
    {
        public required string Name { get; set; }
        public required string Url { get; set; }
    }

}