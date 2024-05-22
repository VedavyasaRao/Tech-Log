using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace ConfigEditor
{
    static class Program
    {
        public static string settingsfile;
        public static string title = "";

        static private void ShowSyntax()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Syntax:");
            sb.AppendLine("ConfigEditor -f:<settingsfile> -t:title");
            MessageBox.Show(sb.ToString());
            Environment.Exit(0);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //System.Diagnostics.Debugger.Launch();
            for (int i = 0; i < args.Length; ++i)
            {
                string a = args[i];
                if (a.ToLower().Contains("?"))
                {
                    ShowSyntax();
                }
                if (a.ToLower().Contains("-f:"))
                {
                    settingsfile = a.Substring(a.IndexOf(':') + 1);
                }

                if (a.ToLower().Contains("-t:"))
                {
                    title = a.Substring(a.IndexOf(':') + 1);
                }
            }

            if (string.IsNullOrEmpty(settingsfile))
            {
                ShowSyntax();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainFrm());
        }
    }
}
