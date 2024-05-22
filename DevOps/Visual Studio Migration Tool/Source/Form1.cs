using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace MigrationHelper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            driver.logtxt = run_output;
            driver.pbar = progressBar1;
        }

        void getfolder(TextBox intxt)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            fd.ShowNewFolderButton = true;
            fd.SelectedPath = intxt.Text;
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                intxt.Text =  fd.SelectedPath;
            

        }


        private void up_xv_btn_Click(object sender, EventArgs e)
        {
            getfolder(up_xv_txt);

        }

        private void up_output_btn_Click(object sender, EventArgs e)
        {
            getfolder(up_output_txt);

        }

        private void list_xv_btn_Click(object sender, EventArgs e)
        {
            getfolder(list_xv_txt);

        }

        private void list_output_btn_Click(object sender, EventArgs e)
        {
            getfolder(list_output_txt);

        }


        private void list_run_Click(object sender, EventArgs e)
        {
            run_output.Text = "";
            driver.pbar = progressBar1;
            Cursor.Current = Cursors.WaitCursor;
            driver.ListProjectsandSolutions(list_xv_txt.Text, list_output_txt.Text);
            Cursor.Current = Cursors.Arrow;
        }

        private void up_run_Click(object sender, EventArgs e)
        {
            run_output.Text = "";
            driver.pbar = progressBar1;
            Cursor.Current = Cursors.WaitCursor;
            driver.UpdateProjects(up_xv_txt.Text, up_output_txt.Text);
            Cursor.Current = Cursors.Arrow;

        }


        private void vs_xv_btn_Click(object sender, EventArgs e)
        {
            getfolder(vs_xv_txt);

        }

        private void vs_output_btn_Click(object sender, EventArgs e)
        {
            getfolder(vs_output_txt);

        }

        private void vs_run_btn_Click(object sender, EventArgs e)
        {
            run_output.Text = "";
            driver.pbar = progressBar1;
            Cursor.Current = Cursors.WaitCursor;
            driver.UpgradeProjects(vs_xv_txt.Text, vs_output_txt.Text, up_rad_sln.Checked);
            Cursor.Current = Cursors.Arrow;

        }


        private void button1_Click(object sender, EventArgs e)
        {
            run_output.Text = "";
            driver.pbar = progressBar1;
            Cursor.Current = Cursors.WaitCursor;
            driver.Getruntimes(list_xv_txt.Text, list_output_txt.Text);
            Cursor.Current = Cursors.Arrow;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            run_output.Text = "";
            driver.pbar = progressBar1;
            Cursor.Current = Cursors.WaitCursor;
            driver.Dependents(list_xv_txt.Text, list_output_txt.Text);
            Cursor.Current = Cursors.Arrow;

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            savesettings();
        }

        private  void loadsettings()
        {
            if (ConfigurationManager.AppSettings["list_xv_txt"] == null)
                ConfigurationManager.AppSettings["list_xv_txt"] = @"";

            if (ConfigurationManager.AppSettings["list_output_txt"] == null)
                ConfigurationManager.AppSettings["list_output_txt"] = @"";

            if (ConfigurationManager.AppSettings["vs_xv_txt"] == null)
                ConfigurationManager.AppSettings["vs_xv_txt"] = @"";

            if (ConfigurationManager.AppSettings["vs_output_txt"] == null)
                ConfigurationManager.AppSettings["vs_output_txt"] = @"";

            if (ConfigurationManager.AppSettings["up_xv_txt"] == null)
                ConfigurationManager.AppSettings["up_xv_txt"] = @"";

            if (ConfigurationManager.AppSettings["up_output_txt"] == null)
                ConfigurationManager.AppSettings["up_output_txt"] = @"";

            if (ConfigurationManager.AppSettings["up_tool_ver_txt"] == null)
                ConfigurationManager.AppSettings["up_tool_ver_txt"] = "14";

            if (ConfigurationManager.AppSettings["up_toolset_txt"] == null)
                ConfigurationManager.AppSettings["up_toolset_txt"] = "v140";

            if (ConfigurationManager.AppSettings["up_fw_txt"] == null)
                ConfigurationManager.AppSettings["up_fw_txt"] = "4.5.2";

            list_xv_txt.Text = ConfigurationManager.AppSettings["list_xv_txt"];
            list_output_txt.Text = ConfigurationManager.AppSettings["list_output_txt"];
            vs_xv_txt.Text = ConfigurationManager.AppSettings["vs_xv_txt"];
            vs_output_txt.Text = ConfigurationManager.AppSettings["vs_output_txt"];
            up_xv_txt.Text = ConfigurationManager.AppSettings["up_xv_txt"];
            up_output_txt.Text = ConfigurationManager.AppSettings["up_output_txt"];

        }
        private void savesettings()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            config.AppSettings.Settings.Remove("list_xv_txt");
            config.AppSettings.Settings.Remove("list_output_txt");
            config.AppSettings.Settings.Remove("vs_xv_txt");
            config.AppSettings.Settings.Remove("vs_output_txt");
            config.AppSettings.Settings.Remove("up_xv_txt");
            config.AppSettings.Settings.Remove("up_output_txt");
            config.AppSettings.Settings.Remove("up_tool_ver_txt");
            config.AppSettings.Settings.Remove("up_toolset_txt");
            config.AppSettings.Settings.Remove("up_fw_txt");

            config.AppSettings.Settings.Add("list_xv_txt", list_xv_txt.Text);
            config.AppSettings.Settings.Add("list_output_txt",list_output_txt.Text);
            config.AppSettings.Settings.Add("vs_xv_txt", vs_xv_txt.Text);
            config.AppSettings.Settings.Add("vs_output_txt", vs_output_txt.Text);
            config.AppSettings.Settings.Add("up_xv_txt", up_xv_txt.Text);
            config.AppSettings.Settings.Add("up_output_txt", up_output_txt.Text);
            config.Save(ConfigurationSaveMode.Modified);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadsettings();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
