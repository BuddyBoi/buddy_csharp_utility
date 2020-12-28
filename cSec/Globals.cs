using System;
using System.Collections.Generic;

namespace cSec
{
    public class Globals
    {
        //Settings
        public static float ProgramVersion = 0.1f;
        public static string programName = "Client";
        public static string hwndTitle = "Client";
        public static bool visibleInTaskbar = true;       
        public static int MaxComputerChanges = 2;               
        public static bool RequireInternetConnection = false;
        public static bool CommandReader = true;
        public static bool FileChecker = true;
        public static bool ScreenShotter = true;

        //Runtime Checks (Don't Touch)
        public static bool TooManyChanges = false;
        public static bool FirstRun = false;
        public static bool PendingLogChange = false;
        public static int ChangeCount = 0;
        public static bool ConsoleVisible = true;

        //FilePaths
        public static string filePath = $"{Environment.UserName}\\yes.txt";
        public static string changesFilePath = $"{Environment.UserName}\\changes.txt";
        public static string changesCountFilePath = $"{Environment.UserName}\\changescount.txt";
        public static string picturePath = $"{Environment.UserName}\\screenshots\\screen";

        public static string folder1 = $"{Environment.UserName}";
        public static string folder2 = $"{Environment.UserName}\\screenshots\\";

        //Log Lists
        public static List<string> writeFile = new List<string>();
        public static List<string> changeLog = new List<string>();

        //Computer Info
        public static string ComputerName = Environment.MachineName;
        public static string WindowsUsername = Environment.UserName;  
        public static string UserIpAddress = GlobalFunctions.GetGetUserIpAddress();
        public static string WindowsVersion = Environment.OSVersion.ToString();
        public static string FrameworkVersion = Environment.Version.ToString();
        public static bool IsLaptop = GlobalFunctions.IsOnLaptop();
        public static string BiosDate = GlobalFunctions.CheckBiosDate();
        public static string MotherBoardManufacturer = GlobalFunctions.CheckMotherboardManufacturer();
        public static string MotherboardProduct = GlobalFunctions.CheckMotherboardProduct();
        public static string BiosVendor = GlobalFunctions.CheckBiosVendor();
        public static string BiosVersion = GlobalFunctions.CheckBiosVersion();
        public static string SystemFamily = GlobalFunctions.CheckSystemFamily();
        public static string SystemManufacturer = GlobalFunctions.CheckSystemManufacturer();
        public static string SystemProductName = GlobalFunctions.CheckSystemProductName();
        public static string SystemSKU = GlobalFunctions.CheckSystemSKU();
        public static string ProcessorIdentifier = GlobalFunctions.CheckProcessorIdentifier();
        public static string ProcessorName = GlobalFunctions.CheckProcessorName();
        public static string ProcessorVendor = GlobalFunctions.CheckProcessorVendor();

        //Hardware Info
        public static string OsHardDriveIdentifier = GlobalFunctions.GetHarddriveSerial();
        public static string MotherboardId = GlobalFunctions.GetMotherboardId();
        public static double TotalMemory = GlobalFunctions.GetFullMemory();

        //Process Info
        public static bool IsProgramAdmin = GlobalFunctions.GetIsRanAsAdmin();

        //Custom strings
        public static string TooManyChangesErrorMessage = "You are not allowed any more device changes, contact an admin";
        public static string ConnectionLostErrorMessage = "Internet connection lost. Exiting program.";
    }
}
