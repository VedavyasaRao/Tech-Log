using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Diagnostics;
using COMLibHelper;

namespace LBCodeGenerator
{
    static class Program
    {
        public static MainFrm mainfrm;
        public enum lbops { cs = 1, vb = 2, reg = 4, wcf = 8, net35 = 16, incopt = 32, sscfserver=64, sscfclient=128, complus=256 };
        static bool createwcfProject(string outputfoldr, string wcfns, bool bnet35)
        {
            string wcfprojfolder = outputfoldr + "\\..\\temp\\" + "wcfproject";
            DirectoryInfo di = new DirectoryInfo(wcfprojfolder);
            try
            {
                di.Delete(false);
            }
            catch
            {

            }
            di.Create();
            di.CreateSubdirectory("Properties");

            string s = bnet35?LBCodeGenerator.Resource2.projectfile35:LBCodeGenerator.Resource2.projectfile;
            s = s.Replace("#1#", Guid.NewGuid().ToString());
            s = s.Replace("#2#", wcfns);
            File.WriteAllText(wcfprojfolder + "\\wcfproj.csproj", s);

            s = LBCodeGenerator.Resource2.assemblyinfo;
            s = s.Replace("#1#", wcfns);
            s = s.Replace("#2#", Guid.NewGuid().ToString());
            File.WriteAllText(wcfprojfolder + "\\Properties\\AssemblyInfo.cs", s);

            File.WriteAllBytes(wcfprojfolder + "\\uiadriver.snk", LBCodeGenerator.Resource2.uiadriver);

            s = CodeGenerator.sbwcf.ToString();
            s = s.Replace("#1#", wcfns);
            File.WriteAllText(wcfprojfolder + "\\class1.cs", s);

            string sdkpath = System.Environment.ExpandEnvironmentVariables(String.Format("%windir%\\Microsoft.NET\\Framework\\{0}", bnet35 ? "v3.5" : "v4.0.30319"));
            string buildexe = sdkpath + "\\MSBuild.exe";
            System.Environment.SetEnvironmentVariable("MSBuildToolsPath", sdkpath);

            Process p = new Process();
            p.StartInfo.FileName = buildexe;
            p.StartInfo.Arguments = '"'+wcfprojfolder + "\\wcfproj.csproj\"  /p:Configuration=Release";
            UpdateStatus("building " + p.StartInfo.FileName + "  " + p.StartInfo.Arguments);
            p.Start();
            p.WaitForExit();
            var lst = Directory.GetFiles(wcfprojfolder + "\\bin\\release", "*.dll").ToArray();
            if (lst.Count() > 0)
            {
                string fname = lst[0];
                File.Copy(fname, outputfoldr + "\\" + Path.GetFileName(fname), true);
                return true;
            }
            return false;
        }


        public static void Process(List<Comobjectstorage> sellist, Program.lbops ops, string wcfns, string outputfoldr)
        {
            string csfilename = "outputfile.cs";
            string vbsfilename = "outputfile.vbs";
            string regfilename = "outputfile.reg";

            bool bcs = ((ops & Program.lbops.cs) != 0);
            bool bvb = ((ops & Program.lbops.vb) != 0);
            bool breg = ((ops & Program.lbops.reg) != 0);
            bool bwcf = ((ops & Program.lbops.wcf) != 0);
            bool bnet35 = ((ops & Program.lbops.net35) != 0);
            bool bsscfserver = ((ops & Program.lbops.sscfserver) != 0);
            bool bsscfclient = ((ops & Program.lbops.sscfclient) != 0);

            UpdateStatus("Reading type information");
            CodeGenerator.Generatecode(sellist, ops);

            if (bcs || bwcf || bsscfserver)
            {
                csfilename = outputfoldr + "\\" + csfilename;
                UpdateStatus("writing " + csfilename);
                string s = CodeGenerator.sbcs.ToString();
                s = s.Replace("#1#", wcfns);
                System.IO.File.WriteAllText(csfilename, s);
            }

            if (bvb || bwcf || bsscfserver)
            {
                vbsfilename = outputfoldr + "\\" + vbsfilename;
                UpdateStatus("writing " + vbsfilename);
                string s = CodeGenerator.sbvbs.ToString();
                s = s.Replace("#1#", wcfns);
                System.IO.File.WriteAllText(vbsfilename, s);
            }

            if (breg)
            {
                regfilename = outputfoldr + "\\" + regfilename;
                UpdateStatus("writing " + regfilename);
                System.IO.File.WriteAllText(regfilename, CodeGenerator.sbreg.ToString());
            }

            if (bwcf)
            {
                if (!createwcfProject(outputfoldr, wcfns, bnet35))
                {
                    throw new Exception("wcf project cannot be created");
                }
            }
        }

        public static void UpdateStatus(string msg)
        {
            mainfrm.updatestatus(msg);
        }


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainfrm = new MainFrm();
            Application.Run(mainfrm);
        }
    }
}
