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
    public partial class Form2 : Form
    {
        public string inoutdir;
        public string parentpath;
        public string updatepath;
        public int mode = 1;
        public bool bfilter = true;
        public bool bskiporincludeflag = true;
        public string filteroptions = "";
        string extns = "";


        public Form2()
        {
            InitializeComponent();
            ImageList ilst = new ImageList();
            ilst.Images.Add(Resource1.left);
            ilst.Images.Add(Resource1.right);
            ilst.Images.Add(Resource1.diff);
            //previewTree.ImageList = ilst;
            driver.previewtree = previewTree;

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (mode ==1 )
                driver.PreviewPatchTree(parentpath, inoutdir, chksingle.Checked, txtextensions.Text.ToLower(), ref extns);
            else
                driver.PreviewPatchTree(txtextensions.Text.ToLower(), ref extns, chksingle.Checked, bfilter, bskiporincludeflag, filteroptions);

            txtextns.Text = "("+extns+")";
            Cursor.Current = Cursors.Arrow;

        }

        private void btncollapse_Click(object sender, EventArgs e)
        {
            if (previewTree.SelectedNode == null)
                return;
            previewTree.SelectedNode.Collapse();
        }

        private void btnexpand_Click(object sender, EventArgs e)
        {
            if (previewTree.SelectedNode == null)
                return;
            previewTree.SelectedNode.ExpandAll();
        }

        private void btnshow_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (mode == 1)
                driver.PreviewPatchTree(parentpath, inoutdir, chksingle.Checked, txtextensions.Text.ToLower(), ref extns);
            else
                driver.PreviewPatchTree(txtextensions.Text.ToLower(), ref extns, chksingle.Checked, bfilter, bskiporincludeflag, filteroptions);

            txtextns.Text = "(" + extns + ")";
            Cursor.Current = Cursors.Arrow;

        }

        private void previewTree_DoubleClick(object sender, EventArgs e)
        {
            string fdexe = ConfigurationManager.AppSettings["filediffexe"];
            string fdargs = ConfigurationManager.AppSettings["filediffargs"];
            string file1 = "";
            string file2 = "";


            if (previewTree.SelectedNode == null || previewTree.SelectedNode.Tag == null)
                return;
            string[] parts  = (string[])previewTree.SelectedNode.Tag;

            if (mode == 1)
            {
                driver.CreatePatchFile(inoutdir, parts);

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
                    }
                    else
                    {
                        file1 = string.Format("\"{0}\\{1}\"", driver.outputpath, "\\rd\\" + System.IO.Path.GetFileName(parts[0]));
                        file2 = string.Format("\"{0}\\{1}\"", driver.outputpath, "\\ld\\" + System.IO.Path.GetFileName(parts[0]));
                    }

                    fdargs = fdargs.Replace("<file 1>", file1);
                    fdargs = fdargs.Replace("<file 2>", file2);

                    p = Process.Start(fdexe, fdargs);
                    p.WaitForExit();
                }
            }
            else
            {
                Process p = null;
                bool bleft = FileEx.Exists(parentpath + "\\" + parts[0]);
                bool bright = FileEx.Exists(updatepath + "\\" + parts[0]);

                if (!bleft)
                    p = Process.Start(string.Format("\"{0}\\{1}\"", updatepath , parts[0]));
                else if (!bright)
                    p = Process.Start(string.Format("\"{0}\\{1}\"", parentpath, parts[0]));
                else
                {
                    file1 = string.Format("\"{0}\\{1}\"", parentpath, parts[0]);
                    file2 = string.Format("\"{0}\\{1}\"", updatepath, parts[0]);
                    fdargs = fdargs.Replace("<file 1>", file1);
                    fdargs = fdargs.Replace("<file 2>", file2);
                    p = Process.Start(fdexe, fdargs);
                    p.WaitForExit();
                }
            }


        }

        private void previewTree_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                if (previewTree.SelectedNode.Tag == null)
                    return;
                string[] parts = (string[])previewTree.SelectedNode.Tag;
                Clipboard.SetText(parts[0]);
            }

        }

        private void chksingle_CheckedChanged(object sender, EventArgs e)
        {
            btnshow_Click(null, null);
        }


    }
}
