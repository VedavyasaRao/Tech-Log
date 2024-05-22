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
    //[ComVisible(true)]
    [Guid("610A910C-7352-4B62-A69E-A90F0AFE159A")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IUIAElement
    {
        string SearchOptions
        {
            get;
            set;
        }

        int Waitms
        {
            get;
            set;
        }

        IUIAConstants Constants
        {
            get;
        }

        IUIAElement ParentWindow
        {
            get;
        }

        bool SetLogFile(string logfilename, bool bappend);

        bool AppendToLog(string message);

        bool SetAutomationElement(string details);

        bool CheckControlVisiblity(string details, bool bvisible, int maxwaitms);


        IUIAWindowProvider ProviderWindow
        {
            get;
        }

        IUIAValueProvider ProviderValue
        {
            get;
        }

        IUIATransformProvider ProviderTransform
        {
            get;
        }

        IUIAToggleProvider ProviderToggle
        {
            get;
        }

        IUIATableItemProvider ProviderTableItem
        {
            get;
        }

        IUIATableProvider ProviderTableProvider
        {
            get;
        }

        IUIASelectionItemProvider ProviderSelectionItem
        {
            get;
        }
        IUIASelectionProvider ProviderSelection
        {
            get;
        }

        IUIARangeValueProvider ProviderRangeValue
        {
            get;
        }

        IUIAInvokeProvider ProviderInvoke
        {
            get;
        }

        IUIAGridItemProvider ProviderGridItem
        {
            get;
        }

        IUIAGridProvider ProviderGrid
        {
            get;
        }

        IUIAGenericProvider ProviderGeneric
        {
            get;
        }

        IUIAExpandCollapseProvider ProviderExpandCollapse
        {
            get;
        }

        IMSAAAccessibleProvider ProviderMSAAAccessible
        {
            get;
        }

        IUIANavigationProvider ProviderNavigation
        {
            get;
        }

        IUIAScrollProvider ProviderScroll
        {
            get;
        }

        IUIAScrollItemProvider ProviderScrollItem
        {
            get;
        }

    }


    [Guid("C13986B4-CD1E-444F-BB78-2FCAE09C6134"),
     ClassInterface(ClassInterfaceType.AutoDispatch),
     ProgId("UIADriverLib.UIAElement"),
     ComDefaultInterfaceAttribute(typeof(IUIAElement))]
    public class UIAElement : IUIAElement
    {
        private AutomationElement uiaae;
        private string controlname;
        private delegate UIADriverPatternBase PattrenObjectInstance(UIAElement owner);

        private UIAElement uiaparentwindow;
        private UIAWindow uiawindow;
        private UIAValue uiavalue;
        private UIATransform uiatranform;
        private UIAToggle uiatoggle;
        private UIATableItem uiatableitem;
        private UIATable uiatable;
        private UIASelectionItem uiaselectionitem;
        private UIASelection uiaselection;
        private UIARangeValue uiarangevalue;
        private UIAInvoke uiainvoke;
        private UIAGridItem uiagriditem;
        private UIAGrid uiagrid;
        private UIAGeneric uiageneric;
        private UIAExpandCollapse uiaexpandcollapse;
        private MSAAElement uiaaccessible;
        private UIAConstants uiaconstants;
        private UIANavigationProvider uianavigation;
        private UIAScroll uiascroll;
        private UIAScrollItem uiascrollitem;
               

        private bool GetElement(string details)
        {
            bool busevalue = false;


            char[] sopts = SearchOptions.ToCharArray();
            busevalue = (Array.FindIndex(sopts, (s => (s == Constants.SearchOptions_UseValue[0]))) != -1);

            uiaae = null;
            uiaparentwindow = null;

            ControlInfoPair ctls = JSONPersister<ControlInfoPair>.SetJSON(details);
            if (ctls.ci == null)
            {
                return false;
            }

            AutomationElementCollection windows = null;

            if (ctls.ciroot.AEAutomationId != "")
            {
                windows = AutomationElement.RootElement.FindAll(TreeScope.Children,
                new PropertyCondition(AutomationElement.AutomationIdProperty, ctls.ciroot.AEAutomationId));
            }

            if (windows == null || windows.Count == 0)
            {
                windows = AutomationElement.RootElement.FindAll(TreeScope.Children,
                new PropertyCondition(AutomationElement.NameProperty, ctls.ciroot.AEText));
            }

            if (windows == null || windows.Count == 0)
            {
                bool bfound=false;
                windows = AutomationElement.RootElement.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.IsWindowPatternAvailableProperty, true));
                foreach (AutomationElement window in windows)
                {
                    string s = window.GetCurrentPropertyValue(AutomationElement.NameProperty) as string;
                    if (s.ToLower().Contains(ctls.ciroot.AEText.ToLower()))
                    {
                        windows = AutomationElement.RootElement.FindAll(TreeScope.Children,
                        new PropertyCondition(AutomationElement.NameProperty, s));
                        bfound=true;
                        break;
                    }
                }
                if (!bfound)
                	windows = null;
            }

            if (windows == null || windows.Count == 0)
            {
                Logger.LogMessage(string.Format("GetElement {0} not found", ctls.ciroot.UserName));
                return false;
            }

            uiaparentwindow = UIAElement.GetInstance(windows[0], null);
            controlname = ctls.ci.UserName;

            if (ctls.ci.Path == "")
            {
                uiaae = windows[0];
                return true;
            }

            
            foreach (AutomationElement window in windows)
            {
                
                foreach( char sop in sopts)
                {
                    if (ctls.ci.AEAutomationId != "" && sop == Constants.SearchOptions_UseAutomationId[0])
                    {
                        uiaae = window.FindFirst(TreeScope.Descendants,
                        new PropertyCondition(AutomationElement.AutomationIdProperty, ctls.ci.AEAutomationId));
                        Logger.LogMessage(string.Format("GetElement {0} automationid {1} {2} FOUND", Getctlname, ctls.ci.AEAutomationId, ((uiaae != null) ? "" : "not")));
                        if (uiaae != null)
                        {
                            return true;
                        }
                    }


                    string[] path = ctls.ci.Path.Trim().Split(new char[] { ' ' });
                    uiaae = window;

                    if (path.Length > 0 && sop == Constants.SearchOptions_UsePath[0])
                    {
                        for (int k = 0; k < path.Length; ++k)
                        {
                            string s = path[k];
                            if (uiaae == null)
                            {
                                break;
                            }

                            uiaae = TreeWalker.RawViewWalker.GetFirstChild(uiaae);
                            if (uiaae == null)
                            {
                                Logger.LogMessage(string.Format("GetElement {0} Level {1} position 0 not found", Getctlname, k));
                                break;
                            }
                            for (int i = 1; i <= int.Parse(s); ++i)
                            {
                                uiaae = TreeWalker.RawViewWalker.GetNextSibling(uiaae);
                                if (uiaae == null)
                                {
                                    Logger.LogMessage(string.Format("GetElement {0} Level {1} position {2} not found", Getctlname, k, i));
                                    break;
                                }
                            }
                        }
                
                        if (uiaae != null && ctls.ci.AEType == uiaae.Current.ControlType.LocalizedControlType && (ctls.ci.AEText == uiaae.Current.Name || !busevalue))
                        {
                            Logger.LogMessage(string.Format("GetElement {0} path  {1} FOUND", Getctlname, ctls.ci.Path.Trim()));
                            return true;
                        }
                    }

                    if (ctls.ci.CenterPoint != "" && sop == Constants.SearchOptions_UseClickPoint[0])
                    {
                        string[] pts = ctls.ci.CenterPoint.Split(new char[] { ',' });
                        Point pt = new Point(Double.Parse(pts[0]), Double.Parse(pts[1]));
                        System.Windows.Rect rect = windows[0].Current.BoundingRectangle;
                        pt.X += rect.Left;
                        pt.Y += rect.Top;
                        uiaae = AutomationElement.FromPoint(pt);
                        string ss = uiaae.Current.ControlType.LocalizedControlType;
                        if (uiaae != null && ctls.ci.AEType == uiaae.Current.ControlType.LocalizedControlType && (ctls.ci.AEText == uiaae.Current.Name || !busevalue))
                        {
                            Logger.LogMessage(string.Format("GetElement {0} point  {1} FOUND", Getctlname, ctls.ci.CenterPoint));
                            return true;
                        }
                        else
                        {
                            Logger.LogMessage(string.Format("GetElement {0} point  {1} was not found", Getctlname, ctls.ci.CenterPoint));
                        }

                    }

                    if (sop == Constants.SearchOptions_SearchTree[0])
                    {
                        uiaae = null;
                        Logger.LogMessage(string.Format("SearchAEElement {0} {1}  {2}:", ctls.ci.UserName, ctls.ci.AEType, ctls.ci.AEText));
                        return SearchAEElement(ctls.ci, window, ref uiaae, busevalue);
                    }
                }
            }

            return false;
        }

        private bool SearchAEElement(ControlInfo actl, AutomationElement ae, ref AutomationElement searchedele, bool busevalue)
        {
            Logger.LogMessage(string.Format("current element {0} {1}", ae.Current.ControlType.LocalizedControlType, ae.Current.Name));
            if (actl.AEType == ae.Current.ControlType.LocalizedControlType && (actl.AEText == ae.Current.Name || !busevalue))
            {
                searchedele = ae;
                Logger.LogMessage(string.Format("current element {0} {1} FOUND", ae.Current.ControlType.LocalizedControlType, ae.Current.Name));
                return true;
            }
            else
            {

                try
                {
                    AutomationElement elementNode = TreeWalker.RawViewWalker.GetFirstChild(ae);

                    while (elementNode != null && searchedele == null)
                    {
                        if (!SearchAEElement(actl, elementNode, ref searchedele, busevalue))
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
            return false;

        }

        T GetPattrenObjectInstance<T>(ref T pattrenobj, PattrenObjectInstance patfunc) where T : UIADriverPatternBase
        {
            if (pattrenobj == null)
            {
                pattrenobj = patfunc(this) as T;
            }

            return pattrenobj;
        }

        internal string Getctlname
        {
            get
            {
                return controlname;
            }
        }

        internal AutomationElement GetAE
        {
            get
            {
                return uiaae;
            }
        }

        internal void CauseDelay()
        {
            Application.DoEvents();
            if (Waitms > 0)
            {
                System.Threading.Thread.Sleep(Waitms);
            }

        }

        internal BasePattern GetPattern<T>(AutomationPattern match) where T : BasePattern
        {
            BasePattern pattern = null;
            try
            {
                if (uiaae != null)
                {
                    pattern = uiaae.GetCurrentPattern(match) as T;
                }

            }
            catch (Exception ex)
            {
                Logger.LogMessage("Exception occured:" + ex.Message);
            }
            return pattern;
        }


        internal static UIAElement GetInstance(AutomationElement ae, UIAElement owner)
        {
            UIAElement ret = new UIAElement();
            ret.uiaae = ae;

            if (owner != null)
            {
                ret.uiaparentwindow = owner.uiaparentwindow;
                ret.controlname = "dynamic";
            }
            return ret;
        }

        public UIAElement()
        {
            SearchOptions = Constants.SearchOptions_Default;
            Waitms = 1000;
        }

        public string SearchOptions
        {
            get;
            set;
        }


        public int Waitms
        {
            get;
            set;
        }


        public bool SetLogFile(string logfilename, bool bappend)
        {
            Logger.SetLogFile(logfilename, bappend);
            return true;
        }


        public bool AppendToLog(string message)
        {
            Logger.LogMessage(string.Format("UserMessage:({0})", message));
            return true;
        }


        public bool SetAutomationElement(string details)
        {
            uiawindow = null;
            uiavalue = null;
            uiatranform = null;
            uiatoggle = null;
            uiatableitem = null;
            uiatable = null;
            uiaselectionitem = null;
            uiaselection = null;
            uiarangevalue = null;
            uiainvoke = null;
            uiagriditem = null;
            uiagrid = null;
            uiageneric = null;
            uiaexpandcollapse = null;
            uiaaccessible = null;
            uianavigation = null;
            uiascroll = null;
            uiascrollitem = null;

            return GetElement(details);
        }

        public bool CheckControlVisiblity(string details, bool bvisible, int maxwaitms)
        {
            bool bret = false;
            int temp = Waitms;
            try
            {
                int startwait = 0;
                Waitms = 1000;
                ControlInfoPair ctls = JSONPersister<ControlInfoPair>.SetJSON(details);
                if (ctls.ci == null)
                {
                    return false;
                }

                Logger.LogMessage(string.Format("CheckControlVisiblity({0},{1},{2})", ctls.ci.UserName, bvisible, maxwaitms));
                while (startwait <= maxwaitms)
                {
                    if (GetElement(details) == bvisible)
                    {
                        bret = true;
                        break;
                    }
                    CauseDelay();
                    startwait += 1000;
                }
            }
            catch (Exception ex)
            {

                Logger.LogMessage("Exception occured:" + ex.Message);

            }
            Waitms = temp;
            return bret;
        }

        public IUIAElement ParentWindow
        {
            get
            {
                return uiaparentwindow;
            }
        }


        public IUIAWindowProvider ProviderWindow
        {
            get
            {
                return GetPattrenObjectInstance< UIAWindow>(ref uiawindow, UIAWindow.GetInstance);
            }
        }


        public IUIAValueProvider ProviderValue
        {
            get
            {
                return GetPattrenObjectInstance<UIAValue>(ref uiavalue, UIAValue.GetInstance);
            }
        }

        public IUIATransformProvider ProviderTransform
        {
            get
            {
                return GetPattrenObjectInstance<UIATransform>(ref uiatranform, UIATransform.GetInstance);
            }
        }

        public IUIAToggleProvider ProviderToggle
        {
            get
            {
                return GetPattrenObjectInstance<UIAToggle>(ref uiatoggle, UIAToggle.GetInstance);
            }
        }

        public IUIATableItemProvider ProviderTableItem
        {
            get
            {
                return GetPattrenObjectInstance<UIATableItem>(ref uiatableitem, UIATableItem.GetInstance);
            }
        }

        public IUIATableProvider ProviderTableProvider
        {
            get
            {
                return GetPattrenObjectInstance<UIATable>(ref uiatable, UIATable.GetInstance);
            }
        }

        public IUIASelectionItemProvider ProviderSelectionItem
        {
            get
            {
                return GetPattrenObjectInstance<UIASelectionItem>(ref uiaselectionitem, UIASelectionItem.GetInstance);
            }
        }

        public IUIASelectionProvider ProviderSelection
        {
            get
            {
                return GetPattrenObjectInstance<UIASelection>(ref uiaselection, UIASelection.GetInstance);
            }
        }

        public IUIARangeValueProvider ProviderRangeValue
        {
            get
            {
                return GetPattrenObjectInstance<UIARangeValue>(ref uiarangevalue, UIARangeValue.GetInstance);
            }
        }


        public IUIAInvokeProvider ProviderInvoke
        {
            get
            {
                return GetPattrenObjectInstance<UIAInvoke>(ref uiainvoke, UIAInvoke.GetInstance);
            }
        }

        public IUIAGridItemProvider ProviderGridItem
        {
            get
            {
                return GetPattrenObjectInstance<UIAGridItem>(ref uiagriditem, UIAGridItem.GetInstance);
            }
        }

        public IUIAGridProvider ProviderGrid
        {
            get
            {
                return GetPattrenObjectInstance<UIAGrid>(ref uiagrid, UIAGrid.GetInstance);
            }
        }

        public IUIAGenericProvider ProviderGeneric
        {
            get
            {
                return GetPattrenObjectInstance<UIAGeneric>(ref uiageneric, UIAGeneric.GetInstance);
            }
        }

        public IUIAExpandCollapseProvider ProviderExpandCollapse
        {
            get
            {
                return GetPattrenObjectInstance(ref uiaexpandcollapse, UIAExpandCollapse.GetInstance);
            }
        }

        public IMSAAAccessibleProvider ProviderMSAAAccessible
        {
            get
            {
                return GetPattrenObjectInstance(ref uiaaccessible, MSAAElement.GetInstance);
            }
        }


        public IUIAScrollProvider ProviderScroll
        {
            get
            {
                return GetPattrenObjectInstance<UIAScroll>(ref uiascroll, UIAScroll.GetInstance);
            }
        }

        public IUIAScrollItemProvider ProviderScrollItem
        {
            get
            {
                return GetPattrenObjectInstance<UIAScrollItem>(ref uiascrollitem, UIAScrollItem.GetInstance);
            }
        }

        public IUIAConstants Constants
        {
            get
            {
                if (uiaconstants == null)
                    uiaconstants = new UIAConstants();

                return uiaconstants;
            }
        }

        public IUIANavigationProvider ProviderNavigation
        {
            get
            {
                return GetPattrenObjectInstance(ref uianavigation, UIANavigationProvider.GetInstance);
            }
        }

    }
}

