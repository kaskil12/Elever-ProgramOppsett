//import windows and all things needed to install vs code and python on a system
#include <iostream>
#include <string>
#include <fstream>
#include <windows.h>

using namespace std;

//main function
int main(){
    if(system("code --version") == 1){
        std::cout<<"VS Code is not installed"<<std::endl;
    }

}
void isVSCodeInstalled(){
    //check if vs code is installed
    system("code --version");
    //if vs code is not installed
    if(system("code --version") == 1){
        //install vs code
        installVSCode();
    }
}
void installVSCode(){
    //install vs code
    system("VSCodeSetup-x64-1.56.2.exe");
}
void isPythonInstalled(){
    //check if python is installed
    system("python --version");
    //if python is not installed
    if(system("python --version") == 1){
        //install python
        installPython();
    }
}
void installPython(){
    //install python
    system("python-3.9.5-amd64.exe");
}