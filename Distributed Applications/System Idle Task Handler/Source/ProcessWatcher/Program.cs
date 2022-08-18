using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProcessWatcher
{
    static class Program
    {
        static ManagementEventWatcher processStopEvent;
        static void customaction()
        {
            System.IO.File.AppendAllText(@"c:\temp\idletask.log", "ended " + System.DateTime.Now.ToString() + "\n");
        }
        static void processStopEvent_EventArrived(object sender, EventArrivedEventArgs e)
        {
            try
            {
                customaction();
                processStopEvent.Stop();
                processStopEvent.EventArrived -= processStopEvent_EventArrived;
                Process.GetCurrentProcess().Kill();
            }
            catch
            {
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            int parentpid = 0;
            using (var query = new ManagementObjectSearcher(
                "SELECT * " +
                "FROM Win32_Process " +
                "WHERE ProcessId=" + System.Diagnostics.Process.GetCurrentProcess().Id))
            {
                parentpid = query
                    .Get()
                    .OfType<ManagementObject>()
                    .Select(p => System.Diagnostics.Process.GetProcessById((int)(uint)p["ParentProcessId"]))
                    .FirstOrDefault().Id;
            };


            processStopEvent = new ManagementEventWatcher("SELECT * FROM Win32_ProcessStopTrace where ProcessID="+ parentpid);
            processStopEvent.EventArrived += processStopEvent_EventArrived;
            processStopEvent.Start();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run();
        }
    }
}
