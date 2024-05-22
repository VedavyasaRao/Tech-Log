using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace InstallerCreator
{
    static class Program
    {
        static MainFrm frm;
        public static string layoutfile = "";
        public static string msifile = "";
        public static bool bsilent = false;
        public static bool bnewpackageid = false;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {

                for (int i = 0; i < args.Length; ++i)
                {
                    string a = args[i];
                    if (a.ToLower().Contains("-l:"))
                    {
                        layoutfile = a.Substring(a.IndexOf(':') + 1);
                        if (!File.Exists(layoutfile))
                        {

                            ShowSyntax(string.Format("bad layoutfile:{0}\r\n", layoutfile));
                        }
                    }
                    else if (a.ToLower().Contains("-o:"))
                    {
                        msifile = a.Substring(a.IndexOf(':') + 1);
                    }
                    else if (a.ToLower().Contains("-s"))
                    {
                        bsilent = true;
                    }
                    else if (a.Contains("-newpackage"))
                    {
                        bnewpackageid = true;
                    }
                    else if (a.Contains("?"))
                    {
                        ShowSyntax("");
                    }
                }

                if (layoutfile == "" || msifile == "" )
                {
                    ShowSyntax("");
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            frm = new MainFrm();
            Application.Run(frm);
        }

        public static void UpdateStatus(string msg)
        {
            frm.updatestatus(msg);
        }

        static private void ShowSyntax(string errmsg)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(errmsg);
            sb.AppendLine("Syntax:");
            sb.AppendLine("FileConfigWizard -l:<layout file> -o:<msi file> -s -newpackage");
            sb.AppendLine("Example:");
            sb.AppendLine("FileConfigWizard.exe -l:\"D:\\IEngine\\AutomatedTests\\Installers\\test.layout\" -o:\"D:\\IEngine\\AutomatedTests\\stage\\xvpc.msi\" ");
            MessageBox.Show(sb.ToString());
            Environment.Exit(0);
        }

    }
}
