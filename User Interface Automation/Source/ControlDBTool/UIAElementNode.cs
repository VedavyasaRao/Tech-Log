using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using UITesting.Automated.ControlInf;


namespace UITesting.Automated.ControlDBTool
{
    class UIAElementNode
    {
        private AutomationElement ae;
        private ControlInfo ctlinfo;
        private string aestr;
        
        public ControlInfo Controlinfo
        {
            get
            {
                if (ctlinfo == null)
                    getControlinfo(false);
                return ctlinfo;

            }
        }

        public ControlInfo Controlinfo_event
        {
            get
            {
                if (ctlinfo == null)
                    getControlinfo(true);
                return ctlinfo;

            }
        }

        public static UIAElementNode GetUIATreeNode(ControlInfo cinf)
        {
            UIAElementNode node = new UIAElementNode();
            node.ae = null;
            node.ctlinfo = cinf;
            return node;
        }

        public static UIAElementNode GetUIATreeNode(AutomationElement ae)
        {
            UIAElementNode node = new UIAElementNode();
            node.ae = ae;
            return node;
        }

        public string AEString
        {
            get
            {
                if (aestr == null)
                    aestr = GetAEString(ae);
                return aestr;
            }
        }

        public AutomationElement AE
        {
            get
            {
                return ae;

            }
        }

        public void UpdateCenterpoint(AutomationElement parentwindowae)
        {
            System.Windows.Rect wrect = parentwindowae.Current.BoundingRectangle;
            System.Windows.Point pt;
            if (!ae.TryGetClickablePoint(out pt))
            {
                System.Windows.Rect rect = ae.Current.BoundingRectangle;
                pt = new System.Windows.Point(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);
            }
            ctlinfo.CenterPoint = string.Format("{0},{1}", (pt.X - wrect.Left) , (pt.Y - wrect.Top));
        }

        public static string GetAEString(AutomationElement ae)
        {
            string ret = "";
            if (ae != null)
            {
                string s = string.Format("{0} {1} [", ae.Current.ControlType.LocalizedControlType, ae.Current.Name);
                object obj = ae.GetCurrentPropertyValue(AutomationElement.RuntimeIdProperty);
                int[] ar = (int[])obj;
                foreach (int x in ar)
                {
                    s += x.ToString();
                    s += " ";
                }
                s += "]";

                ret=s;
            }

            return ret;
        }


        private static string getUserName(ControlInfo cotlinfo, System.Windows.Rect rect)
        {
            Regex rx = new Regex("[^0-9_a-zA-Z]");
            string newusername = string.Format("{0}_{1}_{2}_{3}_{4}", cotlinfo.AEType, cotlinfo.AEText, cotlinfo.AEAutomationId,rect.Left,rect.Top);
            newusername = rx.Replace(newusername, "_");
            while (true)
            {
                string s = newusername.Replace("__", "_");
                if (s == newusername)
                    break;
                newusername = s;
            }
            return newusername.Trim(new char[] { ' ', '_' });
        }

        private void getControlinfo(bool bname)
        {
            if (ae != null)
            {
                ctlinfo = new ControlInfo();
                ctlinfo.AEType = ae.Current.ControlType.LocalizedControlType;
                ctlinfo.AEText = ae.Current.Name;
                ctlinfo.AEAutomationId = ae.Current.AutomationId;
                ctlinfo.UserName = getUserName(ctlinfo, ae.Current.BoundingRectangle);
                if (bname)
                    return;
                UpdateCenterpoint(ae);
                string path = "";
                GetPath(ae, ref path);
                ctlinfo.Path = path;

                ctlinfo.Patterns = "";
                foreach (AutomationPattern pat in ae.GetSupportedPatterns())
                {
                    if (ctlinfo.Patterns != "")
                        ctlinfo.Patterns += ",";
                    ctlinfo.Patterns += Automation.PatternName(pat);
                }


            }
        }

        private void GetPath(AutomationElement aenode, ref string path)
        {
            try
            {
                AutomationElement aenodeold=aenode;
                AutomationElement aenodeparent = TreeWalker.RawViewWalker.GetParent(aenode);
                int idx = -1;
                TreeWalker walker = TreeWalker.RawViewWalker;
                while (aenodeold != null)
                {
                    aenodeold = TreeWalker.RawViewWalker.GetPreviousSibling(aenodeold);
                    idx++;
                    if (aenodeparent == aenodeold)
                        break;
                }
                aenodeold = TreeWalker.RawViewWalker.GetParent(aenode);
                if (aenodeold != AutomationElement.RootElement)
                {
                    path = idx.ToString() + " " + path;
                    GetPath(aenodeold, ref path);
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                path = "";
            }

        }
    }

}
