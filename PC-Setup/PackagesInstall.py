import os
import tkinter as tk
from tkinter import ttk
import subprocess
import winreg

window = tk.Tk()
window.title("Installer")
#tab chooser bools
ProgramsInstaller = True
Fixes = False
#Fixes Tab

#Programs Installer tab
DriveLetter = tk.StringVar(value="D:")
installOffice = False
installTeams = False
installOrdnett = False
installVsCode = False
installChrome = False
installFirefox = False
installPython = False
installGeoGebra = False
createWebShortcut = False
createWebShortcutKI = False

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

isOfficeInstalled = False
isTeamsInstalled = False
isOrdnettInstalled = False
isVsCodeInstalled = False
isChromeInstalled = False
isFirefoxInstalled = False
isPythonInstalled = False
isGeoGebraInstalled = False
uncheckedBox = tk.PhotoImage(file="./Bilder/unchecked.png")
checkedBox = tk.PhotoImage(file="./Bilder/checked.png")

trashIcon = tk.PhotoImage(file="./Bilder/trash.png")
def switch():
    global Fixes, ProgramsInstaller
    if ProgramsInstaller:
        ProgramsInstaller = False
        Fixes = True
    else:
        Fixes = False
        ProgramsInstaller = True
if Fixes:
    def FixButtons():
        switchTabs = tk.Button(window, text="Switch Tabs", command=switch)
        switchTabs.grid(row=0, column=0, padx=10, pady=10)
        ADDRemove = tk.Button(window, text="Add/Remove Programs", command=AddRemove)
        ADDRemove.grid(row=0, column=0, padx=10, pady=10)
        SfcScan = tk.Button(window, text="SFC Scan", command=SfcScan)
        SfcScan.grid(row=1, column=0, padx=10, pady=10)

        window.mainloop()
    
    def AddRemove():
        print("Opening ADD Removed to fix office and teams")
    
    def SfcScan():
        print("Running SFC Scan")
        os.system("sfc /scannow")
if ProgramsInstaller:
    def check_installed_packages():
        global isOfficeInstalled, isTeamsInstalled, isOrdnettInstalled, isVsCodeInstalled, isChromeInstalled, isFirefoxInstalled, isPythonInstalled, isGeoGebraInstalled
        print(os.path.exists("C:/Program Files/Mozilla Firefox/"))
        print("Checking installed packages")
        isOfficeInstalled = os.path.exists(os.path.join(os.path.expanduser('~'), 'AppData', 'Local', 'Microsoft', 'Office'))
        isTeamsInstalled = os.path.exists(os.path.join(os.path.expanduser('~'), 'AppData', 'Local', 'Programs', 'Microsoft VS Code'))
        isOrdnettInstalled = os.path.exists(r"C:/Program Files (x86)/Kunnskapsforlaget/Ordnett Pluss")
        isVsCodeInstalled = os.path.exists(os.path.join(os.path.expanduser('~'), 'AppData', 'Local', 'Programs', 'Microsoft VS Code'))
        isChromeInstalled = os.path.exists("C:\Program Files (x86)\Google\Chrome\Application")
        isFirefoxInstalled = os.path.exists("C:/Program Files/Mozilla Firefox/")
        isPythonInstalled = os.path.exists('C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Python 3.12')
        isGeoGebraInstalled = os.path.exists(os.path.join(os.path.expanduser('~'), 'AppData', 'Roaming', 'GeoGebra'))
        print("Office: " + str(isOfficeInstalled))
        print("Teams: " + str(isTeamsInstalled))
        print("Ordnett: " + str(isOrdnettInstalled))
        print("VsCode: " + str(isVsCodeInstalled))
        print("Chrome: " + str(isChromeInstalled))
        print("Firefox: " + str(isFirefoxInstalled))
        print("Python: " + str(isPythonInstalled))
        print("GeoGebra: " + str(isGeoGebraInstalled))

    def update_progress(total, current):
        percent = (current / total) * 100
        progress_bar['value'] = percent
        progress_label.config(text=f"Progress: {int(percent)}%")
        window.update_idletasks()


    def checkBoxes():
        global progress_bar, progress_label
        check_installed_packages()
        switchTabs = tk.Button(window, text="Switch Tabs", command=switch)
        switchTabs.grid(row=0, column=0, padx=10, pady=10)

        infotext = tk.Label(window, text="Choose the drive letter of the USB drive")
        infotext.grid(row=0, column=0, columnspan=2, padx=10, pady=10, sticky='w')
        
        drive_entry = tk.Entry(window, textvariable=DriveLetter)
        drive_entry.grid(row=1, column=0, columnspan=2, padx=10, pady=10, sticky='w')

        row_num = 2

        def add_check_and_trash(row, label_text, install_var, is_installed_var, uninstall_command):
            check = tk.Checkbutton(window, text=label_text, variable=install_var, image=uncheckedBox, indicatoron=False, selectimage=checkedBox, compound="left", padx=10, borderwidth=0, highlightthickness=0, relief="flat", selectcolor=window.cget("bg"))
            check.grid(row=row, column=0, padx=10, pady=5, sticky='w')

            if is_installed_var:
                trash_button = tk.Button(window, image=trashIcon, command=lambda: uninstall_packages(uninstall_command))
                trash_button.grid(row=row, column=1, padx=10, pady=5, sticky='e')

        # Add the checkboxes with the trash buttons
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
        if program == "Office":
            # os.system('start /wait .\OfficeOffline\setup.exe /configure .\OfficeOffline\ElvisUninstall.xml')
            os.system(r'Get-WmiObject -Query "SELECT * FROM Win32_Product WHERE Name LIKE \'Microsoft Office%\'" | ForEach-Object { $_.Uninstall() }')
        elif program == "Teams":
            os.system(r'Get-WmiObject -Query "SELECT * FROM Win32_Product WHERE Name = \'Microsoft Teams\'" | ForEach-Object { $_.Uninstall() }')
        elif program == "Ordnett":
            os.system(r'Get-WmiObject -Query "SELECT * FROM Win32_Product WHERE Name = \'Ordnett\'" | ForEach-Object { $_.Uninstall() }')
        elif program == "VsCode":
            os.system(r'Get-WmiObject -Query "SELECT * FROM Win32_Product WHERE Name = \'Microsoft Visual Studio Code\'" | ForEach-Object { $_.Uninstall() }')
        elif program == "Chrome":
            os.system(r'Get-WmiObject -Query "SELECT * FROM Win32_Product WHERE Name = \'Google Chrome\'" | ForEach-Object { $_.Uninstall() }')
        elif program == "Firefox":
            os.system(r'Get-WmiObject -Query "SELECT * FROM Win32_Product WHERE Name = \'Mozilla Firefox\'" | ForEach-Object { $_.Uninstall() }')
        elif program == "Python":
            os.system(r'Get-WmiObject -Query "SELECT * FROM Win32_Product WHERE Name LIKE \'Python%\'" | ForEach-Object { $_.Uninstall() }')
        elif program == "GeoGebra":
            os.system(r'Get-WmiObject -Query "SELECT * FROM Win32_Product WHERE Name LIKE \'GeoGebra%\'" | ForEach-Object { $_.Uninstall() }')


    def install_packages():
        global installOffice, installTeams, installOrdnett, installVsCode, installChrome, installFirefox, installPython, installGeoGebra, createWebShortcut
        
        current_drive_letter = DriveLetter.get()
        print(f"Selected Drive: {current_drive_letter}")

        installOffice = installOfficeTK.get() == 1
        installTeams = installTeamsTK.get() == 1
        installOrdnett = installOrdnettTK.get() == 1
        installVsCode = installVsCodeTK.get() == 1
        installChrome = installChromeTK.get() == 1
        installFirefox = installFirefoxTK.get() == 1
        installPython = installPythonTK.get() == 1
        installGeoGebra = installGeoGebraTK.get() == 1
        createWebShortcut = createWebShortcutTK.get() == 1

        selected_count = sum([installOffice, installTeams, installOrdnett, installVsCode, installChrome, installFirefox, installPython, installGeoGebra, createWebShortcut])
        current_step = 0

        if installOffice:
            print("Installing Office")
            os.system("start /wait .\OfficeOffline\setup.exe /configure .\OfficeOffline\Elvis.xml")
            current_step += 1
            update_progress(selected_count, current_step)
        
        if installTeams:
            print("Installing Teams")
            os.system(rf'start /wait .\TeamsOffline\teamsbootstrapper.exe -p -o "{current_drive_letter}\TeamsOffline\MSTeams-x64.msix"')
            current_step += 1
            update_progress(selected_count, current_step)
        
        if installOrdnett:
            print("Installing Ordnett")
            os.system(rf'msiexec /i "{current_drive_letter}\OrdnettOffline\ordnettpluss-3.3.7-innlandet_fylkeskommune.msi" ALLUSERS=2 /qb')
            current_step += 1
            update_progress(selected_count, current_step)
        
        if installVsCode:
            print("Installing Visual Studio Code")
            os.system(rf'start /wait .\VsCodeOffline\Code.exe')
            current_step += 1
            update_progress(selected_count, current_step)
        
        if installChrome:
            print("Installing Google Chrome")
            os.system(rf'start /wait .\ChromeOffline\ChromeStandaloneSetup64.exe')
            current_step += 1
            update_progress(selected_count, current_step)
        
        if installFirefox:
            print("Installing Mozilla Firefox")
            os.system(r'start /wait .\FirefoxOffline\FireFoxInstall.exe /S')
            current_step += 1
            update_progress(selected_count, current_step)

        if installPython:
            print("Installing Python")
            os.system(r'start /wait .\PythonOffline\python-3.12.6-amd64.exe /quiet PrependPath=1')
            current_step += 1
            update_progress(selected_count, current_step)
            
        if installGeoGebra:
            print("Installing GeoGebra")
            os.system(rf'msiexec /i "{current_drive_letter}\GeogebraOffline\GeoGebra-Windows-Installer-6-0-848-0.msi" ALLUSERS=2 /qb')
            current_step += 1
            update_progress(selected_count, current_step)
        if createWebShortcut:
            print("Creating Web Shortcut")
            os.system(r'''powershell -command "$s=(New-Object -COM WScript.Shell).CreateShortcut('%userprofile%\Desktop\ElverumVGS.url');$s.TargetPath='http://elverum.vgs.no';$s.Save()"''')

            os.system(r'''powershell -command "$s=(New-Object -COM WScript.Shell).CreateShortcut('%userprofile%\Desktop\SharePoint.url');$s.TargetPath='https://innlandet.sharepoint.com';$s.Save()"''')

            os.system(r'''powershell -command "$s=(New-Object -COM WScript.Shell).CreateShortcut('%userprofile%\Desktop\VismaInSchool.url');$s.TargetPath='https://elverum-vgs.inschool.visma.no/Login.jsp';$s.Save()"''')
            current_step += 1
            update_progress(selected_count, current_step)
        if createWebShortcutKI:
            print("Creating Web Shortcut For KI")
            #creating karriere innlandet shortcut
            os.system(r'''powershell -command "$s=(New-Object -COM WScript.Shell).CreateShortcut('%userprofile%\Desktop\KarriereInnlandet.url');$s.TargetPath='https://www.karriereinnlandet.no/';$s.Save()"''')
            #creating karriere innsia shortcut
            os.system(r'''powershell -command "$s=(New-Object -COM WScript.Shell).CreateShortcut('%userprofile%\Desktop\KarriereInnsia.url');$s.TargetPath='https://innlandet.sharepoint.com/sites/Voksnedeltakere';$s.Save()"''')
            #creating karriere visma shortcut
            os.system(r'''powershell -command "$s=(New-Object -COM WScript.Shell).CreateShortcut('%userprofile%\Desktop\KarriereVisma.url');$s.TargetPath='https://karriere-innlandet.inschool.visma.no/';$s.Save()"''')
        print("Installation Complete")

        progress_label.config(text="Installation Complete")

        
    try:
        checkBoxes()
    except Exception as e:
        print(f"An error occurred: {e}")
        input("Press Enter to exit...")