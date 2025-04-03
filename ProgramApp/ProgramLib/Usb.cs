using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ProgramLib
{
    public class Usb
    {
        public static bool isUsbDrive = true;
        public static string _currentDriveLetter = "D:";

        public static void DetectRemovableDrive()
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.DriveType == DriveType.Removable)
                {
                    _currentDriveLetter = drive.Name;
                    isUsbDrive = true;
                    break;
                }
                else
                {
                    _currentDriveLetter = "C:";
                    isUsbDrive = false;
                    Log.LogInfo("No drive found. defaulting to C:");
                }
            }
            Log.LogInfo(_currentDriveLetter);
        }
    }
}
