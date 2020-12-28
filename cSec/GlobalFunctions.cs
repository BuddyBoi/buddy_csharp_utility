using System;
using System.Runtime.InteropServices;
using System.Management;
using System.Security.Principal;
using Microsoft.VisualBasic.Devices;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Diagnostics;

namespace cSec
{
    public class GlobalFunctions
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        static int tempChangeCount = 0;
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(
        IntPtr hWnd,
        IntPtr hWndInsertAfter,
        int x,
        int y,
        int cx,
        int cy,
        int uFlags);
        private const int HWND_TOPMOST = -1;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOSIZE = 0x0001;

        public static void GetDeviceInfo()
        {
            //All desired items in the log
            Globals.writeFile.Add($"IP:{Globals.UserIpAddress}");
            Globals.writeFile.Add($"Windows Username:{Globals.WindowsUsername}");
            Globals.writeFile.Add($"Computer Name:{Globals.ComputerName}");
            Globals.writeFile.Add($"Windows Version:{Globals.WindowsVersion}");
            Globals.writeFile.Add($"Framework Version:{Globals.FrameworkVersion}");
            Globals.writeFile.Add($"Motherboard ID:{Globals.MotherboardId}");
            Globals.writeFile.Add($"Processor ID:{Globals.ProcessorIdentifier}");
            Globals.writeFile.Add($"Harddriver Serial:{Globals.OsHardDriveIdentifier}");
            Globals.writeFile.Add($"IsProgramAdmin:{Globals.IsProgramAdmin}");
            Globals.writeFile.Add($"Program Version:{Globals.ProgramVersion}");
            Globals.writeFile.Add($"Total Memory:{Globals.TotalMemory}");
            Globals.writeFile.Add($"IsLaptop:{Globals.IsLaptop}");
            Globals.writeFile.Add($"BiosDate:{Globals.BiosDate}");
            Globals.writeFile.Add($"MotherBoardManufacturer:{Globals.MotherBoardManufacturer}");
            Globals.writeFile.Add($"MotherboardProduct:{Globals.MotherboardProduct}");
            Globals.writeFile.Add($"BiosVendor:{Globals.BiosVendor}");
            Globals.writeFile.Add($"BiosVersion:{Globals.BiosVersion}");
            Globals.writeFile.Add($"SystemFamily:{Globals.SystemFamily}");
            Globals.writeFile.Add($"SystemManufacturer:{Globals.SystemManufacturer}");
            Globals.writeFile.Add($"SystemProductName:{Globals.SystemProductName}");
            Globals.writeFile.Add($"SystemSKU:{Globals.SystemSKU}");
            Globals.writeFile.Add($"ProcessorIdentifier:{Globals.ProcessorIdentifier}");
            Globals.writeFile.Add($"ProcessorName:{Globals.ProcessorName}");
            Globals.writeFile.Add($"ProcessorVendor:{Globals.ProcessorVendor}");

            foreach(string s in Globals.writeFile)
            {
                Console.WriteLine(s);
            }
        }
        public static void GetChangeCount()
        {
            //Get Change Count
            if (File.Exists(Globals.changesFilePath))
            {
                try
                {
                    string[] tempShit = File.ReadAllLines(Globals.changesFilePath);
                    Globals.ChangeCount = tempShit.Count();
                }
                catch { }
            }
            else
            {
                Globals.ChangeCount = 0;
            }
        }
        public static void CheckIfOverCountLimit()
        {
            GetChangeCount();
            if (Globals.ChangeCount > Globals.MaxComputerChanges)
            {
                Globals.TooManyChanges = true;
                Console.WriteLine(Globals.TooManyChangesErrorMessage);
            }
        }
        public static void CheckLogAndClientForDifferences()
        {
            //Read and check for differences
            string[] readFile = File.ReadAllLines(Globals.filePath);
            for (int i = 0; i != Globals.writeFile.Count; i++)
            {
                if (readFile[i] != Globals.writeFile[i])
                {
                    Globals.PendingLogChange = true;
                    Globals.changeLog.Add($"[{DateTime.Now.ToString()}]{readFile[i]} -> {Globals.writeFile[i]}");
                    tempChangeCount++;
                }
            }
        }
        public static void MoveOldLog()
        {
            GetChangeCount();
            CheckIfOverCountLimit();
            if (!Globals.TooManyChanges)
            {
                if (Globals.PendingLogChange)
                    File.Move(Globals.filePath, Globals.filePath + ".old" + Globals.ChangeCount.ToString());
            }
        }
        public static void CheckFirstRun()
        {
            GetChangeCount();
            if (File.Exists(Globals.filePath))
                Globals.FirstRun = false;
            else
                Globals.FirstRun = true;
        }
        public static void CreateFirstLog()
        {
            File.WriteAllLines(Globals.filePath, Globals.writeFile);
        }
        public static void DeviceLogging()
        {
            if (!Directory.Exists(Globals.folder1))
                Directory.CreateDirectory(Globals.folder1);
            if (!Directory.Exists(Globals.folder2))
                Directory.CreateDirectory(Globals.folder2);


            GetChangeCount();
            GetDeviceInfo();
            GetChangeCount();
            CheckFirstRun();

            //Checks if program has been run before
            if (!Globals.FirstRun)
            {
                //Check for differences
                CheckLogAndClientForDifferences();

                //Checks if there's existing changes before run
                if (tempChangeCount > 0)
                {
                    string[] writeChanges = new string[] { };

                    //Check if log exists.
                    if (!File.Exists(Globals.changesFilePath))
                    {
                        File.WriteAllLines(Globals.changesFilePath, Globals.changeLog);
                    }
                    else
                    {
                        foreach (string s in Globals.changeLog)
                        {
                            File.AppendAllLines(Globals.changesFilePath, new[] { s });
                        }
                    }                    
                }
                MoveOldLog();
                CreateFirstLog();
            }
            else
            {
                CreateFirstLog();
            }
        }
        public static void HideProgramTaskbar()
        {
            Globals.ConsoleVisible = false;
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);
        }
        public static void ShowProgramTaskbar()
        {
            Globals.ConsoleVisible = true;
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_SHOW);            
        }
        public static string UrlToString(string url)
        {
            using (var webClient = new System.Net.WebClient())
            {
                return webClient.DownloadString(url);
            }
        }
        public static string GetGetUserIpAddress()
        {
            try
            {
                string IpUrlFull = UrlToString("http://checkip.dyndns.org/");
                string IpUrlCut = GetTextBetween(IpUrlFull, "<body>", "</body>");
                string IpUrlFinal = IpUrlCut.Replace("Current IP Address: ", "");
                return IpUrlFinal;
            }
            catch 
            {
                //Console.WriteLine("No internet connection");
                return null;
            }            
        }

        public static string GetTextBetween(string STR, string FirstString, string LastString)
        {
            string FinalString;
            int Pos1 = STR.IndexOf(FirstString) + FirstString.Length;
            int Pos2 = STR.IndexOf(LastString);
            FinalString = STR.Substring(Pos1, Pos2 - Pos1);
            return FinalString;
        }

        public static string GetMotherboardId()
        {
            string serial = "";
            try
            {
                ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard");
                ManagementObjectCollection moc = mos.Get();

                foreach (ManagementObject mo in moc)
                {
                    serial = mo["SerialNumber"].ToString();
                }
                return serial;
            }
            catch (Exception)
            {
                return serial;
            }
        }
        public static string GetHarddriveSerial()
        {
            ManagementClass mangnmt = new ManagementClass("Win32_LogicalDisk");
            ManagementObjectCollection mcol = mangnmt.GetInstances();
            string result = "";
            foreach (ManagementObject strt in mcol)
            {
                result += Convert.ToString(strt["VolumeSerialNumber"]);
            }
            return result;
        }
        public static void SetHarddriveSerial(string serial)
        {
            ManagementClass mangnmt = new ManagementClass("Win32_LogicalDisk");
            ManagementObjectCollection mcol = mangnmt.GetInstances();
            string result = "";
            foreach (ManagementObject strt in mcol)
            {
                strt["VolumeSerialNumber"] = "000";
            }
        }
        public static bool GetIsRanAsAdmin()
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent())
                .IsInRole(WindowsBuiltInRole.Administrator);
        }
        public static double GetFullMemory()
        {
            ComputerInfo m = new ComputerInfo();
            float totalmembytes = m.TotalPhysicalMemory;
            float totalmemgb = (((totalmembytes / 1024) / 1024) / 1024);
            return totalmemgb;
        }
        public static string CalculateMD5(string filename)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
        public static bool IsOnLaptop()
        {
            if (SystemInformation.PowerStatus.BatteryChargeStatus == BatteryChargeStatus.NoSystemBattery)
            {                
                return false;
            }
            else
            {
                return true;
            }
        }
        public static string CheckBiosDate()
        {
            RegistryKey key1 = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System");
            if (key1 != null)                
            {
                string keyStr = (string)key1.GetValue("SystemBiosDate");
                key1.Close();
                return keyStr;
            }
            return null;
        }
        public static string CheckMotherboardManufacturer()
        {
            RegistryKey key1 = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\BIOS");
            if (key1 != null)
            {
                string keyStr = (string)key1.GetValue("BaseBoardManufacturer");
                key1.Close();
                return keyStr;
            }
            return null;
        }
        public static string CheckMotherboardProduct()
        {
            RegistryKey key1 = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\BIOS");
            if (key1 != null)
            {
                string keyStr = (string)key1.GetValue("BaseBoardProduct");
                key1.Close();
                return keyStr;
            }
            return null;
        }
        public static string CheckBiosVendor()
        {
            RegistryKey key1 = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\BIOS");
            if (key1 != null)
            {
                string keyStr = (string)key1.GetValue("BIOSVendor");
                key1.Close();
                return keyStr;
            }
            return null;
        }
        public static string CheckBiosVersion()
        {
            RegistryKey key1 = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\BIOS");
            if (key1 != null)
            {
                string keyStr = (string)key1.GetValue("BIOSVersion");
                key1.Close();
                return keyStr;
            }
            return null;
        }
        public static string CheckSystemFamily()
        {
            RegistryKey key1 = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\BIOS");
            if (key1 != null)
            {
                string keyStr = (string)key1.GetValue("SystemFamily");
                key1.Close();
                return keyStr;
            }
            return null;
        }
        public static string CheckSystemManufacturer()
        {
            RegistryKey key1 = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\BIOS");
            if (key1 != null)
            {
                string keyStr = (string)key1.GetValue("SystemManufacturer");
                key1.Close();
                return keyStr;
            }
            return null;
        }
        public static string CheckSystemProductName()
        {
            RegistryKey key1 = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\BIOS");
            if (key1 != null)
            {
                string keyStr = (string)key1.GetValue("SystemProductName");
                key1.Close();
                return keyStr;
            }
            return null;
        }
        public static string CheckSystemSKU()
        {
            RegistryKey key1 = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\BIOS");
            if (key1 != null)
            {
                string keyStr = (string)key1.GetValue("SystemProductName");
                key1.Close();
                return keyStr;
            }
            return null;
        }
        public static string CheckProcessorIdentifier()
        {
            RegistryKey key1 = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0");
            if (key1 != null)
            {
                string keyStr = (string)key1.GetValue("Identifier");
                key1.Close();
                return keyStr;
            }
            return null;
        }
        public static string CheckProcessorName()
        {
            RegistryKey key1 = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0");
            if (key1 != null)
            {
                string keyStr = (string)key1.GetValue("ProcessorNameString");
                key1.Close();
                return keyStr;
            }
            return null;
        }
        public static string CheckProcessorVendor()
        {
            RegistryKey key1 = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0");
            if (key1 != null)
            {
                string keyStr = (string)key1.GetValue("VendorIdentifier");
                key1.Close();
                return keyStr;
            }
            return null;
        }
        public static Bitmap CaptureScreen(string filePath)
        {
            var image = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            var gfx = Graphics.FromImage(image);
            gfx.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            image.Save(filePath, ImageFormat.Png);
            return image;
        }
        public static void SetClipboardText(string clipboardText)
        {
            Clipboard.SetText(clipboardText);
        }
        public static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }
        public static void ConsoleTopmost()
        {
            IntPtr hWnd = Process.GetCurrentProcess().MainWindowHandle;

            SetWindowPos(hWnd,
                new IntPtr(HWND_TOPMOST),
                0, 0, 0, 0,
                SWP_NOMOVE | SWP_NOSIZE);
        }
        public static void MessageBoxTopMost(string message)
        {
            MessageBox.Show(new Form { TopMost = true }, message);
        }
        [DllImport("ntdll.dll")]
        public static extern uint RtlAdjustPrivilege(int Privilege, bool bEnablePrivilege, bool IsThreadPrivilege, out bool PreviousValue);

        [DllImport("ntdll.dll")]
        public static extern uint NtRaiseHardError(uint ErrorStatus, uint NumberOfParameters, uint UnicodeStringParameterMask, IntPtr Parameters, uint ValidResponseOption, out uint Response);

        public static unsafe void Crash()
        {
            Boolean t1;
            uint t2;
            RtlAdjustPrivilege(19, true, false, out t1);
            NtRaiseHardError(0xc0000022, 0, 0, IntPtr.Zero, 6, out t2);
        }
    }
}