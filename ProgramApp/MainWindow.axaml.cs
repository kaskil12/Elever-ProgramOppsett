using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Diagnostics;
namespace ProgramApp;
using System.IO;
using Avalonia;

public partial class MainWindow : Window
{
    private bool _officeDownload;
    private bool _teamsDownload;
    private bool _ordnettDownload;
    private bool _vsCodeDownload;
    private bool _thonnyDownload;
    private bool _chromeDownload;
    private bool _firefoxDownload;
    private bool _pythonDownload;
    private bool _geoGebraDownload;
    private bool _webShortcut;
    private bool _webShortcutKi;
    private bool _ejectDisk;
    private string _currentDriveLetter = "D:";

    public MainWindow()
    {
        InitializeComponent();
        Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
        this.WindowStartupLocation = WindowStartupLocation.Manual;

        this.Opened += (sender, e) =>
        {
            var screen = Screens.Primary ?? throw new InvalidOperationException("No primary screen detected.");
            var screenWidth = screen.Bounds.Width;
            var windowWidth = this.Width;

            this.Position = new PixelPoint((int)((screenWidth - windowWidth) / 2), 0);
        };
    }
    public void InstallButton(object sender, RoutedEventArgs e)
    {
        ProgressBarInstall.Value = 0;
        _officeDownload = false;
        _teamsDownload = false;
        _ordnettDownload = false;
        _vsCodeDownload = false;
        _thonnyDownload = false;
        _chromeDownload = false;
        _firefoxDownload = false;
        _pythonDownload = false;
        _geoGebraDownload = false;
        _webShortcut = false;
        _webShortcutKi = false;
        _ejectDisk = false;
        if (OfficeCheckBox.IsChecked == true)
        {
            _officeDownload = true;
        }
        if (TeamsCheckBox.IsChecked == true)
        {
            _teamsDownload = true;
        }
        if (OrdnettCheckBox.IsChecked == true)
        {
            _ordnettDownload = true;
        }
        if (VsCodeCheckBox.IsChecked == true)
        {
            _vsCodeDownload = true;
        }
        if (ThonnyCheckBox.IsChecked == true)
        {
            _thonnyDownload = true;
        }
        if (ChromeCheckBox.IsChecked == true)
        {
            _chromeDownload = true;
        }
        if (FirefoxCheckBox.IsChecked == true)
        {
            _firefoxDownload = true;
        }
        if (PythonCheckBox.IsChecked == true)
        {
            _pythonDownload = true;
        }
        if (GeoGebraCheckBox.IsChecked == true)
        {
            _geoGebraDownload = true;
        }
        if (WebShortcutCheckBox.IsChecked == true)
        {
            _webShortcut = true;
        }
        if (WebShortcutKICheckBox.IsChecked == true)
        {
            _webShortcutKi = true;
        }
        if(EjectDiskCheckBox.IsChecked == true){
            _ejectDisk = true;
        }
        InstallPrograms();
    }
    private void InstallPrograms()
    {
        foreach (var drive in DriveInfo.GetDrives())
        {
            if (drive.IsReady && drive.DriveType == DriveType.Removable)
            {
                _currentDriveLetter = drive.Name; 
                break; 
            }
        }
        string cmdText = "";
        ProcessStartInfo processStartInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        if (_officeDownload)
        {
            ProgressBarInstall.Value = 0;
            cmdText = $"/c start /wait .\\pkgs\\OfficeOffline\\setup.exe /configure .\\pkgs\\OfficeOffline\\Elvis.xml";
            processStartInfo.Arguments = cmdText;
            var process = Process.Start(processStartInfo);
            if (process != null)
            {
                process.WaitForExit();
            }
        }

        if (_teamsDownload)
        {
            ProgressBarInstall.Value = 0;
            cmdText = $"/c start /wait .\\pkgs/TeamsOffline\\teamsbootstrapper.exe -p -o \"{_currentDriveLetter}\\pkgs\\TeamsOffline\\MSTeams-x64.msix\"";
            processStartInfo.Arguments = cmdText;
            var process = Process.Start(processStartInfo);
            if (process != null)
            {
                process.WaitForExit();
            }
        }

        if (_ordnettDownload)
        {
            ProgressBarInstall.Value = 0;
            cmdText = $"/c msiexec /i \"{_currentDriveLetter}\\pkgs\\OrdnettOffline\\ordnettpluss-3.3.7-innlandet_fylkeskommune.msi\" ALLUSERS=2 /qb";
            processStartInfo.Arguments = cmdText;
            var process = Process.Start(processStartInfo);
            if (process != null)
            {
                process.WaitForExit();
            }
        }

        if (_vsCodeDownload)
        {
            ProgressBarInstall.Value = 0;
            cmdText = "/c start /wait .\\pkgs\\VsCodeOffline\\VSCodeSetup-x64-1.96.4 /silent /mergetasks=!runcode";
            processStartInfo.Arguments = cmdText;
            var process = Process.Start(processStartInfo);
            if (process != null)
            {
                process.WaitForExit();
            }
        }
        if(_thonnyDownload)
        {
            ProgressBarInstall.Value = 0;
            cmdText = "/c start /wait .\\pkgs\\ThonnyOffline\\thonny-4.0.0 - 64bit.exe /S";
            processStartInfo.Arguments = cmdText;
            var process = Process.Start(processStartInfo);
            if (process != null)
            {
                process.WaitForExit();
            }
            System.Diagnostics.Process.Start("https://micropython.org/");
        }

        if (_chromeDownload)
        {
            ProgressBarInstall.Value = 0;
            cmdText = $"/c start /wait .\\pkgs\\ChromeOffline\\ChromeStandaloneSetup64.exe";
            processStartInfo.Arguments = cmdText;
            var process = Process.Start(processStartInfo);
            if (process != null)
            {
                process.WaitForExit();
            }
        }
        if (_firefoxDownload)
        {
            ProgressBarInstall.Value = 0;
            cmdText = $"/c start /wait .\\pkgs\\FirefoxOffline\\FireFoxInstall.exe /S";
            processStartInfo.Arguments = cmdText;
            var process = Process.Start(processStartInfo);
            if (process != null)
            {
                process.WaitForExit();
            }
        }
        if (_pythonDownload)
        {
            ProgressBarInstall.Value = 0;
            cmdText = $"/c start /wait .\\pkgs\\PythonOffline\\python-3.12.6-amd64.exe /quiet PrependPath=1";
            processStartInfo.Arguments = cmdText;
            var process = Process.Start(processStartInfo);
            if (process != null)
            {
                process.WaitForExit();
            }
        }
        if (_geoGebraDownload)
        {
            ProgressBarInstall.Value = 0;
            cmdText = $"/c msiexec /i \"{_currentDriveLetter}\\pkgs\\GeogebraOffline\\GeoGebra-Windows-Installer-6-0-848-0.msi\" ALLUSERS=2 /qb";
            processStartInfo.Arguments = cmdText;
            var process = Process.Start(processStartInfo);
            if (process != null)
            {
                process.WaitForExit();
            }
        }
        if (_webShortcut)
        {
            ProgressBarInstall.Value = 0;
            cmdText = "$s = (New-Object -COM WScript.Shell).CreateShortcut(\"$env:USERPROFILE\\Desktop\\SharePoint.url\"); $s.TargetPath = 'https://innlandet.sharepoint.com'; $s.Save()";
            System.Diagnostics.Process.Start("powershell.exe", cmdText);

            cmdText = "$s = (New-Object -COM WScript.Shell).CreateShortcut(\"$env:USERPROFILE\\Desktop\\VismaInSchool.url\"); $s.TargetPath = 'https://elverum-vgs.inschool.visma.no/Login.jsp'; $s.Save()";
            System.Diagnostics.Process.Start("powershell.exe", cmdText);

            cmdText = "$s = (New-Object -COM WScript.Shell).CreateShortcut(\"$env:USERPROFILE\\Desktop\\ElverumVGS.url\"); $s.TargetPath = 'http://elverum.vgs.no'; $s.Save()";
            System.Diagnostics.Process.Start("powershell.exe", cmdText);
        }

        if (_webShortcutKi)
        {
            ProgressBarInstall.Value = 0;
            cmdText = "$s = (New-Object -COM WScript.Shell).CreateShortcut(\"$env:USERPROFILE\\Desktop\\KarriereInnlandet.url\"); $s.TargetPath = 'https://www.karriereinnlandet.no/'; $s.Save()";
            System.Diagnostics.Process.Start("powershell.exe", cmdText);

            cmdText = "$s = (New-Object -COM WScript.Shell).CreateShortcut(\"$env:USERPROFILE\\Desktop\\KarriereInnsia.url\"); $s.TargetPath = 'https://innlandet.sharepoint.com/sites/Voksnedeltakere'; $s.Save()";
            System.Diagnostics.Process.Start("powershell.exe", cmdText);

            cmdText = "$s = (New-Object -COM WScript.Shell).CreateShortcut(\"$env:USERPROFILE\\Desktop\\KarriereVisma.url\"); $s.TargetPath = 'https://karriere-innlandet.inschool.visma.no/'; $s.Save()";
            System.Diagnostics.Process.Start("powershell.exe", cmdText);
        }
        //eject disk currently works as a shutdown checkbutton when the program is done installing
        if (_ejectDisk)
        {
            // CMDText = @"/c timeout /t 1 >nul && powershell -Command ""Start-Sleep -Seconds 1; $drive = Get-Volume | Where-Object { $_.DriveType -eq 'Removable' } | Select-Object -First 1; if ($drive) { $driveLetter = $drive.DriveLetter; [System.IO.DriveInfo]::GetDrives() | Where-Object { $_.Name -eq ($driveLetter + ':\\') } | ForEach-Object { $_.Eject(); Start-Sleep -Seconds 1 } }"" && timeout /t 1 >nul";
            // System.Diagnostics.Process.Start("cmd.exe", CMDText);
            Environment.Exit(0);
        }

        ProgressBarInstall.Value = 100;
        OfficeCheckBox.IsChecked = false;
        TeamsCheckBox.IsChecked = false;
        OrdnettCheckBox.IsChecked = false;
        VsCodeCheckBox.IsChecked = false;
        ThonnyCheckBox.IsChecked = false;
        ChromeCheckBox.IsChecked = false;
        FirefoxCheckBox.IsChecked = false;
        PythonCheckBox.IsChecked = false;
        GeoGebraCheckBox.IsChecked = false;
        WebShortcutCheckBox.IsChecked = false;
        WebShortcutKICheckBox.IsChecked = false;
        EjectDiskCheckBox.IsChecked = false;

    }
    
    //Fixes
    public void FaktorKnapp(object sender, RoutedEventArgs e)
    {
        System.Diagnostics.Process.Start("https://aka.ms/mfasetup");
    }

    public void ResetPassord(object sender, RoutedEventArgs e)
    {
        System.Diagnostics.Process.Start("https://start.innlandetfylke.no/");
    }

    public void SkrivUt(object sender, RoutedEventArgs e)
    {
        System.Diagnostics.Process.Start("https://innlandetfylke.eu.uniflowonline.com/");
    }

    public void Dism(object sender, RoutedEventArgs e)
    {
        System.Diagnostics.Process.Start("cmd.exe", "DISM.exe /Online /Cleanup-image /Restorehealth");
    }

    public void UpdatePc(object sender, RoutedEventArgs e)
    {
        //Update PC FUNCTION
    }
    public void RemoveAdd(object sender, RoutedEventArgs e)
    {
        //Remove ADD
    }
    public void SfcScan(object sender, RoutedEventArgs e)
    {
        System.Diagnostics.Process.Start("cmd.exe", $"sfc /Scannow");
    }
    
}
