﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace InstallerCreator
{
    public partial class HelpFrm : Form
    {
        public HelpFrm()
        {
            InitializeComponent();
        }

        private void usermanual_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string helpfile = Path.GetTempPath() + "\\CreatorUseManual.chm";
            File.WriteAllBytes(helpfile, Resource1.InstallerCreator_User_Manual);
            Process.Start(helpfile);
        }

   }
}