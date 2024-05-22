using NICUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NICUtilTest
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            NICHelper.buseimpersonation = false;
        }

        private void btnget_Click(object sender, EventArgs e)
        {
            string ipaddress;
            string subnet=null;
            string gateway;
            string prefixlen=null;
            if (radipv4.Checked)
            {
                NICHelper.GetCurrentIPv4Adress(cmdadapter.SelectedItem.ToString(), radmanual.Checked, out ipaddress, out subnet, out gateway);
            }
            else
            {
                NICHelper.GetCurrentIPv6Adress(cmdadapter.SelectedItem.ToString(), radmanual.Checked, out ipaddress, out prefixlen, out gateway);
            }
            txtip.Text = string.IsNullOrEmpty(ipaddress) ? "" : ipaddress;
            txtsn.Text = string.IsNullOrEmpty(subnet) ? "" : subnet;
            txtgw.Text = "";
            if (radmanual.Checked)
            {
                if (radipv4.Checked)
                    txtsn.Text = string.IsNullOrEmpty(subnet) ? "" : subnet;
                else
                    txtsn.Text = string.IsNullOrEmpty(prefixlen) ? "" : prefixlen;
                txtgw.Text = string.IsNullOrEmpty(gateway) ? "" : gateway;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            NICHelper.bupdaterouterDiscovery = chkupdaedisflag.Checked;
            foreach (var nw in NetworkInterface.GetAllNetworkInterfaces().Where(x=>x.GetPhysicalAddress().GetAddressBytes().Length  > 0 && x.NetworkInterfaceType != NetworkInterfaceType.Tunnel))
            {
                cmdadapter.Items.Add(nw.Name);
            }
            if (cmdadapter.Items.Count > 0)
            {
                cmdadapter.SelectedIndex = 0;
                btnget_Click(null, null);
            }
        }

        private void btnset_Click(object sender, EventArgs e)
        {
            if (radipv4.Checked && radDHCP.Checked)
            {
                NICHelper.SetConfigurationForNetworkAdapterToDhcpEnabledIPv4(cmdadapter.SelectedItem.ToString());
            }
            else if (radipv6.Checked && radDHCP.Checked)
            {
                NICHelper.SetConfigurationForNetworkAdapterToDhcpEnabledIPv6(cmdadapter.SelectedItem.ToString());
            }
            else if (radipv4.Checked && radmanual.Checked)
            {
                NICHelper.SetConfigurationForNetworkAdapterToManualIPv4(cmdadapter.SelectedItem.ToString(), txtip.Text, txtsn.Text, txtgw.Text);
            }
            else if (radipv6.Checked && radmanual.Checked)
            {
                NICHelper.SetConfigurationForNetworkAdapterToManualIPv6(cmdadapter.SelectedItem.ToString(), txtip.Text, txtsn.Text, txtgw.Text);
            }

            btnget_Click(null, null);
        }

        private void radipv6_CheckedChanged(object sender, EventArgs e)
        {
            if (radipv6.Checked)
                btnget_Click(null, null);
        }

        private void radipv4_CheckedChanged(object sender, EventArgs e)
        {
            if (radipv4.Checked)
                btnget_Click(null, null);

        }

        private void radDHCP_CheckedChanged(object sender, EventArgs e)
        {
            if (radDHCP.Checked)
                btnget_Click(null, null);

        }

        private void radmanual_CheckedChanged(object sender, EventArgs e)
        {
            if (radmanual.Checked)
                btnget_Click(null, null);

        }

        private void chkupdaedisflag_CheckedChanged(object sender, EventArgs e)
        {
            NICHelper.bupdaterouterDiscovery = chkupdaedisflag.Checked;

        }

        private void cmdadapter_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnget_Click(null, null);
        }

    }
}
