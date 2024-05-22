using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Build.Evaluation;
using System.Configuration;

namespace MigrationHelper
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }



    class driver
    {

        public static TextBox logtxt;
        public static ProgressBar pbar;

        public static void ListProjectsandSolutions(string parentpath, string outputdir)
        {
            logtxt.Text = "";
            Application.DoEvents();
            System.Threading.Thread.Sleep(0);

            int pl = parentpath.Length + 1;
            string logfile = outputdir + @"\solutions.csv";

            if (!Directory.Exists(outputdir))
            {
                Directory.CreateDirectory(outputdir);
            }

            if (File.Exists(logfile))
                File.Delete(logfile);
            logtxt.AppendText("Reading solutions....\r\n");
            var filelist = Directory.EnumerateFiles(parentpath, "*.*sln", SearchOption.AllDirectories).ToList();
            pbar.Maximum = filelist.Count;
            pbar.Value = 0;
            File.AppendAllText(logfile, "File,Format,Toolset\r\n");
            string temps = "";
            foreach (var f in filelist)
            {
                temps = temps + f + "\r\n";
                File.AppendAllText(logfile, f.Substring(pl));
                string[] lines = File.ReadAllLines(f);
                try
                {
                    foreach (var line in lines)
                    {
                        string pat = "Format Version";
                        if (line.Contains(pat))
                        {
                            int p1 = line.IndexOf(pat);
                            File.AppendAllText(logfile, "," + line.Substring(p1 + pat.Length));
                        }

                        pat = "# Visual Studio";
                        if (line.Contains(pat))
                        {
                            int p1 = line.IndexOf(pat);
                            File.AppendAllText(logfile, "," + line.Substring(p1 + pat.Length));
                        }
                    }
                }
                catch
                {
                    File.AppendAllText(logfile, ",0,0" );
                }
                pbar.Value++;
                File.AppendAllText(logfile, "\r\n");
                if ((pbar.Value) % 100 == 0)
                {
                    logtxt.AppendText(temps);
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(0);
                    temps = "";
                }

            }
            logtxt.AppendText(temps);
            Application.DoEvents();
            System.Threading.Thread.Sleep(0);
            temps = "";

            logfile = outputdir + @"\projects.csv";
            if (File.Exists(logfile))
                File.Delete(logfile);

            File.AppendAllText(logfile, "File,Extension,TargetFrameworkVersion,PlatformToolset\r\n");
            logtxt.AppendText("Reading projects....\r\n");
            filelist = Directory.EnumerateFiles(parentpath, "*.*proj", SearchOption.AllDirectories).ToList();
            pl = parentpath.Length + 1;
            pbar.Maximum = filelist.Count;
            pbar.Value = 0;
            foreach (var f in filelist)
            {
                temps = temps + f + "\r\n";
                File.AppendAllText(logfile, f.Substring(pl));
                File.AppendAllText(logfile, "," + Path.GetExtension(f));
                string[] lines = File.ReadAllLines(f);
                bool bfound = false;
                string fw = "";
                string ts = "";
                try
                {
                    foreach (var line in lines)
                    {

                        if (!bfound && line.Contains("TargetFrameworkVersion"))
                        {
                            bfound = true;
                            int p1 = line.IndexOf('>');
                            int p2 = line.IndexOf("</");
                            fw = line.Substring(p1 + 1, p2 - p1 - 1);
                        }

                        if (line.Contains("PlatformToolset"))
                        {
                            int p1 = line.IndexOf('>');
                            int p2 = line.IndexOf("</");
                            ts = line.Substring(p1 + 1, p2 - p1 - 1);
                        }

                    }
                }
                catch
                {
                    fw = "0";
                    ts = "0";
                }

                File.AppendAllText(logfile, String.Format(",{0},{1}\r\n", fw, ts ));
                pbar.Value++;
                if ((pbar.Value) % 100 == 0)
                {
                    logtxt.AppendText(temps);
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(0);
                    temps = "";
                }
            }
            pbar.Value = 0;
            logtxt.AppendText(temps);
            Application.DoEvents();
            System.Threading.Thread.Sleep(0);
            temps = "";
        }

        public static void UpgradeProjects(string parentpath, string outputdir, bool issln)
        {

            if (!Directory.Exists(outputdir))
            {
                Directory.CreateDirectory(outputdir);
            }
            string devenvexe = ConfigurationManager.AppSettings["devenvpath"];
            if (devenvexe == null)
                return;


            var filelist = new List<string> { };
            string logfile;
            int pl=0;
            filelist = Directory.EnumerateFiles(parentpath, issln?"*.*sln":"*.*proj", SearchOption.AllDirectories).ToList();
            logfile = outputdir + (issln ? @"\UpgradeSolutions.htm": @"\UpgradeProjects.htm");
            if (File.Exists(logfile))
                File.Delete(logfile);
            pl = parentpath.Length + 1;

            string html = "<html><head><style>table,th,td{border:1px solid black;}</style></head><body>";
            html = html + "<table style=\"width:100%\">";
            html = html + "<tr><th>File</th><th>Report</th></tr>";
            File.WriteAllText(logfile, html);

            pbar.Maximum = filelist.Count;
            pbar.Value = 0;
            foreach (var f in filelist)
            {
                ProcessStartInfo ps = new ProcessStartInfo();
                ps.FileName = devenvexe;
                ps.Arguments = String.Format("\"{0}\"  /upgrade",  f);
                //ps.WindowStyle = ProcessWindowStyle.Hidden;
                Process p = Process.Start(ps);
                p.WaitForExit();
                html = "<tr><td>" + f.Substring(pl) + "</td>";
                string updfile = Path.GetDirectoryName(f) + "\\UpgradeLog.htm";
                if (File.Exists(updfile))
                    html = html + "<td><a href=\"" + updfile + "\">Log</a></td></tr>";
                else
                    html = html + "<td>Error</td></tr>";

                File.AppendAllText(logfile, html);
                pbar.Value++;
                logtxt.AppendText(f + "\r\n");
                Application.DoEvents();
                System.Threading.Thread.Sleep(0);

            }

            html = "</table></body></html>";
            File.AppendAllText(logfile, html);
            pbar.Value = 0;
        }



        public static void UpdateProjects(string parentpath, string outputdir)
        {
            if (!Directory.Exists(outputdir ))
            {
                Directory.CreateDirectory(outputdir );
            }

            string toolver = ConfigurationManager.AppSettings["ToolsVersion"];
            string toolset = ConfigurationManager.AppSettings["PlatformToolset"];
            string frameworkver = ConfigurationManager.AppSettings["TargetFrameworkVersion"];

            logtxt.Text = "";
            Application.DoEvents();
            System.Threading.Thread.Sleep(0);

            string logfile = outputdir + @"\UpdateProjects.csv";
            if (File.Exists(logfile))
                File.Delete(logfile);

            File.AppendAllText(logfile, "File,Format,TargetFrameworkVersion,PlatformToolset\r\n");
            var filelist = Directory.EnumerateFiles(parentpath, "*.*proj", SearchOption.AllDirectories).ToList();
            pbar.Maximum = filelist.Count;
            pbar.Value = 0;
            foreach (var f in filelist)
            {
                string[] lines = File.ReadAllLines(f);
                bool bfwfound = false;
                bool btsfound = false;
                for (int i = 0; i < lines.Length; ++i)
                {
                    if (lines[i].Contains("ToolsVersion=") && toolver != null)
                    {
                        int p1 = lines[i].IndexOf("ToolsVersion=");
                        int p2 = lines[i].IndexOf('"', p1 + "ToolsVersion=".Length);
                        if (p2 != -1)
                            p2 = lines[i].IndexOf('"', p2+1);
                        if (p2 != -1)
                        {
                            string oldstr = lines[i].Substring(p1,p2-p1+1);
                            string newstr = "ToolsVersion=\""+toolver+'"';
                            lines[i] = lines[i].Replace(oldstr, newstr);
                        }
                    }

                    if (lines[i].Contains("<TargetFrameworkVersion>") && frameworkver != null)
                    {
                        //lines[i] = "<TargetFrameworkVersion>" + frameworkver + "</TargetFrameworkVersion>";
                        int p1 = lines[i].IndexOf("<TargetFrameworkVersion>")+"<TargetFrameworkVersion>".Length;
                        int p2 = lines[i].IndexOf("</TargetFrameworkVersion>");
                        string temps = lines[i].Substring(p1,p2-p1);
                        lines[i]=lines[i].Replace(temps, frameworkver);
                        bfwfound = true;
                    }

                    if (lines[i].Contains("PlatformToolset") && toolset != null)
                    {
                        //lines[i] = "<PlatformToolset>" + toolset + "</PlatformToolset>";
                        int p1 = lines[i].IndexOf("<PlatformToolset>") + "<PlatformToolset>".Length;
                        int p2 = lines[i].IndexOf("</PlatformToolset>");
                        if (p2 <= p1)
                            continue;
                        string temps = lines[i].Substring(p1, p2 - p1);
                        lines[i]=lines[i].Replace(temps, toolset);

                        btsfound = true;
                    }

                }
                if (bfwfound || btsfound)
                {
                    logtxt.AppendText(f + "\r\n");
                    driver.RemoveReadonly(f);
                    File.WriteAllLines(f, lines);
                    File.AppendAllText(logfile, string.Format("{0},{1},{2},{3}\r\n", f, Path.GetExtension(f), bfwfound, btsfound));
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(0);

                }
                pbar.Value++;
            }

            string SlnFormatver = ConfigurationManager.AppSettings["FormatVersion"];
            string VSName = ConfigurationManager.AppSettings["VSName"];
            string VSver = ConfigurationManager.AppSettings["VisualStudioVersion"];
            string MinVsver = ConfigurationManager.AppSettings["MinimumVisualStudioVersion"];
            filelist = Directory.EnumerateFiles(parentpath, "*.*sln", SearchOption.AllDirectories).ToList();
            pbar.Maximum = filelist.Count;
            pbar.Value = 0;
            foreach (var f in filelist)
            {
                List<string> lines = File.ReadAllLines(f).ToList();
                bool bfound = false;
                for (int i = 0; i < lines.Count; ++i)
                {

                    if (lines[i].Contains("Microsoft Visual Studio Solution File, Format Version") && SlnFormatver != null)
                    {
                        lines[i] = "Microsoft Visual Studio Solution File, Format Version " + SlnFormatver;
                        bfound = true;
                        if (VSName != null)
                        {
                            string vsnamestr = "# Visual Studio " + VSName;
                        if (lines[i + 1].Contains("# Visual Studio"))
                            lines[i + 1] = vsnamestr;
                        else
                            lines.Insert(i + 1, vsnamestr);

                        if (VSver != null)
                            {
                                bool bf = false;
                                string vs = "VisualStudioVersion = " + VSver;
                                foreach (string l in lines)
                                    if (l == vs)
                                        bf = true;
                                if (!bf)
                                    lines.Insert(i + 2, vs);
                            }

                        if (MinVsver != null)
                            {
                                bool bf = false;
                                string mv = "MinimumVisualStudioVersion = " + MinVsver;
                                foreach (string l in lines)
                                    if (l == mv)
                                        bf = true;
                                if (!bf)
                                    lines.Insert(i + 3, mv);
                            }
                        }
                    }
                }

                if (bfound)
                {
                    logtxt.AppendText(f + "\r\n");
                    driver.RemoveReadonly(f);
                    File.WriteAllLines(f, lines);
                    File.AppendAllText(logfile, string.Format("{0},{1},{2},{3}\r\n", f, Path.GetExtension(f), false, true));
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(0);

                }
                pbar.Value++;
            }
            pbar.Value = 0;

        }
        public static void RemoveReadonly(string filename)
        {
            if (!File.Exists(filename))
                return;

            FileAttributes attributes = File.GetAttributes(filename);
            if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                attributes &= ~FileAttributes.ReadOnly;
                File.SetAttributes(filename, attributes);
            }

        }

        public static void Getruntimes(string parentpath, string outputdir)
        {
            if (!Directory.Exists(outputdir))
            {
                Directory.CreateDirectory(outputdir);
            }

            string ildasmexe = ConfigurationManager.AppSettings["ildasmpath"];
            if (ildasmexe == null)
                return;
            Environment.SetEnvironmentVariable("ildasmpath", ildasmexe, EnvironmentVariableTarget.Process);

            string dumpbinexe = ConfigurationManager.AppSettings["dumpbinpath"];
            if (dumpbinexe == null)
                return;
            Environment.SetEnvironmentVariable("dumpbinpath", dumpbinexe, EnvironmentVariableTarget.Process);

            string tempfile = outputdir + @"\a.txt";
            string tempfile2 = outputdir + @"\b.txt";
            string outputfile = outputdir + @"\runtimes.csv";

            if (File.Exists(outputfile))
                File.Delete(outputfile);
            File.AppendAllText(outputfile, "Directory,File name,Version,CLR,CRT\n");
            logtxt.AppendText("getting files...\r\n");
            var filelist = Directory.EnumerateFiles(parentpath, "*.exe", SearchOption.AllDirectories).ToList();
            filelist.AddRange(Directory.EnumerateFiles(parentpath, "*.dll", SearchOption.AllDirectories).ToList());
            pbar.Maximum = filelist.Count;
            pbar.Value = 0;
            foreach (var f in filelist)
            {
                //if (!f.Contains("iAssets.Interfaces.BasicTypes1.0.dll"))
                //    continue;

                if (File.Exists(tempfile))
                    File.Delete(tempfile);
                logtxt.AppendText(f + "\r\n");
                string ver = "n/a";
                string ver2 = "n/a";
                string piiver = "n/a";
                Process p = new Process();
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.FileName = "try.bat";
                p.StartInfo.Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\"", f, tempfile, tempfile2);
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.Start();
                p.WaitForExit();

                string s = File.ReadAllText(tempfile);
                int p1 = s.IndexOf("Metadata section:");
                if (p1 != -1)
                {
                    p1 = s.IndexOf("version:", p1);
                    if (p1 != -1)
                    {

                        p1 = s.IndexOf("version:", p1 + 1);
                        if (p1 != -1)
                            ver = s.Substring(p1 + 10, 10);
                    }
                }

                //if (ver != "0")
                {
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(f);
                    piiver = fvi.FileVersion;
                    if (string.IsNullOrEmpty(piiver))
                        piiver = "n/a";
                }


                s = File.ReadAllText(tempfile2);
                p1 = s.IndexOf("msvc");
                if (p1 != -1)
                {
                    var p2 = s.IndexOf(".dll", p1);
                    if (p2 != -1)
                        ver2 = s.Substring(p1, p2 - p1).Trim();

                }

                File.AppendAllText(outputfile, string.Format("{0},{1},\"{2}\",{3},{4}\n", Path.GetDirectoryName(f), Path.GetFileName(f), piiver, ver.Trim(), ver2.Trim()));
                pbar.Value++;
                Application.DoEvents();
                System.Threading.Thread.Sleep(0);
            }
            pbar.Value = 0;

        }



        static List<string> assemlies = new List<string>();
        static bool mixed = false;
        static string parentpath, outputfile, outputfile2;

        static void PrintAssembly(string filename, string fullname, int indent)
        {
            try
            {
                Assembly a = null;
                if (filename != null)
                    a = Assembly.LoadFrom(filename);
                else
                    a = Assembly.Load(fullname);

                try
                {
                    var attribute = a.GetCustomAttributes(typeof(AssemblyProductAttribute), false)[0] as AssemblyProductAttribute;
                    var isFrameworkAssembly = (attribute.Product == "Microsoft® .NET Framework");
                    if (isFrameworkAssembly)
                        return;
                }
                catch
                {
                }
                if (assemlies.Contains(a.FullName))
                    return;
                assemlies.Add(a.FullName);
                Display(indent, false, "Assembly identity={0}", a.FullName);
                Display(indent, false, "Referenced assemblies:");
                foreach (AssemblyName an in a.GetReferencedAssemblies())
                {
                    Display(indent + 1, false, "Name={0}, Version={1}, Culture={2}, PublicKey token={3}", an.Name, an.Version, an.CultureInfo.Name, (BitConverter.ToString(an.GetPublicKeyToken())));
                    if (an.Name.Contains("System"))
                        continue;
                    PrintAssembly(an.CodeBase, an.FullName, indent + 1);
                }
            }
            catch (Exception ex)
            {
                Display(indent, true, ex.Message);
                if (ex.Message.Contains("Mixed mode assembly is built"))
                    mixed = true;
            }

        }

        static Assembly MyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            Assembly a = null;
            string[] searchdirs = { };
            if (ConfigurationManager.AppSettings["searchdirs"] != null)
                searchdirs = ConfigurationManager.AppSettings["searchdirs"].Split(new char[] { ';' });

            foreach (var dir in searchdirs)
            {
                string filename = string.Format("{0}{1}.dll", parentpath + dir, args.Name.Split(',')[0]);
                if (filename.Contains("iAssets.Infra.Domain.XrayModality.Cwis.dll"))
                {
                    a = null;
                }

                try
                {
                    a = Assembly.LoadFrom(filename);
                    return a;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Mixed mode assembly is built"))
                    {
                        Display(0, true, ex.Message);
                        mixed = true;
                        return a;
                    }
                }
            }
            return a;
        }

        public static void Dependents(string ppath,string outputpath)
        {
            if (!Directory.Exists(outputpath))
            {
                Directory.CreateDirectory(outputpath);
            }

            parentpath = ppath;
            outputfile = outputpath + @"\dependetns.txt";
            outputfile2 = outputpath + @"\dependetns_detail.txt";

            if (File.Exists(outputfile))
                File.Delete(outputfile);
            if (File.Exists(outputfile2))
                File.Delete(outputfile2);

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(MyResolveEventHandler);


            string[] projects = Directory.GetFiles(parentpath, "*.*proj", SearchOption.AllDirectories);
            pbar.Maximum = projects.Length;
            pbar.Value = 0;

            foreach (string pf in projects)
            {
                if (!pf.ToLower().Contains(".csproj") && !pf.ToLower().Contains(".vcxproj"))
                    continue;
                logtxt.AppendText(pf + "\r\n");
                Project p = new Project(pf);
                if (!p.GetProperty("OutputType").EvaluatedValue.ToLower().Contains("exe"))
                    continue;
                if (!File.Exists(p.GetProperty("TargetPath").EvaluatedValue))
                    continue;
                if (pf.ToLower().Contains(".vcxproj") && p.GetProperty("CLRSupport").EvaluatedValue == "false")
                    continue;
                string msg = string.Format("{0},{1}", p.FullPath, p.GetProperty("OutputType").EvaluatedValue);
                logtxt.AppendText(msg+"\r\n");
                AddtoLog(outputfile, msg);
                AddtoLog(outputfile2, msg);
                bool bload = false;
                foreach (ProjectItem pi in p.GetItems("Reference"))
                {
                    if (!pi.EvaluatedInclude.Contains("System"))
                        bload = true;
                }
                if (!bload)
                    continue;
                assemlies = new List<string>();
                mixed = false;
                try
                {
                    Assembly.LoadFrom(p.GetProperty("TargetPath").EvaluatedValue);
                    PrintAssembly(p.GetProperty("TargetPath").EvaluatedValue, null, 0);
                }
                catch
                {
                    foreach (ProjectItem pi in p.GetItems("Reference"))
                    {
                        if (pi.EvaluatedInclude.Contains("System"))
                            continue;
                        PrintAssembly(null, pi.EvaluatedInclude, 1);
                    }

                }
                if (mixed)
                    File.AppendAllText(outputfile, pf + "\n");
                pbar.Value++;
                Application.DoEvents();
                System.Threading.Thread.Sleep(0);
            }
            pbar.Value = 0;
        }

        // Display a formatted string indented by the specified amount.
        public static void Display(Int32 indent, bool ball, string format, params object[] param)
        {
            string msg = String.Format(new string(' ', indent * 2));
            string msg2 = String.Format(format, param);
            AddtoLog(outputfile2, msg + msg2 + "\n");
            if (ball)
                AddtoLog(outputfile, msg + msg2 + "\n");
        }

        public static void AddtoLog(string logfile, string msg)
        {
            File.AppendAllText(logfile, msg + "\n");
        }



    }
}
