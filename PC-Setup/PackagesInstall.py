import os
import tkinter as tk
from tkinter import ttk
import subprocess

window = tk.Tk()
window.title("Installer")

DriveLetter = tk.StringVar(value="D:")  # Using StringVar to bind the value
installOffice = False
installTeams = False
installOrdnett = False
installVsCode = False
installChrome = False
installFirefox = False
installPython = False
installGeoGebra = False
createWebShortcut = False

installTeamsTK = tk.IntVar()
installOfficeTK = tk.IntVar()
installOrdnettTK = tk.IntVar()
installVsCodeTK = tk.IntVar()
installChromeTK = tk.IntVar()
installFirefoxTK = tk.IntVar()
installPythonTK = tk.IntVar()
installGeoGebraTK = tk.IntVar()
createWebShortcutTK = tk.IntVar()

isOfficeInstalled = False
isTeamsInstalled = False
isOrdnettInstalled = False
isVsCodeInstalled = False
isChromeInstalled = False
isFirefoxInstalled = False
isPythonInstalled = False
isGeoGebraInstalled = False

uncheckedBox = tk.PhotoImage(file=".\\PC-Setup\\Bilder\\unchecked.png")
checkedBox = tk.PhotoImage(file=".\\PC-Setup\\Bilder\\checked.png")

def is_installed(powershell_command):
    return "True" if subprocess.run(
        ["powershell", "-Command", powershell_command], 
        capture_output=True, text=True
    ).stdout.strip() else "False"

def check_installed_packages():
    global isOfficeInstalled, isTeamsInstalled, isOrdnettInstalled, isVsCodeInstalled, isChromeInstalled, isFirefoxInstalled, isPythonInstalled, isGeoGebraInstalled
    
    isOfficeInstalled = is_installed('Get-WmiObject -Class Win32_Product | Where-Object { $_.Name -like "*Office*" }') == "True"
    isTeamsInstalled = is_installed('Get-WmiObject -Class Win32_Product | Where-Object { $_.Name -like "*Teams*" }') == "True"
    isOrdnettInstalled = is_installed('Get-WmiObject -Class Win32_Product | Where-Object { $_.Name -like "*Ordnett*" }') == "True"
    isVsCodeInstalled = is_installed('Get-WmiObject -Class Win32_Product | Where-Object { $_.Name -like "*Visual Studio Code*" }') == "True"
    isChromeInstalled = is_installed('Get-WmiObject -Class Win32_Product | Where-Object { $_.Name -like "*Chrome*" }') == "True"
    isFirefoxInstalled = is_installed('Get-WmiObject -Class Win32_Product | Where-Object { $_.Name -like "*Firefox*" }') == "True"
    isPythonInstalled = is_installed('Get-WmiObject -Class Win32_Product | Where-Object { $_.Name -like "*Python*" }') == "True"
    isGeoGebraInstalled = is_installed('Get-WmiObject -Class Win32_Product | Where-Object { $_.Name -like "*GeoGebra*" }') == "True"
    print(isOfficeInstalled, isTeamsInstalled, isOrdnettInstalled, isVsCodeInstalled, isChromeInstalled, isFirefoxInstalled, isPythonInstalled, isGeoGebraInstalled)

def update_progress(total, current):
    percent = (current / total) * 100
    progress_bar['value'] = percent
    progress_label.config(text=f"Progress: {int(percent)}%")
    window.update_idletasks()

def checkBoxes():
    global progress_bar, progress_label
    
    infotext = tk.Label(window, text="Choose the drive letter of the USB drive")
    infotext.pack()
    
    drive_entry = tk.Entry(window, textvariable=DriveLetter)
    drive_entry.pack()
    
    officeCheck = tk.Checkbutton(window, text="Office", variable=installOfficeTK, image=uncheckedBox, indicatoron=False, selectimage=checkedBox, compound="left", padx=10, borderwidth=0, highlightthickness=0, relief="flat", selectcolor=window.cget("bg")  )
    teamsCheck = tk.Checkbutton(window, text="Teams", variable=installTeamsTK, image=uncheckedBox, indicatoron=False, selectimage=checkedBox, compound="left",padx=10, borderwidth=0, highlightthickness=0, relief="flat", selectcolor=window.cget("bg")  )
    ordnettCheck = tk.Checkbutton(window, text="Ordnett", variable=installOrdnettTK, image=uncheckedBox, indicatoron=False, selectimage=checkedBox, compound="left",padx=10, borderwidth=0, highlightthickness=0, relief="flat", selectcolor=window.cget("bg")  )
    vsCodeCheck = tk.Checkbutton(window, text="Visual Studio Code", variable=installVsCodeTK, image=uncheckedBox, indicatoron=False, selectimage=checkedBox, compound="left",padx=10, borderwidth=0, highlightthickness=0, relief="flat", selectcolor=window.cget("bg")  )
    chromeCheck = tk.Checkbutton(window, text="Google Chrome", variable=installChromeTK, image=uncheckedBox, indicatoron=False, selectimage=checkedBox, compound="left",padx=10, borderwidth=0, highlightthickness=0, relief="flat", selectcolor=window.cget("bg")  )
    firefoxCheck = tk.Checkbutton(window, text="Mozilla Firefox", variable=installFirefoxTK, image=uncheckedBox, indicatoron=False, selectimage=checkedBox, compound="left",padx=10, borderwidth=0, highlightthickness=0, relief="flat", selectcolor=window.cget("bg")  )
    pythonCheck = tk.Checkbutton(window, text="Python", variable=installPythonTK, image=uncheckedBox, indicatoron=False, selectimage=checkedBox, compound="left",padx=10, borderwidth=0, highlightthickness=0, relief="flat", selectcolor=window.cget("bg")  )
    geogebraCheck = tk.Checkbutton(window, text="GeoGebra", variable=installGeoGebraTK, image=uncheckedBox, indicatoron=False, selectimage=checkedBox, compound="left",padx=10, borderwidth=0, highlightthickness=0, relief="flat", selectcolor=window.cget("bg")  )
    WebShortcutCheck = tk.Checkbutton(window, text="Nettside snarveier", variable=createWebShortcutTK, image=uncheckedBox, indicatoron=False, selectimage=checkedBox, compound="left",padx=10, borderwidth=0, highlightthickness=0, relief="flat", selectcolor=window.cget("bg")  )

    
    officeCheck.pack(anchor='w', side='top')
    teamsCheck.pack(anchor='w', side='top')
    ordnettCheck.pack(anchor='w', side='top')
    vsCodeCheck.pack(anchor='w', side='top')
    chromeCheck.pack(anchor='w', side='top')
    firefoxCheck.pack(anchor='w', side='top')
    pythonCheck.pack(anchor='w', side='top')
    geogebraCheck.pack(anchor='w', side='top')
    WebShortcutCheck.pack(anchor='w', side='top')

    progress_bar = ttk.Progressbar(window, orient='horizontal', length=300, mode='determinate')
    progress_bar.pack(pady=10)
    
    progress_label = tk.Label(window, text="Progress: 0%")
    progress_label.pack()

    installButton = tk.Button(window, text="Install", command=install_packages)
    installButton.pack()

    window.mainloop()

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
    print("Installation Complete")

checkBoxes()
