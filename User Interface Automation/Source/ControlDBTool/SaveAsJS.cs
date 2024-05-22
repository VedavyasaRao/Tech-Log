using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Resources;
using System.Reflection;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UITesting.Automated.ControlInf;
using UITesting.Automated.JSonSerializer;

//TICS -7@101  -- not relevant here
//TICS -10@301  -- not relevant here

namespace UITesting.Automated.ControlDBTool
{
    /// <summary>Form2</summary>
    public partial class SaveAsJS : Form
    {
        public ContainerDetails cd = new ContainerDetails("sample","sample");
        public string filename;
        private List<ControlInfoPair> selectednodes;
        private bool bsavemode;

        public SaveAsJS(string fname, ContainerDetails cd, bool bsavemode, List<ControlInfoPair> selnodes)
        {
            InitializeComponent();
            this.cd = cd;
            filename = fname;
            selectednodes = selnodes;
            this.bsavemode = bsavemode;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

            txtLoc.Text = filename == "" ? filename  : new FileInfo(filename).DirectoryName;
            txtid.Text = cd.id;
            txtVer.Text = cd.ver.ToString();
            txtDesc.Text = cd.desc;

            if (!bsavemode)
            {
                btnFF.Visible = false;
                txtLoc.Visible = false;
                lblselection.Visible = false;
                savejs.Visible = false;
                savewsc.Visible = false;
                btnsave.Text = "OK";
            }
        }

        private void btnFF_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.ShowNewFolderButton=true;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtLoc.Text = dlg.SelectedPath;

            }

        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            if (txtid.Text == "")
            {
                MessageBox.Show("Container Id cannot be blank");
                return;
            }

            Regex rx = new Regex("^[a-zA-Z][_a-zA-Z0-9]*?$");
            if (!rx.IsMatch(txtid.Text))
            {
                MessageBox.Show("Bad container Id");
                return;
            }

            if (txtVer.Text == "")
            {
                MessageBox.Show("Version cannot be blank");
                return;
            }

            int ver = 0;
            if (!int.TryParse(txtVer.Text, out ver))
            {
                MessageBox.Show("Bad container Version");
                return;
            }

            if (txtDesc.Text == "")
            {
                MessageBox.Show("Description cannot be blank");
                return;

            }

            if (bsavemode && txtLoc.Text == "")
            {
                MessageBox.Show("File location cannot be blank");
                return;

            }

            cd = new ContainerDetails("sample", "sample");
            cd.id = txtid.Text;
            cd.ver = ver;
            cd.desc = txtDesc.Text;

            DialogResult = DialogResult.OK;

            if (!bsavemode)
                return;
            
            if (savejs.Checked)
            {
                string fname = string.Format ("{0}\\{1}_{2}.JS", txtLoc.Text,cd.id,cd.ver);
                if (File.Exists(fname) && 
                    MessageBox.Show(fname + " exists \r\n Do you want to overwrite?", "Save", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                        return;

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0}\r\n", JSONPersister<object[]>.GetJSON(new object[] { cd, selectednodes }));
                File.WriteAllText(fname, sb.ToString());
                filename = fname;
            }

            if (savewsc.Checked)
            {
                string fname = string.Format("{0}\\{1}_{2}.WSC", txtLoc.Text, cd.id, cd.ver);
                string guid = "";
                if (!File.Exists(fname))
                {
                    guid = Guid.NewGuid().ToString();
                }
                else
                {
                    if (MessageBox.Show(fname + " exists \r\n Do you want to overwrite?", "Save", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                        return;
                    
                    string s = File.ReadAllText(fname);
                    int p1 = s.IndexOf("classid=");
                    if (p1 == -1)
                        return;
                    p1 = s.IndexOf('{', p1);
                    int p2 = s.IndexOf('}', p1+1);
                    guid = s.Substring(p1 + 1, p2 - p1-1);


                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<?xml version=\"1.0\"?>");
                sb.AppendLine("<component>");
                sb.AppendLine("<?component error=\"true\" debug=\"true\"?>");
                sb.AppendLine("<registration");
                sb.AppendFormat("description=\"{0}\"",cd.desc);
                sb.AppendLine();
                sb.AppendFormat("progid=\"{0}.WSC\"",cd.id);
                sb.AppendLine();
                sb.AppendFormat("version=\"{0}\"",cd.ver);
                sb.AppendLine();
                sb.AppendFormat("classid=\"{0}{1}{2}\"/>",'{',guid,'}');
                sb.AppendLine();
                sb.AppendLine("<public>");
                foreach (ControlInfoPair cp  in selectednodes)
                {
                    sb.AppendFormat("<property name=\"{0}\">",cp.ci.UserName);
                    sb.AppendLine();
                    sb.AppendLine("<get/>");
                    sb.AppendLine("</property>");
                }
                sb.AppendLine("</public>");
                
                sb.AppendLine("<script language=\"vbScript\">");
                sb.AppendLine("<![CDATA[");

                foreach (ControlInfoPair cip  in selectednodes)
                {
                    sb.AppendFormat("'{0}", cip.ci.Patterns);
                    sb.AppendLine();
                    sb.AppendFormat("Function get_{0}()", cip.ci.UserName);
                    sb.AppendLine();
                    sb.AppendFormat("    get_{0}=\"{1}\"", cip.ci.UserName, JSONPersister<ControlInfoPair>.GetJSON(cip).Replace("\"", "\"\"").Replace("\r\n", ""));
                    sb.AppendLine();
                    sb.AppendLine("End function");
                }


                sb.AppendLine("]]>");
                sb.AppendLine("</script>");
                sb.AppendLine("</component>");
    
                File.WriteAllText(fname, sb.ToString());
                filename = fname;
            }


        }

    }
}
