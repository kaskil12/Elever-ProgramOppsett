import os
import tkinter as tk

window = tk.Tk()

installOffice = False
installTeams = False
installTeamsTK = tk.IntVar()
installOfficeTK = tk.IntVar()

def checkBoxes():
    global installOffice, installTeams
    
    officeCheck = tk.Checkbutton(window, text="Office", variable=installOfficeTK)
    teamsCheck = tk.Checkbutton(window, text="Teams", variable=installTeamsTK)
    officeCheck.pack()
    teamsCheck.pack()
    
    installButton = tk.Button(window, text="Install", command=install_packages)
    installButton.pack()

    window.mainloop()

def install_packages():
    global installOffice, installTeams
    
    installOffice = installOfficeTK.get() == 1
    installTeams = installTeamsTK.get() == 1
    # os.system("REG Add HKLM\SOFTWARE\Policies\Microsoft\Windows\Appx /v AllowAllTrustedApps /t REG_DWORD /d 1")
    if installOffice:
        print("Installing Office")
        os.system("start .\OfficeOffline\setup.exe /configure .\OfficeOffline\Elvis.xml")
    if installTeams:
        print("Installing Teams")
        os.system("Add-AppxPackage -Path .\TeamsOffline\MSTeams-x64.msix")
    print("Done")

checkBoxes()