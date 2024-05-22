using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CustomActions
{
    [RunInstaller(true)]
    public partial class InstallerCA : System.Configuration.Install.Installer
    {
        [DllImport("User32.dll", EntryPoint = "MessageBox", CharSet = CharSet.Auto)]
        public static extern int MsgBox(int hWnd, String text, String caption, uint type);

        private void writestatus(string statfile, string msg)
        {
            File.WriteAllText(statfile, msg);
        }

        private string readtatus(string statfile)
        {
            return File.ReadAllText(statfile);
        }

        public InstallerCA()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary savedState)
        {
            try
            {
                //Debugger.Launch();
                base.Install(savedState);
                string cmdline = Context.Parameters["args"];
                int instoption = cmdline[0] - '0';
                string tgtdir = cmdline.Substring(1, cmdline.Length-2);
                tgtdir = tgtdir + "setup\\";
                string srcdir = tgtdir + "fileinstall.zip";
                string dstdir = tgtdir + "temp";
                if (Directory.Exists(dstdir))
                    Directory.Delete(dstdir, true);
                Directory.CreateDirectory(dstdir);
                string statfile = String.Format("{0}\\status.txt", dstdir);
                writestatus(statfile, "success");

                Process p = new Process();
                p.StartInfo.FileName = String.Format("{0}\\makezip.vbs", tgtdir);
                p.StartInfo.Arguments = String.Format("\"{0}\" \"{1}\"", srcdir, dstdir);
                p.Start();
                p.WaitForExit();

                p = new Process();
                p.StartInfo.FileName = String.Format("\"{0}\\setup\\FileDeploymentWizard.exe\"", dstdir);
                p.StartInfo.Arguments = String.Format("-w:\"{0}\" -i -o:{1}", dstdir, instoption);
                p.Start();
                p.WaitForExit();

                if (readtatus(statfile) != "success")
                    throw new Exception(readtatus(statfile));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
        }

        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }

        public override void Uninstall(IDictionary savedState)
        {
            try
            {
                //Debugger.Launch();
                string cmdline = Context.Parameters["args"];
                Environment.SetEnvironmentVariable("InstallError", "success");
                string tgtdir = cmdline.Substring(0, cmdline.Length - 1);
                tgtdir = tgtdir + "setup\\";
                string dstdir = tgtdir + "temp";
                string statfile = String.Format("{0}\\status.txt", dstdir);
                writestatus(statfile, "success");

                Process p = new Process();
                p.StartInfo.FileName = String.Format("\"{0}\\setup\\FileDeploymentWizard.exe\"", dstdir);
                p.StartInfo.Arguments = String.Format("-w:\"{0}\" -u", dstdir);
                p.Start();
                p.WaitForExit();
                if (readtatus(statfile) != "success")
                    throw new InstallException(readtatus(statfile));
                base.Uninstall(savedState);
                try
                {
                    Directory.Delete(tgtdir, true);
                }
                catch
                {

                }
            }
            catch(Exception ex)
            {
                if (MsgBox(0, "An error occured\r\n" + ex.Message + "\r\nDo you want to continue?", "Uninstall", 4) != 6)
                    throw;

            }




        }

        public static void Main()
        {
        }


    }
}
