using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using COMLibHelper;

using System.Reflection;

namespace LBCodeGenerator
{
    public partial class MainFrm : Form
    {
        Dictionary<string, Comobjectstorage> usersel = new Dictionary<string, Comobjectstorage>();
        delegate void delupdatemsg(string msg);
        delupdatemsg updmsg;

        private void updatemsg(string msg)
        {
            txtstatus.Text = msg;
            Application.DoEvents();
        }

        public void updatestatus(string msg)
        {
            if (InvokeRequired)
                Invoke(updmsg, new object[] { msg });
            else
                updatemsg(msg);
            Application.DoEvents();
        }

        public void Enabledisable(bool benable)
        {
            txtprogid.ReadOnly = !benable;
            btnupdate.Enabled = benable;
        }

        public MainFrm()
        {
            try
            {
                InitializeComponent();
                updmsg = new delupdatemsg(updatemsg);
                txtoutput.Text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\lbwiz";
                if (Directory.Exists(txtoutput.Text))
                    Directory.Delete(txtoutput.Text, true);
                Directory.CreateDirectory(txtoutput.Text);
            }
            catch (Exception ex)
            {
                updatemsg(ex.Message);
            }
        }

        private void btnnew_Click(object sender, EventArgs e)
        {
            try
            {
                Program.UpdateStatus("");
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.CheckFileExists = true;
                ofd.CheckPathExists = true;
                if (tabControl1.SelectedIndex == 3)
                    ofd.Filter = "Interfaces|*.cs";
                else
                    ofd.Filter = "Managed,Unmanaged,WSC|*.dll;*.tlb;*.Wsc";
                if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;
                {
                    if (ofd.FileName.ToLower().Contains(".wsc"))
                    {
                        string sysdir = Environment.ExpandEnvironmentVariables((Environment.OSVersion.Version.Major > 5) ? @"%windir%\SysWOW64\" : @"%Windir%\system32\");
                        Process p = new Process();
                        p.StartInfo.FileName = sysdir + "regsvr32.exe";
                        p.StartInfo.Arguments = "/s " + '"' + ofd.FileName + '"';
                        p.Start();
                        p.WaitForExit();

                        string outputdir = txtoutput.Text + "\\tlb";
                        if (!Directory.Exists(outputdir))
                            Directory.CreateDirectory(outputdir);

                        string tlbfile = outputdir + "\\" + System.IO.Path.ChangeExtension(Path.GetFileName(ofd.FileName), ".tlb");
                        p.StartInfo.FileName = sysdir + "RUNDLL32.EXE";
                        p.StartInfo.Arguments = string.Format("\"{0}scrobj.dll\",GenerateTypeLib -file:\"{1}\" \"{2}\"", sysdir, tlbfile, ofd.FileName);
                        p.Start();
                        p.WaitForExit();
                        if (!File.Exists(tlbfile))
                        {
                            Enabledisable(false);
                            return;
                        }
                        ofd.FileName = tlbfile;
                    }

                    Enabledisable(true);
                    txtprogid.Text = "";
                    txtpath.Text = ofd.FileName;
                    cmbfiles.SelectedIndex = -1;
                    bool bmanaged = true;
                    List<KeyValuePair<string, string>> progclsidlst = CodeGenHelper.GetclsidsfromAssembly(txtpath.Text);
                    if (progclsidlst.Count == 0)
                    {
                        progclsidlst = CodeGenHelper.GetclsidsfromTLB(txtpath.Text);
                        bmanaged = false;
                    }

                    Program.UpdateStatus("# components found:" + progclsidlst.Count.ToString());
                    int sidx = -1;
                    foreach (var kv in progclsidlst)
                    {
                        if (usersel.ContainsKey(kv.Key))
                            continue;
                        sidx = cmbfiles.Items.Add(kv.Key);
                        usersel.Add(kv.Key, new Comobjectstorage { path = txtpath.Text, clsid = new Guid(kv.Key), progid = kv.Value, bmanaged = bmanaged });
                    }
                    if (sidx != -1)
                        cmbfiles.SelectedIndex = sidx;
                }

            }
            catch (Exception ex)
            {
                Program.UpdateStatus(ex.Message);
            }
        }

        private void cmbfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbfiles.SelectedIndex == -1)
                return;

            if (tabControl1.SelectedIndex != 3)
            {
                var v = usersel[(string)cmbfiles.Items[cmbfiles.SelectedIndex]];
                txtprogid.Text = v.progid;
                txtpath.Text = v.path;
                txtprogid.ReadOnly = true;
                btnupdate.Enabled = false;
            }
        }

        private void btnedit_Click(object sender, EventArgs e)
        {
            Enabledisable(true);
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            int idx = cmbfiles.SelectedIndex;
            if (idx == -1)
                return;
            usersel[(string)cmbfiles.Items[idx]].progid = txtprogid.Text;
            Enabledisable(false);
        }

        private void btnremove_Click(object sender, EventArgs e)
        {
            int idx=cmbfiles.SelectedIndex;
            if (idx == -1)
                return;
            if (MessageBox.Show("Do you want to Delete?", "delete", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                return;

            string clsid = cmbfiles.Items[idx].ToString();
            cmbfiles.Items.RemoveAt(idx);
            usersel.Remove(clsid);
            cmbfiles.SelectedIndex = -1;
            if (cmbfiles.Items.Count > 0)
                cmbfiles.SelectedIndex = 0;

        }

        private bool validate()
        {
            Program.UpdateStatus("");
            if (usersel.Count == 0)
            {
                Program.UpdateStatus("At least one component must be selected");
                return false;
            }

            if (tabControl1.SelectedIndex == 0 && !chkcs.Checked && !chkvbs.Checked && !chkregfiles.Checked)
            {
                Program.UpdateStatus("At least one ouput option must be selected");
                return false;
            }

            if (tabControl1.SelectedIndex == 1 && txtwcfns.Text == "")
            {
                Program.UpdateStatus("wcf namespace cannot be blank");
                return false;
            }


            if (txtoutput.Text=="")
            {
                Program.UpdateStatus("output path cannot be blank");
                return false;
            }

            return true;
        }

        private void btngo_Click(object sender, EventArgs e)
        {
            if (!validate())
                return;
            string txtns = "";
            try
            {
                Program.lbops ops = 0;
                if (tabControl1.SelectedIndex == 0)
                {
                    if (chkcs.Checked)
                        ops |= Program.lbops.cs;

                    if (chkvbs.Checked)
                        ops |= Program.lbops.vb;

                    if (chkregfiles.Checked)
                        ops |= Program.lbops.reg;

                    if (chkincoptional.Checked)
                        ops |= Program.lbops.incopt;

                    if (chkruntime35.Checked)
                        ops |= Program.lbops.net35;
                }
                else if (tabControl1.SelectedIndex == 1)
                {
                    txtns = txtwcfns.Text;
                    ops |= Program.lbops.wcf;
                    if (chkwcfruntime35.Checked)
                        ops |= Program.lbops.net35;
                }
                if (chksupportcomplus.Checked)
                    ops |= Program.lbops.complus;

                //else if (tabControl1.SelectedIndex == 2)
                //{
                //    txtns = txtsscfserverns.Text;
                //    ops |= Program.lbops.sscfserver;
                //}
                //else if (tabControl1.SelectedIndex == 3)
                //{
                //    txtns = txtsscfclns.Text;
                //    ops |= Program.lbops.sscfclient;
                //}


                string outputdir = txtoutput.Text + "\\output";

                if (Directory.Exists(outputdir))
                    Directory.Delete(outputdir, true);
                Directory.CreateDirectory(outputdir);

                string tempdir = txtoutput.Text + "\\temp";

                if (Directory.Exists(tempdir))
                    Directory.Delete(tempdir, true);
                Directory.CreateDirectory(tempdir);


                Program.Process((from u in usersel select u.Value).ToList(), ops, txtns, outputdir);
                Program.UpdateStatus("Success");
            }
            catch (Exception ex)
            {
                Program.UpdateStatus("An error occured\r\n" + ex.Message);
            }

        }

        private void btnhelp_Click(object sender, EventArgs e)
        {
            HelpFrm hf = new HelpFrm();
            hf.ShowDialog();
        }


    }
}

