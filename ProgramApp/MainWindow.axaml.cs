using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Diagnostics;
namespace ProgramApp;
using System.IO;
using Avalonia;

public partial class MainWindow : Window
{
    public bool OfficeDownload = false;
    public bool TeamsDownload = false;
    public bool OrdnettDownload = false;
    public bool VsCodeDownload = false;
    public bool ChromeDownload = false;
    public bool FirefoxDownload = false;
    public bool PythonDownload = false;
    public bool GeoGebraDownload = false;
    public bool WebShortcut = false;
    public bool WebShortcutKI = false;
    public bool EjectDisk = false;
    public string currentDriveLetter = "D:";

    public MainWindow()
    {
        InitializeComponent();
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
        OfficeDownload = false;
        TeamsDownload = false;
        OrdnettDownload = false;
        VsCodeDownload = false;
        ChromeDownload = false;
        FirefoxDownload = false;
        PythonDownload = false;
        GeoGebraDownload = false;
        WebShortcut = false;
        WebShortcutKI = false;
        EjectDisk = false;
        if (OfficeCheckBox.IsChecked == true)
        {
            OfficeDownload = true;
        }
        if (TeamsCheckBox.IsChecked == true)
        {
            TeamsDownload = true;
        }
        if (OrdnettCheckBox.IsChecked == true)
        {
            OrdnettDownload = true;
        }
        if (VsCodeCheckBox.IsChecked == true)
        {
            VsCodeDownload = true;
        }
        if (ChromeCheckBox.IsChecked == true)
        {
            ChromeDownload = true;
        }
        if (FirefoxCheckBox.IsChecked == true)
        {
            FirefoxDownload = true;
        }
        if (PythonCheckBox.IsChecked == true)
        {
            PythonDownload = true;
        }
        if (GeoGebraCheckBox.IsChecked == true)
        {
            GeoGebraDownload = true;
        }
        if (WebShortcutCheckBox.IsChecked == true)
        {
            WebShortcut = true;
        }
        if (WebShortcutKICheckBox.IsChecked == true)
        {
            WebShortcutKI = true;
        }
        if(EjectDiskCheckBox.IsChecked == true){
            EjectDisk = true;
        }
        InstallPrograms();
    }
    public void InstallPrograms()
    {
        foreach (var drive in DriveInfo.GetDrives())
        {
            if (drive.IsReady && drive.DriveType == DriveType.Removable)
            {
                currentDriveLetter = drive.Name; 
                break; 
            }
        }
        string CMDText = "";
        ProcessStartInfo processStartInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        if (OfficeDownload)
        {
            CMDText = "/c start /wait .\\pkgs\\OfficeOffline\\setup.exe /configure .\\pkgs\\OfficeOffline\\Elvis.xml";
            processStartInfo.Arguments = CMDText;
            var process = Process.Start(processStartInfo);
            if (process != null)
            {
                process.WaitForExit();
            }
        }

        if (TeamsDownload)
        {
            CMDText = $"/c start /wait .\\pkgs/TeamsOffline\\teamsbootstrapper.exe -p -o \"{currentDriveLetter}\\pkgs\\TeamsOffline\\MSTeams-x64.msix\"";
            processStartInfo.Arguments = CMDText;
            var process = Process.Start(processStartInfo);
            if (process != null)
            {
                process.WaitForExit();
            }
        }

        if (OrdnettDownload)
        {
            CMDText = $"/c msiexec /i \"{currentDriveLetter}\\pkgs\\OrdnettOffline\\ordnettpluss-3.3.7-innlandet_fylkeskommune.msi\" ALLUSERS=2 /qb";
            processStartInfo.Arguments = CMDText;
            var process = Process.Start(processStartInfo);
            if (process != null)
            {
                process.WaitForExit();
            }
        }

        if (VsCodeDownload)
        {
            CMDText = "/c start /wait .\\pkgs\\VsCodeOffline\\Code.exe";
            processStartInfo.Arguments = CMDText;
            var process = Process.Start(processStartInfo);
            if (process != null)
            {
                process.WaitForExit();
            }
        }

        if (ChromeDownload)
        {
            CMDText = $"/c start /wait .\\pkgs\\ChromeOffline\\ChromeStandaloneSetup64.exe";
            processStartInfo.Arguments = CMDText;
            var process = Process.Start(processStartInfo);
            if (process != null)
            {
                process.WaitForExit();
            }
        }
        if (FirefoxDownload)
        {
            CMDText = $"/c start /wait .\\pkgs\\FirefoxOffline\\FireFoxInstall.exe /S";
            processStartInfo.Arguments = CMDText;
            var process = Process.Start(processStartInfo);
            if (process != null)
            {
                process.WaitForExit();
            }
        }
        if (PythonDownload)
        {
            CMDText = $"/c start /wait .\\pkgs\\PythonOffline\\python-3.12.6-amd64.exe /quiet PrependPath=1";
            processStartInfo.Arguments = CMDText;
            var process = Process.Start(processStartInfo);
            if (process != null)
            {
                process.WaitForExit();
            }
        }
        if (GeoGebraDownload)
        {
            CMDText = $"/c msiexec /i \"{currentDriveLetter}\\pkgs\\GeogebraOffline\\GeoGebra-Windows-Installer-6-0-848-0.msi\" ALLUSERS=2 /qb";
            processStartInfo.Arguments = CMDText;
            var process = Process.Start(processStartInfo);
            if (process != null)
            {
                process.WaitForExit();
            }
        }
        if (WebShortcut)
        {
            CMDText = "powershell -command \"$s=(New-Object -COM WScript.Shell).CreateShortcut('%userprofile%\\Desktop\\SharePoint.url');$s.TargetPath='https://innlandet.sharepoint.com';$s.Save()\"";
            System.Diagnostics.Process.Start("powershell.exe", CMDText);
            CMDText = "powershell -command \"$s=(New-Object -COM WScript.Shell).CreateShortcut('%userprofile%\\Desktop\\VismaInSchool.url');$s.TargetPath='https://elverum-vgs.inschool.visma.no/Login.jsp';$s.Save()\"";
            System.Diagnostics.Process.Start("powershell.exe", CMDText);
            CMDText = "powershell -command \"$s=(New-Object -COM WScript.Shell).CreateShortcut('%userprofile%\\Desktop\\ElverumVGS.url');$s.TargetPath='http://elverum.vgs.no';$s.Save()\"";
            System.Diagnostics.Process.Start("powershell.exe", CMDText);

        }
        if (WebShortcutKI)
        {
            CMDText = "powershell -command \"$s=(New-Object -COM WScript.Shell).CreateShortcut('%userprofile%\\Desktop\\KarriereInnlandet.url');$s.TargetPath='https://www.karriereinnlandet.no/';$s.Save()\"";
            System.Diagnostics.Process.Start("powershell.exe", CMDText);
            CMDText = "powershell -command \"$s=(New-Object -COM WScript.Shell).CreateShortcut('%userprofile%\\Desktop\\KarriereInnsia.url');$s.TargetPath='https://innlandet.sharepoint.com/sites/Voksnedeltakere';$s.Save()\"";
            System.Diagnostics.Process.Start("powershell.exe", CMDText);
            CMDText = "powershell -command \"$s=(New-Object -COM WScript.Shell).CreateShortcut('%userprofile%\\Desktop\\KarriereVisma.url');$s.TargetPath='https://karriere-innlandet.inschool.visma.no/';$s.Save()\"";
            System.Diagnostics.Process.Start("powershell.exe", CMDText);
        }
        //eject disk currently works as a shutdown checkbutton when the program is done installing
        if (EjectDisk)
        {
            // CMDText = @"/c timeout /t 1 >nul && powershell -Command ""Start-Sleep -Seconds 1; $drive = Get-Volume | Where-Object { $_.DriveType -eq 'Removable' } | Select-Object -First 1; if ($drive) { $driveLetter = $drive.DriveLetter; [System.IO.DriveInfo]::GetDrives() | Where-Object { $_.Name -eq ($driveLetter + ':\\') } | ForEach-Object { $_.Eject(); Start-Sleep -Seconds 1 } }"" && timeout /t 1 >nul";
            // System.Diagnostics.Process.Start("cmd.exe", CMDText);
            Environment.Exit(0);
        }
        OfficeCheckBox.IsChecked = false;
        TeamsCheckBox.IsChecked = false;
        OrdnettCheckBox.IsChecked = false;
        VsCodeCheckBox.IsChecked = false;
        ChromeCheckBox.IsChecked = false;
        FirefoxCheckBox.IsChecked = false;
        PythonCheckBox.IsChecked = false;
        GeoGebraCheckBox.IsChecked = false;
        WebShortcutCheckBox.IsChecked = false;
        WebShortcutKICheckBox.IsChecked = false;
        EjectDiskCheckBox.IsChecked = false;

    }
}