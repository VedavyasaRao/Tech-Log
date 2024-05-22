using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MigrationHelper;
using System.Diagnostics;

namespace PortablePatchTool
{
    public partial class Form4 : Form
    {
        public bool bcreatepatch = true;
        public string rootdir = "";
        public bool bincludesrc = false;
        public string importfile = "";
        public string inoutdir = "";

        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            filestype.SelectedIndex = 0;
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            txtexlude.Text = appSettings["exclude"];
            grppatch.Enabled=bcreatepatch;
            chksource.Checked = bincludesrc;
            txtimport.Text = importfile;
            fdexetxt.Text = appSettings["filediffexe"];
            fdargstxt.Text = appSettings["filediffargs"];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txtexlude.Text == "")
            {
                MessageBox.Show("Cannot be blank");
            }
            else
            {
                System.Configuration.Configuration config =
                              ConfigurationManager.OpenExeConfiguration(
                              ConfigurationUserLevel.None);
                config.AppSettings.Settings.Remove("exclude");
                config.AppSettings.Settings.Add("exclude", txtexlude.Text);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                string fname = (filestype.SelectedIndex==0)?(inoutdir + "\\addtional_input.txt") : (inoutdir + "\\removal_input.txt");
                if (bcreatepatch)
                {
                    if (FileEx.Exists(fname))
                        FileEx.Delete(fname);
                    foreach (var li in lstfiles.Items)
                        FileEx.AppendAllText(fname, li.ToString() + "\r\n");

                    bincludesrc = chksource.Checked;
                    importfile = txtimport.Text;

                }
                config.AppSettings.Settings.Remove("filediffexe");
                config.AppSettings.Settings.Add("filediffexe", fdexetxt.Text);

                config.AppSettings.Settings.Remove("filediffargs");
                config.AppSettings.Settings.Add("filediffargs", fdargstxt.Text);
                config.Save();
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.InitialDirectory = rootdir;
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (var fname in ofd.FileNames)
                {
                    if (fname.IndexOf(rootdir) == 0)
                        lstfiles.Items.Add(fname.Substring(rootdir.Length + 1));
                }
            }
        }

        private void btnremove_Click(object sender, EventArgs e)
        {
            if (lstfiles.SelectedIndex != -1)
            {
                lstfiles.Items.RemoveAt(lstfiles.SelectedIndex);
                lstfiles.SelectedIndex = -1;
            }
        }

        private void cmdaddfiles_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.Filter = "File List|.txt";
            if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            lstfiles.Items.Clear();
            string fname = ofd.FileName;
            if (bcreatepatch && FileEx.Exists(fname))
            {
                string[] lines = FileEx.ReadAllLines(fname);
                foreach (var l in lines)
                    lstfiles.Items.Add(l);
            }

        }

        private void btnimport_Click(object sender, EventArgs e)
        {
            txtimport.Text = "";
            var ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.Filter = "File List|*.txt";
            if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            txtimport.Text = ofd.FileName;

        }

        private void btnimportarchive_Click(object sender, EventArgs e)
        {
            txtimport.Text = "";
            chksource.Checked = false;
            var ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.Filter = "Patch Archive File|*.zip";
            if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            
            string zipfile = ofd.FileName;

            string stagedir = driver.outputpath + "\\stage";
            DirectoryEx.Delete(stagedir,true);
            DirectoryEx.CreateDirectory(stagedir);

            if (FileEx.Exists(zipfile))
            {
                string mkzipfile = driver.outputpath + "\\mkzip.vbs";
                FileEx.WriteAllText(mkzipfile, Resource1.makezip);
                Process p = new Process();
                p.StartInfo.FileName = mkzipfile;
                p.StartInfo.Arguments = string.Format("\"{1}\" \"{0}\"", stagedir, zipfile);
                p.Start();
                p.WaitForExit();

                string[] cpfiles = { "\\Patch_meta.csv", "\\Patch_data.dat", "\\addtional_input.txt", "\\Patch_src_data.dat", "\\removal_input.txt" };
                foreach (var f in cpfiles)
                {
                    if (FileEx.Exists(inoutdir + f))
                        FileEx.Delete(inoutdir + f);
                    if (FileEx.Exists(stagedir + f))
                        FileEx.Move(stagedir + f, inoutdir + f);
                }

                filestype.SelectedIndex = 0;
                //lstfiles.Items.Clear();
                //string fname = inoutdir + "\\addtional_input.txt";
                //if (File.Exists(fname))
                //{
                //    string[] lines = FileEx.ReadAllLines(fname);
                //    foreach (var l in lines)
                //        lstfiles.Items.Add(l);
                //}

                chksource.Checked = FileEx.Exists(inoutdir + "\\Patch_src_data.dat");
            }

        }

        private void fdexebtn_Click(object sender, EventArgs e)
        {
            fdexetxt.Text = "";
            var ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.Filter = "Exe|*.exe";
            if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            fdexetxt.Text = ofd.FileName;

        }

        private void filestype_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstfiles.Items.Clear();
            string fname = (filestype.SelectedIndex == 0) ? (inoutdir + "\\addtional_input.txt") : (inoutdir + "\\removal_input.txt");
            if (bcreatepatch && FileEx.Exists(fname))
            {
                string[] lines = FileEx.ReadAllLines(fname);
                foreach (var l in lines)
                    lstfiles.Items.Add(l);
            }
        }
    }
}
