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
    
    if installOffice:
        print("Installing Office")
        # os.system("start /wait OfficeSetup.exe")
    if installTeams:
        print("Installing Teams")
        # os.system("start /wait TeamsSetup.exe")
    print("Done")

checkBoxes()
