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
    public partial class Form3 : Form
    {
        public string inoutdir;
        public string parentpath;
        //public List<string> ignorelist = new List<string>();
        private int sortcol=-1;
        bool asc=true;
        public List<string> selectedfiles = new List<string>();

        bool bfilter = true;
        bool bskiporincludeflag = true;
        string filteroptions = "";

        public Form3()
        {
            InitializeComponent();
            driver.previewlist = previewlist;
        }

        private void reload()
        {
            Cursor.Current = Cursors.WaitCursor;
            this.previewlist.ItemChecked -= new System.Windows.Forms.ItemCheckedEventHandler(this.previewlist_ItemChecked);
            chkall.Checked = false;
            driver.PreviewPatchList(parentpath, inoutdir, sortcol, asc, chkselonly.Checked ? false : bfilter, bskiporincludeflag, filteroptions);
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

        public bool Overwrite
        {
            get
            {
                return false;
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
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
                txtsort.Text = "Manual Merge";
            else if (sortcol == 3)
                txtsort.Text = "Directory";
            else if (sortcol == 4)
                txtsort.Text = "Remark";
            txtsort.Text = txtsort.Text + " " + (asc ? "Ascending" : "Desnding");
            reload();

        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            sortcol = -1;
            asc = true;
            txtsort.Text = "";
            reload();
        }

        private void previewlist_DoubleClick(object sender, EventArgs e)
        {
            string fdexe = ConfigurationManager.AppSettings["filediffexe"];
            string fdargs = ConfigurationManager.AppSettings["filediffargs"];
            string file1 = "";
            string file2 = "";

            if (previewlist.SelectedItems[0].Tag == null)
                return;
            string[] parts = (string[])previewlist.SelectedItems[0].Tag;
            driver.CreatePatchFile(inoutdir,parts);
            Process p = null;
            if ((parentpath != "" && !FileEx.Exists(parentpath + "\\" + parts[0]) || (parentpath == "" && !FileEx.Exists(driver.outputpath + "\\rd\\" + System.IO.Path.GetFileName(parts[0])))))
            {
                p = Process.Start(string.Format("\"{0}\\{1}\"", driver.outputpath, "ld\\" + System.IO.Path.GetFileName(parts[0])));
            }
            else
            {
                if (parentpath != "")
                {
                    file1 = string.Format("\"{0}\\{1}\"", parentpath, parts[0]);
                    file2 = string.Format("\"{0}\\{1}\"", driver.outputpath, "\\ld\\" + System.IO.Path.GetFileName(parts[0]));
                    fdargs = fdargs.Replace("<file 1>", file1);
                    fdargs = fdargs.Replace("<file 2>", file2);
                    p = Process.Start(fdexe, fdargs);
                }
                else
                {
                    file1 = string.Format("\"{0}\\{1}\"", driver.outputpath, "\\rd\\" + System.IO.Path.GetFileName(parts[0]));
                    file2 = string.Format("\"{0}\\{1}\"", driver.outputpath, "\\ld\\" + System.IO.Path.GetFileName(parts[0]));
                    fdargs = fdargs.Replace("<file 1>", file1);
                    fdargs = fdargs.Replace("<file 2>", file2);
                    p = Process.Start(fdexe, fdargs);
                }
                p.WaitForExit();
            }

        }

        private void btnapply_Click(object sender, EventArgs e)
        {

            driver.pbar = progressBar1;
            Cursor.Current = Cursors.WaitCursor;
            driver.ApplyPatch(parentpath, inoutdir, selectedfiles, Overwrite, txtfname);
            Cursor.Current = Cursors.Arrow;

        }

        private void chkall_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem li in previewlist.Items)
            {
                li.Checked = chkall.Checked;
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

        private void btnoptios_Click(object sender, EventArgs e)
        {
            var frm = new Form7();
            frm.bpatch = true;
            frm.bexclude = bskiporincludeflag;
            frm.filtertext = filteroptions;
            if (frm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            bfilter = true;
            bskiporincludeflag = frm.bexclude;
            filteroptions = frm.filtertext;
            reload();
        }

        private void chkselonly_CheckedChanged(object sender, EventArgs e)
        {
            reload();
        }


        private void previewlist_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                if (previewlist.SelectedItems.Count == 0 ||  previewlist.SelectedItems[0].Tag == null)
                    return;
                string[] parts = (string[])previewlist.SelectedItems[0].Tag;
                Clipboard.SetText(parts[0]);
            }
        }


    }
}
