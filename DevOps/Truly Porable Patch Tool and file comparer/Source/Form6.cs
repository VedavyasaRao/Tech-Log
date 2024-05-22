using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using PortablePatchTool;
namespace MigrationHelper
{
    public partial class Form6 : Form
    {
        public string leftdir = "";
        public string rightdir = "";
        public string inoutdir;
        public List<string> selectedfiles = new List<string>();
        private int sortcol = -1;
        bool asc = true;

        public Form6()
        {
            InitializeComponent();
            driver.previewlist = previewlist;
            driver.pbar = progressBar1;
        }


        private void reload(bool bfilter=true)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.previewlist.ItemChecked -= new System.Windows.Forms.ItemCheckedEventHandler(this.previewlist_ItemChecked);
            checkBox1.Checked = false;
            driver.SortComparefiles( inoutdir, sortcol, asc, bfilter);
            foreach (ListViewItem li in previewlist.Items)
            {
                string filename = ((string[])li.Tag)[0];
                li.Checked = selectedfiles.Contains(filename);
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


        private void Form5_Load(object sender, EventArgs e)
        {
            reload();

        }

        private void btnright_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = previewlist.CheckedItems.Count;
            progressBar1.Value = 0;
            List<string> files = new List<string>();
            foreach (ListViewItem li in previewlist.CheckedItems)
            {
                string filename = ((string[])li.Tag)[0];
                if ("Left" != ((string[])li.Tag)[1])
                    files.Add(filename);
                progressBar1.Value++;
                Application.DoEvents();
            }

            Cursor.Current = Cursors.WaitCursor;
            driver.CreatePatch(leftdir, rightdir, inoutdir, chkincsf.Checked, files.ToArray());
            Cursor.Current = Cursors.Arrow;

            progressBar1.Value = 0;

        }

        private void previewlist_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (previewlist.SelectedItems[0].Tag == null)
                return;
            string filename = ((string[])previewlist.SelectedItems[0].Tag)[0];
            string remark = ((string[])previewlist.SelectedItems[0].Tag)[1];
            string cmd = "";
            string args="";
            if (remark == "Left")
            {
                cmd = "notepad.exe";
                args = string.Format("\"{0}\\{1}\"", leftdir, filename);
            }
            else if (remark ==  "Right")
            {
                cmd = "notepad.exe";
                args = string.Format("\"{0}\\{1}\"", rightdir, filename);
            }
            else
            {
                cmd = "C:\\Program Files (x86)\\WinMerge\\WinMergeU.exe";
                args  = string.Format("\"{0}\\{1}\"  \"{2}\\{3}\"", leftdir, filename, rightdir, filename);
            }

            Process p = Process.Start(cmd,args);
            p.WaitForExit();

        }

        private void btnoptions_Click(object sender, EventArgs e)
        {
            if (new Form7().ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            reload();
        }


        private void previewlist_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            string filename = ((string[])e.Item.Tag)[0];
            if (e.Item.Checked && !selectedfiles.Contains(filename))
                selectedfiles.Add(filename);
            else
                selectedfiles.Remove(((string[])e.Item.Tag)[0]);
        }


        private void button4_Click(object sender, EventArgs e)
        {
            reload(false);
            foreach (ListViewItem li in previewlist.Items)
            {
                if (!li.Checked)
                    li.Remove();
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem li in previewlist.Items)
            {
                li.Checked = checkBox1.Checked;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            reload(false);
        }

    }
}
