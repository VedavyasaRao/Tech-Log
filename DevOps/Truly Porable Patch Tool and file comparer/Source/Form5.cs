using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using PortablePatchTool;
using System.Configuration;
namespace MigrationHelper
{
    public partial class Form5 : Form
    {
        public string leftdir = "";
        public string rightdir = "";
        public List<string> selectedfiles = new List<string>();

        private int sortcol = -1;
        bool asc = true;
        bool bfilter=true;
        bool bskiporincludeflag = true;
        string filteroptions = "";

        public Form5()
        {
            InitializeComponent();
            driver.previewlist = previewlist;
            driver.pbar = progressBar1;
        }


        private void reload()
        {
            this.previewlist.ItemChecked -= new System.Windows.Forms.ItemCheckedEventHandler(this.previewlist_ItemChecked);
            chkall.Checked = false;
            Cursor.Current = Cursors.WaitCursor;
            driver.SortComparefiles(sortcol, asc, chkselonly.Checked ? false : bfilter, bskiporincludeflag, filteroptions);
            foreach (ListViewItem li in previewlist.Items)
            {
                string filename = ((string[])li.Tag)[0];
                li.Checked = selectedfiles.Contains(filename);
                if (chkselonly.Checked && !li.Checked)
                    li.Remove();
            }
            this.previewlist.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.previewlist_ItemChecked);
            Cursor.Current = Cursors.Arrow;

        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            sortcol = -1;
            asc = true;
            txtsort.Text = "";
            reload();
        }

        private void previewlist_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (sortcol == e.Column)
                asc = !asc;
            else
                asc = true;
            sortcol = e.Column;
            if (sortcol == 0)
                txtsort.Text = "FileName";
            else if (sortcol == 1)
                txtsort.Text = "Extension";
            else if (sortcol == 2)
                txtsort.Text = "Remark";
            else if (sortcol == 3)
                txtsort.Text = "Directory";
            txtsort.Text = txtsort.Text + " " + (asc ? "Ascending" : "Desnding");
            reload();

        }


        private void chkall_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem li in previewlist.Items)
            {
                li.Checked = chkall.Checked;
            }
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            reload();

        }



        private void btnleft_Click(object sender, EventArgs e)
        {
            if (previewlist.CheckedItems.Count == 0)
                return;
            var tgtfldr = "";
            var tgtpath = "";

            if (chkdifffldr.Checked)
            {
                FolderBrowserDialog fd = new FolderBrowserDialog();
                fd.ShowNewFolderButton = true;
                if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    tgtfldr = fd.SelectedPath;
                else
                    return;
            }

            progressBar1.Maximum = previewlist.CheckedItems.Count;
            progressBar1.Value = 0;
            foreach (ListViewItem li in previewlist.CheckedItems)
            {
                string filename = ((string[])li.Tag)[0];
                if (!chkdifffldr.Checked)
                {
                    string dirname = System.IO.Path.GetDirectoryName(rightdir + "\\" + filename);
                    if (!DirectoryEx.Exists(dirname))
                        DirectoryEx.CreateDirectory(dirname);
                    tgtpath = rightdir + "\\" + filename;
                }
                else
                    tgtpath = tgtfldr + "\\" + System.IO.Path.GetFileName(filename);

                if (chkReadonly.Checked)
                    driver.RemoveReadonly(rightdir + "\\" + filename);

                if ((FileEx.GetAttributes(leftdir + "\\" + filename) & System.IO.FileAttributes.ReparsePoint) != System.IO.FileAttributes.ReparsePoint)
                    FileEx.Copy(leftdir + "\\" + filename, tgtpath, true);
                else
                    driver.logtxt.AppendText(String.Format("Could not copy file {0}\\{1} \r\n", leftdir, filename));


                txtfn.Text = filename;
                progressBar1.Value++;
                Application.DoEvents();
            }
            progressBar1.Value = 0;
        }
        private void btnright_Click(object sender, EventArgs e)
        {
            if (previewlist.CheckedItems.Count == 0)
                return;
            var tgtfldr = "";
            var tgtpath = "";

            if (chkdifffldr.Checked)
            {
                FolderBrowserDialog fd = new FolderBrowserDialog();
                fd.ShowNewFolderButton = true;
                if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    tgtfldr = fd.SelectedPath;
                else
                    return;
            }

            progressBar1.Maximum = previewlist.CheckedItems.Count;
            progressBar1.Value = 0;
            foreach (ListViewItem li in previewlist.CheckedItems)
            {
                string filename = ((string[])li.Tag)[0];
                if (!chkdifffldr.Checked)
                {
                    string dirname = System.IO.Path.GetDirectoryName(leftdir + "\\" + filename);
                    if (!DirectoryEx.Exists(dirname))
                        DirectoryEx.CreateDirectory(dirname);
                    tgtpath = leftdir + "\\" + filename;
                }
                else
                    tgtpath = tgtfldr + "\\" + System.IO.Path.GetFileName(filename);

                if (chkReadonly.Checked)
                    driver.RemoveReadonly(rightdir + "\\" + filename);

                if ((FileEx.GetAttributes(rightdir + "\\" + filename) & System.IO.FileAttributes.ReparsePoint) != System.IO.FileAttributes.ReparsePoint)
                    FileEx.Copy(rightdir + "\\" + filename, tgtpath, true);
                else
                    driver.logtxt.AppendText(String.Format("Could not copy file {0}\\{1} \r\n", rightdir, filename));


                txtfn.Text = filename;
                progressBar1.Value++;
                Application.DoEvents();
            }
            progressBar1.Value = 0;
        }

        private void olfbtnright_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = previewlist.CheckedItems.Count;
            progressBar1.Value = 0;
            foreach (ListViewItem li in previewlist.CheckedItems)
            {
                string filename = ((string[])li.Tag)[0];
                if (chkReadonly.Checked)
                    driver.RemoveReadonly(leftdir + "\\" + filename);
                string dirname = System.IO.Path.GetDirectoryName(leftdir + "\\" + filename);
                if (!DirectoryEx.Exists(dirname))
                    DirectoryEx.CreateDirectory(dirname);
                try_again:
                try
                {
                    FileEx.Copy(rightdir + "\\" + filename, leftdir + "\\" + filename, true);
                }
                catch (Exception ex)
                {
                    if (ex.GetType() == typeof(System.UnauthorizedAccessException))
                    {
                        driver.RemoveReadonly(leftdir + "\\" + filename);
                        goto try_again;
                    }
                    throw;
                }
                txtfn.Text = filename;
                progressBar1.Value++;
                Application.DoEvents();
            }
            progressBar1.Value = 0;

        }

        private void previewlist_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (previewlist.SelectedItems.Count == 0)
                return;
            string filename = ((string[])previewlist.SelectedItems[0].Tag)[0];
            string remark = ((string[])previewlist.SelectedItems[0].Tag)[1];
            string cmd = "";
            string args="";
            if (remark == "Left")
            {
                cmd = string.Format("\"{0}\\{1}\"", leftdir, filename);
            }
            else if (remark ==  "Right")
            {
                cmd = string.Format("\"{0}\\{1}\"", rightdir, filename);
            }
            else
            {
                cmd = ConfigurationManager.AppSettings["filediffexe"];
                args = ConfigurationManager.AppSettings["filediffargs"];
                string file1 = "";
                string file2 = "";
                file1 = string.Format("\"{0}\\{1}\"", leftdir, filename);
                file2 = string.Format("\"{0}\\{1}\"", rightdir, filename);
                args = args.Replace("<file 1>", file1);
                args = args.Replace("<file 2>", file2);
            }

            Process p = Process.Start(cmd,args);
            p.WaitForExit();

        }

        private void removeonefolder(string folder)
        {
            if (DirectoryEx.Exists(folder) && DirectoryEx.GetFiles(folder).Length == 0 && DirectoryEx.GetDirectories(folder).Length == 0)
            {
                string temp = DirectoryEx.GetParent(folder).FullName.RemoveUNC();
                DirectoryEx.Delete(folder);
                txtfn.Text = folder;
                progressBar1.Value++;
                removeonefolder(temp);
            }

        }

        private void cmdel_Click(object sender, EventArgs e)
        {
            List<string> folderlist = new List<string>();

            progressBar1.Maximum = previewlist.CheckedItems.Count;
            progressBar1.Value = 0;
            foreach (ListViewItem li in previewlist.CheckedItems)
            {
                string filename = ((string[])li.Tag)[0];
                if (li.SubItems[2].Text == "Left")
                {
                    driver.RemoveReadonly(leftdir + "\\" + filename);
                    FileEx.Delete(leftdir + "\\" + filename);
                    string folder = System.IO.Path.GetDirectoryName(leftdir + "\\" + filename);
                    if (!folderlist.Contains(folder))
                        folderlist.Add(folder);
                }
                else if (li.SubItems[2].Text == "Right")
                {
                    driver.RemoveReadonly(rightdir + "\\" + filename);
                    FileEx.Delete(rightdir + "\\" + filename);
                    string folder = System.IO.Path.GetDirectoryName(rightdir + "\\" + filename);
                    if (!folderlist.Contains(folder))
                        folderlist.Add(folder);
                }
                txtfn.Text = filename;
                progressBar1.Value++;
                Application.DoEvents();
                previewlist.Items.Remove(li);
            }
            
            progressBar1.Value = 0;
            Application.DoEvents();
            foreach (string folder in folderlist)
            {
                removeonefolder(folder);
                txtfn.Text = folder;
                progressBar1.Value++;
                Application.DoEvents();
            }
            progressBar1.Value = 0;
            Application.DoEvents();



        }

        private void btnoptions_Click(object sender, EventArgs e)
        {
            var frm = new Form7();
            frm.bpatch = false;
            frm.bexclude = bskiporincludeflag;
            frm.filtertext = filteroptions;
            if (frm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            bfilter = true;
            bskiporincludeflag = frm.bexclude;
            filteroptions = frm.filtertext;
            reload();
        }

        private void cmdsave_Click(object sender, EventArgs e)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.CheckPathExists = true;
            ofd.AddExtension = true;
            ofd.DefaultExt = ".txt";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fname = ofd.FileName;
                if (FileEx.Exists(fname))
                    FileEx.Delete(fname);
                foreach (ListViewItem li in previewlist.Items)
                {
                    string filename = ((string[])li.Tag)[0];
                    if (li.Checked)
                        FileEx.AppendAllText(fname, filename + "\r\n");
                }
            }

            
        }

        private void previewlist_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            string filename = ((string[])e.Item.Tag)[0];
            if (e.Item.Checked && !selectedfiles.Contains(filename))
                selectedfiles.Add(filename);
            else
                selectedfiles.Remove(((string[])e.Item.Tag)[0]);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            bfilter = false;
            reload();
            foreach (ListViewItem li in previewlist.Items)
            {
                if (!li.Checked)
                    li.Remove();
            }

        }

        private void chkselonly_CheckedChanged(object sender, EventArgs e)
        {
            reload();
        }

        private void btnTV_Click(object sender, EventArgs e)
        {
            Form2 pfrm = new Form2();
            pfrm.mode = 2;
            pfrm.parentpath = leftdir;
            pfrm.updatepath = rightdir;
            pfrm.bfilter = chkselonly.Checked ? false : bfilter;
            pfrm.bskiporincludeflag = bskiporincludeflag;
            pfrm.filteroptions = filteroptions;
            pfrm.ShowDialog();

        }


        private void previewlist_MouseClick(object sender, MouseEventArgs e)
        {
            if (previewlist.SelectedItems.Count == 0)
                return;
            string filename = ((string[])previewlist.SelectedItems[0].Tag)[0];
            string remark = ((string[])previewlist.SelectedItems[0].Tag)[1];
            System.IO.FileInfo fil =  FileInfoEx.FileInfo(leftdir + "\\" + filename);
            System.IO.FileInfo fir =  FileInfoEx.FileInfo(rightdir + "\\" + filename);
            this.toolStripStatusLabel1.Text = "n/a";
            this.toolStripStatusLabel2.Text = "n/a";
            this.toolStripStatusLabel3.Text = "n/a";
            this.toolStripStatusLabel4.Text = "n/a";
            if (fil.Exists)
            {
                this.toolStripStatusLabel1.Text = fil.Length.ToString();
                this.toolStripStatusLabel2.Text = fil.LastWriteTime.ToString();
            }

            if (fir.Exists)
            {
                this.toolStripStatusLabel3.Text = fir.Length.ToString();
                this.toolStripStatusLabel4.Text = fir.LastWriteTime.ToString();
            }
        }

    }
}
