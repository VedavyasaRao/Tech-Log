using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Media;

namespace FileOrganiser.Compare
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        public fileitem root;
        public fileitem srcroot;
        public fileitem tgtroot;
        public List<Tuple<fileitem, List<fileitem>>> duplist;
        bool bshowdup = true;
        bool bshownondup = true;
        bool bHierarchical = true;
        public Window3()
        {
            InitializeComponent();
        }


        void BuildTree()
        {
            driver.logit("Building tree .... please wait");
            driver.pmon.initpbar(duplist.Count);

            var newduplist = new List<Tuple<fileitem, List<fileitem>>>();
            if (bshowdup )
            {
                newduplist.AddRange((from tp in duplist where tp.Item2.Count > 0 select tp).ToList());
            }

            if (bshownondup)
            {
                newduplist.AddRange((from tp in duplist where tp.Item2.Count == 0 select tp).ToList());
            }

            Dictionary<string, fileitem> parentnodes = new Dictionary<string, fileitem>();
            root = new fileitem { _title = srcroot._fullPath, _fullPath = srcroot._fullPath, _parent = null };
            root.Items.Add(driver.tviutil.dummy);
            if (bHierarchical)
            {
                foreach (var tp in newduplist)
                {
                    var fic = tp.Item1;
                    fic._title = Path.GetFileName(fic._fullPath);
                    var parts = fic._fullPath.Replace(srcroot._fullPath + "\\", "").Split(new char[] { '\\' });
                    var ppn = root;
                    var pp = root._fullPath;
                    for (var i = 0; i < parts.Length - 1; ++i)
                    {
                        pp += "\\" + parts[i];
                        if (!parentnodes.ContainsKey(pp))
                        {
                            var tempnode = new fileitem { _title = parts[i], _fullPath = pp, _parent = ppn };
                            tempnode.Items.Add(driver.tviutil.dummy);
                            ppn._Items.Add(tempnode);
                            parentnodes.Add(pp, tempnode);
                        }
                        ppn = parentnodes[pp];
                    }
                    ppn._Items.Add(fic);
                    fic._parent = ppn;
                    driver.pmon.updatepbar();
                }
                driver.logit("updating parents .... please wait");
                driver.pmon.initpbar(duplist.Count);
                foreach (var tp in newduplist)
                {
                    var fic = tp.Item1;
                    if (fic._Items.Count == 0)
                        continue;
                    fic._size += (fic._size * fic._Items.Count);
                    var ppn = fic._parent;
                    while (ppn != null)
                    {
                        ppn._dupcount += fic._Items.Count;
                        ppn._size += fic._size;
                        ppn = ppn._parent;
                    }
                    driver.pmon.updatepbar();
                }
            }
            else
            {
                var temps = root._fullPath + "\\";
                foreach (var tp in newduplist)
                {
                    var fic = tp.Item1;
                    fic._title = fic._fullPath.Replace(temps,"");
                    root._Items.Add(fic);
                    root._dupcount += fic._Items.Count;
                    root._size += fic._size;
                    driver.pmon.updatepbar();
                }

            }
            driver.logit("Building tree .... done");
            driver.pmon.closebar();

        }
        public void Load()
        {
            BuildTree();
            driver.disp.Invoke(new Action(() => {
                TvDirFiles.Items.Clear();
                TvDirFiles.Items.Add(new fileitem { _title = "Files", Color=Brushes.Coral, _dupcount=-1, _size=-1, _parent = null });
                TvDirFiles.Items.Add(root);
                Refresh();
                driver.goback(this);
            }));
        }


        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var fi = TvDirFiles.SelectedValue as fileitem;

            if (fi == null || fi._Items.Count == 0)
                return;
            fi.toggleselected(((System.Windows.Controls.CheckBox)e.Source).IsChecked);
        }


        void cpynodes(object data)
        {
            var nodes = (List<string>)((object[])data)[0];
            var tgtfldr = (string)((object[])data)[1];


            driver.pmon.busycursor();

            foreach (var node in (List < string > )nodes)
            {
                try
                {
                    driver.logit("copying ..." + node);
                    var tgtfile = tgtfldr + "\\" + Path.GetFileName(node);
                    System.IO.File.Copy(node,tgtfile,true);
                }
                catch(Exception ex)
                {
                    driver.logit(ex.Message);
                }
            }
            driver.pmon.normalcursor();

        }
        private void Exp_Click(object sender, RoutedEventArgs e)
        { 
            driver.logit("Exporting .... please wait");
            driver.pmon.initpbar(duplist.Count);

            var newduplist = new List<Tuple<fileitem, List<fileitem>>>();
            if (bshowdup )
            {
                newduplist.AddRange((from tp in duplist where tp.Item2.Count > 0 select tp).ToList());
            }

            if (bshownondup)
            {
                newduplist.AddRange((from tp in duplist where tp.Item2.Count == 0 select tp).ToList());
            }

            string exportfile = driver.outputpath + "\\" + srcroot._fullPath.Replace(':', '_').Replace('\\', '_')+","+tgtroot._fullPath.Replace(':', '_').Replace('\\', '_') + ".csv";
            if (File.Exists(exportfile))
                File.Delete(exportfile);

            foreach (var tp in newduplist)
            {
                var fic = tp.Item1;
                if (fic._Items.Count>0)
                {
                    foreach (var ficc in fic._Items)
                    {
                        File.AppendAllText(exportfile, string.Format("{0}|{1}|{2}|{3}\r\n", fic._fullPath, fic._md5, ficc._fullPath,ficc._md5));
                    }
                }
                else
                    File.AppendAllText(exportfile, string.Format("{0}|{1}|\r\n", fic._fullPath,fic._md5));
                driver.pmon.updatepbar();
            }
            driver.pmon.closebar();
            driver.logit("Exporting .... done");
        }
        private void Cpy_Click(object sender, RoutedEventArgs e)
        {
            var fi = TvDirFiles.SelectedValue as fileitem;

            if (fi == null || fi._Items.Count == 0)
                return;

            List<string> nodes = new List<string>();
            fi.getselected2(ref nodes);
            if (nodes.Count == 0)
                return;

            var copytodir = "";
            if (System.Windows.MessageBox.Show("Do you really want to copy selected files?", "Delete", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                copytodir = dialog.SelectedPath;
            }



            System.Threading.Thread t = new System.Threading.Thread(cpynodes);
            t.Start(new object[] { nodes, copytodir });

        }

        

        private void TreeView_OnCollapsed(object sender, RoutedEventArgs e)
        {
            fileitem fi = (fileitem)((TreeViewItem)e.OriginalSource).Header;
            if (fi == null)
                return;
            driver.tviutil.closenode(fi);
        }
        private void TreeView_OnExpanded(object sender, RoutedEventArgs e)
        {
            fileitem fi = (fileitem)((TreeViewItem)e.OriginalSource).Header;
            if (fi == null)
                return;
            driver.tviutil.expandnode(fi);
        }

        private void TvDirFiles_Selected(object sender, RoutedEventArgs e)
        {

            TreeViewItem tvi = e.OriginalSource as TreeViewItem;
            if (tvi == null)
                return;

            if (!tvi.IsExpanded)
                return;
            var fi = (fileitem)TvDirFiles.SelectedValue;
            driver.tviutil.selectnode(fi);
        }

        private void slider_DragCompleted(object sender, RoutedEventArgs e)
        {
            var fi = (fileitem)TvDirFiles.SelectedValue;
            driver.tviutil.dragslider(fi);
        }

        private void TvDirFiles_LostFocus(object sender, RoutedEventArgs e)
        {
            driver.tviutil.lostfocus();
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


        private void Window_Closing(object sender, CancelEventArgs e)
        {
            System.Configuration.Configuration config =
                                      ConfigurationManager.OpenExeConfiguration(
                                      ConfigurationUserLevel.None);

            config.AppSettings.Settings.Remove("cmpwd");
            config.AppSettings.Settings.Add("cmpwd", width.Text);

            config.AppSettings.Settings.Remove("cmpwwd");
            config.AppSettings.Settings.Add("cmpwwd", this.Width.ToString());

            config.AppSettings.Settings.Remove("cmpsort");
            config.AppSettings.Settings.Add("cmpsort", sort.SelectedIndex.ToString());

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Configuration.Configuration config =
                                      ConfigurationManager.OpenExeConfiguration(
                                      ConfigurationUserLevel.None);

            if (config.AppSettings.Settings["cmpwd"] != null)
                width.Text = config.AppSettings.Settings["cmpwd"].Value;

            if (config.AppSettings.Settings["cmpwwd"] != null)
                this.Width = double.Parse(config.AppSettings.Settings["cmpwwd"].Value);

            if (config.AppSettings.Settings["cmpsort"] != null)
                sort.SelectedIndex = int.Parse(config.AppSettings.Settings["cmpsort"].Value);

            System.Threading.Thread t = new System.Threading.Thread(Load);
            t.Start();
        }

        private void Refresh()
        {
            if (TvDirFiles == null)
                return;
            SortConverter.sel = sort.SelectedIndex;
            TvDirFiles.Items.Refresh();
            driver.tviutil.clear(sliderpnl, slider, (fileitemComp.fileitemsort)sort.SelectedIndex);
            if (driver.root != null)
                driver.root.updatesorted();


        }
        private void sort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void unit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fileitem.sel = 1 * (Math.Pow(1000.0, (unit.SelectedIndex + 1.0)));
            Refresh();
        }

        private void chkshowdup_Click(object sender, RoutedEventArgs e)
        {
            bshowdup = chkshowdup.IsChecked ?? false;
            Load();

        }

        private void chkshownondup_Click(object sender, RoutedEventArgs e)
        {
            bshownondup = chkshownondup.IsChecked ?? false;
            Load();
        }

        private void chkexpclpall_Click(object sender, RoutedEventArgs e)
        {
            bHierarchical = chkexpclpall.IsChecked ?? false;
            Load();
        }

    }

    public class ChkSelctionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((((bool)value==false)) ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    public class SortConverter : IValueConverter
    {
        public static int sel = 2;
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
                view.SortDescriptions.Add(new SortDescription("_size", ListSortDirection.Descending));
            }
            else if (sel == 2)
            {
                view.SortDescriptions.Add(new SortDescription("_dupcount", ListSortDirection.Descending));
            }
            return view;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

}


