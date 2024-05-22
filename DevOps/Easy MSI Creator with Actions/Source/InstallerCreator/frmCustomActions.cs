using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SetupLayout;

namespace InstallerCreator
{
    public partial class frmCustomActions : Form
    {
        public Dictionary<string, Customaction> actions = new Dictionary<string, Customaction>();
        Customaction curaction;
        public frmCustomActions(Dictionary<string, Customaction> actions)
        {
            InitializeComponent();
            this.actions = actions;
        }

        private void populate(Customaction action, bool bview)
        {
            if (bview)
            {
                txtname.Text = action.name;
                txtargs.Text = action.args;
                txtexename.Text = action.executable;
                chkfolder.Checked = action.folderlevel;
                chkleaf.Checked = action.leaflelvel;
                txtfilter.Text = action.filter;
                cmbpriority.SelectedIndex = action.priority;
                chkgrp.Checked = action.grpaction;
                chkwfe.Checked = action.wfe;
                chkInstall.Checked = action.binstall;
                chkuinstall.Checked = action.buninstall;
                txttooltip.Text = action.tooltip;
            }
            else
            {
                action.name= txtname.Text;
                action.args = txtargs.Text;
                action.executable = txtexename.Text;
                action.folderlevel = chkfolder.Checked;
                action.leaflelvel = chkleaf.Checked;
                action.filter = txtfilter.Text;
                action.priority= cmbpriority.SelectedIndex;
                action.grpaction = chkgrp.Checked;
                action.wfe = chkwfe.Checked;
                action.binstall=chkInstall.Checked ;
                action.buninstall=chkuinstall.Checked;
                action.tooltip = txttooltip.Text;
            }

        }

        private bool Validate(Customaction action)
        {
            if (action.name == "")
            {
                MessageBox.Show("name is blank");
                return false;
            }
            else if (!action.folderlevel && !action.leaflelvel)
            {
                MessageBox.Show("at least folder or leaf should be checked");
                return false;
            }

            else if (!action.binstall && !action.buninstall)
            {
                MessageBox.Show("at least install or uninstall should be checked");
                return false;
            }
            
            return true;
        }

        private void btnnew_Click(object sender, EventArgs e)
        {
            curaction = new Customaction();
            populate(curaction,true);
            txtname.Enabled = true;
            btnupdate.Enabled = true;
            cmbActions.SelectedIndex = -1;
            groupBox1.Enabled = true;
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            populate(curaction, false);
            if (Validate(curaction))
            {
                if (actions.ContainsKey(curaction.name))
                    actions[curaction.name] = curaction;
                else
                {
                    actions.Add(curaction.name, curaction);
                    int x = cmbActions.Items.Add(curaction.name);
                    cmbActions.SelectedIndex = x;
                }
            }

        }

        private void cmbActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbActions.SelectedIndex == -1)
                return;
            curaction=actions[(string)cmbActions.Items[cmbActions.SelectedIndex]];
            populate(curaction, true);
            btnupdate.Enabled = true;
            txtname.Enabled = false;
            groupBox1.Enabled = !curaction.builtin;
        }

        private void btndel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do ou want to delete?", "delete", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                return;
            string key=(string)cmbActions.Items[cmbActions.SelectedIndex];
            cmbActions.Items.Remove(key);
            actions.Remove(key);
            if (cmbActions.Items.Count > 0)
                cmbActions.SelectedIndex = 0;
            else
                btnupdate.Enabled = false;

        }

        private void btnsave_Click(object sender, EventArgs e)
        {

        }

        private void frmCustomActions_Load(object sender, EventArgs e)
        {
            foreach (var kv in actions.Keys)
            {
                cmbActions.Items.Add(kv);
            }
            if (cmbActions.Items.Count > 0)
                cmbActions.SelectedIndex = 0;
        }

        private void chkleaf_CheckedChanged(object sender, EventArgs e)
        {
            txtfilter.Enabled = chkleaf.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists=true;
            ofd.CheckPathExists=true;
            ofd.Multiselect = false;
            ofd.Filter = "Exe  files|*.exe|VBS files|*.vbs|all files|*.*";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                txtexename.Text = ofd.FileName;
        }

    }
}
