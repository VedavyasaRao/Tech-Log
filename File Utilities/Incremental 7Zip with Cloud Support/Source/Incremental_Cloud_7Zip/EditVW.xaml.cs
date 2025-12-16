using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;

using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BackupRestoreTool
{
    /// <summary>
    /// Interaction logic for RestoreVW.xaml
    /// </summaryBackupstoRestore
    public partial class EditVW : Window
    {
        TVItmUtil tviutil = new TVItmUtil();
        fileitem treenode;
        BackupInfo selbackup = null;
        bool started;
        string outputfile = App.outputpath + "\\targetfile.txt";
        string exportfile = App.outputpath + "\\exportednodes.txt";

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
            if (App.arm.selarchive == null)
            {
                App.logit("no collection selected");
                return false;
            }
            if (backups.SelectedIndex == -1)
            {
                App.logit("no backups selected for the collection");
                return false;
            }
            return true;
        }

        public EditVW()
        {
            InitializeComponent();
            App.arm.mappeddirs = new List<Tuple<string, string>>();
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
            App.arm.statp += StartShowprogress;
            App.arm.stopp += StopShowprogress;

            TvDirFiles.Items.Clear();

            if (config.AppSettings.Settings["dirszsort"] != null)
                sort.SelectedIndex = int.Parse(config.AppSettings.Settings["dirszsort"].Value);

            if (config.AppSettings.Settings["dirszwd"] != null)
                width.Text = config.AppSettings.Settings["dirszwd"].Value;

            if (config.AppSettings.Settings["dirszwwd"] != null)
                this.Width = double.Parse(config.AppSettings.Settings["dirszwwd"].Value);
            collections_SelectionChanged(null, null);
            backups.SelectedIndex = Backups.Count - 1;
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

            //if (!tvi.IsExpanded)
            //    return;
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

            var fi = ((fileitem)((System.Windows.FrameworkElement)(((System.Windows.FrameworkContentElement)(((CheckBox)e.OriginalSource).Parent)).Parent)).DataContext);


            if (fi == null || fi._Items.Count == 0)
                return;
            fi.toggleselected(((CheckBox)e.Source).IsChecked, false);

        }

        private void Load_Tree(object o)
        {
            App.pmon.busycursor();
            App.bringfront(this);

            App.disp.Invoke(new Action(() =>
            {
                TvDirFiles.Items.Clear();
            }));
            if ((bool)o)
                App.arm.selarchive.LoadBackupForRestore(selbackup, false,false);
            else
                App.disp.Invoke(new Action(() =>
                {
                    App.arm.selarchive.togglearchivednodes(!chkhidearchived.IsChecked ?? false, "Archived");
                }));
           
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
            IsEnabled = false;
            this.chkhidearchived.Click -= new System.Windows.RoutedEventHandler(this.chkhidearchived_Click);
            chkhidearchived.IsChecked = true;
            this.chkhidearchived.Click += new System.Windows.RoutedEventHandler(this.chkhidearchived_Click);

            System.Threading.Thread t = new System.Threading.Thread(Load_Tree);
            t.Start(true);
        }

        private void Backup_Now_Click(object sender, RoutedEventArgs e)
        {
            App.pmon.busycursor();
            App.bringfront(this);
            IsEnabled = false;
            System.Threading.Thread t = new System.Threading.Thread(Zip_files);
            t.Start();

        }
        public void Zip_files()
        {
            App.pmon.busycursor();
            App.bringfront(this);

            App.disp.Invoke(new Action(() =>
            {
                TvDirFiles.Items.Clear();
                App.mainwindow.Topmost = false;
                this.Topmost = false;
            }));


            App.arm.selarchive.Zip_files();
            App.logit("Updating integrated view....");
            App.disp.Invoke(new Action(() =>
            {
                foreach (var node in App.arm.selarchive.nodes)
                {
                    TvDirFiles.Items.Add(node);
                }
                Refresh();
                collections_SelectionChanged(null, null);
                backups.SelectedIndex = Backups.Count - 1;
            }));
            App.logit("Updating integrated view .... done");
            App.pmon.normalcursor();
            App.goback(this);
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

        private fileitem findmatch(fileitem parentnode, fileitem newnode)
        {
            foreach (var itm in parentnode.Items)
            {
                if (itm._title.Equals(newnode._title, StringComparison.InvariantCultureIgnoreCase))
                    return itm;
            }

            return null;
        }
        private fileitem findmatch_(fileitem parentnode, fileitem newnode)
        {
            foreach (var itm in parentnode._Items)
            {
                if (itm._title.Equals(newnode._title, StringComparison.InvariantCultureIgnoreCase))
                    return itm;
            }

            return null;
        }

        private void updatefiledir(fileitem srcnode, string fldrname)
        {
            int fldrnamelen = fldrname.Length;
            foreach (var sitm in srcnode._Items)
            {
                if (sitm.isfile)
                {
                    sitm._fullPath = fldrname + sitm._fullPath.Substring(fldrnamelen);
                }
                else
                {
                    updatefiledir(sitm, fldrname);
                }
            }
        }

        private void mergenodes(fileitem srcnode, fileitem tgtnode)
        {
            foreach (var sitm in srcnode._Items)
            {
                if (!sitm.isfile)
                    continue;
                var titm = findmatch(tgtnode, sitm);
                if (titm != null)
                {
                    sitm._fullPath = titm._fullPath;
                    App.disp.Invoke(new Action(() =>
                    {
                        tgtnode.Items.Remove(titm);
                        tgtnode._Items.Remove(findmatch_(tgtnode, sitm));
                        tgtnode._status = "Changed";
                    }));
                }
            }

            string fldrname = tgtnode._fullPath;
            int fldrnamelen = fldrname.Length;
            foreach (var sitm in srcnode._Items)
            {
                if (!sitm.isfile)
                    continue;
                sitm._fullPath = fldrname + sitm._fullPath.Substring(fldrnamelen);
                sitm._parent = tgtnode;
                App.disp.Invoke(new Action(() =>
                {
                    tgtnode.Items.Add(sitm);
                    tgtnode._Items.Add(sitm);
                    tgtnode._status = "Changed";
                }));
            }

            foreach (var sitm in srcnode._Items)
            {
                if (sitm.isfile)
                    continue;
                var titm = findmatch(tgtnode, sitm);
                if (titm != null)
                {
                    mergenodes(sitm, titm);
                    App.disp.Invoke(new Action(() =>
                    {
                        tgtnode._status = "Changed";
                    }));
                }
                else
                {
                    updatefiledir(sitm, fldrname);
                    sitm._parent = tgtnode;
                    App.disp.Invoke(new Action(() =>
                    {
                        tgtnode.Items.Add(sitm);
                        tgtnode._Items.Add(sitm);
                        tgtnode._status = "Changed";
                    }));
                }
            }
        }

        void updatelocmapdir(List<Tuple<string, string>> oldlocmap, List<string> filelist, List<Tuple<string, string>> locmap)
        {
            foreach(var kv in oldlocmap)
            {
                foreach (var fn in filelist)
                {
                    if (kv.Item1.Equals(fn, StringComparison.InvariantCultureIgnoreCase))
                    {
                        locmap.Add(new Tuple<string, string>(fn, kv.Item2));
                    }
                }
            }
        }

        private void Load_Folder(object o)
        {
            App.pmon.busycursor();
            App.bringfront(this);
            var selpath = "";
            var selectednode = (fileitem)((object[])o)[0];
            var selectedpath = (string[])((object[])o)[1];
            var selnode = selectednode;
            while (selnode != null)
            {
                selpath = selnode._title + "\\" + selpath;
                selnode = selnode._parent;
            }

            var locmap = new List<Tuple<string, string>>();
            var newnode = App.arm.selarchive.AddNode(selectedpath, selpath.Substring(0, selpath.Length - 1),ref locmap);
            mergenodes(newnode, selectednode);
            List<fileitem> filelist = new List<fileitem>();
            selectednode.getleaves(ref filelist);
            var locmap2 = new List<Tuple<string, string>>();
            updatelocmapdir(locmap, (from fi in filelist select fi._fullPath).ToList(), locmap2);
            App.arm.mappeddirs.AddRange(locmap2);
            App.disp.Invoke(new Action(() =>
            {
                selnode = selectednode;
                while (selnode != null)
                {
                    selnode._status = "Changed";
                    selnode = selnode._parent;
                }
                Refresh();
            }));
            savenodes();


            App.pmon.normalcursor();
            App.goback(this);
        }

        #region pogressbar
        void StartShowprogress()
        {
            started = true;
            System.Threading.Thread t = new System.Threading.Thread(showLoadprogress);
            t.Start();
        }
        void StopShowprogress()
        {
            started = false;
            App.pmon.normalcursor();
        }

        void showLoadprogress()
        {
            int i = 0;
            int j = 0;
            App.pmon.initpbar(100);
            while (started)
            {
                var fi =  FileInfoEx.FileInfo(outputfile);
                if (fi.Exists)
                {
                    fi.Refresh();
                    j = (int)(fi.Length / 104857.6);
                }
                if (j > i)
                {
                    i = j;
                    App.pmon.updatepbar();
                }
                System.Threading.Thread.Sleep(2000);
            }
            App.pmon.closebar();
        }
        #endregion

        private void TvDirFiles_Drop(object sender, DragEventArgs e)
        {
            fileitem fi = ((System.Windows.Controls.TreeView)e.Source).SelectedItem as fileitem;
            if (fi == null || fi.isfile)
                return;

            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                var maploc = ((string[])(e.Data.GetData(DataFormats.FileDrop, false)));
                var ret = MessageBoxResult.No;
                App.Current.Dispatcher.Invoke(() =>
                {
                    ret = MessageBox.Show(Application.Current.MainWindow, "Do you want to add selected file(s) to  " + fi._fullPath + "?\r\nWARNING: Existing files will be overwritten", "Edit", MessageBoxButton.YesNo);
                });
                if (ret == MessageBoxResult.No)
                    return;
                System.Threading.Thread t = new System.Threading.Thread(Load_Folder);
                t.Start(new object[] { fi, maploc });
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var fi = (fileitem)TvDirFiles.SelectedValue;
            if (fi == null || fi.isfile)
                return;

            var folder = Microsoft.VisualBasic.Interaction.InputBox("Enter the folder name", "Save files", "New Folder", -1, -1);
            if (folder == "")
                return;

            var tempnode = new fileitem { _title = folder, _fullPath = fi._fullPath + "\\" + folder, _parent = fi,isfile=false };
            App.disp.Invoke(new Action(() =>
            {
                fi.Items.Add(tempnode);
                fi._Items.Add(tempnode);
                var selnode = fi;
                while (selnode != null)
                {
                    selnode._status = "Changed";
                    selnode = selnode._parent;
                }
                Refresh();
            }));
            savenodes();
        }

        void savenodes()
        {
            App.disp.Invoke(new Action(() =>
            { 
                List < fileitem> nodes = new List<fileitem>();
                foreach (var node in TvDirFiles.Items)
                {
                    nodes.Add((fileitem)node);
                }
                App.arm.Serialize(nodes, ref App.arm.Khrishadat);
            }));

        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            var fi = (fileitem)TvDirFiles.SelectedValue;
            if (fi == null )
                return;
            if (MessageBox.Show(Application.Current.MainWindow, "Do you want to delete " + fi._fullPath + "?", "Edit", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;
            var parentnode = fi._parent;
            App.disp.Invoke(new Action(() =>
            {
                parentnode.Items.Remove(fi);
                foreach (var itm in parentnode._Items)
                {
                    if (itm._fullPath == fi._fullPath && itm._crc == fi._crc)
                    {
                        parentnode._Items.Remove(itm);
                        break;
                    }
                }
                Refresh();
            }));
            savenodes();

        }

        private void unit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fileitem.sel = 1 * (Math.Pow(1000.0, (unit.SelectedIndex + 1.0)));
            Refresh();

        }

        private void chkhidearchived_Click(object sender, RoutedEventArgs e)
        {
            Load_Tree(false);
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            
            App.logit("Gathering data to export...");
            App.arm.selarchive.Export_files(exportfile);
        }
    }

}
