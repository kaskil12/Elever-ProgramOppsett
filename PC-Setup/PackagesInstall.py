import os
import tkinter as tk
from tkinter import ttk

window = tk.Tk()
window.title("Installer")

DriveLetter = "D:"
installOffice = False
installTeams = False
installOrdnett = False
installVsCode = False
installChrome = False
installFirefox = False

installTeamsTK = tk.IntVar()
installOfficeTK = tk.IntVar()
installOrdnettTK = tk.IntVar()
installVsCodeTK = tk.IntVar()
installChromeTK = tk.IntVar()
installFirefoxTK = tk.IntVar()

def update_progress(total, current):
    percent = (current / total) * 100
    progress_bar['value'] = percent
    progress_label.config(text=f"Progress: {int(percent)}%")
    window.update_idletasks()

def checkBoxes():
    global installOffice, installTeams, installOrdnett, installVsCode, installChrome, installFirefox
    
    infotext = tk.Label(window, text="Choose the drive letter of the USB drive")
    infotext.pack()
    drive_entry = tk.Entry(window)
    drive_entry.pack()
    drive_entry.insert(0, "D:")
    
    officeCheck = tk.Checkbutton(window, text="Office", variable=installOfficeTK)
    teamsCheck = tk.Checkbutton(window, text="Teams", variable=installTeamsTK)
    ordnettCheck = tk.Checkbutton(window, text="Ordnett", variable=installOrdnettTK)
    vsCodeCheck = tk.Checkbutton(window, text="Visual Studio Code", variable=installVsCodeTK)
    chromeCheck = tk.Checkbutton(window, text="Google Chrome", variable=installChromeTK)
    firefoxCheck = tk.Checkbutton(window, text="Mozilla Firefox", variable=installFirefoxTK)
    
    officeCheck.pack()
    teamsCheck.pack()
    ordnettCheck.pack()
    vsCodeCheck.pack()
    chromeCheck.pack()
    firefoxCheck.pack()

    global progress_bar, progress_label
    progress_bar = ttk.Progressbar(window, orient='horizontal', length=300, mode='determinate')
    progress_bar.pack(pady=10)
    
    progress_label = tk.Label(window, text="Progress: 0%")
    progress_label.pack()

    installButton = tk.Button(window, text="Install", command=install_packages)
    installButton.pack()

    window.mainloop()

def install_packages():
    global installOffice, installTeams, installOrdnett, installVsCode, installChrome, installFirefox

    installOffice = installOfficeTK.get() == 1
    installTeams = installTeamsTK.get() == 1
    installOrdnett = installOrdnettTK.get() == 1
    installVsCode = installVsCodeTK.get() == 1
    installChrome = installChromeTK.get() == 1
    installFirefox = installFirefoxTK.get() == 1

    selected_count = sum([installOffice, installTeams, installOrdnett, installVsCode, installChrome, installFirefox])
    current_step = 0

    if installOffice:
        print("Installing Office")
        os.system("start .\OfficeOffline\setup.exe /configure .\OfficeOffline\Elvis.xml")
        current_step += 1
        update_progress(selected_count, current_step)
    
    if installTeams:
        print("Installing Teams")
        os.system(r'start .\TeamsOffline\teamsbootstrapper.exe -p -o "{drive_letter}\TeamsOffline\MSTeams-x64.msix"')
        current_step += 1
        update_progress(selected_count, current_step)
    
    if installOrdnett:
        print("Installing Ordnett")
        os.system(r'msiexec /i "{drive_letter}\OrdnettOffline\ordnettpluss-3.3.7-innlandet_fylkeskommune.msi" ALLUSERS=2 /qb')
        current_step += 1
        update_progress(selected_count, current_step)
    
    if installVsCode:
        print("Installing Visual Studio Code")
        os.system(r'start .\VsCodeOffline\Code.exe')
        current_step += 1
        update_progress(selected_count, current_step)
    
    if installChrome:
        print("Installing Google Chrome")
        os.system(r'start .\ChromeOffline\ChromeStandaloneSetup64.exe')
        current_step += 1
        update_progress(selected_count, current_step)
    
    if installFirefox:
        print("Installing Mozilla Firefox")
        os.system(r'msiexec /i "{drive_letter}\FireFoxOffline\Firefox Setup 130.0.msi" ALLUSERS=2 /qb')
        current_step += 1
        update_progress(selected_count, current_step)
    
    print("Installation Complete")

checkBoxes()
