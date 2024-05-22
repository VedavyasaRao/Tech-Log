using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SetupLayout;
using JSonSerializer;
using System.Diagnostics;
using System.Configuration;


namespace InstallerCreator
{
    public partial class savefrm : Form
    {

        public string filename 
        {
            get
            {
                return txtloc.Text;
            }

        }

        storagenode sn;
        container cn = new container();
        public savefrm(storagenode sn, string filename, bool bfrmmsi)
        {
            try
            {
                InitializeComponent();
                this.sn = sn;
                if (File.Exists(filename))
                {
                    cn = layout.getcontainer(filename);

                    txtname.Text = cn.propmapinfo["ProductName"];
                    txtloc.Text = filename;
                    txtpreinstall.Text = cn.preinstall;
                    txtpreinstallargs.Text = cn.preinstallargs;
                    txtpreinstallargs.Enabled = (cn.preinstall != "");
                    txtpostinstall.Text = cn.postinstall;
                    txtpostinstallargs.Text = cn.postinstallargs;
                    txtpostinstallargs.Enabled = (cn.postinstall != "");
                    txtpreuninstall.Text = cn.preuninstall;
                    txtpreuninstallargs.Text = cn.preuninstallargs;
                    txtpreuninstallargs.Enabled = (cn.preuninstall != "");
                    txtpostuninstall.Text = cn.postuninstall;
                    txtpostuninstallargs.Text = cn.postuninstallargs;
                    txtpostuninstallargs.Enabled = (cn.postuninstall != "");
                    chkfolders.Checked = cn.bask;
                }

                if (bfrmmsi)
                {
                    txtname.Text = "sample";
                    txtloc.Text = filename;
                }
                else
                {
                    List<string> parlist = new List<string>();
                    getparameters(sn, ref parlist);
                    foreach (string s in parlist)
                    {
                        string temp;
                        if (!cn.dirmapinfo.TryGetValue(s, out temp))
                        {
                            string key = '%' + s + '%';
                            string val = Environment.ExpandEnvironmentVariables(key);
                            cn.dirmapinfo.Add(s, (val == key) ? "" : val);
                        }
                    }

                }

                groupBox1.Enabled = !bfrmmsi;
            }
            catch (Exception ex)
            {
                MessageBox.Show("an exception occured in savefrm()\r\n" + ex.Message);
            }
        }

        private void btnloc_Click(object sender, EventArgs e)
        {
            
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            txtloc.Text = dlg.SelectedPath + "\\" + txtname.Text + "_" + cn.propmapinfo["ProductVersion"] + ".layout";
        }

        private void btnok_Click(object sender, EventArgs e)
        {
            if (txtname.Text == "")
            {
                MessageBox.Show("name cannot be blank");
                return;
            }


            if (txtloc.Text == "")
            {
                MessageBox.Show("location cannot be blank");
                return;
            }
            writedata(sn);
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();

        }

        private void writedata(storagenode node)
        {
            try
            {
                cn.propmapinfo["ProductName"] = txtname.Text;
                cn.preinstall = txtpreinstall.Text;
                cn.preinstallargs = txtpreinstallargs.Text;
                cn.postinstall = txtpostinstall.Text;
                cn.postinstallargs = txtpostinstallargs.Text;
                cn.preuninstall = txtpreuninstall.Text;
                cn.preuninstallargs = txtpreuninstallargs.Text;
                cn.postuninstall = txtpostuninstall.Text;
                cn.postuninstallargs = txtpostuninstallargs.Text;
                cn.packageguid = Guid.NewGuid().ToString("B");
                cn.bask = chkfolders.Checked;

                if (chkrelative.Checked)
                {
                    layout.mergepath(cn, Path.GetDirectoryName(filename),true);
                    layout.mergepath(node, Path.GetDirectoryName(filename), true);
                }
                layout.write(filename, cn, node);
            }
            catch (Exception ex)
            {
                MessageBox.Show("an exception occured in savefrm()\r\n" + ex.Message);
            }
        }

        private void getparameters(storagenode node, ref List<string> parlist)
        {
            string[] oneparlist = node.nodedata.name.Split(new char[] { '%' });
            if (oneparlist.Length > 1)
            {
                foreach (string s in oneparlist)
                {
                    if (s != "" && s.IndexOf('\\') == -1)
                        parlist.Add(s);
                }
            }
            if (node.children.Count > 0)
            {
                foreach (storagenode sn in node.children)
                {
                    getparameters(sn, ref parlist);
                }
            }
        }


        private void btnSetDir_Click(object sender, EventArgs e)
        {
            string filename = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\temp.settings";
            string exename = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\ConfigEditor.exe";
            JSONPersister<Dictionary<string, string>>.Write(filename, cn.dirmapinfo);
            Process p = new Process();

            p.StartInfo.FileName = exename;
            p.StartInfo.Arguments = "-f:\""+ filename + "\" -t:\"Update Target Directories\"";
            p.Start();
            p.WaitForExit();
            cn.dirmapinfo = JSONPersister<Dictionary<string, string>>.Read(filename);
            Activate();


        }

        private void txtloc_DoubleClick(object sender, EventArgs e)
        {
            TextBox tbox = ((TextBox)sender);
            if (MessageBox.Show("Do you want to clear contents?", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                return;
            tbox.Text = "";
            if (tbox.Name == "txtpreinstall")
            {
                txtpreinstallargs.Text = "";
                txtpreinstallargs.Enabled = false;
            }
            else if (tbox.Name == "txtpostinstall")
            {
                txtpostinstallargs.Text = "";
                txtpostinstallargs.Enabled = false;
            }
            else if (tbox.Name == "txtpreuninstall")
            {
                txtpreuninstallargs.Text = "";
                txtpreuninstallargs.Enabled = false;
            }
            else if (tbox.Name == "txtpostuninstall")
            {
                txtpostuninstallargs.Text = "";
                txtpostuninstallargs.Enabled = false;
            }

        }


        private void btnpreinstall_Click(object sender, EventArgs e)
        {
            txtpreinstall.Text = "";
            txtpreinstallargs.Enabled = false;
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtpreinstall.Text = ofd.FileName;
                txtpreinstallargs.Enabled = true;
            }

        }

        private void btnpostinstall_Click(object sender, EventArgs e)
        {
            txtpostinstall.Text = "";
            txtpostinstallargs.Enabled = false;
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtpostinstall.Text = ofd.FileName;
                txtpostinstallargs.Enabled = true;
            }

        }

        private void btnpreuninstall_Click(object sender, EventArgs e)
        {

            txtpreuninstall.Text = "";
            txtpreuninstallargs.Enabled = false;
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtpreuninstall.Text = ofd.FileName;
                txtpreuninstallargs.Enabled = true;
            }
        }

        private void btnpostuninstall_Click(object sender, EventArgs e)
        {
            txtpostuninstall.Text = "";
            txtpostuninstallargs.Enabled = false;
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtpostuninstall.Text = ofd.FileName;
                txtpostuninstallargs.Enabled = true;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var temps = ConfigurationSettings.AppSettings["Customize"];
            if (string.IsNullOrEmpty(temps))
            {
                string[] props = temps.Split(';');
                foreach (var pp in props)
                {
                    if (!cn.propmapinfo.ContainsKey(pp))
                        cn.propmapinfo.Add(pp, "");
                }
            }
            string filename = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\temp.settings";
            string exename = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\ConfigEditor.exe";

            JSONPersister<Dictionary<string, string>>.Write(filename, cn.propmapinfo);
            Process p = new Process();
            p.StartInfo.FileName =  exename;
            p.StartInfo.Arguments = "-f:\"" + filename + "\" -t:\"Update Installer properties\"";
            p.Start();
            p.WaitForExit();
            cn.propmapinfo = JSONPersister<Dictionary<string, string>>.Read(filename);
            Activate();
        }
    }
}
