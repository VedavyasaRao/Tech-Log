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
    public interface IUIANavigationProvider
    {
        IUIAElement FindElement(string criteria);


        IUIAElement Parent
        {
            get;
        }

        IUIAElement FirstChild
        {
            get;
        }

        IUIAElement LastChild
        {
            get;
        }

        IUIAElement NextSibling
        {
            get;
        }

        IUIAElement PreviousSibling
        {
            get;
        }

    }

    class UIANavigationProvider : UIADriverPatternBase, IUIANavigationProvider
    {
        internal static UIANavigationProvider GetInstance(UIAElement owner)
        {
            return GetPatternObject<UIANavigationProvider>(owner);
        }

        private Dictionary<AutomationProperty, string> ParseCriteria(string criteria)
        {
            Dictionary<AutomationProperty, string> ret = new Dictionary<AutomationProperty, string>();
            string[] tokens = criteria.Split(new char[] { ',' });
            foreach (string tok in tokens)
            {
                string[] items = tok.Split(new char[] { '=' });
                ret.Add(AutomationProperty.LookupById(int.Parse(items[0])), items[1]);
            }

            return ret;
        }

        private bool MatchCriteria(Dictionary<AutomationProperty, string> criteria, AutomationElement ae)
        {
            string curprops = "";
            bool b = true;
            foreach (AutomationProperty id in criteria.Keys)
            {
                string s = JSonSerializer.JSONPersister<object>.GetRawJSON(ae.GetCurrentPropertyValue(id));
                if (criteria[id] != s)
                {
                    b = false;
                }
                curprops = curprops + "   " + s;
            }
            Log(string.Format("Matching({0})", curprops));
            return b;
        }

        private void FindElement(Dictionary<AutomationProperty, string> criteria, AutomationElement rootelement, ref AutomationElement ret)
        {

            if (rootelement != null && ret == null)
            {
                TreeWalker walker = TreeWalker.RawViewWalker;
                if (!MatchCriteria(criteria, rootelement))
                {
                    AutomationElement ae = TreeWalker.RawViewWalker.GetFirstChild(rootelement);
                    if (ae != null)
                    {
                        FindElement(criteria, ae, ref ret);
                    }

                    if ((ae = TreeWalker.RawViewWalker.GetNextSibling(rootelement)) != null)
                    {
                        FindElement(criteria, ae, ref ret);
                    }
                }
                else
                    ret = rootelement;
            }
        }

        public IUIAElement FindElement(string criteria)
        {
            UIAElement ret = null;
            AutomationElement aeret = null; 
            Log(string.Format("FindElement({0})", criteria));
            try
            {
                Dictionary<AutomationProperty, string> cirteriadata = ParseCriteria(criteria);
                AutomationElement ae = owner.GetAE;
                if (MatchCriteria(cirteriadata, ae))
                {

                    aeret = ae;
                }
                else
                {
                    ae = TreeWalker.RawViewWalker.GetFirstChild(ae);
                    FindElement(cirteriadata, ae, ref aeret);
                }
                if (aeret != null)
                    ret = UIAElement.GetInstance(aeret, owner);

                CauseDelay();

            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
            return ret;
        }


        public IUIAElement Parent
        {
            get 
            { 
                UIAElement ret = null;
                Log("Parent");
                try
                {
                    TreeWalker walker = TreeWalker.RawViewWalker;
                    AutomationElement ae =TreeWalker.RawViewWalker.GetParent(owner.GetAE);
                    ret = UIAElement.GetInstance(ae, owner);
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;

            }
        }

        public IUIAElement FirstChild
        {
            get
            {
                UIAElement ret = null;
                Log("FirstChild");
                try
                {
                    TreeWalker walker = TreeWalker.RawViewWalker;
                    AutomationElement ae = TreeWalker.RawViewWalker.GetFirstChild(owner.GetAE);
                    ret = UIAElement.GetInstance(ae, owner);
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;

            }
        }

        public IUIAElement LastChild
        {
            get
            {
                UIAElement ret = null;
                Log("FirstChild");
                try
                {
                    TreeWalker walker = TreeWalker.RawViewWalker;
                    AutomationElement ae = TreeWalker.RawViewWalker.GetLastChild(owner.GetAE);
                    ret = UIAElement.GetInstance(ae, owner);
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;

            }
        }

        public IUIAElement NextSibling
        {
            get
            {
                UIAElement ret = null;
                Log("NextSibling");
                try
                {
                    TreeWalker walker = TreeWalker.RawViewWalker;
                    AutomationElement ae = TreeWalker.RawViewWalker.GetNextSibling(owner.GetAE);
                    ret = UIAElement.GetInstance(ae, owner);
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;

            }
        }

        public IUIAElement PreviousSibling
        {
            get
            {
                UIAElement ret = null;
                Log("PreviousSibling");
                try
                {
                    TreeWalker walker = TreeWalker.RawViewWalker;
                    AutomationElement ae = TreeWalker.RawViewWalker.GetPreviousSibling(owner.GetAE);
                    ret = UIAElement.GetInstance(ae, owner);
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;

            }
        }

    }

}

