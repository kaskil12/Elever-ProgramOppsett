using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Diagnostics;
namespace ProgramApp;

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

    public MainWindow()
    {
        InitializeComponent();
    }
    public void InstallButton(object sender, RoutedEventArgs e)
    {
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
        //Install the programs
        InstallPrograms();
    }
    public void InstallPrograms()
    {
        string CMDText = "";
        //If user wants to download Office
        if (OfficeDownload)
        {
            //Download Office
            CMDText = "start /wait .\\pkgs\\OfficeOffline\\setup.exe /configure .\\pkgs\\OfficeOffline\\Elvis.xml";
            System.Diagnostics.Process.Start("CMD.exe", CMDText);
        }
        //If user wants to download Teams
        if (TeamsDownload)
        {
            //Download Teams
            CMDText = "start /wait .\\pkgs/TeamsOffline\\teamsbootstrapper.exe -p -o \"{current_drive_letter}/pkgs/TeamsOffline/MSTeams-x64.msix\"";
            System.Diagnostics.Process.Start("CMD.exe", CMDText);
        }
        //If user wants to download Ordnett
        if (OrdnettDownload)
        {
            //Download Ordnett
            CMDText = "msiexec /i \"{current_drive_letter}\\pkgs\\OrdnettOffline\\ordnettpluss-3.3.7-innlandet_fylkeskommune.msi\" ALLUSERS=2 /qb";
            System.Diagnostics.Process.Start("CMD.exe", CMDText);
        }
        //If user wants to download VsCode
        if (VsCodeDownload)
        {
            //Download VsCode
            CMDText = "start /wait .\\pkgs\\VsCodeOffline\\Code.exe";
            System.Diagnostics.Process.Start("CMD.exe", CMDText);
        }
        //If user wants to download Chrome
        if (ChromeDownload)
        {
            //Download Chrome
            CMDText = "start /wait .\\pkgs\\ChromeOffline\\ChromeStandaloneSetup64.exe";
            System.Diagnostics.Process.Start("CMD.exe", CMDText);
        }
        //If user wants to download Firefox
        if (FirefoxDownload)
        {
            //Download Firefox
            CMDText = "start /wait .\\pkgs\\FirefoxOffline\\FireFoxInstall.exe /S";
            System.Diagnostics.Process.Start("CMD.exe", CMDText);
        }
        //If user wants to download Python
        if (PythonDownload)
        {
            //Download Python
            CMDText = "start /wait .\\pkgs\\PythonOffline\\python-3.12.6-amd64.exe /quiet PrependPath=1";
            System.Diagnostics.Process.Start("CMD.exe", CMDText);
        }
        //If user wants to download GeoGebra
        if (GeoGebraDownload)
        {
            //Download GeoGebra
            CMDText = "msiexec /i \"{current_drive_letter}\\pkgs\\GeogebraOffline\\GeoGebra-Windows-Installer-6-0-848-0.msi\" ALLUSERS=2 /qb";
            System.Diagnostics.Process.Start("CMD.exe", CMDText);
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
    }
}