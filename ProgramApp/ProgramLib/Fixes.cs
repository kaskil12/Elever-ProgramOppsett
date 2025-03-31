using System;
using System.Collections.Generic;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.IO;
namespace ProgramLib{
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