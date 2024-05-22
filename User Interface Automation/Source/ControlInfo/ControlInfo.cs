using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UITesting.Automated.ControlInf
{
    /// <summary>ControlInfo </summary>
    public class ControlInfo
    {
        /// <summary>UserName </summary>
        public string UserName { get; set; }
        /// <summary>AEType </summary>
        public string AEType { get; set; }
        /// <summary>AEText </summary>
        public string AEText { get; set; }
        /// <summary>AEAutomationId </summary>
        public string AEAutomationId { get; set; }
        /// <summary>Patterns </summary>
        public string Patterns { get; set; }
        /// <summary>Path </summary>
        public string Path { get; set; }
        public string CenterPoint { get; set; }
    }

    public struct ControlInfoPair
    {
        public ControlInfoPair(ControlInfo pci, ControlInfo pciroot)
        {
            ci = pci;
            ciroot = pciroot;
        }

        public ControlInfo ci;
        public ControlInfo ciroot;
    }

}
