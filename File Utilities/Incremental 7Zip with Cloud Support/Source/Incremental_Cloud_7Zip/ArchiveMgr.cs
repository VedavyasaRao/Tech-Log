using Incremental_Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using static BackupRestoreTool.ArchiveInfo;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
namespace BackupRestoreTool
{
    public delegate void startprogress();
    public delegate void stopprogress();
    public class ArchiveMgr
    {

        public  startprogress statp{get;set;}
        public  stopprogress stopp { get; set; }
        public ArchiveInfo selarchive=null;
        public List<Tuple<string, string>> mappeddirs = new List<Tuple<string, string>>();
        public System.IO.MemoryStream Khrishadat;
        public System.IO.MemoryStream Testdat;
        Dictionary<string, string> archivepwdmap = new Dictionary<string, string>();
        public Downloader dl = null;

        public void createcloudarchive()
        {
            var parts = App.archivedir.Substring(3).Split('\\');
            UInt64 pid = 0;
            foreach (var p in parts)
            {
                var fid = dl.find_filefolder(pid, p);
                if (fid == 0)
                {
                    dl.create_folder(pid, p);
                    fid = dl.find_filefolder(pid, p);
                    if (fid == 0)
                        return;
                }
                pid = fid;
            }
            dl.create_folder(pid, "backups");
            dl.upload_file(pid, App.archivedir + "\\brinfoK.dat");
            dl.upload_file(pid, App.archivedir + "\\exclude.txt");
            App.unmapdrive(App.clouddrive);
            App.archivedir = "";
            App.clouddrivepath = "";
            App.clouddrive = "";
            dl = null;

        }

        public bool CreateArchive(ArchiveInfo archinfo)
        {
            try
            {
                string archdir = App.archivedir;
                if (DirectoryEx.Exists(archdir))
                {
                    DirectoryEx.Delete(archdir, true);
                }
                DirectoryEx.CreateDirectory(archdir);
                if (App.isclouddrive())
                    DirectoryEx.CreateDirectory(archdir+"\\backups");
                string exfile = archdir + "\\exclude.txt";
                string[] dirs = new string[] {
                    @"c:\Program Files",
                    @"c:\Program Files (x86)",
                    @"c:\Windows"
                };
                FileEx.WriteAllLines(exfile, dirs);
                Persist(archinfo);
                if (App.isclouddrive())
                    createcloudarchive();
            }
            catch
            {
                App.Current.Dispatcher.Invoke(() =>
                {

                    System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow, "cannot create archive");
                });
                return false;
            }
            return true;
        }
        public void uploadfilestocloud(string path)
        {
            dl.uploadfilestocloud(path);
        }

        public void LoadArchive()
        {
            App.logit("Loading Archives....");
            App.pmon.busycursor();
            System.Threading.Thread t = new System.Threading.Thread(Loadit);
            t.Start();
        }
        public  void Loadit()
        {
            Load();
            App.pmon.normalcursor();
            App.EnableDisable(true);
            App.logit("Loading Archives.... done");
        }

        public void Load()
        {
            selarchive = null;
            string archpath = App.archivedir + "\\brinfoK.dat";
            App.logit("Getting Archive Info....");
            App.logit(String.Format("Loading Archive {0}....", archpath));
            if (!archivepwdmap.ContainsKey(archpath))
            {
                var pwd = Microsoft.VisualBasic.Interaction.InputBox("Enter the password", "Load Archive", "", -1, -1);
                if (pwd == "")
                {
                    App.logit("Loading archive failed");
                    return;
                }
                archivepwdmap[archpath] = pwd;
            }
            string[] args = new string[] { "e",  archivepwdmap[archpath], archpath };
            object o=null;
            PipeServer.driver(o, args, out selarchive);
            if (selarchive == null)
            {
                App.logit("Loading archive failed. Invalid password");
                return;
            }

            if (selarchive.archive_size == 0)
                selarchive.archive_size = 4096;
            if (!App.isclouddrive())
                selarchive.cleanup();
            selarchive.PopulateFileItemsFromAllBackups();
            selarchive.UpdateMaps();
            selarchive.archiveddirs = new List<KeyValuePair<string,bool>>();
            var dirs = new List<string>();
            foreach (var bk in selarchive.backups)
            {
                dirs.AddRange(bk.dirs);
            }
            dirs = dirs.Distinct().ToList();
            foreach (var d in dirs)
            {
                selarchive.archiveddirs.Add(new KeyValuePair<string,bool>(d, true));
            }
        }
        public void LoadCloudArchive()
        {
            App.logit("Loading Cloud Archives....");
            App.pmon.busycursor();
            System.Threading.Thread t = new System.Threading.Thread(Loadcloud);
            t.Start();
            App.EnableDisable(false);
        }

        public void Loadcloud()
        {
            if (dl == null)
                dl = new Downloader();
            if (string.IsNullOrEmpty(App.clouddrivepath))
            {
                App.clouddrivepath = App.getoutputpath("3");
                if (DirectoryEx.Exists(App.clouddrivepath))
                {
                    DirectoryEx.Delete(App.clouddrivepath, true);
                    DirectoryEx.CreateDirectory(App.clouddrivepath);
                }

                App.clouddrive = App.getfreedrive();
                App.mapdrive(App.clouddrive, App.clouddrivepath);

                var archivelist = dl.getarchiveinfo();
                if (archivelist == null)
                    return;

                foreach (var item in archivelist)
                {
                    var dirname = App.clouddrivepath + "\\" + Path.GetDirectoryName(item.path);
                    if (!Directory.Exists(dirname))
                        Directory.CreateDirectory(dirname);
                }
            }
            App.archivedir = null;
            var dlgret = System.Windows.Forms.DialogResult.OK;
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.SelectedPath = App.clouddrive + "\\";
            dlgret = dialog.ShowDialog();
            if (dlgret == System.Windows.Forms.DialogResult.OK)
            {
                App.archivedir = dialog.SelectedPath;
            }

        }

        public void SelectCloudArchive()
        {
            App.logit("Select Cloud Archives....");
            App.pmon.busycursor();
            System.Threading.Thread t = new System.Threading.Thread(Selectcloud);
            t.Start();
            App.EnableDisable(false);
        }


        public void Selectcloud()
        {
            if (dl == null)
                dl = new Downloader();
            if (string.IsNullOrEmpty(App.clouddrivepath))
            {
                App.clouddrivepath = App.getoutputpath("3");
                if (DirectoryEx.Exists(App.clouddrivepath))
                {
                    DirectoryEx.Delete(App.clouddrivepath, true);
                    DirectoryEx.CreateDirectory(App.clouddrivepath);
                }

                App.clouddrive = App.getfreedrive();
                App.Current.Dispatcher.Invoke(() =>
                {
                    var msg = "Do you want to select a different path for cloud drive?";
                    if (System.Windows.MessageBox.Show(msg, "Open Cloud", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        var dialog = new System.Windows.Forms.FolderBrowserDialog();
                        if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            App.clouddrivepath = dialog.SelectedPath;
                        }
                    }

                    ((MainWindow)App.mainwindow).srcfolder.Text = App.clouddrive + "\\";
                    ((MainWindow)App.mainwindow).IsEnabled = true;

                });

                App.mapdrive(App.clouddrive, App.clouddrivepath);

                var archivelist = dl.getarchiveinfo();
                if (archivelist == null)
                    return;

                foreach (var item in archivelist)
                {
                    Directory.CreateDirectory(App.clouddrivepath + "\\" + item.path);
                }
            }

            var dlgret = System.Windows.Forms.DialogResult.OK;
            App.disp.Invoke(new Action<MainWindow>((sender) => 
            {
                var dialog = new System.Windows.Forms.FolderBrowserDialog();
                dialog.SelectedPath = ((MainWindow)App.mainwindow).srcfolder.Text;
                dlgret = dialog.ShowDialog();
                if (dlgret == System.Windows.Forms.DialogResult.OK)
                {
                    App.archivedir = dialog.SelectedPath;
                    ((MainWindow)App.mainwindow).srcfolder.Text = App.archivedir;
                }
            }), new object[] { null });

            if (dlgret != System.Windows.Forms.DialogResult.OK)
                return;

            dl.updateselection(App.archivedir);
            var bkupdir = App.archivedir + "\\backups\\";
            if (!Directory.Exists(bkupdir))
            {
                Directory.CreateDirectory(bkupdir);

                dl.downloadbrinfo(App.archivedir);
            }

            if (!Validate())
            {
                App.archivedir = null;
                return;
            }
            App.arm.LoadArchive();
        }

        public bool Validate()
        {
            App.logit("Validating Arcive Dir...");
            if (!string.IsNullOrEmpty(App.archivedir))
            {
                if (DirectoryEx.Exists(App.archivedir))
                    if (FileEx.Exists(App.archivedir + "\\brinfoK.dat"))
                    {
                        App.logit("Validating Arcive Dir...successful");
                        return true;
                    }
            }
            App.logit("Incorrect Arcive Dir");
            App.logit("Validating Arcive Dir...failed");
            return false;
        }

        public void Serialize(object src, ref System.IO.MemoryStream ms)
        {
            try
            {
                if (ms != null && ms.CanRead)
                    ms.Close();
            }
            catch
            {
            }
            App.logit("Updating the archive repository....");
            ms = new System.IO.MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(ms, src);
            }
            catch (SerializationException e)
            {
                App.logit("Failed to serialize. Reason: " + e.Message);
            }
            finally
            {
                App.logit("Updating the archive repository .... done");
            }
        }

        public  object Deserialize(System.IO.MemoryStream ms, bool bclose)
        {
            object root = null;
            // Open the file containing the data that you want to deserialize.
            //FileStream fs = new FileStream(filename, FileMode.Open);
            ms.Seek(0, System.IO.SeekOrigin.Begin);

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();

                // Deserialize the hashtable from the file and 
                // assign the reference to the local variable.
                root = formatter.Deserialize(ms);
            }
            catch (SerializationException e)
            {
                App.logit("Failed to deserialize. Reason: " + e.Message);
            }
            finally
            {
                if (bclose)
                    ms.Close();
            }
            return root;
        }
        public void Persist(ArchiveInfo arcinfo)
        {
            string archfile = App.archivedir + "\\brinfoK.dat";
            string backuparchfile = App.archivedir + "\\brinfoK.dat.bak";
            if (FileEx.Exists(backuparchfile))
                FileEx.Delete(backuparchfile);
            if (FileEx.Exists(archfile))
            {
                FileEx.Copy(archfile, backuparchfile, true);
                FileEx.Delete(archfile);
            }
            //Serialize(arcinfo, archfile);

            if (!archivepwdmap.ContainsKey(archfile))
            {
                var pwd = Microsoft.VisualBasic.Interaction.InputBox("Enter the password", "Save Archive", "", -1, -1);
                if (pwd == "")
                {
                    App.logit("Saving archive failed");
                    return;
                }
                archivepwdmap[archfile] = pwd;
            }

            string[] args = new string[]{ "a", App.sevenzpath, archivepwdmap[archfile], archfile };
            ArchiveInfo o;
            PipeServer.driver(arcinfo, args, out o);
        }

        public bool isfile(string apath)
        {
            System.IO.FileAttributes attr = FileEx.GetAttributes(apath);

            return !(attr.HasFlag(System.IO.FileAttributes.Directory));
        }
    }
}
