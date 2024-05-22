using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Media;

namespace FileOrganiser.Duplicates
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window4 : Window
    {
        public fileitem root;
        public fileitem srcroot;
        public string targetfolder;
        public List<Tuple<fileitem, List<fileitem>>> duplist;
        bool bshowdup = true;
        bool bshownondup = false;
        bool bHierarchical = true;
        public Window4()
        {
            InitializeComponent();
        }


        void BuildTree()
        {
            driver.logit("Building tree .... please wait");

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
            if (newduplist.Count == 0)
                return;
            if (bHierarchical)
            {
                driver.pmon.initpbar(newduplist.Count);
                foreach (var tp in newduplist)
                {
                    var fic = tp.Item1;
                    fic._title = System.IO.Path.GetFileName(fic._fullPath);
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
                driver.pmon.closebar();

                driver.logit("updating parents .... please wait");
                driver.pmon.initpbar(newduplist.Count);
                foreach (var tp in newduplist)
                {
                    var fic = tp.Item1;
                    if (fic._Items.Count > 0)
                    {
                        var ppn = fic._parent;
                        var fic_size = (fic._size * fic._Items.Count);
                        while (ppn != null)
                        {
                            ppn._dupcount += fic._Items.Count;
                            ppn._size += fic_size;
                            ppn = ppn._parent;
                        }
                    }
                    driver.pmon.updatepbar();
                }
                driver.pmon.closebar();
                driver.logit("updating parents .... done");
            }
            else
            {
                driver.pmon.initpbar(newduplist.Count);
                var temps = root._fullPath + "\\";
                root._size = 0;
                foreach (var tp in newduplist)
                {
                    var fic = tp.Item1;
                    var fic_size = (fic._size * fic._Items.Count);
                    fic._title = fic._fullPath.Replace(temps,"");
                    root._Items.Add(fic);
                    if (fic._Items.Count > 0)
                    {
                        root._dupcount += fic._Items.Count;
                        root._size += fic_size;
                    }
                    driver.pmon.updatepbar();
                }
                driver.pmon.closebar();

            }
            driver.logit("Building tree .... done");

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


        private void Exp_Click(object sender, RoutedEventArgs e)
        {
            string exportfile = driver.outputpath + "\\" + srcroot._fullPath.Replace(':', '_').Replace('\\', '_');
            if (!String.IsNullOrEmpty(targetfolder))
                exportfile += ("," + targetfolder.Replace(':', '_').Replace('\\', '_'));
            exportfile += ".csv";

            driver.logit("Exporting "+ exportfile + "   .... please wait");
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

            if (FileEx.Exists(exportfile))
                FileEx.Delete(exportfile);

            foreach (var tp in newduplist)
            {
                var fic = tp.Item1;
                if (fic._Items.Count>0)
                {
                    foreach (var ficc in fic._Items)
                    {
                        FileEx.AppendAllText(exportfile, string.Format("{0}|{1}|{2}|{3}\r\n", fic._fullPath, fic._md5, ficc._fullPath,ficc._md5));
                    }
                }
                else
                    FileEx.AppendAllText(exportfile, string.Format("{0}|{1}|\r\n", fic._fullPath,fic._md5));
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


            System.Threading.Thread t = new System.Threading.Thread(cpydelnodes);
            t.Start(new object[] { nodes, copytodir, true });

            //System.Threading.Thread t = new System.Threading.Thread(cpynodes);
            //t.Start(new object[] { nodes, copytodir });

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
            if (!string.IsNullOrEmpty(targetfolder))
            {
                Title = "Compare Folders View";
            }
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
        private void Del_Click(object sender, RoutedEventArgs e)
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

            System.Threading.Thread t = new System.Threading.Thread(cpydelnodes);
            t.Start(new object[] { nodes, "", false });

            //System.Threading.Thread t = new System.Threading.Thread(cpynodes);
            //t.Start(new object[] { nodes, copytodir });
        }


        void cpydelnodes(object data)
        {
            var nodes = (List<string>)((object[])data)[0];
            var tgtfldr = (string)((object[])data)[1];
            var bcpy = (bool)((object[])data)[2];


            driver.pmon.busycursor();

            foreach (var node in (List<string>)nodes)
            {
                try
                {
                    if (bcpy)
                    {
                        driver.logit("copying ..." + node);
                        var tgtfile = tgtfldr + "\\" + System.IO.Path.GetFileName(node);
                        FileEx.Copy(node, tgtfile, true);
                    }
                    else
                    {
                        driver.logit("deleting ..." + node);
                        FileEx.Delete(node);
                    }
                }
                catch (Exception ex)
                {
                    driver.logit(ex.Message);
                }
            }
            driver.pmon.normalcursor();

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


