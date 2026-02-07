using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Configuration;
using System.Net.Http;

namespace FileParser
{
    internal class Program
    {

        static string capture_folders = "";
        static string adbpath = @"platform-tools\adb";

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
            //string output = process.StandardOutput.ReadToEnd();

            // 5. Wait for the process to exit
            process.WaitForExit();
        }

        static void downloadfilelist(string outputfile)
        {
            var folders = capture_folders.Split(',');
            if (File.Exists(outputfile))
                File.Delete(outputfile);

            foreach (var f in folders)
            {
                var cmdline = string.Format(@"/c {0} shell ls -lR sdcard/{1}  >> {2}", adbpath, f, outputfile);
                launchcmdwindow(cmdline, "downloading file list from " + f);

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
                    //curfolder = l.Replace("sdcard/", "").Replace("android/media/com.whatsapp/", "").Replace(":", "");
                    curfolder = l.Replace("sdcard/", "").Replace(":", "");
                }
                else
                {
                    Match match = Regex.Match(l, pattern);
                    if (match.Success)
                    {
                        var sz = match.Groups[1].Value;
                        var dm = DateTimeOffset.Parse(match.Groups[2].Value).ToUnixTimeSeconds();
                        var filename = match.Groups[3].Value;
                        if (filename.EndsWith("."))
                        {
                            filename = filename.Substring(0, filename.LastIndexOf("."));
                        }
                        var line = string.Format("sdcard|{0}\\{1}|{2}*{3}\n", curfolder.Replace("/", "\\"), filename.Replace("/", "\\"), dm * 1000, sz);
                        File.AppendAllText(dstfile, line);
                    }
                }
            }
        }
        static void compare(string firstfile, string secondfile, string exportfile)
        {
            var first = File.ReadAllLines(firstfile).Select(f => { var parts = f.Split(new char[] { '|' }); var n = parts.Length; return new KeyValuePair<string, string>(parts[n - 2], parts[n - 1].Substring(parts[n - 1].IndexOf('*') + 1)); }).ToDictionary(f => f.Key, f => f.Value);
            var second = new Dictionary<string, string>();
            if (File.Exists(secondfile))
                second = File.ReadAllLines(secondfile).Select(f => { var parts = f.Split(new char[] { '|' }); var n = parts.Length; return new KeyValuePair<string, string>(parts[n - 2], parts[n - 1].Substring(parts[n - 1].IndexOf('*') + 1)); }).ToDictionary(f => f.Key, f => f.Value);
            var third = File.ReadAllLines(firstfile).Select(f => { var parts = f.Split(new char[] { '|' }); var n = parts.Length; return new KeyValuePair<string, string>(parts[n - 2], parts[n - 3]); }).ToDictionary(f => f.Key, f => f.Value);

            var changed_files = first.Where(kv => second.ContainsKey(kv.Key) && kv.Value != second[kv.Key]).Select(kv2 => kv2.Key);
            var tmppath = Path.GetDirectoryName(exportfile) + "\\changed_" + Path.GetFileName(exportfile);
            File.WriteAllLines(tmppath, changed_files);

            var new_files = first.Where(kv => !second.ContainsKey(kv.Key)).Select(kv2 =>  kv2.Key);
            tmppath = Path.GetDirectoryName(exportfile) + "\\new_" + Path.GetFileName(exportfile);
            File.AppendAllLines(tmppath, new_files);

            var same_files = first.Where(kv => second.Contains(kv)).Select(kv2 => kv2.Key);
            tmppath = Path.GetDirectoryName(exportfile) + "\\same_" + Path.GetFileName(exportfile);
            File.AppendAllLines(tmppath, same_files);
        }

        static void pullfiles(string exportfile, string exportdir, string outputdir)
        {
            var logfile = Path.Combine(outputdir, "outputfile.log");
            if (File.Exists(logfile))
                File.Delete(logfile);

            var fns = new List<string>() { "changed_", "new_" };
            foreach (var ff in fns)
            {
                var tmppath = Path.GetDirectoryName(exportfile) + "\\" + ff + Path.GetFileName(exportfile);
                if (!File.Exists(tmppath))
                    continue;

                var files = File.ReadAllLines(tmppath);
                foreach (var file in files)
                {
                    var dir = exportdir + "\\sdcard\\" + Path.GetDirectoryName(file);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    var cmdline = string.Format("/c {0} pull \"sdcard/{1}\"  \"{2}\" >> {3} 2>&1", adbpath, file.Replace("\\", "/"), exportdir + "\\sdcard\\" + file, logfile);
                    launchcmdwindow(cmdline, "Downloading file " + file);
                }
            }
        }

        static void pushafolder(string srcdir, string path, string logfile)
        {
            var uploaddirs = Directory.GetDirectories(srcdir, "*", SearchOption.AllDirectories);
            foreach (var dir in uploaddirs)
            {
                var subdir = dir.Replace(srcdir + "\\", "").Replace('\\', '/');
                var dstdir = "sdcard/" + path + "/" + subdir;
                var cmdline = string.Format("/c {0} shell mkdir -p '{1}'  >> {2} 2>&1", adbpath, dstdir, logfile);
                launchcmdwindow(cmdline, "creating dir " + dir);
            }

            var files = Directory.GetFiles(srcdir, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var subdir = file.Replace(srcdir + "\\", "").Replace('\\', '/');
                var dstfile = "sdcard/" + path + "/" + subdir;
                var cmdline = string.Format("/c {0} push \"{1}\"  \"{2}\" >> {3} 2>&1", adbpath, file, dstfile, logfile);
                launchcmdwindow(cmdline, "Uploading file " + file);
            }
        }

        static void pushfiles( string uploaddir, string outputdir)
        {
            var logfile = Path.Combine(outputdir, "outputfile.log");
            if (File.Exists(logfile))
                File.Delete(logfile);

            var capparts = capture_folders.Split(',');
            var uploaddirs = Directory.GetDirectories(uploaddir);

            foreach (var capd in capparts)
            {
                var capdf = Path.GetFileName(capd);
                foreach (var upd in uploaddirs)
                {
                    var updf = Path.GetFileName(upd);
                    if (updf == capdf)
                    {
                        pushafolder(upd, capd, logfile);
                     }
                }
            }
        }

        static string get_capture_folders()
        {
            System.Configuration.Configuration config =
                           ConfigurationManager.OpenExeConfiguration(
                           ConfigurationUserLevel.None);
            var devices = config.AppSettings.Settings.AllKeys;

            string msg = "Enter the number of the device listed below(e.g, 1):\n\n";
            int i = 0;
            Dictionary<int, string> devicemap = new Dictionary<int, string>();
            foreach (var adevice in devices)
            {
                devicemap.Add(++i, adevice);
                msg += $"{i}.        {adevice}\n({config.AppSettings.Settings[adevice].Value.Replace(",",",\n")})\n\n";
            }
            msg = msg.Substring(0, msg.Length - 2);
            string sel = Microsoft.VisualBasic.Interaction.InputBox(msg, "Enter the device number", "", -1, -1);
            if (sel == "")
                return sel;

            int isel = int.Parse(sel);
            if (!devicemap.ContainsKey(isel))
                return "";

            var device = devicemap[isel];
            if (config.AppSettings.Settings[device] != null)
                capture_folders = config.AppSettings.Settings[device].Value;

            return sel;

        }

        static void startadb()
        {
            var p = Process.GetProcessesByName("adb");
            if (p.Length == 1)
                p[0].Kill();
            System.Threading.Thread.Sleep(3000);
            ProcessStartInfo ps = new ProcessStartInfo();
            ps.CreateNoWindow = true;
            ps.WindowStyle = ProcessWindowStyle.Hidden;
            ps.FileName = "adb.exe";
            Process.Start(ps);
        }

        static string mainmenu()
        {
            var msg = "Select an option.\n1.    Download file list\n2.    Backup\n3.    Restore\n";
            string sel = Microsoft.VisualBasic.Interaction.InputBox(msg, "Operations", "1", -1, -1);
            return sel;
        }

        static void downloadfilelist()
        {
            var savefile = "";
            var savdialog = new SaveFileDialog();
            savdialog.Title = "Select Location...";
            savdialog.FileName = "filelist.txt";
            savdialog.DefaultExt = "*.txt";
            if (savdialog.ShowDialog() == DialogResult.OK)
                savefile = savdialog.FileName;
            downloadfilelist(savefile);
        }

        static void preupdatefiles(string processfile)
        {
            var fldrs = capture_folders.Split(',');

            var filetxt = File.ReadAllText(processfile);
            foreach (var fl in fldrs)
            {
                var dirname = Path.GetFileName(fl);
                filetxt = filetxt.Replace("\\" + dirname + "\\", "|" + dirname + "\\");
                filetxt = filetxt.Replace("/" + dirname + "/", "|" + dirname + "/");
            }
            File.WriteAllText(processfile, filetxt);
        }

        static void postupdatefiles(string processfile)
        {
            var fldrs = capture_folders.Split(',');

            var filetxt = File.ReadAllText(processfile);
            foreach (var fl in fldrs)
            {
                var dirname = Path.GetFileName(fl);
                filetxt = filetxt.Replace(dirname + "\\", fl.Replace('/','\\') + "\\");
            }
            File.WriteAllText(processfile, filetxt);
        }


        static void backupfiles()
        {
            var outputdir = "output";

            //1 Select a device to capture folders
            Console.WriteLine("Select a device to capture folders to download");
            //1 Select device
            if (get_capture_folders() == "")
            {
                var msg = "Wrong device or No device selected!\n" +
                    "Edit AndroidStorageUtility.exe.config using notepad to update device list";
                MessageBox.Show(msg, "Capture Folders", MessageBoxButtons.OK);
                return;
            }

            System.Console.WriteLine($"Capture Folders:\n{capture_folders}\n");       

            if (Directory.Exists(outputdir))
                Directory.Delete(outputdir, true);
            Directory.CreateDirectory(outputdir);

            //2 Download dirs
            System.Console.WriteLine("Downloading file lists...");
            var srcfile = Path.Combine(outputdir, "outputfile.txt");
            downloadfilelist(srcfile);

            //3 parse
            System.Console.WriteLine("Parsing files...");
            var dstfile = Path.Combine(outputdir, "outputfile.csv");
            parse(srcfile, dstfile);

            //4 select archive list location
            System.Console.WriteLine("Select Archive File List Location (Cancel if none)...");
            var currentfilelst = "";
            var dialog = new OpenFileDialog();
            dialog.Title = "Select Archive File List Location (Cancel if none)...";
            if (dialog.ShowDialog() == DialogResult.OK)
                currentfilelst = dialog.FileName;
            var newfilename = "";
            if (!string.IsNullOrEmpty(currentfilelst))
            {
                newfilename = outputdir + "\\" + Path.GetFileName(currentfilelst);
                File.Copy(currentfilelst, newfilename);
            }

            //5 change files for comparision
            var fns = new List<string>() { dstfile, newfilename };
            foreach (var ff in fns)
            {
                if (!File.Exists(ff))
                    continue;
                preupdatefiles(ff);
            }

            //6 comparing file lists
            System.Console.WriteLine("Comparing files...");
            compare(dstfile, newfilename, srcfile);

            //7 change filename for writing to disk
            fns = new List<string>() { "changed_", "new_" };
            foreach (var ff in fns)
            {
                var tmppath = outputdir + "\\" + ff + "outputfile.txt";
                if (!File.Exists(tmppath))
                    continue;
                postupdatefiles(tmppath);
            }

            //7 select the Download Folder Location...
            System.Console.WriteLine("Select the Download Folder Location...");
            var exportdir = outputdir;
            var tempfrm = new System.Windows.Forms.Form { TopMost = true };
            var fbd = new FolderBrowserDialog();
            fbd.Description = "Select the Download Folder Location...";
            if (fbd.ShowDialog(tempfrm) == DialogResult.OK)
                exportdir = fbd.SelectedPath;
            else
                return;

            //8 Download files
            System.Console.WriteLine("Downloading files...");
            pullfiles(srcfile, exportdir, outputdir);
        }

        static void restorefiles()
        {
            //1 Select a device to capture folders
            Console.WriteLine("Select a device to restore folders to upload");
            //1 Select device
            if (get_capture_folders() == "")
            {
                var msg = "Wrong device or No device selected!\n" +
                    "Edit AndroidStorageUtility.exe.config using notepad to update device list";
                MessageBox.Show(msg, "Capture Folders", MessageBoxButtons.OK);
                return;
            }

            System.Console.WriteLine($"Restore Folders:\n{capture_folders}\n");

            var outputdir = "output";
            if (Directory.Exists(outputdir))
                Directory.Delete(outputdir, true);
            Directory.CreateDirectory(outputdir);

            //2 select the Download Folder Location...
            System.Console.WriteLine("Select the Upload Folder Location...");
            var tempfrm = new System.Windows.Forms.Form { TopMost = true };
            var fbd = new FolderBrowserDialog();
            fbd.Description = "Select the Upload Folder Location...";
            if (fbd.ShowDialog(tempfrm) != DialogResult.OK)
                return;
            
            var uploaddir = fbd.SelectedPath;

            //3 Upload files
            System.Console.WriteLine("Uploading files...");
            pushfiles (uploaddir, outputdir);
        }

        [STAThread]
        static void Main(string[] args)
        {
            var sel = mainmenu();
            if (sel != "1" && sel != "2" && sel != "3")
            {
                var msg = "Wrong Operation selected!\n";
                MessageBox.Show(msg, "Main Menu", MessageBoxButtons.OK);
                return;
            }

            if (sel == "1")
            {
                downloadfilelist();
                return;
            }
            else if (sel == "2")
            {
                backupfiles();
                return;
            }
            else if (sel == "3")
            {
                restorefiles();
                return;
            }
        }
    }
}
