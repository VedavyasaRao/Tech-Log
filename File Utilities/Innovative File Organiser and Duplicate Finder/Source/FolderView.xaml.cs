using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Media;

namespace FileOrganiser.Treeview
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public string srcfldr;

        public Window1()
        {
            InitializeComponent();
        }

        private void unit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fileitem.sel = 1 * (Math.Pow(1000.0, (unit.SelectedIndex + 1.0)));
            Refresh();
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
            driver.tviutil.clear(sliderpnl, slider, (fileitemComp.fileitemsort)sort.SelectedIndex);
            if (driver.root != null)
                driver.root.updatesorted();

        }

        public void LoadSize(object parentfi2)
        {
            try
            {
                fileitem parentfi = parentfi2 as fileitem;
                List<fileitem> leaves = new List<fileitem>();
                parentfi.getleaves(ref leaves);
                driver.CorrectFilenames(leaves);
                driver.logit("Updating file items .... please wait");
                driver.pmon.initpbar(leaves.Count);
                foreach (var fi in leaves)
                {
                    double sz;
                    try
                    {
                        fi.isdup = true;
                        System.IO.FileInfo finf = FileInfoEx.FileInfo(fi._fullPath);
                        sz = finf.Length;
                        fileitem fic = fi;
                        while (fic != null)
                        {
                            fic._size += sz;
                            if (!fic.isfile)
                                fic._count++;
                            fic = fic._parent;
                        }
                    }
                    catch (Exception ex)
                    {
                        driver.logit(ex.Message);
                    }

                    driver.pmon.updatepbar();

                }
                driver.pmon.closebar();

                driver.logit("Updating file items .... done");
                driver.pmon.closebar();
                driver.goback(this);
                driver.disp.Invoke(new Action(() => {
                    Refresh();
                }));

            }

            catch (Exception ex)
            {
                driver.logit(ex.Message);
            }
        }

        public void Load()
        {
            string parentpath = srcfldr;
            driver.bringfront(this);
            LoadSize(driver.root);
            driver.disp.Invoke(new Action(() => {
                TvDirFiles.Items.Clear();
                TvDirFiles.Items.Add(new fileitem { _title = "Files", Color = Brushes.Coral, _count = -1, _size = -1, _parent = null });
                TvDirFiles.Items.Add(driver.root);
                Refresh();
            }));
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            System.Configuration.Configuration config =
                           ConfigurationManager.OpenExeConfiguration(
                           ConfigurationUserLevel.None);

            config.AppSettings.Settings.Remove("dirszunit");
            config.AppSettings.Settings.Add("dirszunit", unit.SelectedIndex.ToString());

            config.AppSettings.Settings.Remove("dirszsort");
            config.AppSettings.Settings.Add("dirszsort", sort.SelectedIndex.ToString());

            config.AppSettings.Settings.Remove("dirszwd");
            config.AppSettings.Settings.Add("dirszwd", width.Text);

            config.AppSettings.Settings.Remove("dirszwwd");
            config.AppSettings.Settings.Add("dirszwwd", this.Width.ToString());

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Configuration.Configuration config =
                           ConfigurationManager.OpenExeConfiguration(
                           ConfigurationUserLevel.None);

            if (config.AppSettings.Settings["dirszunit"] != null)
                unit.SelectedIndex = int.Parse(config.AppSettings.Settings["dirszunit"].Value);

            if (config.AppSettings.Settings["dirszsort"] != null)
                sort.SelectedIndex = int.Parse(config.AppSettings.Settings["dirszsort"].Value);

            if (config.AppSettings.Settings["dirszwd"] != null)
                width.Text= config.AppSettings.Settings["dirszwd"].Value;

            if (config.AppSettings.Settings["dirszwwd"] != null)
                this.Width = double.Parse(config.AppSettings.Settings["dirszwwd"].Value);

            System.Threading.Thread t = new System.Threading.Thread(Load);
            t.Start();
        }

        void delnodes(object node)
        {
            string target = node as string;
            driver.pmon.busycursor();
            try
            {
                if (DirectoryEx.Exists(target))
                    DirectoryEx.Delete(target, true);
                else
                    FileEx.Delete(target);
            }
            catch (Exception ex)
            {
                driver.logit(ex.Message);
            }
            driver.pmon.normalcursor();

        }



        private void Del_Click(object sender, RoutedEventArgs e)
        {

            List<string> nodes = new List<string>();
            driver.root.getselected(ref nodes);
            if (nodes.Count == 0)
                return;

            var copytodir = "";
            if (System.Windows.MessageBox.Show("Do you really want to delete selected files?", "Delete", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;
            System.Threading.Thread t = new System.Threading.Thread(cpydelnodes);
            t.Start(new object[] { nodes, copytodir, false });
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            List<string> nodes = new List<string>();
            driver.root.getselected(ref nodes);
            if (nodes.Count == 0)
                return;

            var copytodir = "";
            if (System.Windows.MessageBox.Show("Do you really want to copy selected files?", "Copy", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                copytodir = dialog.SelectedPath;
            }
            else
                return;


            System.Threading.Thread t = new System.Threading.Thread(cpydelnodes);
            t.Start(new object[] { nodes, copytodir,true });
        }
        private void Export_Click(object sender, RoutedEventArgs e)
        {
            var fi = TvDirFiles.SelectedValue as fileitem;

            if (fi == null || fi._Items.Count == 0)
                return;

            string exportfile = driver.outputpath + "\\" + fi._fullPath.Replace(':', '_').Replace('\\', '_')+"_folders.csv";
            if (FileEx.Exists(exportfile))
                FileEx.Delete(exportfile);
            driver.logit("Exporting to " + exportfile);
            System.Threading.Thread t = new System.Threading.Thread(exportnodes);
            t.Start(new object[] { fi,exportfile});

        }
        private void TreeView_OnCollapsed(object sender, RoutedEventArgs e)
        {
            fileitem fi = (fileitem)((TreeViewItem)e.OriginalSource).Header;
            driver.tviutil.closenode(fi);
        }
        private void TreeView_OnExpanded(object sender, RoutedEventArgs e)
        {
            fileitem fi = (fileitem)((TreeViewItem)e.OriginalSource).Header;
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

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var fi = TvDirFiles.SelectedValue as fileitem;

            if (fi == null || fi._Items.Count == 0)
                return;
            fi.toggleselected(((System.Windows.Controls.CheckBox)e.Source).IsChecked);
        }

        void exportnodes(object data)
        {
            var node = (fileitem)((object[])data)[0];
            var exportfile = (string)((object[])data)[1];
            FileEx.AppendAllText(exportfile, string.Format("{0}|{1}\r\n", node._fullPath, node._size));
            driver.pmon.busycursor();
            foreach (var fi in node._Items)
            {
                try
                {
                    exportnodes(new object[] { fi, exportfile });
                }
                catch (Exception ex)
                {
                    driver.logit(ex.Message);
                }
            }
            driver.pmon.normalcursor();

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

    }

    public class ChkSelctionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((((bool)value == false)) ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible);
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
                view.SortDescriptions.Add(new SortDescription("_size", ListSortDirection.Descending));
            }
            return view;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }    
    
}
