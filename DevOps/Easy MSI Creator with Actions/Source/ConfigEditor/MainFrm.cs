using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using JSonSerializer;

namespace ConfigEditor
{
    public partial class MainFrm : Form
    {

        public MainFrm()
        {
            InitializeComponent();

        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            if (Program.title != "")
                this.Text = this.Text + " - " + Program.title;

            dataGridView1.Rows.Clear();

            var settings = JSONPersister<Dictionary<string, string>>.Read(Program.settingsfile);
            foreach (KeyValuePair<string, string> kv in settings)
            {
                dataGridView1.Rows.Add(new string[] { kv.Key, kv.Value });
            }
        }


        private void cmdSave_Click(object sender, EventArgs e)
        {
            var settings = new Dictionary<string, string>();
            foreach (DataGridViewRow dgr  in dataGridView1.Rows)
            {
                if (dgr.Cells[0].Value == null)
                {
                    MessageBox.Show("Name cannot be null");
                    return;
                }

                string val = "";
                if (dgr.Cells[1].Value != null)
                    val = dgr.Cells[1].Value.ToString();

                settings.Add(dgr.Cells[0].Value.ToString(), val);
            }
            JSONPersister<Dictionary<string, string>>.Write(Program.settingsfile, settings);
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void cmdenvvars_Click(object sender, EventArgs e)
        {
            new ShowEnvVar.Showvars().ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var browser = new FolderBrowserDialog();
            if (browser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Clipboard.SetText(browser.SelectedPath);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Guid.NewGuid().ToString("B"));

        }
    }
}
