using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Management;

namespace NICUtil
{
    public static class NICHelper
    {
        private const string ExternalProcess = "netsh.exe";
        private const string IPConfigProcess = "ipconfig.exe";
        private const string Fieldservice = "Fieldservice";
        static public bool buseimpersonation = true;
        static public bool bupdaterouterDiscovery = false;
        static public bool bpersist = true;

        static public void GetCurrentIPv4Adress(string netConnectionId, bool manual, out string ipaddress, out string subnet, out string defaultIpGateway)
        {
            ipaddress = null;
            subnet = null;
            defaultIpGateway = null;
            var networkInterface = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(i => i.Name == netConnectionId);
            if (networkInterface != null && networkInterface.GetIPProperties().GetIPv4Properties() != null)
            {
                var uni = networkInterface.GetIPProperties().UnicastAddresses.Where(x =>
                    (x.Address.AddressFamily == AddressFamily.InterNetwork && IsValidIPAddress(manual, x))).FirstOrDefault();
                if (uni != null)
                {
                    ipaddress = uni.Address.ToString();
                    subnet = (uni.IPv4Mask != null) ? uni.IPv4Mask.ToString() : null;
                    var gw = networkInterface.GetIPProperties().GatewayAddresses.Where(x =>
                        x.Address.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();
                    if (gw != null)
                        defaultIpGateway = gw.Address.ToString();
                }
            }
        }


        static public void GetCurrentIPv6Adress(string netConnectionId, bool manual, out string ipaddress, out string prefixlen, out string defaultIpGateway)
        {
            ipaddress = null;
            defaultIpGateway = null;
            prefixlen = null;
            var networkInterface = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(i => i.Name == netConnectionId);
            if (networkInterface != null && networkInterface.GetIPProperties().GetIPv6Properties() != null)
            {
                var uni = networkInterface.GetIPProperties().UnicastAddresses.Where(x =>
                    (x.Address.AddressFamily == AddressFamily.InterNetworkV6 && 
                    IsValidIPAddress(manual, x)
                    )).FirstOrDefault();
                if (uni != null)
                {
                    ipaddress = uni.Address.ToString();
                    var gw = networkInterface.GetIPProperties().GatewayAddresses.Where(x =>
                        x.Address.AddressFamily == AddressFamily.InterNetworkV6 &&
                        !x.Address.IsIPv6LinkLocal &&
                        !x.Address.IsIPv6SiteLocal).FirstOrDefault();
                    if (gw != null)
                        defaultIpGateway = gw.Address.ToString();
                    prefixlen = GetIPv6Subnet(ipaddress);
                }
            }
        }

        static public void DeleteCurrentIPv6Address(string netConnectionId, string ipaddress, string gateway)
        {
            var commandLine = "interface ipv6 delete address \"" + netConnectionId + "\" " + ipaddress + " store=" + (bpersist ? "persistent" : "active");
            UpdateNetworkAdapterConfiguration(ExternalProcess, commandLine);
            if (!string.IsNullOrEmpty(gateway))
            {
                commandLine = "int ipv6 delete route ::/0  \"" + netConnectionId + "\"  " + gateway + " store=" + (bpersist ? "persistent" : "active");
                UpdateNetworkAdapterConfiguration(ExternalProcess, commandLine);
            }
        }

        static public bool SetConfigurationForNetworkAdapterToDhcpEnabledIPv4(string netConnectionId)
        {
            try
            {
                string ipaddress;
                string subnet;
                string defaultIpGateway;
                
                GetCurrentIPv4Adress(netConnectionId, false, out ipaddress, out subnet, out defaultIpGateway);
                if (string.IsNullOrEmpty(ipaddress))
                {
                    string commandLine = "interface ipv4 set address \"" + netConnectionId + "\" source=dhcp store=" + (bpersist?"persistent":"active");
                    UpdateNetworkAdapterConfiguration(ExternalProcess, commandLine);
                    
                    commandLine = "/renew \"" + netConnectionId;
                    UpdateNetworkAdapterConfiguration(IPConfigProcess, commandLine);

                    RefreshNetworkAdapters(netConnectionId);
                }

                return true;
            }
            catch
            {
            }
            return false;
        }

        static public bool SetConfigurationForNetworkAdapterToDhcpEnabledIPv6(string netConnectionId)
        {
            try
            {
                string ipaddress;
                string prefixlen;
                string gateway;
                string commandLine = "";
                if (bupdaterouterDiscovery)
                {
                    commandLine = "int ipv6 set int \"" + netConnectionId + "\" routerdiscovery=enabled store=" + (bpersist ? "persistent" : "active");
                    UpdateNetworkAdapterConfiguration(ExternalProcess, commandLine);
                }
                commandLine = "int ipv6 set int \"" + netConnectionId + "\" managedaddress=enabled store=" + (bpersist ? "persistent" : "active");
                UpdateNetworkAdapterConfiguration(ExternalProcess, commandLine);
                GetCurrentIPv6Adress(netConnectionId, true, out ipaddress, out prefixlen, out gateway);
                if (!string.IsNullOrEmpty(ipaddress))
                {
                    DeleteCurrentIPv6Address(netConnectionId, ipaddress, gateway);
                }

                commandLine = "/renew6 \"" + netConnectionId;
                UpdateNetworkAdapterConfiguration(IPConfigProcess, commandLine);
                
                RefreshNetworkAdapters(netConnectionId);
                return true;
            }
            catch
            {
            }
            return false;

        }

        static public bool SetConfigurationForNetworkAdapterToManualIPv4(string netConnectionId, string ipAddress, string subnetMask, string gateway)
        {
            try
            {
                string commandLine = "";

                commandLine = "interface ipv4 set address \"" + netConnectionId + "\" source=static " + "address=" + ipAddress + " mask=" + subnetMask;

                if (!String.IsNullOrEmpty(gateway))
                {
                    commandLine += " gateway=" + gateway;
                }
                else
                {
                    commandLine += " gateway=0.0.0.0";
                }

                commandLine += " store=" + (bpersist ? "persistent" : "active");
                UpdateNetworkAdapterConfiguration(ExternalProcess, commandLine);
                RefreshNetworkAdapters(netConnectionId);
                return true;
            }
            catch
            {
            }
            return false;

        }

        static public bool SetConfigurationForNetworkAdapterToManualIPv6(string netConnectionId, string ipAddress, string prefixlen, string gateway)
        {
            try
            {
                string commandLine = "";
                if (bupdaterouterDiscovery)
                {
                    commandLine = "int ipv6 set int \"" + netConnectionId + "\" routerdiscovery=disabled store=" + (bpersist ? "persistent" : "active");
                    UpdateNetworkAdapterConfiguration(ExternalProcess, commandLine);
                }

                commandLine = "int ipv6 set int \"" + netConnectionId + "\" otherstateful=enabled store=" + (bpersist ? "persistent" : "active");
                UpdateNetworkAdapterConfiguration(ExternalProcess, commandLine);

                commandLine = "/release6 \"" + netConnectionId + "\"";
                UpdateNetworkAdapterConfiguration(IPConfigProcess, commandLine);

                string oldipaddress;
                string oldprefixlen;
                string oldgateway;
                GetCurrentIPv6Adress(netConnectionId, true, out oldipaddress, out oldprefixlen, out oldgateway);
                if (!string.IsNullOrEmpty(oldipaddress))
                {
                    DeleteCurrentIPv6Address(netConnectionId, oldipaddress, oldgateway);
                }

                commandLine = "interface ipv6 set address \"" + netConnectionId + "\" " + ipAddress + ((string.IsNullOrEmpty(prefixlen) ? "" : ("/" + prefixlen))) + "   type=unicast validlifetime=infinite preferredlifetime=infinite store=" + (bpersist ? "persistent" : "active");
                UpdateNetworkAdapterConfiguration(ExternalProcess, commandLine);

                if (!String.IsNullOrEmpty(gateway))
                {
                    commandLine = "int ipv6 add route ::/0  \"" + netConnectionId + "\"  " + gateway + " store=" + (bpersist ? "persistent" : "active");
                    UpdateNetworkAdapterConfiguration(ExternalProcess, commandLine);
                }
                RefreshNetworkAdapters(netConnectionId);
                return true;
            }
            catch
            {
            }
            return false;

        }

        static public string GetHospitalIPAddress()
        {
            string netAdapater = string.Empty;
            string networkConnectionName = string.Empty;
            string ipAddress = null;
            const string HospitalNetwork = "Hospital Network";

            var networkInterface = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(i => i.Name.Equals(HospitalNetwork, StringComparison.InvariantCulture));
            if (networkInterface != null)
            {
                netAdapater = networkInterface.Description;
                var uni = networkInterface.GetIPProperties().UnicastAddresses.Where
                    (x => ((IsValidIPAddress(true, x) || IsValidIPAddress(false, x))
                        )).FirstOrDefault();
                if (uni != null)
                {
                    ipAddress = uni.Address.ToString();
                }
            }

            return ipAddress;
        }

        static public bool UpdateNetworkAdapterConfiguration(string fileName, string commandLine)
        {
            return UpdateNetworkAdapterConfigurationNormal(fileName, commandLine);
        }

        static private bool IsValidIPAddress(bool manual, UnicastIPAddressInformation uniad)
        {
            return (
                (uniad.PrefixOrigin == PrefixOrigin.Dhcp && uniad.SuffixOrigin == SuffixOrigin.OriginDhcp && !manual) ||
                (uniad.PrefixOrigin == PrefixOrigin.RouterAdvertisement && uniad.SuffixOrigin == SuffixOrigin.LinkLayerAddress && !manual) ||
                (uniad.PrefixOrigin == PrefixOrigin.Manual && uniad.SuffixOrigin == SuffixOrigin.Manual && manual)
                );

        }

        static private void RefreshNetworkAdapters(string netConnectionId)
        {
            //UpdateNetworkAdapterConfiguration(ExternalProcess, String.Format("interface set interface \"{0}\" DISABLED", netConnectionId));
            //UpdateNetworkAdapterConfiguration(ExternalProcess, String.Format("interface set interface \"{0}\" ENABLED", netConnectionId));
        }


        static private bool UpdateNetworkAdapterConfigurationNormal(string fileName, string commandLine)
        {
            bool isSuccess = false;
            const int TimeOut = 2500;
            try
            {
                int exitCode = -1;

                string command = String.Format("{0} {1}", fileName, commandLine);


                Process p = Process.Start(fileName, commandLine);
                if (!p.WaitForExit(4 * 60 * 1000))
                {
                    p.Kill();
                    exitCode = 1;
                }
                else
                    exitCode = p.ExitCode;

                if (exitCode != 0)
                {
                    string msg = "exitCode: " + exitCode + "fileName: " + fileName + ", commandline: " + commandLine;
                    throw new Exception("Execution failed");
                }
                else
                {
                    isSuccess = true;
                }

                // Sleep for a while to let Windows to take up the updated network settings
                Thread.Sleep(TimeOut);
            }
            catch (Exception e)
            {
                File.AppendAllText("d:\\niclog.txt", "An exception occurred while executing " + fileName + ". Reason: " + e.Message);
                throw;

            }
            return isSuccess;
        }

        static string GetIPv6Subnet(string ipaddress)
        {
            bool bfound = false;
            string subnet = null;
            ManagementObjectCollection objects = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration").Get();
            foreach (ManagementObject mObject in objects)
            {
                string[] addresses = (string[])mObject["IPAddress"];
                string[] subnets = (string[])mObject["IPSubnet"];
                if (addresses == null && subnets == null)
                    continue;
                for (int i = 0; i < addresses.Length; ++i)
                {
                    if (addresses[i].Equals(ipaddress))
                    {

                        bfound = true;
                        if (subnets.Length > i)
                            subnet = subnets[i];
                        break;
                    }
                    if (bfound)
                        break;
                }
            }

            return subnet;
        }

    }
}
