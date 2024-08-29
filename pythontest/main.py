import os
import subprocess

VsCodeInstalled = False
PythonInstalled = False

def main():
    # Test to see if check is working
    print("Checking if VS Code is installed...")
    # Check if VS Code is installed
    isVSCodeInstalled()
    
    # Test to see if check is working
    print("Checking if Python is installed...")
    # Check if Python is installed
    isPythonInstalled()

    # Create a folder and file for Python and open in VS Code and create a Python file
    if VsCodeInstalled and PythonInstalled:
        createFolderAndFile()
    else:
        print("Installation failed")

def isVSCodeInstalled():
    global VsCodeInstalled
    try:
        # Check if VS Code is installed
        subprocess.run(["code", "--version"], check=True)
        VsCodeInstalled = True
    except subprocess.CalledProcessError:
        # If VS Code is not installed
        installVSCode()

def isPythonInstalled():
    global PythonInstalled
    try:
        # Check if Python is installed
        subprocess.run(["python", "--version"], check=True)
        PythonInstalled = True
    except subprocess.CalledProcessError:
        # If Python is not installed
        installPython()

def installVSCode():
    # Step 1: Download and install VS Code silently with PATH modification
    os.system("C:\\VSCodeUserSetup-x64-1.92.2.exe /verysilent /mergetasks=!runcode,addtopath")

    # Step 2: Clean up by removing the installer file
    os.remove("C:\\VSCodeUserSetup-x64-1.92.2.exe")

    # Step 3: Verify installation by checking VS Code version
    subprocess.run(["code", "--version"], check=True)
    global VsCodeInstalled
    VsCodeInstalled = True

def installPython():
    # Download and install Python silently with PATH modification
    os.system("./python-3.12.5-amd64.exe /quiet InstallAllUsers=1 PrependPath=1")

    # Clean up by removing the installer file
    os.remove("C:\\python-3.12.5-amd64.exe")

    # Verify installation by checking Python version
    subprocess.run(["python", "--version"], check=True)
    global PythonInstalled
    PythonInstalled = True

def createFolderAndFile():
    desktopPath = os.path.join(os.environ['USERPROFILE'], 'Desktop')
    os.makedirs(os.path.join(desktopPath, "PythonFolder"), exist_ok=True)
    with open(os.path.join(desktopPath, "PythonFolder", "PythonFile.py"), 'w') as f:
        pass
    subprocess.run(["code", desktopPath])

if __name__ == "__main__":
    main()
