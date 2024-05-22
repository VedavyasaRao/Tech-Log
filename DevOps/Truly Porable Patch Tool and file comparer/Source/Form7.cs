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

namespace PortablePatchTool
{
    public partial class Form7 : Form
    {
        public string filtertext="";
        public bool bexclude;
        public bool bpatch = false;

        public Form7()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bexclude = optionsrad.Checked;
            filtertext = txtinclude.Text;
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void link_Click(string opt)
        {
            optionsrad.Checked=string.IsNullOrEmpty(opt);
            optionsrad2.Checked = !optionsrad.Checked;
            filtertext = opt;
            bexclude = optionsrad.Checked;
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            optionsrad.Checked = bexclude;
            optionsrad2.Checked = !bexclude;
            txtinclude.Text = filtertext;
            linkLabel3.Text = bpatch ? "New" : "Left";
            linkLabel4.Text = bpatch ? "Same" : "Right";

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            link_Click("");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            link_Click(bpatch ? "Diff" : ",Diff");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            link_Click(bpatch ? "New" : ",Left");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            link_Click(bpatch ? "Same" : ",Right");
        }

        private void linkLabel5_Click(object sender, EventArgs e)
        {
            link_Click(bpatch ? "Same" : ",Same");

        }
    }
}
