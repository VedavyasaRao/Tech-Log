using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace ShowEnvVar
{
    public partial class Showvars : Form
    {
        public Showvars()
        {
            InitializeComponent();
            var allvars = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process);
            if (!allvars.Contains("InstallAppDir"))
                allvars.Add("InstallAppDir", "<Folder where this application is installed>");
            
            List<string>  avs = new List<string>();
            foreach (DictionaryEntry de in allvars)
                avs.Add(de.Key.ToString());
            avs.Sort();
            foreach (var k in  avs)
            {
                if (k == "Path" || (!allvars[k].ToString().Contains(':') && k != "InstallAppDir"))
                    continue;
                var li = listView1.Items.Add(k.ToUpper()).SubItems.Add(allvars[k].ToString());
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
                Clipboard.SetText("%"+listView1.SelectedItems[0].Text+"%");
            Close();

        }
    }
}
