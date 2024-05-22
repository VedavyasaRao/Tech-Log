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
    public class UseractionData
    {
        public AutomationElement invokedlement;
        public ControlInfoPair aelement;
        public List<AutomationEvent> aeventlist = new List<AutomationEvent>();
        public KeyEventArgs keydowndata;
        public Keys modifier;
        public MouseEventArgs mousedata;
        public bool isdoubleclick;
        public bool ishandled = false;
        public System.Windows.Point clickPt;
        public string id="";

        public UseractionData() { }
        public UseractionData(AutomationElement ielementarg, MouseEventArgs mousedataarg, bool isdoubleclickarg)
        {
            invokedlement = ielementarg;
            System.Windows.Rect boundingRect = (System.Windows.Rect)invokedlement.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty);
            clickPt = new System.Windows.Point(mousedataarg.X - boundingRect.Left, mousedataarg.Y - boundingRect.Top);
            mousedata = mousedataarg;
            isdoubleclick = isdoubleclickarg;
            aeventlist = new List<AutomationEvent>();
            ishandled = false;
        }

        public UseractionData(AutomationElement ielementarg, KeyEventArgs keyddownataarg)
        {
            invokedlement = ielementarg;
            keydowndata = keyddownataarg;
            aeventlist = new List<AutomationEvent>();
            ishandled = false;
            modifier = Keys.None;
        }


        public override string ToString()
        {
            return string.Format("received\tAction on {0}\t\tby {1} \tdetails: {2}", actionname, actiontype, actiondetails);
        }

        public void captureelement()
        {

            if (ishandled)
                return;

            LinkedList<AutomationElement> selectedpath = new LinkedList<AutomationElement>();
            try
            {
                TreeWalker walker = TreeWalker.RawViewWalker;
                AutomationElement aenode = invokedlement;
                while (aenode != AutomationElement.RootElement)
                {
                    selectedpath.AddFirst(aenode);
                    aenode = TreeWalker.RawViewWalker.GetParent(aenode);
                }
            }
            catch (Exception ex)
            {
                UIARecorder.uiactl.UpdateStatus("Exception occured:" + ex.Message);
                selectedpath = null;
            }

            if (selectedpath == null || selectedpath.Count == 0)
                return;
            string errors = "";
            try
            {
                UIAElementNode seluiaelement = UIAElementNode.GetUIATreeNode(selectedpath.ElementAt(selectedpath.Count - 1));
                UIAElementNode seluiaelementparent = UIAElementNode.GetUIATreeNode(selectedpath.ElementAt(0));
                ControlInfo ciparent = seluiaelementparent.Controlinfo;
                ControlInfo cinode = seluiaelement.Controlinfo;
                seluiaelement.UpdateCenterpoint(seluiaelementparent.AE);
                aelement=new ControlInfoPair(seluiaelement.Controlinfo, ciparent);
                errors = ToString();
            }
            catch (Exception ex)
            {
                errors = "Exception occured:" + ex.Message;
                selectedpath = null;
            }
            ishandled = true;
        }

        public bool isgood
        {
            get
            {
                return (aelement.ci != null);
            }
        }

        public string actionname
        {

            get
            {
                return isgood ? aelement.ci.UserName : "<unknown>";
            }

        }

        public string actiontype
        {

            get
            {
                return mousedata != null ? "Mouse" : "Key Down";
            }

        }

        public string userevents
        {
            get
            {
                string events = "";
                foreach (var ue in aeventlist)
                    events = events + " " + UseractionEventData.geteventstring(ue);
                return events;
            }
        }



        public string actiondetails
        {

            get
            {
                if (keydowndata != null)
                {
                    ImageKeyConverter kc = new ImageKeyConverter();
                    return string.Format("{0} a:{1} c:{2} s:{3}", kc.ConvertToString(keydowndata.KeyCode), ((modifier & Keys.Alt) == Keys.Alt)?"T":"F",
                        ((modifier & Keys.Control) == Keys.Control) ? "T" : "F", ((modifier & Keys.Shift) == Keys.Shift) ? "T" : "F");
                }
                else
                {
                    return string.Format("{0} dbl:{1} x={2} y={3}", mousedata.Button.ToString(), isdoubleclick?"T" : "F",clickPt.X.ToString("0000"),clickPt.Y.ToString("0000"));

                }
            }

        }

        public UseractionData Clone()
        {
            UseractionData uacp = new UseractionData();
            uacp.aelement = JSonSerializer.JSONPersister<ControlInfoPair>.SetJSON(JSonSerializer.JSONPersister<ControlInfoPair>.GetJSON(aelement));
            uacp.aeventlist.InsertRange(0, aeventlist);
            uacp.keydowndata = keydowndata;
            uacp.modifier = modifier;
            uacp.mousedata = mousedata;
            uacp.isdoubleclick = isdoubleclick;
            uacp.ishandled = ishandled;
            uacp.clickPt = clickPt;
            uacp.id = id;
            return uacp;
        }
    }
    
    public class UseractionManager  
    {
        const string CLASS_NAME = "UseractionManager";
        const uint WM_PROCESS_ACTION = CustomWindow.WM_USER + 500;
        public List<UseractionData> useractions = new List<UseractionData>();
        CustomWindow actionwnd;

        public IntPtr UserActionHandler(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (WM_PROCESS_ACTION == msg)
            {
                UseractionData uad = IntPtrConverter<UseractionData>.FromIntPtr(wParam);
                DoAddUserAction(uad);
                return IntPtr.Zero;
            }

            return CustomWindow.DefHandler(hWnd, msg, wParam, lParam);
        }

        public UseractionManager()
        {
            WindowManager.ClassData cd = WindowManager.CreateClass(CLASS_NAME, new NativeWndProc(UserActionHandler));
            cd.thread.Priority = ThreadPriority.AboveNormal;
            WindowManager.Start(CLASS_NAME);
            actionwnd = WindowManager.CreateWindow(CLASS_NAME, CLASS_NAME);
        }

        public void AddUserAction(AutomationElement ielementarg, KeyEventArgs keypdowndataarg, KeyPressEventArgs keypressdataarg, MouseEventArgs mousedataarg, bool isdoubleclickarg)
        {
            UseractionData ua = null;
            if (keypdowndataarg != null)
            {
                ua = new UseractionData(ielementarg, keypdowndataarg);
            }
            else if (mousedataarg != null)
            {
                ua = new UseractionData(ielementarg, mousedataarg, isdoubleclickarg);
            }
            actionwnd.SendNotifyMessage(WM_PROCESS_ACTION, IntPtrConverter<UseractionData>.ToIntPtr(ua), IntPtr.Zero);
        }

        public void Updateuseractionevent(List<UseractionEventData> useractionevents)
        {
            List<UseractionData> newuseractions = new List<UseractionData>();
            Keys m = Keys.None;
            foreach (UseractionData ua in useractions)
            {
                if (ua.keydowndata != null)
                {
                    if (ua.keydowndata.KeyCode == Keys.LShiftKey || ua.keydowndata.KeyCode == Keys.RShiftKey)
                    {
                        m |= Keys.Shift;
                        continue;
                    }
                    else if (ua.keydowndata.KeyCode == Keys.LMenu || ua.keydowndata.KeyCode == Keys.RMenu)
                    {
                        m |= Keys.Alt;
                        continue;
                    }
                    else if (ua.keydowndata.KeyCode == Keys.LControlKey || ua.keydowndata.KeyCode == Keys.RControlKey)
                    {
                        m |= Keys.Control;
                        continue;
                    }
                    ua.modifier = m;
                    m = Keys.None;
                }
                newuseractions.Add(ua);

            }
            useractions = newuseractions;
            foreach (UseractionData uaout in useractions)
            {
                if (!(uaout.isgood && uaout.aelement.ci.Patterns.Contains("Text") && uaout.aelement.ci.AEAutomationId != ""))
                    continue;
                foreach (UseractionData uain in useractions)
                {
                    if (uain.isgood && uain.aelement.ci.AEAutomationId == uaout.aelement.ci.AEAutomationId && uain.aelement.ciroot.UserName == uaout.aelement.ciroot.UserName)
                    {
                        uain.aelement.ci.UserName = uaout.aelement.ci.UserName;
                    }
                }
            }

            while (true)
            {
                bool bfound = false;
                foreach (UseractionEventData ue in useractionevents)
                {
                    if (!ue.isgood)
                        continue;
                    foreach (UseractionData ua in useractions)
                    {
                        if (!ua.isgood)
                            continue;

                        if (ua.aelement.ci.UserName == ue.ci.UserName)
                        {
                            if (ua.aeventlist.Find((e) => (e == ue.aevent)) != null)
                                continue;
                            ua.aeventlist.Add(ue.aevent);
                            useractionevents.Remove(ue);
                            bfound = true;
                            break;
                        }
                    }
                    if (bfound)
                        break;
                }
                if (!bfound)
                    break;
            }


        }

        public void Close()
        {
            actionwnd.Close();
            WindowManager.Stop(CLASS_NAME);
        }

        private void DoAddUserAction(UseractionData uad)
        {
            try
            {
                if (Process.GetCurrentProcess().Id == uad.invokedlement.Current.ProcessId)
                    return;
                uad.captureelement();
                useractions.Add(uad);
                UIARecorder.UpdateStatus(string.Format("Received Action\tName:{0}\t\t\tType:{1}\t\t\tDetails:{2}",uad.actionname,uad.actiontype,uad.actiondetails)) ;
            }
            catch (Exception ex)
            {
                UIARecorder.AppendtoLog("Exception occured in AddUserAction" + ex.Message);

            }
        }

    }


}
