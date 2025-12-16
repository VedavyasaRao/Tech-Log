using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BackupRestoreTool
{
    /// <summary>
    /// Interaction logic for BackupVW.xaml
    /// </summary>
    public partial class BackupVW : Window
    {
        TVItmUtil tviutil = new TVItmUtil();
        string outputfile = App.outputpath + "\\targetfile.txt";


        bool started;
        public BackupVW()
        {
            InitializeComponent();
            App.arm.mappeddirs = new List<Tuple<string, string>>();

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

        private bool ValidateLoad()
        {
            if (App.arm.selarchive  == null)
            {
                App.logit("Nothing to backup");
                return false;
            }
            return true;
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateLoad())
                return;
            this.chkhidearchived.Click -= new System.Windows.RoutedEventHandler(this.chkhidearchived_Click);
            chkhidearchived.IsChecked = true;
            this.chkhidearchived.Click += new System.Windows.RoutedEventHandler(this.chkhidearchived_Click);
            System.Threading.Thread t = new System.Threading.Thread(Load_Tree);
            t.Start(true);
        }

        private void Load_Tree(object o)
        {
            App.pmon.busycursor();
            App.bringfront(this);

            App.disp.Invoke(new Action(() =>
            {
                TvDirFiles.Items.Clear();
            }));
            if ((bool)o == true)
                App.arm.selarchive.LoadArchiveForBackup();
            else
                App.arm.selarchive.togglearchivednodes(!chkhidearchived.IsChecked??false, "Archived");

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
            Load_Click(null, null);
        }

        #region treeview
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

        #endregion

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var fi = ((fileitem)((System.Windows.FrameworkElement)(((System.Windows.FrameworkContentElement)(((CheckBox)e.OriginalSource).Parent)).Parent)).DataContext);
            if (fi == null || fi._Items.Count == 0)
                return;
            fi.toggleselected(((CheckBox)e.Source).IsChecked,false);

        }


        private void collections_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            TvDirFiles.Items.Clear();
            tviutil.clear(sliderpnl, slider, (fileitemComp.fileitemsort)sort.SelectedIndex);
            foreach (var f in App.arm.selarchive.selecteddirs())
            {
                var treenode = new fileitem { _title = f, _fullPath = f, _parent = null,_status=""};
                TvDirFiles.Items.Add(treenode);
            }
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
            }));
            App.logit("Updating integrated view .... done");
            App.pmon.normalcursor();
            App.goback(this);
        }

        private void chkhidearchived_Click(object sender, RoutedEventArgs e)
        {
            Load_Tree(false);
        }

        private void unit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fileitem.sel = 1 * (Math.Pow(1000.0, (unit.SelectedIndex + 1.0)));
            Refresh();

        }

    }

    public class ChkStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (  (((string)value == "Archived" || (string)value == "" || (string)value == "Status")) ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    public class SortConverter : IValueConverter
    {
        public static int sel = 0;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Collections.IList collection = value as System.Collections.IList;
            ListCollectionView view = new ListCollectionView(collection);
            if (sel == 0)
            {
                view.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
            }
            else if (sel == 1)
            {
                view.SortDescriptions.Add(new SortDescription("_count", ListSortDirection.Descending));
            }
            else if (sel == 2)
            {
                //view.SortDescriptions.Add(new SortDescription("_size", ListSortDirection.Ascending));
            }
            else if (sel == 3)
            {
                view.SortDescriptions.Add(new SortDescription("_status", ListSortDirection.Ascending));
            }
            return view;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

}
