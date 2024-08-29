import os
import subprocess

VsCodeInstalled = False
PythonInstalled = False

def main():
    try:
        print("Checking if VS Code is installed...")
        isVSCodeInstalled()
        
        print("Checking if Python is installed...")
        isPythonInstalled()

        if VsCodeInstalled and PythonInstalled:
            createFolderAndFile()
        else:
            print("Installation failed. Ensure both VS Code and Python are installed correctly.")
    except Exception as e:
        print(f"An error occurred during the main process: {e}")

def isVSCodeInstalled():
    global VsCodeInstalled
    try:
        subprocess.run(["code", "--version"], check=True)
        VsCodeInstalled = True
        print("VS Code is installed.")
    except subprocess.CalledProcessError:
        print("VS Code is not installed or not found in PATH.")
        try:
            installVSCode()
        except Exception as e:
            print(f"Failed to install VS Code: {e}")
    except FileNotFoundError:
        print("VS Code executable not found. It might not be installed.")
    except Exception as e:
        print(f"An unexpected error occurred while checking for VS Code: {e}")

def isPythonInstalled():
    global PythonInstalled
    try:
        subprocess.run(["python", "--version"], check=True)
        PythonInstalled = True
        print("Python is installed.")
    except subprocess.CalledProcessError:
        print("Python is not installed or not found in PATH.")
        try:
            installPython()
        except Exception as e:
            print(f"Failed to install Python: {e}")
    except FileNotFoundError:
        print("Python executable not found. It might not be installed.")
    except Exception as e:
        print(f"An unexpected error occurred while checking for Python: {e}")

def installVSCode():
    try:
        print("Attempting to install VS Code...")
        os.system("C:\\VSCodeUserSetup-x64-1.92.2.exe /verysilent /mergetasks=!runcode,addtopath")
        os.remove("C:\\VSCodeUserSetup-x64-1.92.2.exe")
        subprocess.run(["code", "--version"], check=True)
        global VsCodeInstalled
        VsCodeInstalled = True
        print("VS Code installation successful.")
    except FileNotFoundError:
        print("VS Code installer not found. Please check the path and try again.")
    except subprocess.CalledProcessError:
        print("Failed to verify VS Code installation.")
    except Exception as e:
        print(f"An unexpected error occurred during VS Code installation: {e}")

def installPython():
    try:
        print("Attempting to install Python...")
        os.system("C:\\python-3.12.5-amd64 /quiet InstallAllUsers=1 PrependPath=1")
        os.remove("C:\\python-3.12.5-amd64")
        subprocess.run(["python", "--version"], check=True)
        global PythonInstalled
        PythonInstalled = True
        print("Python installation successful.")
    except FileNotFoundError:
        print("Python installer not found. Please check the path and try again.")
    except subprocess.CalledProcessError:
        print("Failed to verify Python installation.")
    except Exception as e:
        print(f"An unexpected error occurred during Python installation: {e}")

def createFolderAndFile():
    try:
        desktopPath = os.path.join(os.environ['USERPROFILE'], 'Desktop')
        folderPath = os.path.join(desktopPath, "PythonFolder")
        os.makedirs(folderPath, exist_ok=True)
        filePath = os.path.join(folderPath, "PythonFile.py")
        with open(filePath, 'w') as f:
            pass
        subprocess.run(["code", folderPath], check=True)
        print("Folder and file created successfully, and opened in VS Code.")
    except Exception as e:
        print(f"An error occurred while creating the folder and file: {e}")

if __name__ == "__main__":
    main()
