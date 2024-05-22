using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Configuration;

namespace SourceIndexer
{
    class Program
    {
        static string sourcefileslocation = @"D:\Github\TechBlog\COM+\APITester - Test any COM component using its typelibrary";
        static string pdblocation = @"D:\Github\TechBlog\COM+\APITester - Test any COM component using its typelibrary\bin";
        static string lscmlocation = @"C:\Program Files (x86)\IBM\TeamConcert\scmtools\eclipse\lscm.bat";
        static string repository = "https://rtc.vrrao.com:9443/ccm/";
        static string snapshot = "APITESTER_1969";
        static string[] components = { "Source" };
        static string[] componentids = { "Khri$ha"};
        static ProcessStartInfo startInfo;

        static Dictionary<string, List<string>> pdbfileset = new Dictionary<string, List<string>>();
        static string workingdir;
        static string outputdir;
        static string outputfile;
        static string logfile;
        static string[][] sourcefiles = new string[3][];
        static void GetSourceFiles()
        {
            for (int i = 0; i < components.Length; ++i)
            {
                logit("Gathering source files for " + components[i]);
                sourcefiles[i] = Directory.GetFiles(sourcefileslocation + "\\" + components[i], "*.*", SearchOption.AllDirectories);
                sourcefiles[i] = (from f in sourcefiles[i] select f.Replace(sourcefileslocation + "\\" + components[i] + "\\", "")).ToArray();
            }
        }

        static void LoadConfig()
        {
            System.Configuration.Configuration config =
                                      ConfigurationManager.OpenExeConfiguration(
                                      ConfigurationUserLevel.None);

            if (config.AppSettings.Settings["sourcefileslocation"] != null)
                sourcefileslocation = config.AppSettings.Settings["sourcefileslocation"].Value;

            if (config.AppSettings.Settings["pdblocation"] != null)
                pdblocation = config.AppSettings.Settings["pdblocation"].Value;

            if (config.AppSettings.Settings["lscmlocation"] != null)
                lscmlocation = config.AppSettings.Settings["lscmlocation"].Value;

            if (config.AppSettings.Settings["repository"] != null)
                repository = config.AppSettings.Settings["repository"].Value;

            if (config.AppSettings.Settings["snapshot"] != null)
                snapshot = config.AppSettings.Settings["snapshot"].Value;

            if (config.AppSettings.Settings["components"] != null)
                components = config.AppSettings.Settings["components"].Value.Split(',');

            if (config.AppSettings.Settings["componentids"] != null)
                componentids = config.AppSettings.Settings["componentids"].Value.Split(',');

        }

        static void SaveConfig()
        {
            System.Configuration.Configuration config =
                                      ConfigurationManager.OpenExeConfiguration(
                                      ConfigurationUserLevel.None);

            config.AppSettings.Settings.Remove("sourcefileslocation");
            config.AppSettings.Settings.Add("sourcefileslocation", sourcefileslocation);

            config.AppSettings.Settings.Remove("pdblocation");
            config.AppSettings.Settings.Add("pdblocation", pdblocation);

            config.AppSettings.Settings.Remove("lscmlocation");
            config.AppSettings.Settings.Add("lscmlocation", lscmlocation);

            config.AppSettings.Settings.Remove("repository");
            config.AppSettings.Settings.Add("repository", repository);

            config.AppSettings.Settings.Remove("snapshot");
            config.AppSettings.Settings.Add("snapshot", snapshot);

            config.AppSettings.Settings.Remove("components");
            string temps="";
            foreach (var s in components) temps = temps + s + ",";
            temps = temps.Substring(0, temps.Length-1);
            config.AppSettings.Settings.Add("components", temps);

            config.AppSettings.Settings.Remove("componentids");
            temps = "";
            foreach (var s in componentids) temps = temps + s + ",";
            temps = temps.Substring(0, temps.Length - 1);
            config.AppSettings.Settings.Add("componentids", temps);

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

        }

        static void GetPDBandSourcefiles()
        {
            outputfile = outputdir + "\\files.txt";
            string[] compsrchs = (from c in components select "\\" + c.ToLower() + "\\").ToArray();
            int[] compsrchlens = (from c in compsrchs select c.Length).ToArray();

            foreach (var fi in new DirectoryInfo(pdblocation).EnumerateFiles("*.pdb", SearchOption.AllDirectories))
            {
                logit("Extracting source files for "+fi.FullName);
                startInfo.FileName = "\"" + workingdir + "\\runsrctool.cmd\"";
                startInfo.Arguments = "\"" + fi.FullName + "\" \"" + outputfile + "\"";
                Process proc = new Process();
                proc.StartInfo = startInfo;
                proc.Start();
                proc.WaitForExit();
                FileInfo fiout = new FileInfo(outputfile);
                if (!fiout.Exists)
                {
                    logit("No Source files were found");
                    continue;
                }
                List<string> pdbsrcfiles = new List<string>();
                string[] srcfiles = File.ReadAllLines(outputfile);
                srcfiles = (from f in srcfiles where compsrchs.Any(c => f.ToLower().Contains(c)) && !f.ToLower().Contains("build") select f).ToArray();
                if (srcfiles.Length > 0)
                {
                    for (int x = 0; x < compsrchs.Length; ++x)
                        pdbsrcfiles.AddRange((from f in srcfiles join s in sourcefiles[x] on f.ToLower().Substring(f.ToLower().IndexOf(compsrchs[x]) + compsrchlens[x]) equals s.ToLower() select f + "*" + s.Replace("\\", "/") + "*" + x.ToString()));
                    if (pdbsrcfiles.Count() > 0)
                    {
                        logit("Source files were found");
                        pdbfileset.Add(fi.FullName, pdbsrcfiles);
                    }
                    else
                        logit("No Source files were found");
                }
            }
        }

        public static void CreateandPersistSourceServerStream()
        {
            outputfile = outputdir + "\\pdbout.txt";

            foreach (var kv in pdbfileset)
            {
                logit("Updating source index stream for: " + kv.Key);
                string SourceStream = outputdir+"\\"+Path.GetFileName(kv.Key)+".ssidx";
                StreamWriter SourceServerStream = new StreamWriter(SourceStream);

                SourceServerStream.WriteLine("SRCSRV: ini ------------------------------------------------");
                SourceServerStream.WriteLine("VERSION=1");
                SourceServerStream.WriteLine("INDEXVERSION=2");
                SourceServerStream.WriteLine("VERCTRL=RTC");
                SourceServerStream.WriteLine("SRCSRV: variables ------------------------------------------");
                SourceServerStream.WriteLine("SRCSRVTRG=%TARG%\\%VAR6%\\%fnbksl%(%VAR5%)");
                SourceServerStream.WriteLine("SRCSRVCMD=cmd /c md \"{6}\" & call \"{0}\" get file -r {1}  -s {2} -c {3}  -f \"{4}\" -o  {5}", lscmlocation, "%VAR2%", "%VAR3%", "%VAR4%", "%VAR5%", "%SRCSRVTRG%", "%TARG%\\%VAR6%\\%VAR7%");
                SourceServerStream.WriteLine("SRCSRV: source files ---------------------------------------");

                foreach (var SourceFile in kv.Value)
                {
                    string[] parts = SourceFile.Split(new char[] { '*' });
                    int idx = int.Parse(parts[2]);
                    SourceServerStream.WriteLine("{0}*\"{1}\"*\"{2}\"*\"{3}\"*{4}*{5}*{6}", parts[0],repository, snapshot, componentids[idx], parts[1], components[idx], Path.GetDirectoryName(parts[1]));
                }

                SourceServerStream.WriteLine("SRCSRV: end ------------------------------------------------");
                SourceServerStream.Close();

                startInfo.FileName = "\"" + workingdir + "\\runpdbstr.cmd\"";
                startInfo.Arguments = "\"" + kv.Key + "\" \"" + SourceStream + "\"  \"" + outputfile + "\"";
                Process proc = new Process();
                proc.StartInfo = startInfo;
                proc.Start();
                proc.WaitForExit();
            }
        }
    
        static void logit(string msg)
        {

            Console.WriteLine(msg);
            File.AppendAllText(logfile, msg + "\r\n");
        }

        static void Main(string[] args)
        {
            System.Configuration.Configuration config =
                          ConfigurationManager.OpenExeConfiguration(
                          ConfigurationUserLevel.None);
            workingdir = Path.GetDirectoryName(config.AppSettings.CurrentConfiguration.FilePath);
            outputdir = workingdir + "\\output";
            if (Directory.Exists(outputdir))
                Directory.Delete(outputdir, true);
            Directory.CreateDirectory(outputdir);
            logfile = outputdir + "\\" + "SourceIndexer.log";
            LoadConfig();
            startInfo = new ProcessStartInfo();
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            logit("Getting source files...");
            GetSourceFiles();
            logit("Getting PDB files...");
            GetPDBandSourcefiles();
            if (pdbfileset.Count > 0)
            {
                logit("Updating source Index stream for PDB files...");
                CreateandPersistSourceServerStream();
            }
            logit("\n\nDone\n Check SourceInder.log for details\nPress any key to continue...");
            SaveConfig();
            Console.ReadKey();
        }

    }
}
