import os
import tkinter as tk

window = tk.Tk()

DriveLetter = "D:"
installOffice = False
installTeams = False
installOrdnett = False
installTeamsTK = tk.IntVar()
installOfficeTK = tk.IntVar()
installOrdnettTK = tk.IntVar()

def checkBoxes():
    global installOffice, installTeams
    
    #choose drive letter
    driveLabel = tk.Label(window, text="Choose drive letter")
    #text field for drive letter
    driveLetterEntry = tk.Entry(window)
    driveLabel.pack()
    driveLetterEntry.pack()
    
    officeCheck = tk.Checkbutton(window, text="Office", variable=installOfficeTK)
    teamsCheck = tk.Checkbutton(window, text="Teams", variable=installTeamsTK)
    ordnettCheck = tk.Checkbutton(window, text="Ordnett", variable=installOrdnettTK)
    officeCheck.pack()
    teamsCheck.pack()
    ordnettCheck.pack()
    
    installButton = tk.Button(window, text="Install", command=install_packages)
    installButton.pack()

    window.mainloop()

def install_packages():
    global installOffice, installTeams, installOrdnett

    
    installOffice = installOfficeTK.get() == 1
    installTeams = installTeamsTK.get() == 1
    installOrdnett = installOrdnettTK.get() == 1
    # os.system("REG Add HKLM\SOFTWARE\Policies\Microsoft\Windows\Appx /v AllowAllTrustedApps /t REG_DWORD /d 1")
    if installOffice:
        print("Installing Office")
        os.system("start .\OfficeOffline\setup.exe /configure .\OfficeOffline\Elvis.xml")
    if installTeams:
        print("Installing Teams")
        os.system(r'start .\TeamsOffline\teamsbootstrapper.exe -p -o "D:\TeamsOffline\MSTeams-x64.msix"')
    if installOrdnett:
        print("Installing Ordnett")
        os.system(r'msiexec /i "D:\OrdnettOffline\ordnettpluss-3.3.7-innlandet_fylkeskommune.msi" ALLUSERS=2 /qb')
    print("Done")

checkBoxes()
