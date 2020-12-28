using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Globalization;

namespace cSec
{
    public class Program
    {
        //Intitialize class and exit Main()
        static Program p = new Program();        
        static void Main() => p.Startup();
        //Stepthrough of program
        //Clipboard need attribute
        [STAThreadAttribute]
        void Startup()
        {
            Console.WriteLine(SystemInformation.BootMode.ToString());

            /*NetworkChecks();
            ClientChecks();
            InitializeEnvironment();
            GlobalThreads.InitThreads();

            Console.Write("Threads active");
            Console.ReadLine();*/
        }
        
        //Heartbeat server checks
        void NetworkChecks()
        {
        }

        //Software protection
        void ClientChecks()
        {
            //Device checking and logging
            GlobalFunctions.DeviceLogging();
            GlobalFunctions.GetChangeCount();
            Console.WriteLine(Globals.ChangeCount);
        }

        //Setup for console environment
        void InitializeEnvironment()
        {
            if (!Globals.visibleInTaskbar)
                GlobalFunctions.HideProgramTaskbar();
            Console.Title = Globals.hwndTitle;
        }

        //Clean uninstall / stop
        void InitializeExit()
        {
            Environment.Exit(0);
        }
    }
}