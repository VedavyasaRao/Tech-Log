using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Shapes;
using static System.Windows.Forms.AxHost;

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
                driver.sourcedir= dialog.SelectedPath;
                if (!driver.sourcedir.EndsWith("\\"))
                    driver.sourcedir += "\\";
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
            if (!parentpath.EndsWith("\\"))
                parentpath = parentpath+ '\\';

            try
            {
                fileitem parentfi = driver.root;
                var leaves = new List<fileitem>();
                parentfi.getleaves(ref leaves);
                //driver.CorrectFilenames(leaves);
                var parts = driver.skipfolders4export.Split(new char[] { ',' });
                leaves = (from l in leaves where !(parts.Any(p => (parentpath + l._fullPath).IndexOf(p, StringComparison.InvariantCultureIgnoreCase) >= 0)) select l).ToList();
                string exportfile = driver.outputpath + "\\" + ((parentpath + DateTime.Now.ToString()).Replace(':', '_').Replace('\\', '_')) + ".csv";
                var sdialog = new SaveFileDialog();
                sdialog.FileName = exportfile;
                if (sdialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    exportfile = sdialog.FileName;
                else
                    return;

                if (FileEx.Exists(exportfile))
                    FileEx.Delete(exportfile);

                driver.logit("Calucalating MD5 .... please wait");
                MD5Util md5 = new MD5Util( driver.bfastmd5);
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
                        FileEx.AppendAllText(exportfile, String.Format("{0}|{1}|{2}\n",  parentpath, fi._fullPath, fi._md5));
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

            if (config.AppSettings.Settings["skipfolders4export"] != null)
                driver.skipfolders4export = config.AppSettings.Settings["skipfolders4export"].Value;

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
            config.AppSettings.Settings.Remove("skipfolders4export");
            config.AppSettings.Settings.Add("skipfolders4export", driver.skipfolders4export);

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
                    //driver.CorrectFilenames(leaves);
                    //driver.logit("Updating File Items .... please wait");
                    //driver.pmon.initpbar(leaves.Count);
                    //foreach (var fi in leaves)
                    //{
                    //    try
                    //    {
                    //        System.IO.FileInfo fif =  FileInfoEx.FileInfo(fi._fullPath);
                    //        fi._size = fif.Length;
                    //        fi._dateupdated = fif.LastWriteTime.ToFileTime();
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        driver.logit(ex.Message);
                    //    }
                    //    driver.pmon.updatepbar();
                    //}
                    //driver.pmon.closebar();

                    //driver.logit("Updating File Items .... done");

                    driver.logit("Calucalating MD5 .... please wait");
                    MD5Util md5 = new MD5Util( driver.bfastmd5);
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
                    //driver.CorrectFilenames(leaves);

                    driver.logit("Updating File Items .... please wait");
                    //driver.pmon.initpbar(leaves.Count);
                    //foreach (var fi in leaves)
                    //{
                    //    try
                    //    {
                    //        System.IO.FileInfo fif = FileInfoEx.FileInfo(fi._fullPath);
                    //        fi._size = fif.Length;
                    //        fi._dateupdated = fif.LastWriteTime.ToFileTime();
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        driver.logit(ex.Message);
                    //    }
                    //    driver.pmon.updatepbar();
                    //}
                    //driver.pmon.closebar();

                    driver.logit("Updating File Items .... done");

                    driver.logit("Calucalating MD5 .... please wait");
                    MD5Util md5 = new MD5Util(driver.bfastmd5);
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

        private void chkfastmd5_Click(object sender, RoutedEventArgs e)
        {
            driver.bfastmd5 = chkfastmd5.IsChecked ?? false;

        }

        private void DiffBtn_Click(object sender, RoutedEventArgs e)
        {
            string firstfile = "", secondfile="";

            var dialog = new OpenFileDialog();
            dialog.CheckFileExists = true;
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                firstfile = dialog.FileName;
            else
                return;

            dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                secondfile = dialog.FileName;
            else
                return;

            var first = File.ReadAllLines(firstfile).Select(f => { var parts = f.Split(new char[] { '|' }); return new KeyValuePair<string, string>(parts[1], parts[2]); }).ToDictionary(f => f.Key, f => f.Value);
            var second = File.ReadAllLines(secondfile).Select(f => { var parts = f.Split(new char[] { '|' }); return new KeyValuePair<string, string>(parts[1], parts[2]); }).ToDictionary(f => f.Key, f => f.Value);
            var third = File.ReadAllLines(firstfile).Select(f => { var parts = f.Split(new char[] { '|' }); return new KeyValuePair<string, string>(parts[1], parts[0]); }).ToDictionary(f => f.Key, f => f.Value);

            string parentpath = srcfolder.Text;
            string exportfile = string.Format("diff_{0}_{1}.html", System.IO.Path.GetFileNameWithoutExtension(firstfile), System.IO.Path.GetFileNameWithoutExtension(secondfile));
            var sdialog = new SaveFileDialog();
            sdialog.FileName = exportfile;
            if (sdialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                exportfile = sdialog.FileName;
            else
                return;

            string htmlheadtxt =
"<!DOCTYPE html>" +
"<html>" +
"<head>" +
"<meta name=\"viewport\" content=\"width = device - width, initial - scale = 1\">" +
"<style>" +
".collapsible {" +
"  background-color: #777;" +
"  color: white;" +
"  cursor: pointer;" +
"  padding: 18px;" +
"  width: 100%;" +
"  border: none;" +
"  text-align: left;" +
"  outline: none;" +
"  font-size: 15px;" +
"}" +
"" +
".active, .collapsible:hover {" +
"  background-color: #555;" +
"}" +
"" +
".content {" +
"  padding: 0 18px;" +
"  display: none;" +
"  overflow: hidden;" +
"  background-color: #f1f1f1;" +
"}" +
"</style>" +
"</head>";

            var scripttxt =
"<script>" +
"var coll = document.getElementsByClassName(\"collapsible\");" +
"var i;" +
"" +
"for (i = 0; i < coll.length; i++) {" +
"  coll[i].addEventListener(\"click\", function() {" +
"    this.classList.toggle(\"active\");" +
"    var content = this.nextElementSibling;" +
"    if (content.style.display === \"block\") {" +
"      content.style.display = \"none\";" +
"    } else {" +
"      content.style.display = \"block\";" +
"    }" +
"  });" +
"}" +
"</script>";


            File.WriteAllText(exportfile, htmlheadtxt);
            File.AppendAllText(exportfile, "<body>\n");

            File.AppendAllText(exportfile, "<button type=\"button\" class=\"collapsible\">Changed_Files</button><div class=\"content\"><p>");
            var changed_files = first.Where(kv => second.ContainsKey(kv.Key) && kv.Value != second[kv.Key])
                .Select(kv2 => (third[kv2.Key] + "\\" + kv2.Key + "<br />"));
            File.AppendAllLines(exportfile, changed_files);
            File.AppendAllText(exportfile, "</p></div>");

            File.AppendAllText(exportfile, "<button type=\"button\" class=\"collapsible\">New_Files</button><div class=\"content\"><p>");
            var new_files = first.Where(kv => !second.ContainsKey(kv.Key))
                .Select(kv2 => (third[kv2.Key] + "\\" + kv2.Key + "<br />"));
            File.AppendAllLines(exportfile, new_files);
            File.AppendAllText(exportfile, "</p></div>");

            File.AppendAllText(exportfile, "<button type=\"button\" class=\"collapsible\">Same_Files</button><div class=\"content\"><p>");
            var same_files = first.Where(kv => second.Contains(kv))
                .Select(kv2 => (third[kv2.Key] + "\\" + kv2.Key + "<br />"));
            File.AppendAllLines(exportfile, same_files);
            File.AppendAllText(exportfile, "</p></div>");

            File.AppendAllText(exportfile, scripttxt);
            File.AppendAllText(exportfile, "</body></html>");


        }
    }
}
