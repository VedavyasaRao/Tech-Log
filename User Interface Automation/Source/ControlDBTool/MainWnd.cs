using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Windows.Automation;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UITesting.Automated.ControlInf;
using UITesting.Automated.JSonSerializer;
using UITesting.Automated.MouseKeyboardActivityMonitor;
using UITesting.Automated.MouseKeyboardActivityMonitor.WinApi;

namespace UITesting.Automated.ControlDBTool
{
    /// <summary>Form1</summary>
    public partial class MainWnd : Form
    {

        int oldheight;
        int oldwidth;
        bool btoolbarmode;
        /// <summary>Form1</summary>
        public MainWnd()
        {
            try
            {
                Nativemethods.SetProcessDPIAware();
            }
            catch (EntryPointNotFoundException)
            {
                // Not running under Vista.
            }
            InitializeComponent();
            btoolbarmode = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !uiSelectionCtl1.CanClose();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            splitContainer1.SplitterDistance = splitContainer1.Height - 60;
        }

        public void ToggleTBMode(bool btoggle)
        {
            btoolbarmode = btoggle;
            splitContainer1.Panel2Collapsed = btoggle;
            TopMost = btoggle;
            if (btoggle)
            {
                oldheight = Height;
                oldwidth = Width;
                WindowState = FormWindowState.Normal;
                Height = 100;
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
                Width = 180;
            }
            else
            {
                Height = oldheight;
                Width = oldwidth;
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            }
        }

        private void uiSelectionCtl1_Load(object sender, EventArgs e)
        {
            uiSelectionCtl1.HostForm = this;
        }

        public void UpdateStatus(string message)
        {
            toolStripStatusLabel1.Text = message;
        }

        private void MainWnd_Resize(object sender, EventArgs e)
        {
            if (btoolbarmode && this.WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }
        }
    }
}
