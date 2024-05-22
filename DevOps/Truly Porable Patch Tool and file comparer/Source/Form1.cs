using PortablePatchTool;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;

using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MigrationHelper
{
    public partial class Form1 : Form
    {
        public bool bincludesrc = false;
        public string importfile = "";

        public Form1()
        {
            InitializeComponent();
            driver.logtxt = run_output;
            driver.pbar = progressBar1;

            Loadsettings();
            tabControl.SelectedTab = tabControl.TabPages["comparepage"];
            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                compare_left_txt.Text = args[1];
                if (args.Length > 2)
                    compare_right_txt.Text = args[2];
            }
        }

        void cleartext(TextBox intxt)
        {
            intxt.Text = "";
        }

        void getfolder(TextBox intxt)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            fd.ShowNewFolderButton = true;
            fd.SelectedPath = intxt.Text;
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                intxt.Text = fd.SelectedPath;
        }

        private void apply_xv_btn_Click(object sender, EventArgs e)
        {
            getfolder(apply_xv_txt);
        }

        private void apply_output_btn_Click(object sender, EventArgs e)
        {
            getfolder(apply_output_txt);
        }

        private void create_xv_original_btn_Click(object sender, EventArgs e)
        {
            getfolder(create_xv_original_txt);
        }

        private void create_xv_updated_btn_Click(object sender, EventArgs e)
        {
            getfolder(create_xv_updated_txt);
        }

        private void create_output_btn_Click(object sender, EventArgs e)
        {
            getfolder(create_output_txt);
        }


        private void create_run_btn_Click(object sender, EventArgs e)
        {
            run_output.Text = "";
            driver.pbar = progressBar1;
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();
            if (importfile == "")
                driver.Comparefolders(create_xv_original_txt.Text, create_xv_updated_txt.Text, true);
            else
                FileEx.Copy(importfile, driver.outputpath + "\\dir_diff.csv", true);
            
            run_output.Text = "";
            driver.CreatePatch(create_xv_original_txt.Text, create_output_txt.Text, create_xv_updated_txt.Text, bincludesrc);
            Cursor.Current = Cursors.Arrow;

        }

        private void apply_run_Click(object sender, EventArgs e)
        {
            run_output.Text = "";
            driver.pbar = progressBar1;
            Form3 pfrm = new Form3();
            pfrm.parentpath = apply_xv_txt.Text;
            pfrm.inoutdir = apply_output_txt.Text;
            pfrm.ShowDialog();
        }

        private void btnanalyze_Click(object sender, EventArgs e)
        {
            Form2 pfrm = new Form2();
            pfrm.mode = 1;
            pfrm.parentpath = apply_xv_txt.Text;
            pfrm.inoutdir = apply_output_txt.Text;
            pfrm.ShowDialog();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var frm = new Form4();
            frm.importfile = importfile;
            frm.rootdir = create_xv_updated_txt.Text;
            frm.inoutdir = create_output_txt.Text;
            frm.bincludesrc = FileEx.Exists(create_output_txt.Text + "\\Patch_src_data.dat");
            if (frm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            bincludesrc = frm.bincludesrc;
            importfile = frm.importfile;
        }

        private void compare_left_btn_Click(object sender, EventArgs e)
        {
            getfolder(compare_left_txt);
        }

        private void compare_right_btn_Click(object sender, EventArgs e)
        {
            getfolder(compare_right_txt);
        }

        private void Loadsettings()
        {
            string outputdir = driver.outputpath;
            if (DirectoryEx.Exists(outputdir))
                DirectoryEx.Delete(outputdir, true);
            DirectoryEx.CreateDirectory(outputdir);
            DirectoryEx.CreateDirectory(outputdir + "\\stage");

            System.Configuration.Configuration config =
                          ConfigurationManager.OpenExeConfiguration(
                          ConfigurationUserLevel.None);

            if (config.AppSettings.Settings["compare_left"] != null)
                compare_left_txt.Text = config.AppSettings.Settings["compare_left"].Value;

            if (config.AppSettings.Settings["compare_right"] != null)
                compare_right_txt.Text = config.AppSettings.Settings["compare_right"].Value;

            if (config.AppSettings.Settings["create_xv_updated"] != null)
                create_xv_updated_txt.Text = config.AppSettings.Settings["create_xv_updated"].Value;

            if (config.AppSettings.Settings["create_xv_original"] != null)
                create_xv_original_txt.Text = config.AppSettings.Settings["create_xv_original"].Value;

            if (config.AppSettings.Settings["create_output"] != null)
                create_output_txt.Text = config.AppSettings.Settings["create_output"].Value;

            if (config.AppSettings.Settings["apply_xv_txt"] != null)
                apply_xv_txt.Text = config.AppSettings.Settings["apply_xv_txt"].Value;

            if (config.AppSettings.Settings["apply_output_txt"] != null)
                apply_output_txt.Text = config.AppSettings.Settings["apply_output_txt"].Value;

            if (config.AppSettings.Settings["exclude"] == null)
            {
                config.AppSettings.Settings.Add("exclude", ".copyarea.db;\\.svn;.jazz");
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }

            if (config.AppSettings.Settings["filediffexe"] == null || !FileEx.Exists(config.AppSettings.Settings["filediffexe"].ToString()))
            {
                string filename = driver.outputpath + "\\windiff.exe";
                FileEx.WriteAllBytes(filename, Resource1.windiff);
                if (config.AppSettings.Settings["filediffexe"] != null)
                {
                    config.AppSettings.Settings.Remove("filediffexe");
                    config.AppSettings.Settings.Remove("filediffargs");
                }
                config.AppSettings.Settings.Add("filediffexe", '"' + filename + '"');
                config.AppSettings.Settings.Add("filediffargs", "<file 1> <file 2>");

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }

        }

        private void Savesettings()
        {
            System.Configuration.Configuration config =
                          ConfigurationManager.OpenExeConfiguration(
                          ConfigurationUserLevel.None);

            config.AppSettings.Settings.Remove("compare_left");
            config.AppSettings.Settings.Add("compare_left", compare_left_txt.Text);

            config.AppSettings.Settings.Remove("compare_right");
            config.AppSettings.Settings.Add("compare_right", compare_right_txt.Text);

            config.AppSettings.Settings.Remove("create_xv_updated");
            config.AppSettings.Settings.Add("create_xv_updated", create_xv_updated_txt.Text);

            config.AppSettings.Settings.Remove("create_xv_original");
            config.AppSettings.Settings.Add("create_xv_original", create_xv_original_txt.Text);

            config.AppSettings.Settings.Remove("create_output");
            config.AppSettings.Settings.Add("create_output", create_output_txt.Text);

            config.AppSettings.Settings.Remove("apply_xv_txt");
            config.AppSettings.Settings.Add("apply_xv_txt", apply_xv_txt.Text);

            config.AppSettings.Settings.Remove("apply_output_txt");
            config.AppSettings.Settings.Add("apply_output_txt", apply_output_txt.Text);

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }


        private void btncompareload_Click(object sender, EventArgs e)
        {
            driver.pbar = progressBar1;
            Cursor.Current = Cursors.WaitCursor;
            driver.Comparefolders(compare_left_txt.Text, compare_right_txt.Text, false);
            Cursor.Current = Cursors.Arrow;

            if (driver.isfile(compare_left_txt.Text) || driver.isfile(compare_right_txt.Text))
                return;

            run_output.Text = "";
            Form5 cmpfrm = new Form5();
            cmpfrm.leftdir = compare_left_txt.Text;
            cmpfrm.rightdir = compare_right_txt.Text;
            cmpfrm.ShowDialog();
        }

        private void optionsbtn_Click(object sender, EventArgs e)
        {
            Form4 optfrm = new Form4();
            optfrm.ShowDialog();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Savesettings();
        }

        private void btnarchivefile_Click(object sender, EventArgs e)
        {
            getfolder(create_output_txt);
        }

        private void cmdoptions_Click(object sender, EventArgs e)
        {
            var frm = new Form4();
            frm.bcreatepatch = false;
            frm.ShowDialog();

        }

        private void btnexport_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.ValidateNames = true;
            sfd.Filter = "Patch Files|.zip";
            sfd.CheckFileExists = false;
            sfd.CheckPathExists = true;
            if (sfd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            string inoutdir = expimp_output_txt.Text;
            string stagedir = driver.outputpath + "\\stage";
            DirectoryEx.Delete(stagedir,true);
            DirectoryEx.CreateDirectory(stagedir);

            string[] cpfiles = { "\\Patch_meta.csv", "\\Patch_data.dat", "\\addtional_input.txt", "\\Patch_src_data.dat" };
            foreach (var f in cpfiles)
            {
                if (FileEx.Exists(inoutdir + f))
                    FileEx.Copy(inoutdir + f, stagedir + f,true);
            }

            string zipfile = sfd.FileName;
            string mkzipfile = driver.outputpath + "\\mkzip.vbs";
            FileEx.WriteAllBytes(zipfile, Resource1.dummy);
            FileEx.WriteAllText(mkzipfile, Resource1.makezip);
            Process p = new Process();
            p.StartInfo.FileName = mkzipfile;
            p.StartInfo.Arguments = string.Format("\"{0}\" \"{1}\"", stagedir, zipfile);
            p.Start();
            p.WaitForExit();
        }

        private void btnimporta_Click(object sender, EventArgs e)
        {
            string inoutdir = expimp_output_txt.Text;
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

                string[] cpfiles = { "\\Patch_meta.csv", "\\Patch_data.dat", "\\addtional_input.txt", "\\Patch_src_data.dat" };
                foreach (var f in cpfiles)
                {
                    if (FileEx.Exists(inoutdir + f))
                        FileEx.Delete(inoutdir + f);
                    if (FileEx.Exists(stagedir + f))
                        FileEx.Move(stagedir + f, inoutdir + f);
                }
            }
        }

        private void label6_DoubleClick(object sender, EventArgs e)
        {
            cleartext(create_xv_original_txt);
        }

        private void label7_DoubleClick(object sender, EventArgs e)
        {
            cleartext(create_xv_updated_txt);
        }

        private void label5_DoubleClick(object sender, EventArgs e)
        {
            cleartext(create_output_txt);
        }

        private void label10_DoubleClick(object sender, EventArgs e)
        {
            cleartext(apply_xv_txt);

        }

        private void label9_DoubleClick(object sender, EventArgs e)
        {
            cleartext(apply_output_txt);
        }

        private void label3_DoubleClick(object sender, EventArgs e)
        {
            cleartext(compare_left_txt);
        }

        private void label1_DoubleClick(object sender, EventArgs e)
        {
            cleartext(compare_right_txt);
        }

        private void btextract_Click(object sender, EventArgs e)
        {
            driver.ExtractFiles(expimp_output_txt.Text);

        }

        private void expimp_output_btn_Click(object sender, EventArgs e)
        {
            getfolder(expimp_output_txt);

        }

        private void textBoxDescription_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string filename = driver.outputpath + "\\Portable_Patching_Tool.chm";
            FileEx.WriteAllBytes(filename, Resource1.Portable_Patching_Tool);
            Process p = Process.Start(filename);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void compare_left_txt_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }

        private void compare_left_txt_DragOver(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[]; // get all files droppeds  
            if (files != null && files.Any())
                compare_left_txt.Text = files.First(); //select the first one  
        }

        private void compare_right_txt_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;

        }

        private void compare_right_txt_DragOver(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[]; // get all files droppeds  
            if (files != null && files.Any())
                compare_right_txt.Text = files.First(); //select the first one  

        }
    }
}
