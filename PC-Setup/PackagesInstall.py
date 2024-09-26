import os
import tkinter as tk
from tkinter import ttk

# Initialize window
window = tk.Tk()
window.title("Installer")

# Tab control variables
ProgramsInstaller = True
Fixes = False

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
uncheckedBox = tk.PhotoImage(file="./Bilder/unchecked.png")
checkedBox = tk.PhotoImage(file="./Bilder/checked.png")
trashIcon = tk.PhotoImage(file="./Bilder/trash.png")

#Eject USB Drive if wanted
EjectDrive = False
EjectDriveTK = tk.IntVar()

# Tab switch function
def switch():
    global Fixes, ProgramsInstaller
    ProgramsInstaller = not ProgramsInstaller
    Fixes = not Fixes

# Fixes Tab
if Fixes:
    def FixButtons():
        switchTabs = tk.Button(window, text="Switch Tabs", command=switch)
        switchTabs.grid(row=0, column=0, padx=10, pady=10)
        ADDRemove = tk.Button(window, text="Add/Remove Programs", command=AddRemove)
        ADDRemove.grid(row=0, column=1, padx=10, pady=10)
        SfcScan = tk.Button(window, text="SFC Scan", command=SfcScan)
        SfcScan.grid(row=1, column=0, padx=10, pady=10)
        window.mainloop()
    
    def AddRemove():
        print("Opening Add/Remove Programs")

    def SfcScan():
        print("Running SFC Scan")
        os.system("sfc /scannow")

# Programs Installer tab
if ProgramsInstaller:
    def check_installed_packages():
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

    def update_progress(total, current):
        percent = (current / total) * 100
        progress_bar['value'] = percent
        progress_label.config(text=f"Progress: {int(percent)}%")
        window.update_idletasks()

    def add_check_and_trash(row, label_text, install_var, is_installed_var, uninstall_command):
        check = tk.Checkbutton(window, text=label_text, variable=install_var, image=uncheckedBox, indicatoron=False, selectimage=checkedBox, compound="left", padx=10, borderwidth=0, highlightthickness=0, relief="flat", selectcolor=window.cget("bg"))
        check.grid(row=row, column=0, padx=10, pady=5, sticky='w')

        if is_installed_var:
            trash_button = tk.Button(window, image=trashIcon, command=lambda: uninstall_packages(uninstall_command))
            trash_button.grid(row=row, column=1, padx=10, pady=5, sticky='e')

    def checkBoxes():
        global progress_bar, progress_label

        check_installed_packages()

        switchTabs = tk.Button(window, text="Switch Tabs", command=switch)
        switchTabs.grid(row=0, column=1, padx=10, pady=10)

        infotext = tk.Label(window, text="Choose the drive letter of the USB drive")
        infotext.grid(row=0, column=0, columnspan=2, padx=10, pady=10, sticky='w')

        ejectCheckBox = tk.Checkbutton(window, text="Eject USB Drive", variable=EjectDrive, image=uncheckedBox, indicatoron=False, selectimage=checkedBox, compound="left", padx=10, borderwidth=0, highlightthickness=0, relief="flat", selectcolor=window.cget("bg"))
        ejectCheckBox.grid(row=0, column=2, padx=10, pady=5, sticky='w')

        drive_entry = tk.Entry(window, textvariable=DriveLetter)
        drive_entry.grid(row=1, column=0, columnspan=2, padx=10, pady=10, sticky='w')

        row_num = 2

        # Add checkboxes with trash buttons
        add_check_and_trash(row_num, "Office", installOfficeTK, isOfficeInstalled, "Office")
        row_num += 1
        add_check_and_trash(row_num, "Teams", installTeamsTK, isTeamsInstalled, "Teams")
        row_num += 1
        add_check_and_trash(row_num, "Ordnett", installOrdnettTK, isOrdnettInstalled, "Ordnett")
        row_num += 1
        add_check_and_trash(row_num, "Visual Studio Code", installVsCodeTK, isVsCodeInstalled, "VsCode")
        row_num += 1
        add_check_and_trash(row_num, "Google Chrome", installChromeTK, isChromeInstalled, "Chrome")
        row_num += 1
        add_check_and_trash(row_num, "Mozilla Firefox", installFirefoxTK, isFirefoxInstalled, "Firefox")
        row_num += 1
        add_check_and_trash(row_num, "Python", installPythonTK, isPythonInstalled, "Python")
        row_num += 1
        add_check_and_trash(row_num, "GeoGebra", installGeoGebraTK, isGeoGebraInstalled, "GeoGebra")

        # Web shortcuts
        WebShortcutCheck = tk.Checkbutton(window, text="Nettside snarveier", variable=createWebShortcutTK, image=uncheckedBox, indicatoron=False, selectimage=checkedBox, compound="left", padx=10, borderwidth=0, highlightthickness=0, relief="flat", selectcolor=window.cget("bg"))
        WebShortcutCheck.grid(row=row_num, column=0, padx=10, pady=5, sticky='w')
        row_num += 1
        WebShortcutCheckKI = tk.Checkbutton(window, text="Nettside snarveier Karriere", variable=createWebShortcutTKKI, image=uncheckedBox, indicatoron=False, selectimage=checkedBox, compound="left", padx=10, borderwidth=0, highlightthickness=0, relief="flat", selectcolor=window.cget("bg"))
        WebShortcutCheckKI.grid(row=row_num, column=0, padx=10, pady=5, sticky='w')

        # Progress bar and installation button
        progress_bar = ttk.Progressbar(window, orient='horizontal', length=300, mode='determinate')
        progress_bar.grid(row=row_num+1, column=0, columnspan=2, padx=10, pady=10)

        progress_label = tk.Label(window, text="Progress: 0%")
        progress_label.grid(row=row_num+2, column=0, columnspan=2, padx=10, pady=5)

        installButton = tk.Button(window, text="Install", command=install_packages)
        installButton.grid(row=row_num+3, column=0, columnspan=2, padx=10, pady=10)

        window.mainloop()

    def uninstall_packages(program):
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

    def install_packages():
        total_installations = 8
        current_installation = 0
        current_drive_letter = DriveLetter.get()
        if installOfficeTK.get():
            print("Installing Office")
            os.system("start /wait .\OfficeOffline\setup.exe /configure .\OfficeOffline\Elvis.xml")
            current_installation += 1
            update_progress(total_installations, current_installation)
        
        if installTeams:
            os.system(r"Start /wait setup.exe /configure installOffice.xml")
            current_installation += 1
            update_progress(total_installations, current_installation)

        if installTeamsTK.get():
            print("Installing Teams")
            os.system(rf'start /wait .\TeamsOffline\teamsbootstrapper.exe -p -o "{current_drive_letter}\TeamsOffline\MSTeams-x64.msix"')
            current_installation += 1
            update_progress(total_installations, current_installation)
        
        if installOrdnett:
            os.system(r"msiexec /i Teams_windows_x64.msi")
            current_installation += 1
            update_progress(total_installations, current_installation)

        if installOrdnettTK.get():
            print("Installing Ordnett")
            os.system(rf'msiexec /i "{current_drive_letter}\OrdnettOffline\ordnettpluss-3.3.7-innlandet_fylkeskommune.msi" ALLUSERS=2 /qb')
            current_installation += 1
            update_progress(total_installations, current_installation)
        
        if installVsCode:
            print("Installing Visual Studio Code")
            os.system(rf'start /wait .\VsCodeOffline\Code.exe')
            current_installation += 1
            update_progress(total_installations, current_installation)
        
        if installChrome:
            os.system(r"msiexec /i Ordnett.exe")
            current_installation += 1
            update_progress(total_installations, current_installation)

        if installVsCodeTK.get():
            print("Installing VS Code")
            os.system(r"VSCodeUserSetup.exe /S")
            current_installation += 1
            update_progress(total_installations, current_installation)

        if installChromeTK.get():
            print("Installing Google Chrome")
            os.system(rf'start /wait .\ChromeOffline\ChromeStandaloneSetup64.exe')
            current_installation += 1
            update_progress(total_installations, current_installation)
        
        if installFirefox:
            print("Installing Mozilla Firefox")
            os.system(r'start /wait .\FirefoxOffline\FireFoxInstall.exe /S')
            current_installation += 1
            update_progress(total_installations, current_installation)

        if installPython:
            os.system(r"ChromeSetup.exe /silent /install")
            current_installation += 1
            update_progress(total_installations, current_installation)

        if installFirefoxTK.get():
            print("Installing Firefox")
            os.system(r"FirefoxInstaller.exe /S")
            current_installation += 1
            update_progress(total_installations, current_installation)

        if installPythonTK.get():
            print("Installing Python")
            os.system(r'start /wait .\PythonOffline\python-3.12.6-amd64.exe /quiet PrependPath=1')
            current_installation += 1
            update_progress(total_installations, current_installation)
            
        if installGeoGebra:
            os.system(r"python-3.12.0.exe /quiet InstallAllUsers=1 PrependPath=1")
            current_installation += 1
            update_progress(total_installations, current_installation)

        if installGeoGebraTK.get():
            print("Installing GeoGebra")
            os.system(rf'msiexec /i "{current_drive_letter}\GeogebraOffline\GeoGebra-Windows-Installer-6-0-848-0.msi" ALLUSERS=2 /qb')
            current_installation += 1
            update_progress(total_installations, current_installation)
        if createWebShortcut:
            print("Creating Web Shortcut")
            os.system(r'''powershell -command "$s=(New-Object -COM WScript.Shell).CreateShortcut('%userprofile%\Desktop\ElverumVGS.url');$s.TargetPath='http://elverum.vgs.no';$s.Save()"''')

            os.system(r'''powershell -command "$s=(New-Object -COM WScript.Shell).CreateShortcut('%userprofile%\Desktop\SharePoint.url');$s.TargetPath='https://innlandet.sharepoint.com';$s.Save()"''')

            os.system(r'''powershell -command "$s=(New-Object -COM WScript.Shell).CreateShortcut('%userprofile%\Desktop\VismaInSchool.url');$s.TargetPath='https://elverum-vgs.inschool.visma.no/Login.jsp';$s.Save()"''')
            current_installation += 1
            update_progress(total_installations, current_installation)
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
            os._exit(0)
            os.system(f"powershell -command \"$driveEject = New-Object -comObject Shell.Application; $driveEject.Namespace(17).ParseName('{current_drive_letter}').InvokeVerb('Eject')\"")
        

    checkBoxes()
