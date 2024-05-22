using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Provider;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;
using UITesting.Automated.WindowsInput;
using UITesting.Automated.ControlInf;
using UITesting.Automated.JSonSerializer;

namespace UITesting.Automated.UIADriver
{

    internal class Logger
    {
        private static string logfile = "";

        public static bool SetLogFile(string logfilename, bool bappend)
        {
            logfile = logfilename;
            StreamWriter sw = new StreamWriter(logfile, bappend);
            sw.WriteLine("***************open log***********");
            sw.Close();
            return true;
        }

        public static void LogMessage(string msg)
        {
            if (logfile == "")
            {
                return;
            }

            StreamWriter sw = new StreamWriter(logfile, true);
            sw.WriteLine(string.Format("{0} {1}",DateTime.Now,msg));
            sw.Close();
        }

    }


}

