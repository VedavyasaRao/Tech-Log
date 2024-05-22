using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utilities;
using System.Reflection;
using System.IO;

namespace UITesting.Automated.ControlDBTool
{
    class Logger
    {
        const string CLASS_NAME = "CDBTLogger";
        const uint WM_WRITE_LOG = CustomWindow.WM_USER + 500;
        CustomWindow logwnd;
        string logfile;

        public Logger()
        {
            logfile = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\controldb.log";
            if (File.Exists(logfile))
                File.Delete(logfile);

            WindowManager.CreateClass(CLASS_NAME, new NativeWndProc(LogHandler));
            WindowManager.Start(CLASS_NAME);
            logwnd = WindowManager.CreateWindow(CLASS_NAME, CLASS_NAME);

        }

        public void AppendtoLog(string message)
        {
            logwnd.SendNotifyMessage(WM_WRITE_LOG, IntPtrConverter<string>.ToIntPtr(message),IntPtr.Zero);
        }

        public void Close()
        {
            WindowManager.Stop(CLASS_NAME);
        }

        public IntPtr LogHandler(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (WM_WRITE_LOG == msg)
            {
                string message = IntPtrConverter<string>.FromIntPtr(wParam);
                System.IO.File.AppendAllText(logfile, message + "\r\n");
                return IntPtr.Zero;
            }

            return CustomWindow.DefHandler(hWnd, msg, wParam, lParam);
        }


    }
}
