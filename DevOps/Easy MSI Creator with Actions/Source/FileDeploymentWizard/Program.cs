using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using JSonSerializer;
using SetupLayout;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace FileDeploymentWizard
{
    class Program
    {
        [DllImport("User32.dll", EntryPoint = "MessageBox", CharSet = CharSet.Auto)]
        public static extern int MsgBox(int hWnd, String text, String caption, uint type);

        public static string workfolder = "";
        public static   int uilevel= 5;

        static private void ShowSyntax()
        {
            Console.WriteLine("Syntax:");
            Console.WriteLine("DeploymentWizard -w:<work folder> -i |-u -o:<silent option>");
            Environment.Exit(0);
        }

        static private void writestatus(string statfile, string msg)
        {
            File.WriteAllText(statfile, msg);
        }

        static private string readtatus(string statfile)
        {
            return File.ReadAllText(statfile);
        }

        static void Main(string[] args)
        {
            //Debugger.Launch();
            Deployer dp = null;
            bool binstall = true;
            for (int i = 0; i < args.Length; ++i)
            {
                string a = args[i];
                if (a.ToLower().Contains("-w:"))
                {
                    workfolder = a.Substring(a.IndexOf(':') + 1);
                    DirectoryInfo di = new DirectoryInfo(workfolder);
                    if (!di.Exists)
                    {
                        Console.WriteLine("bad workfolder dir:{0}", workfolder);
                        Console.WriteLine();
                        ShowSyntax();
                    }
                    workfolder = di.FullName;
                }
                else if (a.ToLower().Contains("-o:"))
                {
                    uilevel = int.Parse(a.Substring(a.IndexOf(':') + 1));
                }
                else if (a.ToLower().Contains("-i"))
                {
                    binstall = true;
                }
                else if (a.ToLower().Contains("-u"))
                {
                    binstall = false;
                }
                else
                    ShowSyntax();
            }

            string statfile = String.Format("{0}\\status.txt", workfolder);

            try
            {
                if (binstall)
                {
                    dp = new Deployer(binstall);
                    dp.GetSettings();
                    dp.Createenvvars();
                    dp.runprepostrunexe(true, binstall);
                    if (readtatus(statfile) != "success")
                        return;
                    dp.copyfiles();
                    dp.performfileops(binstall);
                    dp.runprepostrunexe(false, binstall);
                    if (readtatus(statfile) != "success")
                        return;
                }
                else
                {
                    dp = new Deployer(binstall);
                    dp.Createenvvars();
                    dp.runprepostrunexe(true, binstall);
                    if (readtatus(statfile) != "success")
                        return;
                    dp.performfileops(binstall);
                    dp.runprepostrunexe(false, binstall);
                    if (readtatus(statfile) != "success")
                        return;
                }
            }
            catch (Exception ex)
            {
                writestatus(statfile, ex.Message);

            }

        }
    }
}
