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
using UITesting.Automated.ControlInf;
using UITesting.Automated.JSonSerializer;

//TICS -7@101  -- not relevant here
//TICS -10@301  -- not relevant here

namespace UITesting.Automated.ControlDBTool
{
    /// <summary>Form2</summary>
    public partial class Form2 : Form
    {
        /// <summary>filename</summary>
        public string filename;
        List<ControlInfo> selectednodes;
        private bool bsaved=false;

        /// <summary>Form2</summary>
        public Form2(string fname, List<ControlInfo> selnodes)
        {
            InitializeComponent();
            filename = fname;
            selectednodes = selnodes;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(txtprogid, "Enter a unique program id. e.g., workspot.wsc");
            toolTip1.SetToolTip(txtDesc, "Enter a description");
            toolTip1.SetToolTip(txtVer, "Enter a version number");

            txtLoc.Enabled = true;
            txtClsid.Enabled = true;
            txtprogid.Enabled = true;
            btnFF.Enabled = true;
            txtLoc.Enabled = false;

            if (filename != "")
            {
                txtLoc.Text = filename;

                string buffer = File.ReadAllText(filename);
                
                int p = buffer.IndexOf("description=\"");
                if (p != -1)
                {
                    p+=13;
                    int p2 = buffer.IndexOf("\"",p);
                    txtDesc.Text = buffer.Substring(p, p2 - p);
                }

                txtClsid.Enabled = false;
                p = buffer.IndexOf("classid=\"");
                if (p != -1)
                {
                    p+=10;
                    int p2 = buffer.IndexOf("\"",p)-1;
                    txtClsid.Text = buffer.Substring(p, p2 - p);
                }

                txtprogid.Enabled = false;
                p = buffer.IndexOf("progid=\"");
                if (p != -1)
                {
                    p += 8;
                    int p2 = buffer.IndexOf(".", p);
                    txtprogid.Text = buffer.Substring(p, p2 - p);
                }

                p = buffer.IndexOf("version=\"",20);
                if (p != -1)
                {
                    p += 9;
                    int p2 = buffer.IndexOf("\"", p);
                    txtVer.Text = buffer.Substring(p, p2 - p);
                }
                btnFF.Enabled = false;
            }
            else
            {
                txtClsid.Text = Guid.NewGuid().ToString();
            }
        }

        private void btnFF_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtLoc.Text = dlg.FileName + ".WSC";
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            bsaved = false;
            if (txtprogid.Text == "")
            {
                MessageBox.Show("Program Id cannot be blank");
                return;

            }

            if (txtVer.Text == "")
            {
                MessageBox.Show("Version cannot be blank");
                return;

            }

            if (txtDesc.Text == "")
            {
                MessageBox.Show("Description cannot be blank");
                return;

            }

            if (txtLoc.Text == "")
            {
                MessageBox.Show("File location cannot be blank");
                return;

            }
            DialogResult = DialogResult.OK;
            string jsbuffer = Resource1.json2;

            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\"?>\r\n");
            sb.Append("<component>\r\n");
            sb.Append("<?component error=\"true\" debug=\"true\"?>\r\n");
            sb.Append("<registration\r\n");
            sb.Append("description=\"Testing TestWSC\"\r\n");
            sb.AppendFormat("progid=\"{0}.WSC\"\r\n",txtprogid.Text);
            sb.AppendFormat("version=\"{0}\"\r\n",txtVer.Text);
            sb.Append("classid=\"{"+txtClsid.Text+"}\"\r\n");
            sb.Append("> </registration>\r\n");
            sb.Append("<public>\r\n");
            foreach (ControlInfo ctlinfo in selectednodes)
            {
                sb.AppendFormat("<property name=\"{0}\">\r\n", ctlinfo.UserName);
                sb.Append("<get/>\r\n");
                sb.Append("</property>\r\n");
            }

            sb.Append("</public>\r\n");

            sb.Append("<script language=\"JScript\">\r\n");
            sb.Append("<![CDATA[\r\n");
            sb.Append(jsbuffer+"\r\n");
            sb.AppendFormat("var usersel = {0}\r\n", JSONPersister<List<ControlInfo>>.GetJSON(selectednodes));
            sb.Append("var description = new TestWSC;\r\n");
            sb.Append("function TestWSC()\r\n");
            sb.Append("{\r\n");
            foreach (ControlInfo ctlinfo in selectednodes)
            {
                sb.AppendFormat("this.get_{0}=get_{1};\r\n", ctlinfo.UserName, ctlinfo.UserName);
            }
            sb.Append("this.GetDetails = GetDetails;\r\n");
            sb.Append("}\r\n");
            
            foreach (ControlInfo ctlinfo in selectednodes)
            {
                sb.AppendFormat(" function get_{0}()\r\n", ctlinfo.UserName);
                sb.Append("{\r\n");
                sb.AppendFormat(" return GetDetails(\"{0}\");\r\n", ctlinfo.UserName);
                sb.Append("}\r\n");
            }

            sb.Append(@"
            function GetDetails(username)
            {
   	        var temp = [];
            for (var i = 0; i < usersel.length; ++i)
            {
            if (usersel[i].UserName == username)
            {
	        temp[0] = usersel[i];
	        break;
            }
            }
        	
            for (var i = 0; i < usersel.length; ++i)
            {
            if (usersel[i].UserName == 'root')
            {
	        temp[1] = usersel[i];
	        break;
            }
            }
            return  JSON.stringify(temp);
            }");

            sb.Append("\r\n]]>\r\n");
            sb.Append("</script>\r\n");
            sb.Append("</component>\r\n");
            File.WriteAllText(txtLoc.Text, sb.ToString());

            filename = txtLoc.Text;
            bsaved = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtLoc.Enabled = false;
            txtLoc.Text = "";

            txtDesc.Enabled = true;
            txtDesc.Text = "";

            txtClsid.Enabled = false;
            txtClsid.Text = Guid.NewGuid().ToString();

            txtprogid.Enabled = true;
            txtprogid.Text = "";

            txtVer.Enabled = true;
            txtVer.Text = "";

            btnFF.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            const int winxp = 5;
            btnsave_Click(sender, e);
            if (bsaved && filename != "")
            {
                string sysdir = "C:\\Windows\\system32\\";
                OperatingSystem os = Environment.OSVersion;
                if (os.Version.Major > winxp)
                {
                    sysdir = "C:\\Windows\\SysWOW64\\";
                }
                string dirname = System.IO.Path.GetDirectoryName(filename);
                string exename = string.Format("{0}cmd.EXE", sysdir);
                string args = string.Format("/c REGSVR32.EXE /s \"{0}\"", filename);
                ProcessStartInfo pst = new ProcessStartInfo(exename, args);
                pst.WorkingDirectory = sysdir;
                Process.Start(pst);

                string tlbfile = System.IO.Path.ChangeExtension(filename, ".tlb");
                args = string.Format("\"{0}scrobj.dll\",GenerateTypeLib -file:\"{1}\" \"{2}\"", sysdir, tlbfile, filename);
                exename = string.Format("{0}RUNDLL32.EXE", sysdir);
                pst = new ProcessStartInfo(exename, args);
                pst.WorkingDirectory = dirname;
                Process.Start(pst);
            }
        }
    }
}
