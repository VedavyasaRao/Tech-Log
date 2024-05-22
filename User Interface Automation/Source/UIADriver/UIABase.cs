using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Provider;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;
using UITesting.Automated.WindowsInput;
using UITesting.Automated.ControlInf;
using UITesting.Automated.JSonSerializer;

namespace UITesting.Automated.UIADriver
{
    [ComVisible(false)]
    public class UIADriverBase
    {
        protected AutomationElement ae_;
        protected AutomationElement window_;
        protected ControlInfo[] ctls_;

        private void Copy(UIADriverBase uidb)
        {
            ae_= uidb.ae_;
            ctls_ = uidb.ctls_;
        }
        
        protected bool GetElement(string details, bool busevalue)
        {
            ae_ = null;
            ctls_ = JSONPersister<ControlInfo[]>.SetJSON(details);
            if (ctls_.Length < 8)
            {
                return false;
            }

            AutomationElementCollection windows = null;

            if (ctls_[1].AEAutomationId != "")
            {
                windows = AutomationElement.RootElement.FindAll(TreeScope.Children,
                new PropertyCondition(AutomationElement.AutomationIdProperty, ctls_[1].AEAutomationId));
            }

            if (windows == null || windows.Count == 0)
            {
                windows = AutomationElement.RootElement.FindAll(TreeScope.Children,
                new PropertyCondition(AutomationElement.NameProperty, ctls_[1].AEText));
            }


            if (windows == null)
            {
                Logger.LogMessage(string.Format("GetElement {0} not found", ctls_[1].UserName));
                return false;
            }

            if (ctls_[0].Path == "")
            {
                ae_ = windows[0];
                return true;
            }

            foreach (AutomationElement window in windows)
            {
                if (ctls_[0].AEAutomationId != "")
                {
                    ae_ = window.FindFirst(TreeScope.Descendants,
                    new PropertyCondition(AutomationElement.AutomationIdProperty, ctls_[0].AEAutomationId));
                    if (ae_ != null)
                    {
                        Logger.LogMessage(string.Format("GetElement {0} automationid {1} found", Getctlname(), ctls_[0].AEAutomationId));
                        return false;
                    }
                    Logger.LogMessage(string.Format("GetElement {0} automationid {1} not found", Getctlname(), ctls_[0].AEAutomationId));
                }

                string[] path = ctls_[0].Path.Trim().Split(new char[] { ' ' });
                ae_ = window;

                for (int k = 0; k < path.Length; ++k)
                {
                    string s = path[k];
                    if (ae_ == null)
                    {
                        break;
                    }

                    ae_ = TreeWalker.RawViewWalker.GetFirstChild(ae_);
                    if (ae_ == null)
                    {
                        Logger.LogMessage(string.Format("GetElement {0} Level {1} position 0 not found", Getctlname(), k));
                        break;
                    }
                    for (int i = 1; i <= int.Parse(s); ++i)
                    {
                        ae_ = TreeWalker.RawViewWalker.GetNextSibling(ae_);
                        if (ae_ == null)
                        {
                            Logger.LogMessage(string.Format("GetElement {0} Level {1} position {2} not found", Getctlname(), k, i));
                            break;
                        }
                    }

                }


                if (ae_ != null && ctls_[0].AEType == ae_.Current.ControlType.LocalizedControlType && (ctls_[0].AEText == ae_.Current.Name || !busevalue))
                {
                    Logger.LogMessage(string.Format("GetElement {0} path  {1} found", Getctlname(), ctls_[0].Path.Trim()));
                    return true;
                }

                ae_ = null;
                if (ae_ == null)
                {
                    Logger.LogMessage(string.Format("SearchAEElement {0} {1}  {2}:", ctls_[0].UserName, ctls_[0].AEType, ctls_[0].AEText));
                    SearchAEElement(ctls_[0], window, ref ae_, busevalue);
                }
                if (ae_ != null)
                {
                    return true;
                }
            }

            return true;
        }

        void SearchAEElement(ControlInfo actl, AutomationElement ae, ref AutomationElement searchedele, bool busevalue)
        {

            if (searchedele != null)
            {
                return;
            }


            Logger.LogMessage(string.Format("current element {0} {1}", ae.Current.ControlType.LocalizedControlType, ae.Current.Name));
            if (actl.AEType == ae.Current.ControlType.LocalizedControlType && (actl.AEText == ae.Current.Name || !busevalue))
            {
                searchedele = ae;
                Logger.LogMessage(string.Format("current element {0} {1} FOUND", ae.Current.ControlType.LocalizedControlType, ae.Current.Name));
                return;
            }

            try
            {
                AutomationElement elementNode = TreeWalker.RawViewWalker.GetFirstChild(ae);

                while (elementNode != null)
                {
                    SearchAEElement(actl, elementNode, ref searchedele, busevalue);
                    if (searchedele != null)
                    {
                        break;
                    }
                    else
                    {
                        elementNode = TreeWalker.RawViewWalker.GetNextSibling(elementNode);
                    }
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

        }

        internal string Getctlname()
        {
            return ctls_[0].UserName;
        }


        internal void CauseDelay(int iwaitms)
        {
            Application.DoEvents();
            if (iwaitms > 0)
            {
                System.Threading.Thread.Sleep(iwaitms);
            }

        }

        internal BasePattern GetPattern<T>(AutomationPattern match) where T : BasePattern
        {
            BasePattern pattern = null;
            try
            {
                if (ae_ != null)
                {
                    pattern = ae_.GetCurrentPattern(match) as T;
                }

            }
            catch (Exception ex)
            {
                Logger.LogMessage("Exception occured:" + ex.Message);
            }
            return pattern;
        }

    }


    
}

