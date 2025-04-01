using System;
using System.Collections.Generic;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.IO;
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
                File.WriteAllText(Usb.logFilePath, ex.ToString());
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
                        Console.WriteLine($"{processName} terminated.");
                    }
                }

                if (Directory.Exists(aadFilePath))
                {
                    Directory.Delete(aadFilePath, true);
                    Console.WriteLine("✅ AAD file successfully deleted.");
                    File.WriteAllText(Usb.logFilePath, "AAD file deleted successfully.");
                }
                else
                {
                    Console.WriteLine("⚠️ AAD file not found.");
                    File.WriteAllText(Usb.logFilePath, "AAD file not found.");
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText(Usb.logFilePath, ex.ToString());
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
    }
}