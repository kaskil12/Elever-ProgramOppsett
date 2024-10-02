import os
import tkinter as tk
from tkinter import ttk
import sys

# Tab control variables
ProgramsInstaller = True
Fixes = False

#root window
root = tk.Tk()
root.title("PC Setup")
root.iconbitmap("./pkgs/icon/ikon.ico")
# Programs Installer tab variables
DriveLetter = tk.StringVar(value="D:")
installOffice = installTeams = installOrdnett = installVsCode = False
installChrome = installFirefox = installPython = installGeoGebra = False
createWebShortcut = createWebShortcutKI = False

installTeamsTK = tk.IntVar()
installOfficeTK = tk.IntVar()
installOrdnettTK = tk.IntVar()
installVsCodeTK = tk.IntVar()
installChromeTK = tk.IntVar()
installFirefoxTK = tk.IntVar()
installPythonTK = tk.IntVar()
installGeoGebraTK = tk.IntVar()
createWebShortcutTK = tk.IntVar()
createWebShortcutTKKI = tk.IntVar()

isOfficeInstalled = isTeamsInstalled = isOrdnettInstalled = False
isVsCodeInstalled = isChromeInstalled = isFirefoxInstalled = False
isPythonInstalled = isGeoGebraInstalled = False

# Load images
uncheckedBox = tk.PhotoImage(file="./pkgs/Bilder/unchecked.png")
checkedBox = tk.PhotoImage(file="./pkgs/Bilder/checked.png")
trashIcon = tk.PhotoImage(file="./pkgs/Bilder/trash.png")

#Eject USB Drive if wanted
EjectDrive = False
EjectDriveTK = tk.IntVar()

# Tab switch function

# Fixes Tab
def FixesTab(tab):
    def FixButtons():
        ADDRemove = tk.Button(tab, text="Add/Remove Programs", command=AddRemove)
        ADDRemove.grid(row=0, column=0, padx=0, pady=10, sticky='w')
        SfcScanButton = tk.Button(tab, text="SFC Scan", command=SfcScan)
        SfcScanButton.grid(row=1, column=0, padx=0, pady=10, sticky='w')

    def AddRemove():
        print("Opening Add/Remove Programs")
        # Implement the actual logic here

    def SfcScan():
        print("Running SFC Scan")
        os.system("sfc /scannow")

    FixButtons()
# Programs Installer tab
def ProgramsInstallerTab(tab):
    label = tk.Label(tab, text="Programs Installer")
    label.grid(padx=20, pady=20)
    def check_installed_packages(tab):
        global isOfficeInstalled, isTeamsInstalled, isOrdnettInstalled, isVsCodeInstalled
        global isChromeInstalled, isFirefoxInstalled, isPythonInstalled, isGeoGebraInstalled

        print("Checking installed packages")
        isOfficeInstalled = os.path.exists(os.path.join(os.path.expanduser('~'), 'AppData', 'Local', 'Microsoft', 'Office'))
        isTeamsInstalled = os.path.exists(os.path.join(os.path.expanduser('~'), 'AppData', 'Local', 'Programs', 'Microsoft VS Code'))
        isOrdnettInstalled = os.path.exists(r"C:/Program Files (x86)/Kunnskapsforlaget/Ordnett Pluss")
        isVsCodeInstalled = os.path.exists(os.path.join(os.path.expanduser('~'), 'AppData', 'Local', 'Programs', 'Microsoft VS Code'))
        isChromeInstalled = os.path.exists("C:/Program Files (x86)/Google/Chrome/Application")
        isFirefoxInstalled = os.path.exists("C:/Program Files/Mozilla Firefox/")
        isPythonInstalled = os.path.exists('C:/ProgramData/Microsoft/Windows/Start Menu/Programs/Python 3.12')
        isGeoGebraInstalled = os.path.exists(os.path.join(os.path.expanduser('~'), 'AppData', 'Roaming', 'GeoGebra'))
        print("Done checking installed packages. These are the results:")
        print(f"Office: {isOfficeInstalled}")
        print(f"Teams: {isTeamsInstalled}")
        print(f"Ordnett: {isOrdnettInstalled}")
        print(f"VsCode: {isVsCodeInstalled}")
        print(f"Chrome: {isChromeInstalled}")
        print(f"Firefox: {isFirefoxInstalled}")
        print(f"Python: {isPythonInstalled}")
        print(f"GeoGebra: {isGeoGebraInstalled}")

    def update_progress(total, current, tab):
        percent = (current / total) * 100
        progress_bar['value'] = percent
        progress_label.config(text=f"Progress: {int(percent)}%")
        tab.update_idletasks()

    def add_check_and_trash(row, label_text, install_var, is_installed_var, uninstall_command, tab):
        check = tk.Checkbutton(tab, text=label_text, variable=install_var, image=uncheckedBox, indicatoron=False, selectimage=checkedBox, compound="left", padx=10, borderwidth=0, highlightthickness=0, relief="flat", selectcolor=tab.tk.eval("ttk::style lookup TFrame -background"))


        check.grid(row=row, column=0, padx=10, pady=5, sticky='w')

        if is_installed_var:
            trash_button = tk.Button(tab, image=trashIcon, command=lambda: uninstall_packages(uninstall_command, tab))
            trash_button.grid(row=row, column=1, padx=10, pady=5, sticky='e')

    def checkBoxes(tab):
        global progress_bar, progress_label
        global installOffice, installTeams, installOrdnett, installVsCode
        global installChrome, installFirefox, installPython, installGeoGebra
        global createWebShortcut, createWebShortcutKI
        global EjectDrive


        check_installed_packages(tab)

        infotext = tk.Label(tab, text="Choose the drive letter of the USB drive")
        infotext.grid(row=0, column=0, columnspan=2, padx=10, pady=10, sticky='w')

        ejectCheckBox = tk.Checkbutton(tab, text="Eject USB Drive", variable=EjectDrive, compound="left", padx=10, borderwidth=0, highlightthickness=0, relief="flat",selectcolor="white")
        ejectCheckBox.grid(row=1, column=1, padx=10, pady=5)

        drive_entry = tk.Entry(tab, textvariable=DriveLetter)
        drive_entry.grid(row=1, column=0, columnspan=2, padx=10, pady=10, sticky='w')

        row_num = 2

        # Add checkboxes with trash buttons
        add_check_and_trash(row_num, "Office", installOfficeTK, isOfficeInstalled, "Office", tab)
        row_num += 1
        add_check_and_trash(row_num, "Teams", installTeamsTK, isTeamsInstalled, "Teams", tab)
        row_num += 1
        add_check_and_trash(row_num, "Ordnett", installOrdnettTK, isOrdnettInstalled, "Ordnett", tab)
        row_num += 1
        add_check_and_trash(row_num, "Visual Studio Code", installVsCodeTK, isVsCodeInstalled, "VsCode", tab)
        row_num += 1
        add_check_and_trash(row_num, "Google Chrome", installChromeTK, isChromeInstalled, "Chrome", tab)
        row_num += 1
        add_check_and_trash(row_num, "Mozilla Firefox", installFirefoxTK, isFirefoxInstalled, "Firefox", tab)
        row_num += 1
        add_check_and_trash(row_num, "Python", installPythonTK, isPythonInstalled, "Python", tab)
        row_num += 1
        add_check_and_trash(row_num, "GeoGebra", installGeoGebraTK, isGeoGebraInstalled, "GeoGebra", tab)

        # Web shortcuts
        WebShortcutCheck = tk.Checkbutton(tab, text="Nettside snarveier", variable=createWebShortcutTK, image=uncheckedBox, indicatoron=False, selectimage=checkedBox, compound="left", padx=10, borderwidth=0, highlightthickness=0, relief="flat", selectcolor=tab.tk.eval("ttk::style lookup TFrame -background"))
        WebShortcutCheck.grid(row=row_num, column=0, padx=10, pady=5, sticky='w')
        row_num += 1
        WebShortcutCheckKI = tk.Checkbutton(tab, text="Nettside snarveier Karriere", variable=createWebShortcutTKKI, image=uncheckedBox, indicatoron=False, selectimage=checkedBox, compound="left", padx=10, borderwidth=0, highlightthickness=0, relief="flat", selectcolor=tab.tk.eval("ttk::style lookup TFrame -background"))
        WebShortcutCheckKI.grid(row=row_num, column=0, padx=10, pady=5, sticky='w')

        # Progress bar and installation button
        progress_bar = ttk.Progressbar(tab, orient='horizontal', length=300, mode='determinate')
        progress_bar.grid(row=row_num+1, column=0, columnspan=2, padx=10, pady=10)

        progress_label = tk.Label(tab, text="Progress: 0%")
        progress_label.grid(row=row_num+2, column=0, columnspan=2, padx=10, pady=5)

        installButton = tk.Button(tab, text="Install", command=lambda: install_packages(tab))
        installButton.grid(row=row_num+3, column=0, columnspan=2, padx=10, pady=10)

    def uninstall_packages(program, tab):
        current_drive_letter = DriveLetter.get()
        uninstall_commands = {
            "Office": "start /wait {current_drive_letter}/OfficeOffline/setup.exe /configure {current_drive_letter}/OfficeOffline/ElvisUninstall.xml",
            "Teams": r'Get-WmiObject -Query "SELECT * FROM Win32_Product WHERE Name = \'Microsoft Teams\'" | ForEach-Object { $_.Uninstall() }',
            "Ordnett": r'Get-WmiObject -Query "SELECT * FROM Win32_Product WHERE Name = \'Ordnett\'" | ForEach-Object { $_.Uninstall() }',
            "VsCode": r'Get-WmiObject -Query "SELECT * FROM Win32_Product WHERE Name = \'Microsoft Visual Studio Code\'" | ForEach-Object { $_.Uninstall() }',
            "Chrome": r'Get-WmiObject -Query "SELECT * FROM Win32_Product WHERE Name = \'Google Chrome\'" | ForEach-Object { $_.Uninstall() }',
            "Firefox": r'Get-WmiObject -Query "SELECT * FROM Win32_Product WHERE Name = \'Mozilla Firefox\'" | ForEach-Object { $_.Uninstall() }',
            "Python": r'Get-WmiObject -Query "SELECT * FROM Win32_Product WHERE Name = \'Python\'" | ForEach-Object { $_.Uninstall() }',
            "GeoGebra": r'Get-WmiObject -Query "SELECT * FROM Win32_Product WHERE Name = \'GeoGebra\'" | ForEach-Object { $_.Uninstall() }'
        }

        uninstall_command = uninstall_commands.get(program, "")
        if uninstall_command:
            os.system(f"powershell.exe -ExecutionPolicy Bypass -Command {uninstall_command}")

    def install_packages(tab):
        global installOffice, installTeams, installOrdnett, installVsCode
        global installChrome, installFirefox, installPython, installGeoGebra
        global createWebShortcut, createWebShortcutKI

        installOffice = installOfficeTK.get()
        installTeams = installTeamsTK.get()
        installOrdnett = installOrdnettTK.get()
        installVsCode = installVsCodeTK.get()
        installChrome = installChromeTK.get()
        installFirefox = installFirefoxTK.get()
        installPython = installPythonTK.get()
        installGeoGebra = installGeoGebraTK.get()
        createWebShortcut = createWebShortcutTK.get()
        EjectDrive = EjectDriveTK.get()
        total_installations = 8
        current_installation = 0
        current_drive_letter = DriveLetter.get()
        if installOffice:
            print("Installing Office")
            os.system("start /wait .\pkgs\OfficeOffline\setup.exe /configure .\pkgs\OfficeOffline\Elvis.xml")
            current_installation += 1
            update_progress(total_installations, current_installation + 1, tab)
        
        if installTeams:
            print("Installing Teams")
            os.system(rf'start /wait .\pkgs\TeamsOffline\teamsbootstrapper.exe -p -o "{current_drive_letter}\pkgs\TeamsOffline\MSTeams-x64.msix"')
            current_installation += 1
            update_progress(total_installations, current_installation + 1, tab)

        if installOrdnett:
            print("Installing Ordnett")
            os.system(rf'msiexec /i "{current_drive_letter}\pkgs\OrdnettOffline\ordnettpluss-3.3.7-innlandet_fylkeskommune.msi" ALLUSERS=2 /qb')
            current_installation += 1
            update_progress(total_installations, current_installation + 1, tab)
        
        if installVsCode:
            print("Installing Visual Studio Code")
            os.system(rf'start /wait .\pkgs\VsCodeOffline\Code.exe')
            current_installation += 1
            update_progress(total_installations, current_installation + 1, tab)
        
        if installChrome:
            print("Installing Google Chrome")
            os.system(rf'start /wait .\pkgs\ChromeOffline\ChromeStandaloneSetup64.exe')
            current_installation += 1
            update_progress(total_installations, current_installation + 1, tab)

        if installVsCode:
            print("Installing VS Code")
            os.system(rf'start /wait .\pkgs\VsCodeOffline\Code.exe')
            current_installation += 1
            update_progress(total_installations, current_installation + 1, tab)

        if installChrome:
            print("Installing Google Chrome")
            os.system(rf'start /wait .\pkgs\ChromeOffline\ChromeStandaloneSetup64.exe')
            current_installation += 1
            update_progress(total_installations, current_installation + 1, tab)
        
        if installFirefox:
            print("Installing Mozilla Firefox")
            os.system(r'start /wait .\pkgs\FirefoxOffline\FireFoxInstall.exe /S')
            current_installation += 1
            update_progress(total_installations, current_installation + 1, tab)

        if installPython:
            os.system(r'start /wait .\pkgs\PythonOffline\python-3.12.6-amd64.exe /quiet PrependPath=1')
            current_installation += 1
            update_progress(total_installations, current_installation + 1, tab)
            
        if installGeoGebra:
            os.system(rf'msiexec /i "{current_drive_letter}\pkgs\GeogebraOffline\GeoGebra-Windows-Installer-6-0-848-0.msi" ALLUSERS=2 /qb')
            current_installation += 1
            update_progress(total_installations, current_installation + 1, tab)

        if createWebShortcut:
            print("Creating Web Shortcut")
            os.system(r'''powershell -command "$s=(New-Object -COM WScript.Shell).CreateShortcut('%userprofile%\Desktop\ElverumVGS.url');$s.TargetPath='http://elverum.vgs.no';$s.Save()"''')

            os.system(r'''powershell -command "$s=(New-Object -COM WScript.Shell).CreateShortcut('%userprofile%\Desktop\SharePoint.url');$s.TargetPath='https://innlandet.sharepoint.com';$s.Save()"''')

            os.system(r'''powershell -command "$s=(New-Object -COM WScript.Shell).CreateShortcut('%userprofile%\Desktop\VismaInSchool.url');$s.TargetPath='https://elverum-vgs.inschool.visma.no/Login.jsp';$s.Save()"''')
            current_installation += 1
            update_progress(total_installations, current_installation, tab)
        if createWebShortcutKI:
            print("Creating Web Shortcut For KI")
            #creating karriere innlandet shortcut
            os.system(r'''powershell -command "$s=(New-Object -COM WScript.Shell).CreateShortcut('%userprofile%\Desktop\KarriereInnlandet.url');$s.TargetPath='https://www.karriereinnlandet.no/';$s.Save()"''')
            #creating karriere innsia shortcut
            os.system(r'''powershell -command "$s=(New-Object -COM WScript.Shell).CreateShortcut('%userprofile%\Desktop\KarriereInnsia.url');$s.TargetPath='https://innlandet.sharepoint.com/sites/Voksnedeltakere';$s.Save()"''')
            #creating karriere visma shortcut
            os.system(r'''powershell -command "$s=(New-Object -COM WScript.Shell).CreateShortcut('%userprofile%\Desktop\KarriereVisma.url');$s.TargetPath='https://karriere-innlandet.inschool.visma.no/';$s.Save()"''')
        print("Installation Complete")
        if EjectDrive:
            run_bat_file()
            shutdown_program() 
    checkBoxes(tab)
def shutdown_program():
    print("Shutting down the program...")
    os._exit(0)

def run_bat_file():
    os.system(f"start /b ./pkgs/UtilsBats/Eject")
        

def main():
    notebook = ttk.Notebook(root)


    tab1 = ttk.Frame(notebook)
    tab2 = ttk.Frame(notebook)

    notebook.add(tab1, text="Programs Installer")
    notebook.add(tab2, text="Fixes")


    ProgramsInstallerTab(tab1)
    FixesTab(tab2)

    notebook.grid(padx=10, pady=10, sticky="nsew")
    root.columnconfigure(0, weight=1)
    root.rowconfigure(0, weight=1)
    root.mainloop()
if __name__ == "__main__":
    main()
