using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UITesting.Automated.ControlInf;

namespace UITesting.Automated.ControlDBTool
{
    public partial class FrmUserActions : Form
    {
        UIARecorder uiarecoder;
        List<UseractionData> useractions = new List<UseractionData>();
        List<UseractionEventData> useractionevents = new List<UseractionEventData>();

        public FrmUserActions(UIARecorder uiarecoder)
        {
            InitializeComponent();
            this.uiarecoder = uiarecoder;
            reload();
            load();

        }

        private void reload()
        {
            useractions = new List<UseractionData>();
            foreach (UseractionData ua in uiarecoder.useractions)
            {
                useractions.Add(ua.Clone());
            }

            useractionevents = new List<UseractionEventData>();
            foreach (UseractionEventData ue in uiarecoder.useractionevents)
            {
                useractionevents.Add(ue.Clone());
            }

        }

        private void load()
        {
            lstactions.Items.Clear();
            foreach (var ua in useractions)
            {
                ListViewItem item1 = new ListViewItem(ua.actionname);
                item1.Checked = false;
                item1.SubItems.Add(ua.actiontype);
                item1.SubItems.Add(ua.userevents);
                item1.SubItems.Add(ua.actiondetails);
                item1.Tag = ua;
                lstactions.Items.Add(item1);
            }

            lstevents.Items.Clear();
            foreach (var ue in useractionevents)
            {
                ListViewItem item1 = new ListViewItem(ue.ci.UserName);
                item1.Checked = false;
                item1.SubItems.Add(UseractionEventData.geteventstring(ue.aevent));
                item1.Tag = ue;
                lstevents.Items.Add(item1);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<UseractionData> useractions = new List<UseractionData>(); 
            foreach (ListViewItem itm in lstactions.Items)
            {
                useractions.Add((UseractionData)itm.Tag);
            }
            uiarecoder.useractions = useractions;
        }


        private void btnattach_Click(object sender, EventArgs e)
        {
            if (lstactions.SelectedItems.Count == 0)
                return;
            UseractionData ua = (UseractionData)lstactions.SelectedItems[0].Tag;

            foreach (ListViewItem li in lstevents.CheckedItems)
            {
                UseractionEventData uesel = (UseractionEventData)li.Tag;
                if (ua.aeventlist.Find((ae) => ae == uesel.aevent) == null)
                    ua.aeventlist.Add(uesel.aevent);
                useractionevents.Remove(uesel);
            }
            load();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (lstactions.SelectedItems.Count == 0)
                return;
            UseractionData ua = (UseractionData)lstactions.SelectedItems[0].Tag;
            useractions.Remove(ua);
            load();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            reload();
            load();
        }

        private void lstactions_DoubleClick(object sender, EventArgs e)
        {
            btnattach_Click(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
