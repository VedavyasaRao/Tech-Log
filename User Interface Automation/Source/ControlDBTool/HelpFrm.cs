using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace UITesting.Automated.ControlDBTool
{
    public partial class HelpFrm : Form
    {
        public HelpFrm()
        {
            InitializeComponent();
        }

        private void usermanual_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string helpfile = @"c:\temp\controldb.chm";
            File.WriteAllBytes(helpfile,Resource1.ControDBTool);
            Process.Start(helpfile);
        }

        private void uiadreference_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string helpfile = @"c:\temp\uiadriver reference.chm";
            File.WriteAllBytes(helpfile, Resource1.uiadriver_reference);
            Process.Start(helpfile);

        }
    }
}
