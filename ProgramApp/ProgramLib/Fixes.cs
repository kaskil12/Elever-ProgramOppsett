using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ProgramLib
{
    public class Fixes
    {
        private readonly Dictionary<string, string> _quickFixUrls = new Dictionary<string, string>
        {
            { "Factor", "https://aka.ms/mfasetup" },
            { "ResetPassword", "https://start.innlandetfylke.no/" },
            { "PrintService", "https://innlandetfylke.eu.uniflowonline.com/" },
        };

        public void OpenWebPage(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button && button.Name != null)
                {
                    string buttonName = button.Name.Replace("Button", "");
                    if (_quickFixUrls.TryGetValue(buttonName, out string? url))
                    {
                        Process.Start(url);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogError($"OpenWebPage[{sender}]", ex);
                File.WriteAllText(Log.logFilePath, ex.ToString());
            }
        }

        public static void RemoveAdd(object sender, RoutedEventArgs e)
        {
            string[] processesToKill = new string[]
            {
                "WINWORD",
                "EXCEL",
                "POWERPNT",
                "OneDrive",
                "Publisher",
                "Teams",
                "msedge",
                "Microsoft.AAD.BrokerPlugin",
                "Microsoft Office Click-to-Run (SxS)",
                "Office",
                "Microsoft Teams",
                "Send to OneNote Tool",
                "OneNote",
                "Microsoft Edge",
                "Microsoft.AAD.BrokerPlugin.exe",
                "AADBrokerPlugin",
                "AADBrokerPlugin.exe",
                "Arbeids- eller skolekonto",
                "Microsoft Office Click-to-Run",
                "Microsoft Office Click-to-Run Service Monitor",
                "Arbeids- eller skolekonto (2)",
            };

            string aadFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Packages",
                "Microsoft.AAD.BrokerPlugin_cw5n1h2txyewy"
            );

            try
            {
                foreach (var processName in processesToKill)
                {
                    var processes = Process.GetProcessesByName(processName);
                    foreach (var process in processes)
                    {
                        process.Kill();
                        process.WaitForExit();
                        Log.LogInfo($"Killed process: {processName}");
                    }
                }

                if (Directory.Exists(aadFilePath))
                {
                    Directory.Delete(aadFilePath, true);
                    Log.LogInfo($"Deleted directory: {aadFilePath}");
                }
                else
                {
                    Console.WriteLine("⚠️ AAD file not found.");
                    Log.LogInfo($"AAD file not found: {aadFilePath}");
                }
            }
            catch (Exception ex)
            {
                Log.LogError($"RemoveAdd[{sender}]", ex);
            }
            finally
            {
                Log.LogInfo("Finished killing processes and deleting AAD file.");
            }
        }

        public static void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch (Exception ex)
            {
                Log.LogError($"OpenUrl[{url}]", ex);
            }
        }
       public static void RunCommand(string executable, string arguments)
        {
            try
            {
                Process.Start(executable, arguments);
            }
            catch (Exception ex)
            {
                Log.LogError($"RunCommand[{executable} {arguments}]", ex);
            }
        }
    }
}
