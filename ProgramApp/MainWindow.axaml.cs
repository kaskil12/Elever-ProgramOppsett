using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Diagnostics;
namespace ProgramApp;
using System.IO;
using Avalonia;

public partial class MainWindow : Window
{
    //bools for if user wants to download programs
    //programs are installOffice = installTeams = installOrdnett = installVsCode = False
    //installChrome = installFirefox = installPython = installGeoGebra = False
    //createWebShortcut = createWebShortcutKI = False
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
    public string currentDriveLetter = "D:"; // Replace with the actual drive letter if known

    public MainWindow()
    {
        InitializeComponent();
        this.WindowStartupLocation = WindowStartupLocation.Manual;

        this.Opened += (sender, e) =>
        {
            var screen = Screens.Primary;
            var screenWidth = screen.Bounds.Width;
            var windowWidth = this.Width;

            // Center horizontally, position at the top
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
        //If user wants to download Office
        if (OfficeCheckBox.IsChecked == true)
        {
            //Set OfficeDownload to true
            OfficeDownload = true;
        }
        //If user wants to download Teams
        if (TeamsCheckBox.IsChecked == true)
        {
            //Set TeamsDownload to true
            TeamsDownload = true;
        }
        //If user wants to download Ordnett
        if (OrdnettCheckBox.IsChecked == true)
        {
            //Set OrdnettDownload to true
            OrdnettDownload = true;
        }
        //If user wants to download VsCode
        if (VsCodeCheckBox.IsChecked == true)
        {
            //Set VsCodeDownload to true
            VsCodeDownload = true;
        }
        //If user wants to download Chrome
        if (ChromeCheckBox.IsChecked == true)
        {
            //Set ChromeDownload to true
            ChromeDownload = true;
        }
        //If user wants to download Firefox
        if (FirefoxCheckBox.IsChecked == true)
        {
            //Set FirefoxDownload to true
            FirefoxDownload = true;
        }
        //If user wants to download Python
        if (PythonCheckBox.IsChecked == true)
        {
            //Set PythonDownload to true
            PythonDownload = true;
        }
        //If user wants to download GeoGebra
        if (GeoGebraCheckBox.IsChecked == true)
        {
            //Set GeoGebraDownload to true
            GeoGebraDownload = true;
        }
        //If user wants to create a web shortcut
        if (WebShortcutCheckBox.IsChecked == true)
        {
            //Set WebShortcut to true
            WebShortcut = true;
        }
        //If user wants to create a web shortcut to KI
        if (WebShortcutKICheckBox.IsChecked == true)
        {
            //Set WebShortcutKI to true
            WebShortcutKI = true;
        }
        if(EjectDiskCheckBox.IsChecked == true){
            EjectDisk = true;
        }
        //Install the programs
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

        // If user wants to download Office
        if (OfficeDownload)
        {
            // Download Office
            CMDText = "/c start /wait .\\pkgs\\OfficeOffline\\setup.exe /configure .\\pkgs\\OfficeOffline\\Elvis.xml";
            processStartInfo.Arguments = CMDText;
            var process = Process.Start(processStartInfo);
            if (process != null)
            {
                process.WaitForExit();
            }
        }

        // If user wants to download Teams
        if (TeamsDownload)
        {
            // Download Teams
            CMDText = $"/c start /wait .\\pkgs/TeamsOffline\\teamsbootstrapper.exe -p -o \"{currentDriveLetter}\\pkgs\\TeamsOffline\\MSTeams-x64.msix\"";
            processStartInfo.Arguments = CMDText;
            var process = Process.Start(processStartInfo);
            if (process != null)
            {
                process.WaitForExit();
            }
        }

        // If user wants to download Ordnett
        if (OrdnettDownload)
        {
            // Download Ordnett
            CMDText = $"/c msiexec /i \"{currentDriveLetter}\\pkgs\\OrdnettOffline\\ordnettpluss-3.3.7-innlandet_fylkeskommune.msi\" ALLUSERS=2 /qb";
            processStartInfo.Arguments = CMDText;
            var process = Process.Start(processStartInfo);
            if (process != null)
            {
                process.WaitForExit();
            }
        }

        // If user wants to download VsCode
        if (VsCodeDownload)
        {
            // Download VsCode
            CMDText = "/c start /wait .\\pkgs\\VsCodeOffline\\Code.exe";
            processStartInfo.Arguments = CMDText;
            var process = Process.Start(processStartInfo);
            if (process != null)
            {
                process.WaitForExit();
            }
        }

        //If user wants to download Chrome
        if (ChromeDownload)
        {
            //Download Chrome
            CMDText = $"/c start /wait .\\pkgs\\ChromeOffline\\ChromeStandaloneSetup64.exe";
            processStartInfo.Arguments = CMDText;
            var process = Process.Start(processStartInfo);
            if (process != null)
            {
                process.WaitForExit();
            }
        }
        //If user wants to download Firefox
        if (FirefoxDownload)
        {
            //Download Firefox
            CMDText = $"/c start /wait .\\pkgs\\FirefoxOffline\\FireFoxInstall.exe /S";
            processStartInfo.Arguments = CMDText;
            var process = Process.Start(processStartInfo);
            if (process != null)
            {
                process.WaitForExit();
            }
        }
        //If user wants to download Python
        if (PythonDownload)
        {
            //Download Python
            CMDText = $"/c start /wait .\\pkgs\\PythonOffline\\python-3.12.6-amd64.exe /quiet PrependPath=1";
            processStartInfo.Arguments = CMDText;
            var process = Process.Start(processStartInfo);
            if (process != null)
            {
                process.WaitForExit();
            }
        }
        //If user wants to download GeoGebra
        if (GeoGebraDownload)
        {
            //Download GeoGebra
            CMDText = $"/c msiexec /i \"{currentDriveLetter}\\pkgs\\GeogebraOffline\\GeoGebra-Windows-Installer-6-0-848-0.msi\" ALLUSERS=2 /qb";
            processStartInfo.Arguments = CMDText;
            var process = Process.Start(processStartInfo);
            if (process != null)
            {
                process.WaitForExit();
            }
        }
        //If user wants to create a web shortcut
        if (WebShortcut)
        {
            //Create a web shortcut
            CMDText = "powershell -command \"$s=(New-Object -COM WScript.Shell).CreateShortcut('%userprofile%\\Desktop\\SharePoint.url');$s.TargetPath='https://innlandet.sharepoint.com';$s.Save()\"";
            System.Diagnostics.Process.Start("powershell.exe", CMDText);
            CMDText = "powershell -command \"$s=(New-Object -COM WScript.Shell).CreateShortcut('%userprofile%\\Desktop\\VismaInSchool.url');$s.TargetPath='https://elverum-vgs.inschool.visma.no/Login.jsp';$s.Save()\"";
            System.Diagnostics.Process.Start("powershell.exe", CMDText);
            CMDText = "powershell -command \"$s=(New-Object -COM WScript.Shell).CreateShortcut('%userprofile%\\Desktop\\ElverumVGS.url');$s.TargetPath='http://elverum.vgs.no';$s.Save()\"";
            System.Diagnostics.Process.Start("powershell.exe", CMDText);

        }
        //If user wants to create a web shortcut to KI
        if (WebShortcutKI)
        {
            //Create a web shortcut to KI
            CMDText = "powershell -command \"$s=(New-Object -COM WScript.Shell).CreateShortcut('%userprofile%\\Desktop\\KarriereInnlandet.url');$s.TargetPath='https://www.karriereinnlandet.no/';$s.Save()\"";
            System.Diagnostics.Process.Start("powershell.exe", CMDText);
            CMDText = "powershell -command \"$s=(New-Object -COM WScript.Shell).CreateShortcut('%userprofile%\\Desktop\\KarriereInnsia.url');$s.TargetPath='https://innlandet.sharepoint.com/sites/Voksnedeltakere';$s.Save()\"";
            System.Diagnostics.Process.Start("powershell.exe", CMDText);
            CMDText = "powershell -command \"$s=(New-Object -COM WScript.Shell).CreateShortcut('%userprofile%\\Desktop\\KarriereVisma.url');$s.TargetPath='https://karriere-innlandet.inschool.visma.no/';$s.Save()\"";
            System.Diagnostics.Process.Start("powershell.exe", CMDText);
        }
        //Eject Disk
        if (EjectDisk){
            CMDText = $"/c start .\\pkgs\\UtilsBats\\Eject.bat";
            processStartInfo.Arguments = CMDText;
            var process = Process.Start(processStartInfo);
            if (process != null)
            {
                process.WaitForExit();
            }
            Environment.Exit(0);
        }
    }
}