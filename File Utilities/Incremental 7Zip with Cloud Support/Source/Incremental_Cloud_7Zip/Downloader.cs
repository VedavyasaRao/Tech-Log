using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace BackupRestoreTool
{

    public class icedrive_folderinfo
    {
        public string path;
        public UInt64 fldrid;
        public UInt64 brinfofid;
        public UInt64 bkupfldrid;
    }

    public class icedrive_fileinfo
    {
        public string filename;
        public UInt64 fileid;
    }

    public class ice_interface
    {
        const string login_exe = "IceDrive\\go-icedrive\\bin\\login.exe";
        const string create_folder_exe = "IceDrive\\go-icedrive\\bin\\create_folder.exe";
        const string delete_filefolder_exe = "IceDrive\\go-icedrive\\bin\\delete_filefolder.exe";
        const string download_file_exe = "IceDrive\\go-icedrive\\bin\\download_file.exe";
        const string find_filefolder_exe = "IceDrive\\go-icedrive\\bin\\find_filefolder.exe";
        const string get_files_exe = "IceDrive\\go-icedrive\\bin\\get_files.exe";
        const string get_folders_exe = "IceDrive\\go-icedrive\\bin\\get_folders.exe";
        const string rename_filefolder_exe = "IceDrive\\go-icedrive\\bin\\rename_filefolder.exe";
        const string upload_file_exe = "IceDrive\\go-icedrive\\bin\\upload_file.exe";
        const int skip_lines = 2;
        string tempfile;

        public ice_interface()
        {
            tempfile = App.outputpath + "\\temp.txt";
        }

        public icedrive_folderinfo selarchive =null;
        public List<icedrive_folderinfo> archive_folder_list = null;
        public List<icedrive_fileinfo> archive_fileslist = null;

        void getuseridpasswd()
        {

            if (App.bearertoken == "")
            {
                App.bearertoken = login();
            }

        }

        void deletetempfile()
        {
            if (File.Exists(tempfile))
                File.Delete(tempfile);

        }
        public string login()
        {
            var loginid = Microsoft.VisualBasic.Interaction.InputBox("Enter the login id", "Enter Credentials", "", -1, -1);
            var passwd = Microsoft.VisualBasic.Interaction.InputBox("Enter the password", "Enter Credentials", "", -1, -1);
            deletetempfile();
            string cmdline = string.Format("/c \"echo logging in... &  {0} {1} {2} > \"{3}\"\"", login_exe, loginid, passwd, tempfile);
            System.Diagnostics.Process.Start("cmd.exe", cmdline).WaitForExit();
            if (!File.Exists(tempfile) || new FileInfo(tempfile).Length == 0)
                return "";
            var lines = File.ReadAllLines(tempfile, System.Text.Encoding.GetEncoding(65001/*437*/)).Skip(skip_lines).ToArray();
            if (lines.Length > 0)
                return lines[0];

            return "";
        }


        public void create_folder(UInt64 fid, string foldername)
        {
            getuseridpasswd();
            string cmdline = string.Format("/c \"echo creating folder... &  {0} {1} {2} \"{3}\"\"", create_folder_exe, App.bearertoken, fid, foldername);
            System.Diagnostics.Process.Start("cmd.exe", cmdline).WaitForExit();
        }

        public void delete_filefolder(UInt64 fid)
        {
            getuseridpasswd();
            string cmdline = string.Format("/c \"echo deleting  file/folder ... & {0} {1} {2} {3}\"", delete_filefolder_exe, App.bearertoken, selarchive.fldrid, fid);
            System.Diagnostics.Process.Start("cmd.exe", cmdline).WaitForExit();
        }

        public void download_file(UInt64 fid, string target_folder)
        {
            getuseridpasswd();
            string cmdline = string.Format("/c \"echo downloading files ... & {0} {1} {2} {3} \"{4}\"\"", download_file_exe, App.bearertoken, selarchive.fldrid, fid, target_folder.Replace('\\', '/'));
            System.Diagnostics.Process.Start("cmd.exe", cmdline).WaitForExit();
        }

        public UInt64 find_filefolder(UInt64 fid, string filefoldername)
        {
            getuseridpasswd();
            deletetempfile();
            string cmdline = string.Format("/c \"echo finding file/folder... &  {0} {1} {2} \"{3}\" > \"{4}\"\"", find_filefolder_exe, App.bearertoken, fid, filefoldername, tempfile);
            System.Diagnostics.Process.Start("cmd.exe", cmdline).WaitForExit();
            if (!File.Exists(tempfile) || new FileInfo(tempfile).Length == 0)
                return 0;
            var lines = File.ReadAllLines(tempfile, System.Text.Encoding.GetEncoding(65001/*437*/)).Skip(skip_lines).ToArray();
            if (lines.Length > 0)
            {
                var parts = lines[0].Split(',');
                return UInt64.Parse(parts[1]);
            }
            return 0;
        }

        public void get_folders()
        {
            getuseridpasswd();
            deletetempfile();

            if (archive_folder_list != null)
                return;
            string cmdline = string.Format("/c \"echo getting folder info... &  {0} {1} {2} ice-drive > \"{3}\"\"", get_folders_exe, App.bearertoken, 0, tempfile);
            System.Diagnostics.Process.Start("cmd.exe", cmdline).WaitForExit();
            if (!File.Exists(tempfile) || new FileInfo(tempfile).Length == 0)
                return;
            var lines = File.ReadAllLines(tempfile, System.Text.Encoding.GetEncoding(65001/*437*/)).Where(l => !l.Contains("\\2024\\")).Skip(skip_lines).Select(l => l.Replace("ice-drive\\", "").Replace("\\backups", ""));
            archive_folder_list = lines.Select(l =>
            {
                var parts = l.Split(',');
                return new icedrive_folderinfo { path = parts[0], fldrid = UInt64.Parse(parts[1]), brinfofid = UInt64.Parse(parts[2]), bkupfldrid = UInt64.Parse(parts[3]) };
            }).ToList();
        }

        public void get_filelist()
        {
            getuseridpasswd();
            deletetempfile();

            if (archive_fileslist != null)
                return;

            string cmdline = string.Format("/c \"echo getting files info... & {0} {1} {2} backups > \"{3}\"\"", get_files_exe, App.bearertoken, selarchive.bkupfldrid, tempfile);
            System.Diagnostics.Process.Start("cmd.exe", cmdline).WaitForExit();
            if (!File.Exists(tempfile) || new FileInfo(tempfile).Length == 0)
                return;

            archive_fileslist = File.ReadAllLines(tempfile, System.Text.Encoding.GetEncoding(65001/*437*/)).Skip(skip_lines).OrderBy(l => l).Select(l =>
            {
                var parts = l.Split(',');
                return new icedrive_fileinfo { filename = parts[0], fileid = UInt64.Parse(parts[1]) };
            }).ToList();
        }

        public void rename_filefolder(UInt64 fid, string target_filefolder)
        {
            getuseridpasswd();
            string cmdline = string.Format("/c \"echo renaming file ... & {0} {1} {2} {3} \"{4}\"\"", rename_filefolder_exe, App.bearertoken, selarchive.fldrid, fid, target_filefolder);
            System.Diagnostics.Process.Start("cmd.exe", cmdline).WaitForExit();
        }

        public void upload_file(UInt64 fid, string filename)
        {
            getuseridpasswd();
            string cmdline = string.Format("/c \"echo uploading file... &  {0} {1} {2} \"{3}\"\"", upload_file_exe, App.bearertoken, fid, filename);
            System.Diagnostics.Process.Start("cmd.exe", cmdline).WaitForExit();
        }

        public void downloadbrinfo(string target_folder)
        {
            download_file(selarchive.brinfofid, target_folder);
        }

        public void updateselection(string foldername)
        {
            selarchive = null;
            foreach (var fldr in archive_folder_list)
            {
                if (foldername.Contains(fldr.path))
                {
                    selarchive = fldr;
                    archive_fileslist = null;
                    break;
                }
            }
        }

        public void uploadfilestocloud(string path)
        {
            var dir = App.archivedir + path;
            var parts = dir.Split('\\');
            var fid = find_filefolder(selarchive.fldrid, "brinfoK.dat.bak");
            if (fid != 0)
                delete_filefolder(fid);
            rename_filefolder(selarchive.brinfofid, "brinfoK.dat.bak");
            upload_file(selarchive.fldrid, App.archivedir + "\\brinfoK.dat"); 
            fid = find_filefolder(selarchive.fldrid, "brinfoK.dat");
            if (fid != 0)
                selarchive.brinfofid = fid;

            create_folder(selarchive.bkupfldrid, Path.GetFileName(dir));
            fid = find_filefolder(selarchive.bkupfldrid, Path.GetFileName(dir));
            foreach (var f in Directory.GetFiles(dir))
            {
                while (true)
                {
                    upload_file(fid, f);
                    var fid2 = find_filefolder(fid, Path.GetFileName(f));
                    if (fid2 != 0)
                        break;
                }

            }
            archive_fileslist = null;
            get_filelist();
        }

    }
    public class Downloader
    {
        public ice_interface icedrive = new ice_interface();

        public void create_folder(UInt64 fid, string foldername)
        {
            icedrive.create_folder(fid, foldername);
        }
        public void delete_filefolder(UInt64 fid)
        {
            icedrive.delete_filefolder(fid);
        }

        public void download_file(UInt64 fid, string foldername)
        {
            icedrive.download_file(fid, foldername);
        }

        public List<icedrive_folderinfo> getarchiveinfo()
        {
            icedrive.get_folders();
            return icedrive.archive_folder_list;
        }

        public List<icedrive_fileinfo> getfilelist()
        {
            icedrive.get_filelist();
            return icedrive.archive_fileslist;
        }

        public UInt64 find_filefolder(UInt64 fid, string filefoldername)
        {
            return icedrive.find_filefolder(fid, filefoldername);
        }

        public void rename_filefolder(UInt64 fid, string target_filefolder)
        {
            icedrive.upload_file(fid, target_filefolder);
        }

        public void upload_file(UInt64 fid, string filename)
        {
            icedrive.upload_file(fid, filename);
        }

        public void downloadbrinfo(string foldername)
        {
            icedrive.downloadbrinfo(foldername);
        }
        public void updateselection(string foldername)
        {
            icedrive.updateselection(foldername);
        }

        public void uploadfilestocloud(string path)
        {
            icedrive.uploadfilestocloud(path);
        }

    }
}
