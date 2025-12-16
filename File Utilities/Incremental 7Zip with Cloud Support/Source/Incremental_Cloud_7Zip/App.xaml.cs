using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml.Linq;

namespace BackupRestoreTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static TextBlock logtxt;
        public static ScrollViewer vsb;
        public static string outputpath = "";
        public static string sevenzpath = "";
        public static string zipfilespath = "";
        public static string unzipfilespath = "";
        public static string copyfilespath = "";
        public static string movefilespath = "";
        public static string makelinkpath = "";
        public static string nircmdpath = "";
        public static string exporthistorypath = "";
        public static string logfile = "";
        public static string archivedir = "";
        public static string bearertoken = "";
        public static string clouddrivepath = "";
        public static string clouddrive = "";
        public static string tempdir = Environment.GetEnvironmentVariable("Temp");

        public static Dispatcher disp;
        public static Window mainwindow;
        public static Window backupvw;
        public static Window restorevw;
        public static Window editvw;
        public static ArchiveMgr arm = new ArchiveMgr();
        public static ProgressMonitor pmon = new ProgressMonitor();
        static object dummy = new object();
        static System.Configuration.Configuration config;

        public static string getoutputpath(string  dirname)
        {
            string outputpath = Environment.GetEnvironmentVariable("TEMP");
            if (!string.IsNullOrEmpty(outputpath))
                outputpath += ("\\backuprestoretool\\"+ dirname);
            else
                outputpath = System.IO.Path.GetDirectoryName(config.AppSettings.CurrentConfiguration.FilePath) + "\\output\\"+dirname;
            try
            {
                RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
                if (DirectoryEx.Exists(outputpath))
                {
                    RemoveDirFile(outputpath, true);
                }
            }
            catch
            {
                logit("Cannot delete " + outputpath + " folder");
            }
            if (!DirectoryEx.Exists(outputpath))
                DirectoryEx.CreateDirectory(outputpath);
            
            return outputpath;
        }

        static App()
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            outputpath = getoutputpath("1");
            if (!DirectoryEx.Exists(outputpath))
                DirectoryEx.CreateDirectory(outputpath);
            var freedrive = getfreedrive();
            mapdrive(freedrive, "\""+ outputpath + "\"");
            outputpath = freedrive;
            logfile = outputpath + "\\BackupRestoreTool_log.txt";
            if (FileEx.Exists(logfile))
                FileEx.Delete(logfile);
            sevenzpath = System.IO.Path.GetDirectoryName(config.AppSettings.CurrentConfiguration.FilePath) + "\\Commands\\7z.exe";
            zipfilespath = System.IO.Path.GetDirectoryName(config.AppSettings.CurrentConfiguration.FilePath) + "\\Commands\\zipunzipfiles.cmd";
            unzipfilespath = System.IO.Path.GetDirectoryName(config.AppSettings.CurrentConfiguration.FilePath) + "\\Commands\\unzipfiles.cmd";
            nircmdpath = System.IO.Path.GetDirectoryName(config.AppSettings.CurrentConfiguration.FilePath) + "\\nircmd.exe";
            copyfilespath = System.IO.Path.GetDirectoryName(config.AppSettings.CurrentConfiguration.FilePath) + "\\Commands\\copy_move_link_files.cmd";
            movefilespath = System.IO.Path.GetDirectoryName(config.AppSettings.CurrentConfiguration.FilePath) + "\\Commands\\copy_move_link_files.cmd";
            makelinkpath = System.IO.Path.GetDirectoryName(config.AppSettings.CurrentConfiguration.FilePath) + "\\Commands\\copy_move_link_files.cmd";
            exporthistorypath = System.IO.Path.GetDirectoryName(config.AppSettings.CurrentConfiguration.FilePath) + "\\Commands\\export_history.cmd";
        }
        void App_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                unmapdrive(outputpath);
                if (App.clouddrive != "")
                    unmapdrive(App.clouddrive);
            }
            catch
            {
            }
        }
        static public bool isclouddrive()
        {
            return !string.IsNullOrEmpty(clouddrivepath);
        }

        public static void RemoveDirFile(string dirfile, bool dir)
        {
            string args = "/c echo removing...&";
            if (dir)
                args += "  rd /s /q \"" + dirfile + "\"";
            else
                args += "  del /f /q \"" + dirfile + "\"";

            var ps = new ProcessStartInfo("cmd", args);
            if (!dir)
                ps.WindowStyle = ProcessWindowStyle.Hidden;
            System.Diagnostics.Process.Start(ps).WaitForExit(); ;
        }

        public static void Escapefile(string outputfile)
        {
            var temptxt = FileEx.ReadAllText(outputfile);
            FileEx.WriteAllText(outputfile, temptxt.Replace("!", "^!"));

        }
        public static string getfreedrive()
        {
            List<string> driveLetters = new List<string>();
            for (var ch = 'C'; ch <= 'Z'; ++ch)
            {
                driveLetters.Add(ch + ":\\");
            }

            driveLetters = driveLetters.Except(System.IO.Directory.GetLogicalDrives()).ToList();
            string ret = driveLetters[0].Substring(0, 2);
            return ret;
        }
        public static void mapdrive(string driveletter, string path)
        {
            Process.Start("cmd", "/c subst " + driveletter + "  "+path).WaitForExit();
        }
        public static void unmapdrive(string driveletter)
        {
            Process.Start("cmd", "/c subst /d " + driveletter).WaitForExit();

        }

        public static void EnableDisable(bool bvalue)
        {
            disp.Invoke(new Action<MainWindow>((sender) => { mainwindow.IsEnabled = bvalue; }), DispatcherPriority.Send, new object[] { null });
        }

        public static void logit(string msg)
        {
            lock (dummy)
            {
                try
                {
                    FileEx.AppendAllText(logfile, msg + "\r\n");
                    disp.Invoke(new Action<MainWindow>((sender) => { logtxt.Text = logtxt.Text + msg + "\r\n"; vsb.ScrollToEnd(); }), new object[] { null });
                }
                catch
                {
                }
            }
        }


        public static void bringfront(Window wnd)
        {
            disp.Invoke(new Action(() => {
                wnd.IsEnabled = false;
                mainwindow.Activate();
                //mainwindow.Topmost = true;
            }));

        }

        public static void goback(Window wnd)
        {
            disp.Invoke(new Action(() => {
                wnd.IsEnabled = true;
                mainwindow.Topmost = false;
                wnd.Activate();
                //wnd.Topmost = true;
            }));

        }

    }
}
