//import windows and all things needed to install vs code and python on a system
#include "main.h"
bool VsCodeInstalled = false;
bool PythonInstalled = false;
//main function
int main(){
    //test to see if check is working
    std::cout << "Checking if VS Code is installed..." << std::endl;
    //check if vs code is installed
    isVSCodeInstalled();
    //test to see if check is working
    std::cout << "Checking if Python is installed..." << std::endl;
    //check if python is installed
    isPythonInstalled();
    //create a folder and file for python and open in vscode and create a python file
    if (VsCodeInstalled && PythonInstalled){
        createFolderAndFile();
    }else{
        std::cout <<"Innstallation failed" << std::endl;
    }
    //return 0
    return 0;

}
void isVSCodeInstalled(){
    //check if vs code is installed
    system("code --version");
    //if vs code is not installed
    if(system("code --version") == 1){
        //install vs code
        installVSCode();
    }else{
        //remove vs code
        // removeVSCode();
        VsCodeInstalled = true;
    }
}
void isPythonInstalled(){
    //check if python is installed
    system("python --version");
    //if python is not installed
    if(system("python --version") == 1){
        //install python
        installPython();
    }else{
        //remove python
        // removePython();
        PythonInstalled = true;
    }
}
void installVSCode() {
    // Step 1: Download the latest VS Code installer
    // std::system("powershell -Command \"Invoke-WebRequest -Uri 'https://update.code.visualstudio.com/latest/win32-x64-user/stable' -OutFile 'C:\\vscode_installer.exe'\"");

    // Step 2: Install VS Code silently with PATH modification
    std::system("C:\\VSCodeUserSetup-x64-1.92.2.exe /verysilent /mergetasks=!runcode,addtopath");

    // Step 3: Clean up by removing the installer file
    std::system("del C:\\VSCodeUserSetup-x64-1.92.2.exe");

    // Step 4: Verify installation by checking VS Code version
    std::system("code --version");
    VsCodeInstalled = true;
}
// void removeVSCode() {
//     std::system("powershell -Command \"& {Get-ItemProperty HKLM:\\Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\* | Where-Object { $_.DisplayName -eq 'Microsoft Visual Studio Code' } | Select-Object -ExpandProperty UninstallString}\" > uninstall_vscode.cmd");

//     std::system("uninstall_vscode.cmd /verysilent");

//     std::system("del uninstall_vscode.cmd");

//     std::system("code --version");
//     isVSCodeInstalled();
// }
void installPython() {
    // std::system("powershell -Command \"Invoke-WebRequest -Uri 'https://www.python.org/ftp/python/3.11.4/python-3.11.4-amd64.exe' -OutFile 'C:\\python_installer.exe'\"");

    std::system("./python-3.12.5-amd64.exe /quiet InstallAllUsers=1 PrependPath=1");

    std::system("del C:\\python-3.12.5-amd64.exe");

    std::system("python --version");
    PythonInstalled = true;
}
// void removePython() {
//     std::system("powershell -Command \"& {Get-ItemProperty HKLM:\\Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\* | Where-Object { $_.DisplayName -like 'Python 3*' } | Select-Object -ExpandProperty UninstallString}\" > uninstall_python.cmd");

//     std::system("uninstall_python.cmd /quiet");

//     // Step 3: Clean up by removing the uninstaller script
//     std::system("del uninstall_python.cmd");

//     std::system("python --version");
//     isPythonInstalled();
// }
void createFolderAndFile(){
    std::string desktopPath = std::getenv("USERPROFILE");
    desktopPath += "\\Desktop";
    std::string command = "cd " + desktopPath + " && mkdir PythonFolder && cd PythonFolder && mkdir PythonFile.py && code .";
    system(command.c_str());
}