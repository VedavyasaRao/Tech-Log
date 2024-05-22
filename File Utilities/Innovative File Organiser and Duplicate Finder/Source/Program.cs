using System;
using System.Collections.Generic;

using System.Diagnostics;
using System.Configuration;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace FileOrganiser
{
    public class driver
    {
        public static TextBlock logtxt;
        public static ScrollViewer vsb;
        public static string outputpath="";
        public static string logfile = "";
        static string outputfile = "";
        public static string sourcedir = "";
        public static Dispatcher disp;
        public static bool started;
        public static fileitem root;
        public static Window mainwindow;
        public static TVItmUtil tviutil = new TVItmUtil();
        public static progressmonitor pmon = new progressmonitor();
        public static int md5size = 5000;


        public static void CorrectFilenames(List<fileitem> leaves)
        {
            logit("Correcting filenames  .... please wait");
            List<fileitem> ucodenodes = (from fic in leaves where !FileEx.Exists(fic._fullPath) select fic).ToList();

            foreach (var fic in ucodenodes)
            {
                List<fileitem> chain = new List<fileitem>();
                chain.Add(fic);
                var fic2 = fic._parent;
                while (fic2 != null)
                {
                    chain.Add(fic2);
                    fic2 = fic2._parent;
                }
                chain.Reverse();
                try
                {
                    foreach (var fic3 in chain)
                    {
                        bool found = (fic3.isfile) ? FileEx.Exists(fic3._fullPath) : DirectoryEx.Exists(fic3._fullPath);
                        if (!found)
                        {
                            var parent = fic3._parent;
                            var fsentreis = DirectoryEx.GetFiles(parent._fullPath).ToList();
                            fsentreis.AddRange(DirectoryEx.GetDirectories(parent._fullPath).ToList());
                            int idx = parent._Items.IndexOf(fic3);
                            if (idx < fsentreis.Count)
                            {
                                fic3._fullPath = fsentreis[idx];
                                fic3._title = (fic3.isfile) ?  FileInfoEx.FileInfo(fic3._fullPath).Name :  DirectoryInfoEx.DirectoryInfo(fic3._fullPath).Name;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logit(ex.Message);
                }
        }
            logit("Correcting filenames  .... done");
        }

        public class progressmonitor
        {
            int i;
            int ticks = 0;
            public System.Windows.Controls.ProgressBar pbar;
            public void initpbar(double max)
            {
                i = 0;
                ticks = (int)max / 100;
                if (ticks == 0)
                    ticks = 100;
                disp.Invoke(new Action(() => { pbar.Minimum = 0; pbar.Maximum = max; pbar.Value = 0; }));
                busycursor();
            }

            public void updatepbar()
            {
                if ((++i % ticks) == 0)
                    disp.Invoke(new Action(() => { pbar.Value = i; }));
            }
            public void closebar()
            {
                disp.Invoke(new Action(() => { pbar.Value = 0; }));
                normalcursor();
            }
            public  void busycursor()
            {
                disp.Invoke(new Action(() => { System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait; }));
            }

            public  void normalcursor()
            {
                disp.Invoke(new Action(() => { System.Windows.Input.Mouse.OverrideCursor = null; }));
            }

        }


        public class TVItmUtil
        {
            public int maxshow = 500;
            public fileitem dummy = new fileitem { _title = "dummy" };
            public Dictionary<fileitem, double> fiitmsmap = new Dictionary<fileitem, double>();
            public Slider slider;
            public StackPanel pnl;
            public fileitemComp.fileitemsort sortby;

            public void clear(StackPanel pnl, Slider slider, fileitemComp.fileitemsort sortby)
            {
                fiitmsmap.Clear();
                this.sortby = sortby;
                this.slider = slider;
                this.pnl = pnl;
                this.slider.TickFrequency = 1;
            }

            public void closenode(fileitem fi)
            {
                pnl.Visibility = Visibility.Hidden;
            }

            public void showitems(fileitem fi)
            {
                fi.Items.Clear();
                int p = (int)fiitmsmap[fi];
                int q = p +  ((fi._Items.Count > maxshow)?maxshow:fi._Items.Count);
                if (!fi._sorted)
                {
                    fi._Items.Sort(new fileitemComp(sortby));
                    fi._sorted = true;
                }
                for (; p < q; ++p)
                {
                    fi.Items.Add(fi._Items[p]);
                }
            }

            public void expandnode(fileitem fi)
            {
                pnl.Visibility = Visibility.Hidden;
                if (fiitmsmap.ContainsKey(fi))
                    return;
                fiitmsmap.Add(fi, 0);
                showitems(fi);
            }

            public void selectnode(fileitem fi)
            {
                if (fiitmsmap.ContainsKey(fi) && fi._Items.Count > maxshow)
                {
                    pnl.Visibility = Visibility.Visible;
                    slider.Value = fiitmsmap[fi];
                    slider.Maximum = fi._Items.Count-maxshow;

                }
            }

            public void dragslider(fileitem fi)
            {
                if (!fiitmsmap.ContainsKey(fi))
                    return;
                if (slider.Value == fiitmsmap[fi])
                    return;
                fiitmsmap[fi] = (int)slider.Value;
                showitems(fi);
            }
            public void lostfocus()
            {
                pnl.Visibility = Visibility.Hidden;
            }



            public void btnCollapseAll_Click(TreeView treeView, fileitem fi)
            {
                DependencyObject dObject = treeView.ItemContainerGenerator.ContainerFromItem(fi);
                var tvitm = ((TreeViewItem)dObject);
                if (tvitm != null && tvitm.HasItems)
                    tvitm.IsExpanded = false;
                foreach (var item in fi.Items)
                {
                    btnCollapseAll_Click(treeView, item);
                }
            }

            public void LoadAll( fileitem fi)
            {
                if (fi.isfile)
                    return;
                expandnode(fi);
                foreach (var item in fi._Items)
                {
                    LoadAll(item);
                }
            }

           

        }
        static driver()
        {
            System.Configuration.Configuration config =
                          ConfigurationManager.OpenExeConfiguration(
                          ConfigurationUserLevel.None);
            outputpath = System.IO.Path.GetDirectoryName(config.AppSettings.CurrentConfiguration.FilePath) + "\\output";
            if (!DirectoryEx.Exists(outputpath))
            {
                DirectoryEx.CreateDirectory(outputpath);
            }

            logfile = outputpath + "\\FileOrganiser_log.txt";
            if (FileEx.Exists(logfile))
                FileEx.Delete(logfile);
        }

        public static void logit(string msg)
        {
            try
            {
                FileEx.AppendAllText(logfile, msg + "\r\n");
                disp.Invoke(new Action<MainWindow>((sender) => { logtxt.Text = logtxt.Text + msg + "\r\n"; vsb.ScrollToEnd(); }), new object[] { null });
            }
            catch
            { }

        }

        private static void showLoadprogress()
        {
            int i = 0;
            int j = 0;
            pmon.initpbar(100);
            while (started)
            {
                var fi = FileInfoEx.FileInfo(outputfile);
                if (fi.Exists)
                {
                    fi.Refresh();
                    j = (int)(fi.Length / 104857.6);
                }
                if (j > i)
                {
                    i = j;
                    pmon.updatepbar();
                }
                System.Threading.Thread.Sleep(2000);
            }
            pmon.closebar();
        }


        public static void bringfront(Window wnd)
        {
            driver.disp.Invoke(new Action(() => {
                wnd.IsEnabled = false;
            driver.mainwindow.Activate();
            driver.mainwindow.Topmost = true;
            }));

        }

        public static void goback(Window wnd)
        {
            driver.disp.Invoke(new Action(() => {
                wnd.IsEnabled = true;
            driver.mainwindow.Topmost = false;
            wnd.Activate();
        }));

        }


        static void GetFolderInfo(object parentpath)
        {
            try
            {
                driver.logit("Collecting data .... please wait");
                driver.logit("Source folder" + parentpath);
                var outputfile = driver.outputpath + "\\targetfile.txt";

                if (FileEx.Exists(outputfile))
                    FileEx.Delete(outputfile);
                pmon.busycursor();
                string args = "";
                args = "/c chcp 65001 & echo getting file info  & (dir /s /b  /A-D  \"" + parentpath + "\" >> \"" + outputfile + "\")";
                var ps = new ProcessStartInfo("cmd", args);
                var p = System.Diagnostics.Process.Start(ps);
                p.WaitForExit();
                driver.logit("Collecting data .... done");
                pmon.normalcursor();
            }
            catch (Exception ex)
            {
                driver.logit(ex.Message);
            }
        }


        static void Loadfileitms(string parentpath)
        {
            string aline = "";

            root = new fileitem { _title = parentpath, _fullPath = parentpath, _parent = null };
            root.Items.Add(driver.tviutil.dummy);
            string outputfile = driver.outputpath + "\\targetfile.txt";

            driver.logit("Creating file items .... please wait");
            Dictionary<string, fileitem> parentnodes = new Dictionary<string, fileitem>();
            String[] lines = FileEx.ReadAllLines(outputfile, System.Text.Encoding.GetEncoding(65001/*437*/));
            lines = lines.Distinct().ToArray();
            driver.pmon.initpbar(lines.Length);
            for (long k = 0; k < lines.Length; ++k)
            {
                try
                {
                    aline = lines[k].Replace(parentpath + "\\", "");

                    var fic = new fileitem { _title = System.IO.Path.GetFileName(aline), _fullPath = aline, isfile = true };
                    var parts = fic._fullPath.Replace(aline + "\\", "").Split(new char[] { '\\' });
                    var ppn = root;
                    var pp = root._fullPath;
                    for (var i = 0; i < parts.Length - 1; ++i)
                    {
                        pp += "\\" + parts[i];
                        if (!parentnodes.ContainsKey(pp))
                        {
                            var tempnode = new fileitem { _title = parts[i], _fullPath = pp, _parent = ppn, isfile = false };
                            tempnode.Items.Add(driver.tviutil.dummy);
                            ppn._Items.Add(tempnode);
                            parentnodes.Add(pp, tempnode);
                        }
                        ppn = parentnodes[pp];
                    }
                    ppn._Items.Add(fic);
                    fic._fullPath = lines[k];
                    fic._parent = ppn;
                }
                catch (Exception ex)
                {
                    driver.logit(ex.Message);
                }
                driver.pmon.updatepbar();
            }
            driver.pmon.closebar();
        }

        //public static void Loadfileitms(string parentpath)
        //{
        //    string aline = "";
        //    try
        //    {
        //        root = new fileitem { _title = parentpath, _fullPath = parentpath, _parent = null };
        //        string outputfile = driver.outputpath + "\\targetfile.txt";

        //        driver.logit("Creating file items .... please wait");
        //        String[] lines = FileEx.ReadAllLines(outputfile, System.Text.Encoding.GetEncoding(1252/*437*/));

        //        driver.pmon.initpbar(lines.Length);
        //        List<fileitem> fldrpath = new List<fileitem> { root };
        //        fileitem curfldr = root;
        //        pmon.initpbar(lines.Length);
        //        for (long i = 3; i < lines.Length; ++i)
        //        {
        //            try
        //            {
        //                aline = lines[i];
        //                if (aline.Contains("No subfolders exist"))
        //                    continue;

        //                if (aline.Replace('|', ' ').Trim().Length == 0)
        //                    continue;
        //                if (aline.Contains("+--") || aline.Contains("\\--"))
        //                {
        //                    int pos = 0;
        //                    if (aline.Contains("+--"))
        //                    {
        //                        pos = aline.IndexOf('+') + 4;
        //                    }
        //                    else
        //                    {
        //                        pos = aline.IndexOf('\\') + 4;
        //                    }

        //                    int kount = pos / 4;
        //                    string title = aline.Substring(pos);
        //                    var fi = new fileitem { _title = title };

        //                    if (fldrpath.Count == kount)
        //                        fldrpath.Add(null);
        //                    else if (fldrpath.Count > kount)
        //                    {
        //                        fldrpath.RemoveRange(kount + 1, fldrpath.Count - kount - 1);
        //                    }
        //                    fldrpath[kount] = fi;
        //                    var parent = fldrpath[kount - 1];
        //                    parent._Items.Add(fi);
        //                    fi._parent = parent;
        //                    fi._fullPath = parent.FullPath + "\\" + fi._title;
        //                    curfldr = fi;
        //                    if (parent.Items.Count == 0)
        //                        parent.Items.Add(driver.tviutil.dummy);
        //                }
        //                else
        //                {
        //                    string[] parts = aline.Split(new char[] { '|' });
        //                    string title = parts[parts.Length - 1].Trim();
        //                    var fi = new fileitem { _title = title, _parent = curfldr, isfile = true };
        //                    curfldr._Items.Add(fi);
        //                    fi._fullPath = curfldr.FullPath + "\\" + fi._title;
        //                    if (curfldr.Items.Count == 0)
        //                        curfldr.Items.Add(driver.tviutil.dummy);
        //                    foreach (var fldr in fldrpath)
        //                        ++fldr._count;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                driver.logit(ex.Message + " -> " + aline);
        //            }
        //            driver.pmon.updatepbar();

        //        }

        //        driver.logit("Creating file items .... done");
        //        driver.pmon.closebar();
        //    }

        //    catch (Exception ex)
        //    {
        //        driver.logit(ex.Message);
        //    }
        //}


        public static void Load(string parentpath, int loadopt)
        {
            try
            {
                if (loadopt == 0)
                    GetFolderInfo(parentpath);
                else if (loadopt == 3)
                    Loadfileitms(parentpath);
            }
            catch (Exception ex)
            {
                logit(ex.Message);
            }
        }

    
    }
}
