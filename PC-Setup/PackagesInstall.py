import os
import tkinter as tk
from tkinter import ttk

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
    
    officeCheck = tk.Checkbutton(window, text="Office", variable=installOfficeTK)
    teamsCheck = tk.Checkbutton(window, text="Teams", variable=installTeamsTK)
    ordnettCheck = tk.Checkbutton(window, text="Ordnett", variable=installOrdnettTK)
    vsCodeCheck = tk.Checkbutton(window, text="Visual Studio Code", variable=installVsCodeTK)
    chromeCheck = tk.Checkbutton(window, text="Google Chrome", variable=installChromeTK)
    firefoxCheck = tk.Checkbutton(window, text="Mozilla Firefox", variable=installFirefoxTK)
    pythonCheck = tk.Checkbutton(window, text="Python", variable=installPythonTK)
    geogebraCheck = tk.Checkbutton(window, text="GeoGebra", variable=installGeoGebraTK)
    WebShortcutCheck = tk.Checkbutton(window, text="Create Web Shortcut", variable=createWebShortcutTK)

    
    officeCheck.pack()
    teamsCheck.pack()
    ordnettCheck.pack()
    vsCodeCheck.pack()
    chromeCheck.pack()
    firefoxCheck.pack()
    pythonCheck.pack()
    geogebraCheck.pack()
    WebShortcutCheck.pack()

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
