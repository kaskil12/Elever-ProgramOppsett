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
    
    #have a dropdown of letters to choose the drive letter but the standard is D: and show it in the input field but the user can change it
    infotext = tk.Label(window, text="Choose the drive letter of the USB drive")
    infotext.pack()
    DriveLetter = tk.Entry(window)
    DriveLetter.pack()
    DriveLetter.insert(0, "D:")
    
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
        os.system(r'start .\TeamsOffline\teamsbootstrapper.exe -p -o "{drive_letter}\TeamsOffline\MSTeams-x64.msix"')
    if installOrdnett:
        print("Installing Ordnett")
        os.system(r'msiexec /i "{drive_letter}\OrdnettOffline\ordnettpluss-3.3.7-innlandet_fylkeskommune.msi" ALLUSERS=2 /qb')
    print("Done")

checkBoxes()
