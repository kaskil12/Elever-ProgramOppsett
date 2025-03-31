using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
namespace ProgramLib{

public class Usb{
        public static string logFilePath = "./log.txt";
        public static string _currentDriveLetter = "D:";

        public static void DetectRemovableDrive()
        {
            
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.DriveType == DriveType.Removable)
                {
                    _currentDriveLetter = drive.Name;
                    break;
                }
            }
        }
    }
}
