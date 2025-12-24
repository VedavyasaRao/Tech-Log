using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

namespace BackupRestoreTool
{
    public class zipitem
    {
        public string filename;
        public string crc;
        public double size;
        public string archive;
    }

    [Serializable]
    public class  BackupInfo
    {
        public BackupInfo()
        {
            name = DateTime.Now.ToString("yyyy-MM-dd HH mm ss");
        }

        public string name;
        public byte version=4;
        public List<string> dirs = new List<string>();
        public List<basicfileitem> backup_data_write = new List<basicfileitem>();
        public List<Guid> backup_guid_write = new List<Guid>();
        public string password;

        [field: NonSerialized]
        public List<fileitem> backup_data = new List<fileitem>();
        [field: NonSerialized]
        public Dictionary<string,fileitem> nameficdic = new Dictionary<string, fileitem>();
        [field: NonSerialized]
        public Lookup<string, fileitem> md2ficdic = (Lookup <string, fileitem>)(Enumerable.Empty<fileitem>().ToLookup(x => default(string)));
        [field: NonSerialized]
        public const int passwdlen = 15;
        [field: NonSerialized]
        public const int base64passwdlen = 20;


        public void PopulateFileItemsFromBackup()
        {
            backup_data = backup_data_write.ConvertAll(
            new Converter<basicfileitem, fileitem>((bfi) => new fileitem(bfi)));

        }
        public void UpdateMaps()
        {
            nameficdic = backup_data.ToDictionary(fic => fic._fullPath);
            md2ficdic = (Lookup<string, fileitem>)backup_data.ToLookup(fic => fic._crc);
        }

        public void preparetozip(List<zipitem> filestozip, string backupdir)
        {
            string ziprootdir = backupdir + "\\root";
            DirectoryEx.CreateDirectory(ziprootdir);
            foreach (var d in dirs)
            {
                var tempdir = d.Replace(':', '_').Replace('\\', '_');
                if (App.arm.mappeddirs.Count == 0)
                    Process.Start("cmd", string.Format("/c {0} /J \"{1}\" \"{2}\"", "mklink", ziprootdir + "\\" + tempdir, d)).WaitForExit();
                filestozip.ForEach(zi => zi.filename=zi.filename.Replace(d + "\\", tempdir + "\\"));

                for (var i = 0; i < App.arm.mappeddirs.Count; ++i)
                {
                    var temp = new Tuple<string, string>(App.arm.mappeddirs[i].Item1.Replace(d + "\\", tempdir + "\\"), App.arm.mappeddirs[i].Item2);
                    App.arm.mappeddirs[i] = temp;
                }
            }

            string filerobocopy = App.outputpath + "\\robocopylist.txt";
            List<string> robocopylist = new List<string>();
            string filemklink = App.outputpath + "\\mklinklist.txt";
            List<string> mklinklist = new List<string>();

            foreach (var kv in App.arm.mappeddirs)
            {
                if (App.arm.isfile(kv.Item2))
                {
                    string tempstr = "\"" + kv.Item2 + "\"|\"" + ziprootdir + "\\" + kv.Item1 + "\"|0";
                    robocopylist.Add(tempstr);
                }
                else
                {
                    string tempstr = "\"" + ziprootdir + "\\" + kv.Item1 +  "\"|\"" + kv.Item2 + "\"";
                    mklinklist.Add(tempstr);
                }
            }
            FileEx.WriteAllLines(filerobocopy, robocopylist);
            App.Escapefile(filerobocopy);
            var logfilespath = App.outputpath + "\\copy_" + Guid.NewGuid().ToString() + ".txt";
            Process.Start(App.copyfilespath, string.Format("\"copy\" \"{0}\"  {1} {2}", filerobocopy, robocopylist.Count, logfilespath)).WaitForExit();

            FileEx.WriteAllLines(filemklink, mklinklist);
            App.Escapefile(filerobocopy);
            logfilespath = App.outputpath + "\\makelink_" + Guid.NewGuid().ToString() + ".txt";
            Process.Start(App.copyfilespath, string.Format("\"makelink\" \"{0}\" {1}", filemklink, mklinklist.Count, logfilespath)).WaitForExit();
        }

        string GetRandomPassword()
        {
            byte[] rgb = new byte[passwdlen];
            RNGCryptoServiceProvider rngCrypt = new RNGCryptoServiceProvider();
            rngCrypt.GetBytes(rgb);
            return Convert.ToBase64String(rgb);
        }

        public void getcompressinfo(string zipfile, string pwd, string outputfile)
        {
            string cmdline = string.Format("/c  \"chcp 65001 & echo getting info... & \"{0}\" l -sccUTF-8 -ba -p\"{1}\" \"{2}\" > \"{3}\"\"", App.sevenzpath, pwd,  zipfile, outputfile);
            Process.Start("cmd.exe", cmdline).WaitForExit();
        }


        void calucalate(string ziproot, List<zipitem> filestozip)
        {

            var lines = File.ReadAllLines(ziproot+"temp.txt", System.Text.Encoding.GetEncoding(65001/*437*/));

            System.Text.RegularExpressions.Regex rg = new Regex(@"\s+(\d+)\s+(\d+)\s+(.+)");

            Dictionary<string, long> filecompinfo = new Dictionary<string, long>();
            foreach(var ln in lines)
            {
                MatchCollection parts = rg.Matches(ln.Substring(26));

                filecompinfo.Add(parts[0].Groups[3].ToString(), long.Parse(parts[0].Groups[2].ToString()));
            }

            int counter = 1;
            int i = 0;
            int kount = filestozip.Count();
            double maxzipsz = App.arm.selarchive.archive_size * 1024 * 1024;
            while (i < kount)
            {
                var archive = ziproot + "archive_" + counter++.ToString("0000") + ".7z.001";
                var compsz = filecompinfo[filestozip[i].filename];
                double bucketsz = 0;
                if (compsz >= maxzipsz)
                    filestozip[i++].archive = archive;
                else
                {
                    while (i < filestozip.Count() && ((bucketsz + compsz) < maxzipsz))
                    {
                        bucketsz += compsz;
                        filestozip[i++].archive = archive;
                        if (i < kount)
                            compsz = filecompinfo[filestozip[i].filename];
                    }
                }
            }
            
        }


        public bool zipper(string args)
        {
            try
            {
                App.logit("zipping files... ");
                System.Diagnostics.Process sevenz = new Process();

                sevenz.StartInfo.FileName = App.zipfilespath;
                sevenz.StartInfo.UseShellExecute = true;
                sevenz.StartInfo.CreateNoWindow = true;
                sevenz.StartInfo.Arguments = args;
                sevenz.Start();
                sevenz.WaitForExit();
                if (sevenz.ExitCode != 0)
                    App.logit("zipping files failed... ");
                else
                    App.logit("zipping files succeeded... ");
                return true;
            }

            catch (Exception ex)
            {
                App.logit(ex.Message);
                return false;
            }

        }

        string getuploadedfiles(string dir)
        {
            var outputfile = App.outputpath + "\\loadedfiles.txt";

            if (File.Exists(outputfile))
                File.Delete(outputfile);

            string cmdline = string.Format("/c \"@For /F \"Delims=\" %A in ('dir /B/S/A-D   \"{0}\"') Do @Echo %~nxA %~zA %~tA >> \"{1}\"\"", dir, outputfile);
            Process.Start("cmd.exe", cmdline).WaitForExit();

            if (File.Exists(outputfile))
                return File.ReadAllText(outputfile);
            return "";
        }

        public bool Zip_files(string archivename)
        {
            var bret = false;
            try
            {
                App.logit("Gathering data to save...");
                string backuproot = App.archivedir + "\\backups\\" + name;
                DirectoryEx.CreateDirectory(backuproot);
                string archdatafile = backuproot + "\\Archive_data_0000.7z.001";
                string filelist = backuproot + "\\filelist.txt";
                List<zipitem> filestozip = new List<zipitem>();

                App.pmon.initpbar(md2ficdic.Count);
                foreach (var md2fic in md2ficdic)
                {
                    var ad = md2fic.ElementAt(0);
                    if (md2fic.Count() > 1)
                    {
                        filestozip.Add(new zipitem { filename = ad.duplicatefile, crc = ad._crc, size = ad._size, archive = archdatafile.Replace(App.archivedir,"") });

                    }
                    else
                    {
                        filestozip.Add(new zipitem { filename = ad._fullPath, crc = ad._crc, size = ad._size, archive = archdatafile.Replace(App.archivedir, "") });
                    }
                    App.pmon.updatepbar();
                }
                App.pmon.closebar();

                preparetozip(filestozip, backuproot);
                if (App.arm.selarchive.arch_loc == ArchiveInfo.archive_location.cloud)
                {
                    FileEx.AppendAllLines(filelist, filestozip.Select(zi => zi.filename).ToArray());
                    string pwd = GetRandomPassword();
                    string args = string.Format("zip \"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\"", backuproot + "\\root", archdatafile.Replace(".001", ""), filelist, pwd, 4096, "off");
                    zipper(args);
                    string outputfile = backuproot + "\\temp.txt";
                    getcompressinfo(archdatafile, pwd, outputfile);
                    calucalate(backuproot + "\\", filestozip);
                    string cmdline = "/c del \"" + archdatafile.Replace(".001", ".*") + '"';
                    Process.Start("cmd.exe", cmdline).WaitForExit();
                    File.Delete(outputfile);
                }

                var l = filestozip.GroupBy(itm => itm.archive, (archive, itms) =>
                    new { Key = archive, fls = itms.Select(fi => fi.filename).ToArray(), crcs = itms.Select(fi => fi.crc).ToArray() }).OrderBy(a => a.Key);
                foreach (var itm in l)
                {
                    File.WriteAllLines(filelist, itm.fls);

                    string pwd = GetRandomPassword();
                    password += pwd;
                    string args = string.Format("zip \"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\"", backuproot + "\\root", itm.Key.Replace(".001", ""), filelist, pwd, App.arm.selarchive.archive_size, "on");
                    zipper(args);
                    foreach (var crc in itm.crcs)
                    {
                        md2ficdic[crc].ToList().ForEach(fi => fi.archive = itm.Key.Replace(App.archivedir, ""));
                    }
                }
                if (DirectoryEx.Exists(backuproot + "\\root"))
                    App.RemoveDirFile(backuproot + "\\root", true);
                FileEx.Delete(filelist);
                bret = true;
            }
            catch (Exception )
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(Application.Current.MainWindow, "Files could not be uploaded", "Upload Files");
                });
                bret=false;
            }
            finally
            {
                App.arm.mappeddirs.Clear();
            }

            return bret;
        }
    }
}
