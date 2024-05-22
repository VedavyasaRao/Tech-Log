using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Automation;
using System.Diagnostics;
using System.Threading;
using UITesting.Automated.MouseKeyboardActivityMonitor;
using UITesting.Automated.MouseKeyboardActivityMonitor.WinApi;
using UITesting.Automated.ControlInf;
using UITesting.Automated.JSonSerializer;
using Common.Utilities;
using System.Runtime.InteropServices;

namespace UITesting.Automated.ControlDBTool
{
    public class UIARecorder
    {
        public static UISelectionCtl uiactl;
        
        private UseractionEventManager uaeventman;
        private UseractionManager uaman;

        public UIARecorder(UISelectionCtl auiactl)
        {
            uiactl = auiactl;
            uaman = new UseractionManager();
            uaeventman = new UseractionEventManager();
        }

        public static void UpdateStatus(string message)
        {
            uiactl.UpdateStatus(message);
        }

        public static void AppendtoLog(string message)
        {
            uiactl.AppendToLog(message);
        }

        public void Start()
        {
            uaman.useractions.Clear();
            uaeventman.useractionevents.Clear();
            uaeventman.Register();
        }

        public void Stop()
        {
            uaeventman.Unregister();

        }

        public void AddUserAction(AutomationElement ielementarg, KeyEventArgs keypdowndataarg, KeyPressEventArgs keypressdataarg, MouseEventArgs mousedataarg, bool isdoubleclickarg)
        {
            uaman.AddUserAction(ielementarg, keypdowndataarg, keypressdataarg, mousedataarg, isdoubleclickarg);
        }

        public void Updateuseractionevent()
        {
            uaman.Updateuseractionevent(uaeventman.useractionevents);
        }

        public List<UseractionData> useractions
        {

            get
            {
                return uaman.useractions;
            }
            set
            {
                uaman.useractions = value;
            }
        }

        public List<UseractionEventData> useractionevents
        {

            get
            {
                return uaeventman.useractionevents;
            }
        }

        public void Close()
        {
            uaman.Close();
            uaeventman.Close();

        }

    }
}
