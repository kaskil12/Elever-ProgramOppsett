import os
import tkinter as tk

window = tk.Tk()

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

def checkBoxes():
    global installOffice, installTeams, installOrdnett, installVsCode, installChrome, installFirefox
    
    infotext = tk.Label(window, text="Choose the drive letter of the USB drive")
    infotext.pack()
    DriveLetter = tk.Entry(window)
    DriveLetter.pack()
    DriveLetter.insert(0, "D:")
    
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
    if installVsCode:
        print("Installing Visual Studio Code")
        os.system(r'start .\VsCodeOffline\Code.exe')
    if installChrome:
        print("Installing Google Chrome")
        os.system(r'start .\ChromeOffline\ChromeStandaloneSetup64.exe')
    if installFirefox:
        print("Installing Mozilla Firefox")
        os.system(r'msiexec /i "{drive_letter}\FireFoxOffline\Firefox Setup 130.0.msi" ALLUSERS=2 /qb')
    print("Done")

checkBoxes()
