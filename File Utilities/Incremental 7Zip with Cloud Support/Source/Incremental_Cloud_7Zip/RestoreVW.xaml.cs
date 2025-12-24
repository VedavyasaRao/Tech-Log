using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Xml.Linq;

namespace BackupRestoreTool
{
    public class Downloadfile
    {
        public void unzipfile(string archivefile, string args, ref System.Diagnostics.Process sevenz)
        {
            string zipfile = archivefile;
            if (App.arm.selarchive.arch_loc == ArchiveInfo.archive_location.cloud &&  App.isclouddrive())
            {
                var filelist = App.arm.dl.getfilelist();

                string[] parts = archivefile.Split(new char[] { '\\' });
                string zipfilekey = parts[parts.Length - 2] + '\\' + parts[parts.Length - 1].Replace(".001", "");
                var sevenzipfiles = filelist.Where(l => l.filename.Contains(zipfilekey)).ToList();
                foreach (var zf in sevenzipfiles)
                {

                    zipfile = App.archivedir + "\\" + zf.filename;
                    int knt = 0;

                    while (!FileEx.Exists(zipfile))
                    {
                        App.arm.dl.download_file(zf.fileid, Path.GetDirectoryName(zipfile) + "\\");
                        if (FileEx.Exists(zipfile))
                            break;
                        
                        if (++knt == 100)
                            break;
                    }

                    if (knt == 100)
                        return;

                    if (!zipfiles.Contains((zipfile)))
                        zipfiles.Add(zipfile);
                }
                zipfile =  zipfiles[0];
                //else if (volumename == App.gdrive)
                //{
                //    foreach (var f in filelist)
                //    {
                //        var cmdline = string.Format("/c  robocopy  \"{0}\" {1} \"{2}\" &pause", Path.GetDirectoryName(f),App.outputpath,Path.GetFileName(f));
                //        Process.Start("cmd.exe", cmdline).WaitForExit();

                //    }

                //    zipfile = App.outputpath + "\\" + System.IO.Path.GetFileName(archivefile);

                //}
            }
            sevenz.StartInfo.Arguments = args.Replace(archivefile, zipfile);
            sevenz.Start();
            sevenz.WaitForExit();
        }

        public void cleanup()
        {
            foreach (var f in zipfiles)
                FileEx.Delete(f);
        }

        List<string> zipfiles = new List<string>();

    }

    /// <summary>
    /// Interaction logic for RestoreVW.xaml
    /// </summaryBackupstoRestore
    public partial class RestoreVW : Window
    {
        string outputdir = "";
        TVItmUtil tviutil = new TVItmUtil();
        fileitem treenode;
        BackupInfo selbackup = null;

        ObservableCollection<string> Backups
        {
            get
            {
                if (App.arm.selarchive == null)
                    return new ObservableCollection<string>();
                return new ObservableCollection<string>((from bk in App.arm.selarchive.backups select bk.name).ToList());
            }
        }


        private bool ValidateLoad()
        {
            if (App.arm.selarchive ==null)
            {
                App.logit("no collection selected");
                return false;
            }
            if (backups.SelectedIndex==-1)
            {
                App.logit("no backups selected for the collection");
                return false;
            }
            return true;
        }

        public RestoreVW()
        {
            InitializeComponent();
        }

        private void sort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            if (TvDirFiles == null)
                return;
            SortConverter.sel = sort.SelectedIndex;
            TvDirFiles.Items.Refresh();
            tviutil.clear(sliderpnl, slider, (fileitemComp.fileitemsort)sort.SelectedIndex);
            foreach (var fi in TvDirFiles.Items)
                ((fileitem)fi).updatesorted();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            System.Configuration.Configuration config =
                           ConfigurationManager.OpenExeConfiguration(
                           ConfigurationUserLevel.None);

            config.AppSettings.Settings.Remove("dirszsort");
            config.AppSettings.Settings.Add("dirszsort", sort.SelectedIndex.ToString());

            config.AppSettings.Settings.Remove("dirszwd");
            config.AppSettings.Settings.Add("dirszwd", width.Text);

            config.AppSettings.Settings.Remove("dirszwwd");
            config.AppSettings.Settings.Add("dirszwwd", this.Width.ToString());

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            e.Cancel = true;
            Hide();
            App.mainwindow.IsEnabled = true;
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Configuration.Configuration config =
                           ConfigurationManager.OpenExeConfiguration(
                           ConfigurationUserLevel.None);

            TvDirFiles.Items.Clear();

            if (config.AppSettings.Settings["dirszsort"] != null)
                sort.SelectedIndex = int.Parse(config.AppSettings.Settings["dirszsort"].Value);

            if (config.AppSettings.Settings["dirszwd"] != null)
                width.Text = config.AppSettings.Settings["dirszwd"].Value;

            if (config.AppSettings.Settings["dirszwwd"] != null)
                this.Width = double.Parse(config.AppSettings.Settings["dirszwwd"].Value);
            collections_SelectionChanged(null, null);

        }

        private void TreeView_OnCollapsed(object sender, RoutedEventArgs e)
        {
            fileitem fi = (fileitem)((TreeViewItem)e.OriginalSource).Header;
            tviutil.closenode(fi);
        }
        private void TreeView_OnExpanded(object sender, RoutedEventArgs e)
        {
            fileitem fi = (fileitem)((TreeViewItem)e.OriginalSource).Header;
            tviutil.expandnode(fi);
        }

        private void TvDirFiles_Selected(object sender, RoutedEventArgs e)
        {

            TreeViewItem tvi = e.OriginalSource as TreeViewItem;
            if (tvi == null)
                return;

            if (!tvi.IsExpanded)
                return;
            var fi = (fileitem)TvDirFiles.SelectedValue;
            tviutil.selectnode(fi);
        }

        private void slider_DragCompleted(object sender, RoutedEventArgs e)
        {
            var fi = (fileitem)TvDirFiles.SelectedValue;
            tviutil.dragslider(fi);
        }

        private void TvDirFiles_LostFocus(object sender, RoutedEventArgs e)
        {
            tviutil.lostfocus();
        }

        private void width_LostFocus(object sender, RoutedEventArgs e)
        {
            fileitem.n = int.Parse(width.Text);
            if (TvDirFiles != null)
            {
                TvDirFiles.Items.Refresh();
                TvDirFiles.UpdateLayout();
            }
        }
        

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {

            var fi = ((fileitem)((System.Windows.FrameworkElement)(((System.Windows.FrameworkContentElement)(((System.Windows.Controls.CheckBox)e.OriginalSource).Parent)).Parent)).DataContext);


            if (fi == null || fi._Items.Count == 0)
                return;
            fi.toggleselected(((System.Windows.Controls.CheckBox)e.Source).IsChecked,!chksync.IsChecked??false);

        }

        private void Load_Tree(object o)
        {
            bool syncfldrs=true;
            bool changesonly = true;
            App.pmon.busycursor();
            App.bringfront(this);

            App.disp.Invoke(new Action(() =>
            {
                TvDirFiles.Items.Clear();
                syncfldrs = chksync.IsChecked.Value;
                changesonly = chkchangeonly.IsChecked.Value;
            }));
            if ((bool)o == true)
                App.arm.selarchive.LoadBackupForRestore(selbackup, syncfldrs, changesonly);
            else
                App.arm.selarchive.togglearchivednodes(chkhidearchived.IsChecked ?? false, "Same");

            App.disp.Invoke(new Action(() =>
            {
                foreach (var node in App.arm.selarchive.nodes)
                {
                    TvDirFiles.Items.Add(node);
                }
                Refresh();
            }));
            App.pmon.normalcursor();
            App.goback(this);
        }

        private void collections_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TvDirFiles.Items.Clear();
            tviutil.clear(sliderpnl, slider, (fileitemComp.fileitemsort)sort.SelectedIndex);
            backups.ItemsSource = Backups;
            selbackup = null;
        }
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateLoad())
                return;
            this.chkhidearchived.Click -= new System.Windows.RoutedEventHandler(this.chkhidearchived_Click);
            chkhidearchived.IsChecked = false;
            this.chkhidearchived.Click += new System.Windows.RoutedEventHandler(this.chkhidearchived_Click);
            this.chkchangeonly.Click -= new System.Windows.RoutedEventHandler(this.chkchangeonly_Click);
            chkchangeonly.IsChecked = false;
            this.chkchangeonly.Click += new System.Windows.RoutedEventHandler(this.chkchangeonly_Click);
            IsEnabled = false;
            System.Threading.Thread t = new System.Threading.Thread(Load_Tree);
            t.Start(true);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            outputdir = "";
            string msg = "WARNING\r\nRestoring will overwrite existing files\r\n\r\n" +
                        "Do you want to restore to a different location?\r\n" +
                        "Click Yes for restoring to different location\r\n" +
                        "Click No for restoring to same location\r\n" +
                        "Click Cancel to exit";
            var ret = System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow, msg, "Save Files", MessageBoxButton.YesNoCancel);
            if (ret == MessageBoxResult.Cancel)
                return;
            if (ret == MessageBoxResult.Yes)
            {
                Form topmostWrapper = new Form { TopMost = true };
                var dialog = new System.Windows.Forms.FolderBrowserDialog();
                if (dialog.ShowDialog(topmostWrapper) == System.Windows.Forms.DialogResult.OK)
                {
                    outputdir = dialog.SelectedPath;
                }
                else
                    return;
            }
            App.pmon.busycursor();
            App.bringfront(this);
            IsEnabled = false;
            System.Threading.Thread t = new System.Threading.Thread(Save_files);
            t.Start();
        }
        private void copy_files(string dupsavepath, List<string> srcfiles, List<fileitem> dupleaves, bool boldarchive)
        {
            var writelist =
                (from fi in dupleaves
                 group fi by fi.duplicatefile into g
                 select new
                 {
                     srcfile = dupsavepath + "\\" + ((!boldarchive) ? App.arm.selarchive.changepath(g.Key, false) : g.Key.Replace(':', '_')),
                     items = g.ToList()
                 }).ToList();

            List<string> copyfileslst = new List<string>();
            int k = 0;
            foreach (var sf in srcfiles)
            {
                var osrcfile = dupsavepath + "\\" + ((!boldarchive) ? sf : sf.Replace(':', '_'));
                foreach (var wi in writelist)
                {
                    var srcfile = wi.srcfile;
                    if (osrcfile != srcfile)
                        continue;
                    foreach (var dl in wi.items)
                    {
                        var tgtdir = System.IO.Path.GetDirectoryName(dl._fullPath);
                        if (outputdir != "")
                            tgtdir = outputdir + "\\" + ((!boldarchive) ? App.arm.selarchive.changepath(tgtdir, true) : tgtdir.Replace(':', '_'));
                        var tgtfile = tgtdir + "\\" + System.IO.Path.GetFileName(dl._fullPath);
                        copyfileslst.Add(string.Format("\"{0}\"|\"{1}\"|\"0\"", srcfile, tgtfile));
                        ++k;
                    }
                    copyfileslst.Add(string.Format("\"{0}\"|\"{1}\"|\"1\"", srcfile, srcfile));
                }
            }
            var copyfilespath = App.outputpath + "\\copyfiles.txt";
            var logfilespath = App.outputpath + "\\dupcopy_" + Guid.NewGuid().ToString()+".txt";
            FileEx.WriteAllLines(copyfilespath, copyfileslst);
            App.Escapefile(copyfilespath);
            Process.Start(App.copyfilespath, string.Format("\"dupcopy\" \"{0}\"  {1}  {2}", copyfilespath, k, logfilespath)).WaitForExit();
        }


        private void move_files(List<string> srcfiles, List<string> dstfiles, string srcfolder, bool boldarchive)
        {
            var dirs = App.arm.selarchive.archiveddirs.Select(d => d.Key).ToList();
            var logfilespath = "";
            foreach (var d in dirs)
            {
                var tempdir = d.Replace(':', '_');
                
                if (!boldarchive)
                    tempdir = tempdir.Replace('\\', '_');
                logfilespath = App.outputpath + "\\prepare_" + Guid.NewGuid().ToString() + ".txt";
                Process.Start(App.movefilespath, string.Format("\"{0}\" \"{1}\" \"{2}\" {3}", "prepare", d, srcfolder+"\\"+tempdir, logfilespath)).WaitForExit();

            }

            List<string> copyfileslst = new List<string>();
            for (int k=0; k<srcfiles.Count; ++k)
            {
                copyfileslst.Add(string.Format("\"{0}\"|\"{1}\"", srcfolder+"\\"+srcfiles[k], dstfiles[k]));
            }
            var copyfilespath = App.outputpath + "\\copyfiles.txt";
            FileEx.WriteAllLines(copyfilespath, copyfileslst);
            App.Escapefile(copyfilespath);
            logfilespath = App.outputpath + "\\move_" + Guid.NewGuid().ToString() + ".txt";
            Process.Start(App.movefilespath, string.Format("\"{0}\" \"{1}\" {2} {3}", "move" , copyfilespath, copyfileslst.Count, logfilespath)).WaitForExit();
        }
        private void recreatefolder(string folder)
        {
            App.RemoveDirFile(folder, false);
            DirectoryEx.CreateDirectory(folder);

        }
        public void arrangefilenames(string zipfile,string pwd, string outputfile)
        {
            var svnzfiles = App.outputpath + "\\Temp.txt";
            string cmdline = string.Format("/c \"chcp 65001  & @ECHO Arranging files... &    \"{0}\" l  -ba -p\"{1}\"  -i{2}\"{3}\" -sccUTF-8 \"{4}\" > \"{5}\"\"", App.sevenzpath, pwd, '@', outputfile, zipfile, svnzfiles);
            Process.Start("cmd.exe", cmdline).WaitForExit();
            var svnzfilelst = (from fi in FileEx.ReadAllLines(svnzfiles, System.Text.Encoding.GetEncoding(65001/*437*/)) select fi.Substring(53)).ToList();
            FileEx.WriteAllLines(outputfile, svnzfilelst);
        }

        private void Save_files()
        {
            App.disp.Invoke(new Action(() =>
            {
                App.mainwindow.Topmost = false;
                this.Topmost = false;
            }));
            App.logit("Gathering data to save...");
            System.Diagnostics.Process sevenz = new Process();
            sevenz.StartInfo.FileName = App.zipfilespath;
            sevenz.StartInfo.UseShellExecute = true;
            sevenz.StartInfo.CreateNoWindow = true;
            List<fileitem> itemstowrite = new List<fileitem>();
            foreach (var node in TvDirFiles.Items)
            {
                fileitem parentfi = (fileitem)node;

                App.logit("Adding files from " + parentfi._title + "  ....Please wait");
                List<fileitem> leaves = new List<fileitem>();
                parentfi.getleaves(ref leaves);
                var selleaves = leaves.FindAll(l => (l.Selected == true));
                if (selleaves.Count == 0)
                {
                    App.logit("nothing to save");
                }
                itemstowrite.AddRange(selleaves);
            }

            var writelist = (from fi in itemstowrite
                             group fi by fi.archive into g
                             orderby g.Key
                             select new
                             {
                                 archive = App.archivedir + "\\"+ g.Key,
                                 items = g.ToList()
                             }).ToList();

            var dupsavepath = App.outputpath + "\\temp";

            foreach (var witm in writelist)
            {
                //bool boldarchive = witm.archive.Contains("2021");
                var verpwd = (from bkup in App.arm.selarchive.backups where witm.archive.Contains("\\" + bkup.name + "\\") select new { ver = bkup.version, pwd = bkup.password ?? Microsoft.VisualBasic.Interaction.InputBox("Enter the password", "Save Archive", "", -1, -1) }).ToArray(); 
                bool boldarchive = (verpwd[0].ver== 1);
                var pwd = verpwd[0].pwd;
                if (verpwd[0].ver == 4)
                {
                    int p = witm.archive.IndexOf("\\archive_");
                    if (p != -1)
                    {
                        int idx= (int.Parse(witm.archive.Substring(p + 9, 4))-1)* BackupInfo.base64passwdlen;
                        pwd = pwd.Substring(idx, BackupInfo.base64passwdlen);

                    }
                }
                string outputfile = App.outputpath + "\\writelist.txt";
                Downloadfile dwl = new Downloadfile();

                try
                {
                    var dupleaves = witm.items.FindAll(l => (l.duplicatefile != "" && l.duplicatefile == l._fullPath));
                    var backuproot = System.IO.Path.GetDirectoryName(witm.archive);
                    if (dupleaves.Count != 0)
                    {
                        App.logit("Extracting self duplicate files from " + witm.archive + "  ....Please wait");
                        var dstfiles = dupleaves.Select(fi => fi._fullPath).ToList();
                        FileEx.WriteAllLines(outputfile, dstfiles);
                        if (!boldarchive)
                            App.arm.selarchive.preparetounzip(outputfile,true);
                        
                        if (!App.isclouddrive())
                            arrangefilenames(witm.archive, pwd, outputfile);

                        if (outputdir != "" || (boldarchive && outputdir == ""))
                        {
                            string args = string.Format("{0} \"{1}\" \"{2}\" \"{3}\" \"{4}\"", (((boldarchive && outputdir == "")) ? "unzipsame" : "unzipdiff"), outputdir, witm.archive, outputfile, pwd);
                            dwl.unzipfile(witm.archive,args,ref sevenz);

                            if (sevenz.ExitCode != 0)
                                App.logit("Extracting non  files from " + witm.archive + "  ....failed");
                            else
                                App.logit("Extracting  files from " + witm.archive + "  ....done");

                        }
                        else
                        {
                            var srcfiles = FileEx.ReadAllLines(outputfile, System.Text.Encoding.GetEncoding(65001/*437*/)).Select(fi => fi).ToList();
                            var srcfilelist = (from fi in srcfiles
                                               group fi by fi[0] into g
                                               select new
                                               {
                                                   dstdrive = g.Key,
                                                   items = g.ToList()
                                               }).ToList();


                            foreach (var switm in srcfilelist)
                            {
                                var frd = App.getfreedrive();
                                var zipoutputpath = switm.dstdrive + ":\\" + Guid.NewGuid().ToString();
                                recreatefolder(zipoutputpath);
                                App.mapdrive(frd, zipoutputpath);
                                FileEx.WriteAllLines(outputfile, switm.items);

                                string args = string.Format("unzipdiff \"{0}\" \"{1}\" \"{2}\" \"{3}\"", zipoutputpath, witm.archive, outputfile, pwd);
                                dwl.unzipfile(witm.archive, args, ref sevenz);

                                App.logit("Waiting for copyfiles from " + witm.archive + "  ....");
                                if (sevenz.ExitCode != 0)
                                {
                                    App.logit("Extracting  files from " + witm.archive + "  ....failed");
                                }
                                else
                                {
                                    App.logit("Extracting  files from " + witm.archive + "  ....done");
                                    App.logit("moving files from " + witm.archive + "  ....");
                                    App.arm.selarchive.preparetounzip(outputfile, false);
                                    var dstfiles2 = FileEx.ReadAllLines(outputfile, System.Text.Encoding.GetEncoding(65001/*437*/)).Select(fi => fi).ToList();
                                    move_files(switm.items, dstfiles2, frd, boldarchive);
                                    App.logit("moving files from " + witm.archive + "....done");
                                }
                                App.unmapdrive(frd);
                                App.RemoveDirFile(zipoutputpath, true);
                            }
                        }
                    }

                    dupleaves = witm.items.FindAll(l => (l.duplicatefile != "" && l.duplicatefile != l._fullPath));
                    if (dupleaves.Count > 0)
                    {
                        App.pmon.busycursor();
                        App.bringfront(this);

                        App.logit("Extracting duplicate files from " + witm.archive + "  ....Please wait");
                        FileEx.WriteAllLines(outputfile, (from dl in dupleaves select dl.duplicatefile).Distinct().ToArray());
                        if (!boldarchive)
                            App.arm.selarchive.preparetounzip(outputfile, true);
                        if (!App.isclouddrive())
                            arrangefilenames(witm.archive, pwd, outputfile);
                        var srcfiles = FileEx.ReadAllLines(outputfile, System.Text.Encoding.GetEncoding(65001/*437*/)).Select(fi => fi).ToList();
                        var srcfilelist = (from fi in srcfiles
                                           group fi by fi[0] into g
                                           select new
                                           {
                                               dstdrive = g.Key,
                                               items = g.ToList()
                                           }).ToList();


                        foreach (var switm in srcfilelist)
                        {
                            var frd = App.getfreedrive();
                            var dstdrive = (outputdir != "") ? outputdir[0] : switm.dstdrive;
                            var zipoutputpath = dstdrive + ":\\" + Guid.NewGuid().ToString();
                            recreatefolder(zipoutputpath);
                            App.mapdrive(frd, zipoutputpath);
                            FileEx.WriteAllLines(outputfile, switm.items);


                            string args = string.Format("unzipdiff \"{0}\" \"{1}\" \"{2}\" \"{3}\"", zipoutputpath, witm.archive, outputfile, pwd);
                            dwl.unzipfile(witm.archive, args, ref sevenz);

                            App.logit("Waiting for copyfiles from " + witm.archive + "  ....");
                            if (sevenz.ExitCode != 0)
                            {
                                App.logit("Extracting  files from " + witm.archive + "  ....failed");
                            }
                            else
                            {
                                App.logit("Extracting  files from " + witm.archive + "  ....done");
                                App.logit("copying duplicate files from " + witm.archive + "  ....");
                                copy_files(frd, switm.items, dupleaves, boldarchive);
                                App.logit("copying duplicate files from " + witm.archive + "....done");
                            }
                            App.unmapdrive(frd);
                            App.RemoveDirFile(zipoutputpath, true);
                        }
                    }
                }

                catch (Exception ex)
                {
                    App.logit(ex.Message);
                    App.logit(witm.archive);
                }
                dwl.cleanup();
            }

            App.disp.Invoke(new Action(() =>
            {
                TvDirFiles.Items.Clear();
                Load_Click(null, null);
            }));
        }

        private void backups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selbackup = App.arm.selarchive.find((string)e.AddedItems[0]);
            TvDirFiles.Items.Clear();
            tviutil.clear(sliderpnl, slider, (fileitemComp.fileitemsort)sort.SelectedIndex);
            foreach (var f in selbackup.dirs)
            {
                treenode = new fileitem { _title = f, _fullPath = f, _parent = null, _status = "" };
                TvDirFiles.Items.Add(treenode);
            }
            Load_Click(null, null);
        }

        private void chkhidearchived_Click(object sender, RoutedEventArgs e)
        {
            Load_Tree(false);
        }

        private void chksync_Click(object sender, RoutedEventArgs e)
        {
            Load_Click(null, null);
        }

        private void chkchangeonly_Click(object sender, RoutedEventArgs e)
        {
            this.chkhidearchived.Click -= new System.Windows.RoutedEventHandler(this.chkhidearchived_Click);
            chkhidearchived.IsChecked = false;
            this.chkhidearchived.Click += new System.Windows.RoutedEventHandler(this.chkhidearchived_Click);

            IsEnabled = false;
            System.Threading.Thread t = new System.Threading.Thread(Load_Tree);
            t.Start(true);
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            //IsEnabled="{Binding HasItems , ElementName=TvDirFiles}"
            if (!TvDirFiles.HasItems && !(exportall.IsChecked??false))
                return;
            var outputfile = App.archivedir.Substring(3).Replace(':', '_').Replace('\\', '_') + ".csv";


            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = outputfile;

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
                outputfile = dlg.FileName;
            else
                return;
            
            if (File.Exists(outputfile))
                File.Delete(outputfile);

            Func<fileitem, string, string> fastcrcfunc =  (fileitem fi,string parentpath) =>
            {
                var dt = DateTime.FromFileTime(fi._dateupdated);
                dt = dt.AddSeconds(-dt.Second);
                return (fi.archive.Replace("\\backups\\","") + "|" + parentpath + "|" + fi._fullPath.Replace(parentpath,"") + "|" + (((DateTimeOffset)dt).ToUnixTimeSeconds()*1000).ToString() + "*" + fi._size.ToString());
            };

            Func<fileitem, string, string> crcfunc = (fileitem fi, string parentpath) =>
            {
                return (fi.archive.Replace("\\backups\\", "") + "|" + parentpath + "|" + fi._fullPath.Replace(parentpath, "") + "|" + fi._crc);
            };

            Func<fileitem, string, string> dirfunc = (fileitem fi, string parentpath) =>
            {
                return (fi.archive.Replace("\\backups\\", "") + "|" + parentpath + "|" + Path.GetDirectoryName(fi._fullPath).Replace(parentpath, ""));
            };

            Func<fileitem, string, string> nocrcfunc = (fileitem fi, string parentpath) =>
            {
                return (fi.archive.Replace("\\backups\\", "") + "|" + parentpath + "|" + fi._fullPath.Replace(parentpath, ""));
            };

            List<string> itemstowrite = new List<string>();
            if ((exportdirs.IsChecked ?? false))
            {
                foreach (var node in TvDirFiles.Items)
                {
                    fileitem parentfi = (fileitem)node;

                    App.logit("Adding dirs from " + parentfi._title + "  ....Please wait");
                    List<fileitem> leaves = new List<fileitem>();
                    parentfi.getleaves(ref leaves);
                    if (leaves.Count > 0)
                    {
                        var fp = parentfi._fullPath;
                        if (!fp.EndsWith("\\"))
                            fp += "\\";
                        var selleaves = (leaves.FindAll(l => (l.Selected == true)).Select(l => dirfunc(l, fp))).ToList();
                        if (selleaves.Count == 0)
                        {
                            App.logit("nothing to save");
                        }
                        itemstowrite.AddRange(selleaves.Distinct());
                    }
                }
            }
            else 
            {
                Func<fileitem, string, string> func = nocrcfunc;
                if (exportall.IsChecked ?? false)
                {
                    bool isfastcrc = fastcrc.IsChecked ?? false;

                    if (isfastcrc)
                        func = fastcrcfunc;
                    else
                        func = crcfunc;
                }

                foreach (var node in TvDirFiles.Items)
                {
                    fileitem parentfi = (fileitem)node;

                    App.logit("Adding files from " + parentfi._title + "  ....Please wait");
                    List<fileitem> leaves = new List<fileitem>();
                    parentfi.getleaves(ref leaves);
                    if (leaves.Count > 0)
                    {
                        var fp = parentfi._fullPath;
                        if (!fp.EndsWith("\\"))
                            fp += "\\";
                        var selleaves = (leaves.FindAll(l => (l.Selected == true)).Select(l => func(l, fp))).ToList();
                        if (selleaves.Count == 0)
                        {
                            App.logit("nothing to save");
                        }
                        itemstowrite.AddRange(selleaves);
                    }
                }
            }
            FileEx.WriteAllLines(outputfile, itemstowrite);
        }

        private void unit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fileitem.sel = 1 * (Math.Pow(1000.0, (unit.SelectedIndex + 1.0)));
            Refresh();

        }
    }

    public class ChkStatusConverterRestore : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((((string)value == "Same") || ((string)value == "") || ((string)value == "Status")) ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

}
