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

namespace FileParser
{
    internal class Program
    {
        static string capture_folders = "DCIM, Download, android/media/com.whatsapp";

        static void downloadfilelist(string outputfile)
        {
            var folders = capture_folders.Split(',');
            if (File.Exists(outputfile))
                File.Delete(outputfile);

            foreach(var f in folders)
            {
                var cmdline = string.Format(@"/c platform-tools\adb shell ls -lR sdcard/{0}  >> {1}", f, outputfile);
                Process.Start("cmd.exe",cmdline).WaitForExit();

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
                        var line = string.Format("sdcard|{0}\\{1}|{2}{3}\n", curfolder.Replace("/", "\\"), filename.Replace("/", "\\"), dm * 1000, sz);
                        File.AppendAllText(dstfile, line);
                    }
                }
            }
        }
        static void compare(string firstfile, string secondfile, string exportfile)
        {
            var first = File.ReadAllLines(firstfile).Select(f => { var parts = f.Split(new char[] { '|' }); return new KeyValuePair<string, string>(parts[1], parts[2]); }).ToDictionary(f => f.Key, f => f.Value);
            var second = File.ReadAllLines(secondfile).Select(f => { var parts = f.Split(new char[] { '|' }); return new KeyValuePair<string, string>(parts[1], parts[2]); }).ToDictionary(f => f.Key, f => f.Value);
            var third = File.ReadAllLines(firstfile).Select(f => { var parts = f.Split(new char[] { '|' }); return new KeyValuePair<string, string>(parts[1], parts[0]); }).ToDictionary(f => f.Key, f => f.Value);

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
            if (Directory.Exists(exportdir))
                Directory.Delete(exportdir, true);
            var logfile = Path.Combine(outputdir, "outputfile.log");
            if (File.Exists(logfile))
                File.Delete(logfile);

            var fns = new List<string>() { "changed_" ,"new_"};
            foreach (var ff in fns)
            {
                var tmppath = Path.GetDirectoryName(exportfile) + "\\" + ff + Path.GetFileName(exportfile);
                var files = File.ReadAllLines(tmppath);
                foreach (var file in files)
                {
                    var dir = Path.GetDirectoryName(file);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                    var cmdline = string.Format("/c platform-tools\\adb pull \"{0}\"  \"{1}\" >> {2} 2>&1", file.Replace("\\", "/").Replace("/WhatsApp/", "/android/media/com.whatsapp/WhatsApp/"), file, logfile);
                    Process.Start("cmd.exe", cmdline).WaitForExit();
                }
            }
        }

        [STAThread]
        static void Main(string[] args)
        {
            var outputdir = "output";

            System.Configuration.Configuration config =
                           ConfigurationManager.OpenExeConfiguration(
                           ConfigurationUserLevel.None);

            if (config.AppSettings.Settings["capture_folders"] != null)
                capture_folders = config.AppSettings.Settings["capture_folders"].Value;


            if (Directory.Exists(outputdir))
                Directory.Delete(outputdir, true);
            Directory.CreateDirectory(outputdir);

            var srcfile = Path.Combine(outputdir, "outputfile.txt");
            downloadfilelist(srcfile);

            var dstfile = Path.Combine(outputdir, "outputfile.csv");
            parse(srcfile, dstfile);

            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            compare(dstfile, dialog.FileName, srcfile);

            copyfiles(srcfile,"sdcard", outputdir);
        }
    }
}
