using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Configuration;
using System.Net.Http;

namespace FileParser
{
    internal class Program
    {
        static string capture_folders = "DCIM,Download,android/media/com.whatsapp";
        static string adbpath = @"..\platform-tools\adb";

        static void launchcmdwindow(string cmdline, string msg)
        {
            System.Console.WriteLine(msg);

            // 1. Create a new Process instance
            Process process = new Process();

            // 2. Configure the start information
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = cmdline;
            process.StartInfo.UseShellExecute = false; // Required to redirect streams
            process.StartInfo.RedirectStandardOutput = true; // Redirect the output
            process.StartInfo.CreateNoWindow = true; // Hide the command window
            process.StartInfo.WorkingDirectory = ""; // WorkingDirectory: Sets the initial directory for the process. 

            // 3. Start the process
            process.Start();

            // 4. Read the output (synchronously)
            string output = process.StandardOutput.ReadToEnd();

            // 5. Wait for the process to exit
            process.WaitForExit();
        }

        static void downloadfilelist(string outputfile)
        {
            var folders = capture_folders.Split(',');
            if (File.Exists(outputfile))
                File.Delete(outputfile);

            foreach(var f in folders)
            {
                var cmdline = string.Format(@"/c {0} shell ls -lR sdcard/{1}  >> {2}", adbpath, f, outputfile);
                launchcmdwindow(cmdline,"downloading file list from "+f);

            }
        }
        static void parse(string srcfile, string dstfile)
        {
            var lines = File.ReadAllLines(srcfile).ToList();
            lines = lines.Where(l => (!l.StartsWith("total") && !l.StartsWith("d")) && !string.IsNullOrEmpty(l)).ToList();

            if (File.Exists(dstfile))
                File.Delete(dstfile);


            string pattern = @" (\d+) (\d\d\d\d-\d\d-\d\d \d\d:\d\d) (.+)";

            var curfolder = "";
            foreach (var l in lines)
            {
                if (l.EndsWith(":"))
                {
                    curfolder = l.Replace("sdcard/", "").Replace("android/media/com.whatsapp/", "").Replace(":", "");
                }
                else
                {
                    Match match = Regex.Match(l, pattern);
                    if (match.Success)
                    {
                        var sz = match.Groups[1].Value;
                        var dm = DateTimeOffset.Parse(match.Groups[2].Value).ToUnixTimeSeconds();
                        var filename = match.Groups[3].Value;
                        if ( filename.EndsWith("."))
                        {
                            filename = filename.Substring(0,filename.LastIndexOf("."));
                        }
                        var line = string.Format("sdcard|{0}\\{1}|{2}*{3}\n", curfolder.Replace("/", "\\"), filename.Replace("/", "\\"), dm * 1000, sz);
                        File.AppendAllText(dstfile, line);
                    }
                }
            }
        }
        static void compare(string firstfile, string secondfile, string exportfile)
        {

            var first = File.ReadAllLines(firstfile).Select(f => { var parts = f.Split(new char[] { '|' }); var n = parts.Length; return new KeyValuePair<string, string>(parts[n - 2], parts[n - 1].Substring(parts[n - 1].IndexOf('*')+1)); }).ToDictionary(f => f.Key, f => f.Value);
            var second = new Dictionary<string, string>();
            if (File.Exists(secondfile))
                second = File.ReadAllLines(secondfile).Select(f => { var parts = f.Split(new char[] { '|' }); var n = parts.Length; return new KeyValuePair<string, string>(parts[n - 2], parts[n - 1].Substring(parts[n - 1].IndexOf('*') + 1)); }).ToDictionary(f => f.Key, f => f.Value);
            var third = File.ReadAllLines(firstfile).Select(f => { var parts = f.Split(new char[] { '|' }); var n = parts.Length; return new KeyValuePair<string, string>(parts[n - 2], parts[n - 3]); }).ToDictionary(f => f.Key, f => f.Value);

            var changed_files = first.Where(kv => second.ContainsKey(kv.Key) && kv.Value != second[kv.Key]).Select(kv2 => third[kv2.Key]+"\\"+kv2.Key);
            var tmppath = Path.GetDirectoryName(exportfile) + "\\changed_" + Path.GetFileName(exportfile);
            File.WriteAllLines(tmppath, changed_files);

            var new_files = first.Where(kv => !second.ContainsKey(kv.Key)).Select(kv2 => third[kv2.Key] + "\\" + kv2.Key);
            tmppath = Path.GetDirectoryName(exportfile) + "\\new_" + Path.GetFileName(exportfile);
            File.AppendAllLines(tmppath, new_files);

            var same_files = first.Where(kv => second.Contains(kv)).Select(kv2 => third[kv2.Key] + "\\" + kv2.Key);
            tmppath = Path.GetDirectoryName(exportfile) + "\\same_" + Path.GetFileName(exportfile);
            File.AppendAllLines(tmppath, same_files);
        }


        static void copyfiles(string exportfile,string exportdir, string outputdir)
        {
            var tempexportdir = exportdir + "\\sdcard";
            if (!Directory.Exists(tempexportdir))
                Directory.CreateDirectory(tempexportdir);

            var logfile = Path.Combine(outputdir, "outputfile.log");
            if (File.Exists(logfile))
                File.Delete(logfile);

            var fns = new List<string>() { "changed_" ,"new_"};
            foreach (var ff in fns)
            {
                var tmppath = Path.GetDirectoryName(exportfile) + "\\" + ff + Path.GetFileName(exportfile);
                if (!File.Exists(tmppath))
                    continue;

                var files = File.ReadAllLines(tmppath);
                foreach (var file in files)
                {
                    var dir = exportdir + "\\" + Path.GetDirectoryName(file);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    var cmdline = string.Format("/c {0} pull \"{1}\"  \"{2}\" >> {3} 2>&1", adbpath, file.Replace("\\", "/").Replace("/WhatsApp/", "/android/media/com.whatsapp/WhatsApp/"), exportdir + "\\" + file, logfile);
                    launchcmdwindow(cmdline, "copying file "+ file);
                }
            }
        }

        static void get_capture_folders()
        {
            System.Configuration.Configuration config =
                           ConfigurationManager.OpenExeConfiguration(
                           ConfigurationUserLevel.None);
            var devices = "";
            if (config.AppSettings.Settings["Devices"] != null)
                devices = config.AppSettings.Settings["Devices"].Value;

            string[] parts = devices.Split(',');
            string msg = "Select number\n";
            int i = 0;
            Dictionary<int, string> devicemap = new Dictionary<int, string>();
            foreach (var part in parts)
            {
                devicemap.Add(++i, part);
                msg += $"{i}.{part}\n";
            }

            string sel = Microsoft.VisualBasic.Interaction.InputBox(msg, "Select a Device", "", -1, -1);
            if (sel == "")
                return;
            sel = devicemap[int.Parse(sel)];
            if (config.AppSettings.Settings[sel] != null)
                capture_folders = config.AppSettings.Settings[sel].Value;

        }

        [STAThread]
        static void Main(string[] args)
        {
            var outputdir = "..\\output";

            get_capture_folders();
            if (capture_folders == "")
                return;

            if (Directory.Exists(outputdir))
                Directory.Delete(outputdir, true);
            Directory.CreateDirectory(outputdir);

            System.Console.WriteLine("Downloading files...");
            var srcfile = Path.Combine(outputdir, "outputfile.txt");
            downloadfilelist(srcfile);

            System.Console.WriteLine("Parsing files...");
            var dstfile = Path.Combine(outputdir, "outputfile.csv");
            parse(srcfile, dstfile);

            var currentfilelst = "";
            var dialog = new OpenFileDialog();
            dialog.Title = "Select Saved File Location...";
            if (dialog.ShowDialog() == DialogResult.OK)
                currentfilelst = dialog.FileName;

            System.Console.WriteLine("Comparing files...");
            compare(dstfile, currentfilelst, srcfile);

            dialog.Title = "Select Export Dir Location...";
            var exportdir = outputdir+"\\sdcard";
            var fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
                exportdir = fbd.SelectedPath;
            else
                return;

            System.Console.WriteLine("Copying files...");
            copyfiles(srcfile, exportdir, outputdir);
        }
    }
}
