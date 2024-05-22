using System;
using System.Collections.Generic;
using System.Configuration;

using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace FileOrganiser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            driver.mainwindow = this;
            driver.vsb = vsb;
            driver.logtxt = log;
            driver.pmon.pbar = progress;
            driver.disp = System.Windows.Application.Current.Dispatcher;
        }

        private void Dir_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                srcfolder.Text = dialog.SelectedPath;
            }
        }

        private void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Threading.Thread t = new System.Threading.Thread(Export_files);
            t.Start(srcfolder.Text);
        }

        private void Export_files(object data)
        {
            driver.disp.Invoke(new Action(delegate {
                IsEnabled = false;
            }));
            string parentpath = (string)data;
            driver.Load((string)parentpath, 0);
            driver.Load((string)parentpath, 3);

            try
            {
                fileitem parentfi = driver.root;
                var leaves = new List<fileitem>();
                parentfi.getleaves(ref leaves);
                driver.CorrectFilenames(leaves);

                string exportfile = driver.outputpath + "\\" + parentpath.Replace(':', '_').Replace('\\', '_') + ".csv";
                if (FileEx.Exists(exportfile))
                    FileEx.Delete(exportfile);

                driver.logit("Updating File Items .... please wait");
                driver.pmon.initpbar(leaves.Count);
                foreach (var fi in leaves)
                {
                    try
                    {
                        System.IO.FileInfo fif = FileInfoEx.FileInfo(fi._fullPath);
                        fi._size = fif.Length;
                        fi._dateupdated = fif.LastWriteTime.ToFileTime();
                    }
                    catch (Exception ex)
                    {
                        driver.logit(ex.Message);
                    }
                    driver.pmon.updatepbar();
                }
                driver.pmon.closebar();
                driver.logit("Updating File Items .... done");

                driver.logit("Calucalating MD5 .... please wait");
                MD5Util md5 = new MD5Util();
                driver.pmon.initpbar(leaves.Count);
                md5.md5threadpool2(leaves);
                driver.pmon.closebar();
                driver.logit("Calucalating MD5 .... done");

                driver.logit("Exporting File Items .... please wait");
                driver.pmon.initpbar(leaves.Count);
                foreach (var fi in leaves)
                {
                    try
                    {
                        FileEx.AppendAllText(exportfile, String.Format("{0}|{1}\n", fi._fullPath, fi._md5));
                    }
                    catch (Exception ex)
                    {
                        driver.logit(ex.Message);
                    }
                    driver.pmon.updatepbar();
                }
                driver.pmon.closebar();
                driver.logit("Exporting File Items to "+ exportfile);
            }

            catch (Exception ex)
            {
                driver.logit(ex.Message);
            }

            driver.logit("Exporting File Items .... done");
            driver.disp.Invoke(new Action(delegate {
                IsEnabled = true;
            }));
        }

        private void Load_Tree(object data)
        {
            string parentpath = (string)((object[])data)[0];
            Window wnd = (Window)((object[])data)[1];
            string outputfile = driver.outputpath + "\\targetfile.txt";

            driver.Load((string)parentpath, 0);
            driver.Load((string)parentpath, 3);
            driver.disp.Invoke(new Action(delegate {
                IsEnabled = true;
                wnd.ShowDialog();
            }));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Configuration.Configuration config =
                           ConfigurationManager.OpenExeConfiguration(
                           ConfigurationUserLevel.None);

            if (config.AppSettings.Settings["sourcedir"] != null)
                srcfolder.Text = config.AppSettings.Settings["sourcedir"].Value;

            if (config.AppSettings.Settings["tvitemscount"] != null)
                driver.tviutil.maxshow = int.Parse(config.AppSettings.Settings["tvitemscount"].Value);

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length == 2)
                srcfolder.Text = args[1];

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Configuration.Configuration config =
                           ConfigurationManager.OpenExeConfiguration(
                           ConfigurationUserLevel.None);

            config.AppSettings.Settings.Remove("sourcedir");
            config.AppSettings.Settings.Add("sourcedir", srcfolder.Text);
            config.AppSettings.Settings.Remove("tvitemscount");
            config.AppSettings.Settings.Add("tvitemscount", driver.tviutil.maxshow.ToString());

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void TreeVW_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new Treeview.Window1();
            wnd.srcfldr = srcfolder.Text;
            System.Threading.Thread t = new System.Threading.Thread(Load_Tree);
            t.Start(new object[] { srcfolder.Text,wnd });
        }

        private void DupVW_Click(object sender, RoutedEventArgs e)
        {
            System.Threading.Thread t = new System.Threading.Thread(find_duplicate_files);
            t.Start(new object[] { srcfolder.Text,  null });

        }

        private void CompareBtn_Click(object sender, RoutedEventArgs e)
        {
            var tgtfolder = "";

            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                tgtfolder = dialog.SelectedPath;
            else
                return;

            System.Threading.Thread t = new System.Threading.Thread(compare_files);
            t.Start(new object[] { srcfolder.Text,  tgtfolder });

        }

        private void compare_files(object data)
        {
            driver.disp.Invoke(new Action(delegate {
                IsEnabled = false;
            }));
            string parentpath = (string)((object[])data)[0];
            string targetpath = (string)((object[])data)[1];

            driver.Load((string)parentpath, 0);
            driver.Load((string)parentpath, 3);
            var srcroot = driver.root;

            driver.Load((string)targetpath, 0);
            driver.Load((string)targetpath, 3);
            var tgtroot = driver.root;

            var leaveslist = new List<List<fileitem>>();
            var roots = new fileitem[] {srcroot,tgtroot };
            try
            {
                foreach (var parentfi in roots)
                {
                    var leaves = new List<fileitem>();
                    parentfi.getleaves(ref leaves);
                    driver.CorrectFilenames(leaves);
                    driver.logit("Updating File Items .... please wait");
                    driver.pmon.initpbar(leaves.Count);
                    foreach (var fi in leaves)
                    {
                        try
                        {
                            System.IO.FileInfo fif =  FileInfoEx.FileInfo(fi._fullPath);
                            fi._size = fif.Length;
                            fi._dateupdated = fif.LastWriteTime.ToFileTime();
                        }
                        catch (Exception ex)
                        {
                            driver.logit(ex.Message);
                        }
                        driver.pmon.updatepbar();
                    }
                    driver.pmon.closebar();

                    driver.logit("Updating File Items .... done");

                    driver.logit("Calucalating MD5 .... please wait");
                    MD5Util md5 = new MD5Util();
                    driver.pmon.initpbar(leaves.Count);
                    md5.md5threadpool2(leaves);
                    driver.pmon.closebar();
                    driver.logit("Calucalating MD5 .... done");
                    leaveslist.Add(leaves);
                }
            }

            catch (Exception ex)
            {
                driver.logit(ex.Message);
            }

            var srcleaves = leaveslist[0];
            var tgtleaves = leaveslist[1];
            var duplist = new List<Tuple<fileitem, List<fileitem>>>();
            driver.logit("Finding duplicates .... please wait");
            driver.pmon.initpbar(srcleaves.Count);
            foreach (var sfi in srcleaves)
            {
                var duplicates = (from dfi in tgtleaves where dfi._md5 == sfi._md5 select dfi).ToList();
                duplist.Add(new Tuple<fileitem, List<fileitem>>(sfi, duplicates));
                driver.pmon.updatepbar();
            }
            driver.pmon.closebar();

            driver.logit("Finding duplicates .... done");
            LoadDupWindow(roots[0], targetpath, duplist);

        }

        private void find_duplicate_files(object data)
        {
            driver.disp.Invoke(new Action(delegate {
                IsEnabled = false;
            }));
            string parentpath = (string)((object[])data)[0];
            string targetpath = (string)((object[])data)[1];

            driver.Load((string)parentpath, 0);
            driver.Load((string)parentpath, 3);
            var srcroot = driver.root;

            var leaveslist = new List<List<fileitem>>();
            var roots = new fileitem[] { srcroot };
            try
            {
                foreach (var parentfi in roots)
                {
                    var leaves = new List<fileitem>();
                    parentfi.getleaves(ref leaves);
                    driver.CorrectFilenames(leaves);

                    driver.logit("Updating File Items .... please wait");
                    driver.pmon.initpbar(leaves.Count);
                    foreach (var fi in leaves)
                    {
                        try
                        {
                            System.IO.FileInfo fif =  FileInfoEx.FileInfo(fi._fullPath);
                            fi._size = fif.Length;
                            fi._dateupdated = fif.LastWriteTime.ToFileTime();
                        }
                        catch (Exception ex)
                        {
                            driver.logit(ex.Message);
                        }
                        driver.pmon.updatepbar();
                    }
                    driver.pmon.closebar();

                    driver.logit("Updating File Items .... done");

                    driver.logit("Calucalating MD5 .... please wait");
                    MD5Util md5 = new MD5Util();
                    driver.pmon.initpbar(leaves.Count);
                    md5.md5threadpool2(leaves);
                    driver.pmon.closebar();
                    driver.logit("Calucalating MD5 .... done");
                    leaveslist.Add(leaves);
                }
            }

            catch (Exception ex)
            {
                driver.logit(ex.Message);
            }

            var srcleaves = leaveslist[0];
            var duplist = new List<Tuple<fileitem, List<fileitem>>>();
            driver.logit("Finding duplicates .... please wait");
            driver.pmon.initpbar(srcleaves.Count);
            var duplicates = (from sfi in srcleaves group sfi by sfi._md5 into dups select new {  items = dups.ToList() }).ToList();
            foreach (var itm in duplicates)
            {
                duplist.Add(new Tuple<fileitem, List<fileitem>>(itm.items[0], itm.items.GetRange(1,itm.items.Count-1)));
                driver.pmon.updatepbar();
            }
            driver.pmon.closebar();

            driver.logit("Finding duplicates .... done");
            LoadDupWindow(roots[0], targetpath, duplist);
        }

        void LoadDupWindow(fileitem srcroot, string targetpath, List<Tuple<fileitem, List<fileitem>>> duplist)
        {
            driver.logit("Updating nodes .... please wait");
            driver.pmon.initpbar(duplist.Count);
            foreach (var tp in duplist)
            {
                var fic = tp.Item1;
                if (tp.Item2.Count > 0)
                {
                    fic._Items = tp.Item2;
                    foreach (var fic2 in fic._Items)
                    {
                        fic2.isdup = true;
                        fic2._parent = fic;
                        fic2._title = fic2._fullPath;
                        fic2.Color = Brushes.Crimson;

                    }
                    fic._dupcount = fic._Items.Count;
                    fic.Items.Add(driver.tviutil.dummy);
                }
                else
                {
                    fic.isdup = true;
                    fic.Color = Brushes.DarkTurquoise;
                    var ficp = fic._parent;
                    var ficp2 = ficp;
                    while (ficp != null)
                    {
                        ficp2 = ficp;
                        ficp = ficp._parent;
                    }
                }
                driver.pmon.updatepbar();
            }

            driver.logit("Updating nodes .... done");
            driver.pmon.closebar();


            driver.disp.Invoke(new Action(delegate {
                FileOrganiser.Duplicates.Window4 wnd = new FileOrganiser.Duplicates.Window4();
                wnd.srcroot = srcroot;
                wnd.targetfolder = targetpath;
                wnd.duplist = duplist;
                IsEnabled = true;
                wnd.ShowDialog();
            }));

        }

    }
}
