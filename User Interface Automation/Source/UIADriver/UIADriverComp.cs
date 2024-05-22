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

/*
    /// <summary>IUIADriverSelection</summary>
    [Guid("D825D5C9-E994-4df6-8440-4C87A1499102")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IUIADriverSelection
    {
        bool CanSelectMultiple();
        public IRawElementProviderSimple[] GetSelection()
        {
            throw new NotImplementedException();
        }

        bool IsSelectionRequired();
    }
 



    /// <summary>IUIADriverWindow</summary>
    [Guid("BB66F73B-A34F-4c4e-B9BD-97748C3804DC")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IUIADriverWindow
    {
        void Close(int iwaitms=1000);
        int GetInteractionState(int iwaitms = 1000);
        bool IsModal(int iwaitms = 1000);
        bool IsTopmost(int iwaitms = 1000);
        bool IsMaximizable(int iwaitms = 1000);
        bool IsMinimizable(int iwaitms = 1000);
        void SetVisualState(int state, int iwaitms = 1000);
        int GetVisualState(int iwaitms = 1000);
        bool WaitForInputIdle(int milliseconds);
    }


*/







    }

    ///// <summary>IUIADriver</summary>
    //[Guid("4E29CFA7-6FFF-44b9-A4B4-CD6B2CA22060")]
    //[InterfaceType(ComInterfaceType.InterfaceIsDual)] 
    //public interface IUIADriver
    //{

    //    /// <summary>SetLogFile</summary>
    //    [DispId(1969030801)]
    //    bool SetLogFile(string logfilename);

    //    /// <summary>Setfocus</summary>
    //    [DispId(1969030802)]
    //    bool Setfocus(string details, int bwaitms);

    //    /// <summary>SetText</summary>
    //    [DispId(1969030803)]
    //    bool SetText(string details, string texttoset, int bwaitms);

    //    /// <summary>ClickButton</summary>
    //    [DispId(1969030804)]
    //    bool ClickButton(string details, int bwaitms);

    //    /// <summary>SelectItem</summary>
    //    [DispId(1969030805)]
    //    bool SelectItem(string details, int bwaitms);

    //    /// <summary>ToggleCheckBox</summary>
    //    [DispId(1969030806)]
    //    bool ToggleCheckBox(string details, int bwaitms);

    //    /// <summary>Click</summary>
    //    [DispId(1969030807)]
    //    bool Click(string details, bool bright, bool bdouble, int bwaitms);

    //    /// <summary>ClickBoundRectangle</summary>
    //    [DispId(1969030808)]
    //    bool ClickBoundRectangle(string details, bool bright, bool bdouble, int bwaitms);

    //    /// <summary>ClickGrid</summary>
    //    [DispId(1969030809)]
    //    bool ClickGrid(string details, bool bright, bool bdouble, int offset, int itemheight, int row, int bwaitms);

    //    /// <summary>MoveandClick</summary>
    //    [DispId(1969030810)]
    //    bool MoveandClick(int x, int y, bool brelative, bool bright, bool bdouble, int bwaitms);

    //    /// <summary>CheckControlVisiblity</summary>
    //    [DispId(1969030811)]
    //    bool CheckControlVisiblity(string details, bool bvisible, int maxwaitms);

    //}


    ///// <summary>UIADriverComp</summary>
    //[Guid("8CD351C8-CB70-446b-8720-9F2504B70F32"),
    // ClassInterface(ClassInterfaceType.AutoDual),
    // ProgId("UIADriver.UIADriverComp"),
    // ComSourceInterfaces(typeof(IUIADriver))]
    //public class UIADriverComp : IUIADriver
    //{
    //    const int DelayMS = 500;
    //    string logfile = "";
    //    MouseSimulator mousesim = new MouseSimulator();
    //    Point curpt = new Point();

    //    void ToScreen(ref Point pt)
    //    {
    //        curpt = pt;
    //        pt.X = (pt.X * 65535.0) / System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width; ;
    //        pt.Y = (pt.Y * 65535.0) / System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

    //    }

    //    void MouseMove(Point pt, bool bright, bool bdouble)
    //    {
    //        ToScreen(ref pt);
    //        mousesim.MoveMouseTo(pt.X, pt.Y);
    //        Application.DoEvents();
    //        System.Threading.Thread.Sleep(DelayMS);

    //        if (bright)
    //        {
    //            if (bdouble)
    //            {
    //                mousesim.RightButtonDoubleClick();
    //            }
    //            else
    //            {
    //                mousesim.RightButtonClick();
    //            }
    //        }
    //        else
    //        {
    //            if (bdouble)
    //            {
    //                mousesim.LeftButtonDoubleClick();
    //            }
    //            else
    //            {
    //                mousesim.LeftButtonClick();
    //            }
    //        }
    //        Application.DoEvents();
    //    }


    //    void MouseClick(Point pt, bool bright, bool bdouble)
    //    {
    //        ToScreen(ref pt);
    //        mousesim.MoveMouseTo(pt.X, pt.Y);
    //        Application.DoEvents();
    //        System.Threading.Thread.Sleep(DelayMS);

    //        if (bright)
    //        {
    //            if (bdouble)
    //            {
    //                mousesim.RightButtonDoubleClick();
    //            }
    //            else
    //            {
    //                mousesim.RightButtonClick();
    //            }
    //        }
    //        else
    //        {
    //            if (bdouble)
    //            {
    //                mousesim.LeftButtonDoubleClick();
    //            }
    //            else
    //            {
    //                mousesim.LeftButtonClick();
    //            }
    //        }
    //        Application.DoEvents();
    //    }

    //    //TICS -6@201  -- not relevant here
    //    AutomationElement GetElement(string details, bool busevalue)
    //    {
    //        ControlInfo[] ctls = JSONPersister<ControlInfo[]>.SetJSON(details);

    //        AutomationElement ae = null;

    //        AutomationElementCollection windows = null;

    //        if (ctls[1].AEAutomationId != "")
    //        {
    //            windows = AutomationElement.RootElement.FindAll(TreeScope.Children,
    //            new PropertyCondition(AutomationElement.AutomationIdProperty, ctls[1].AEAutomationId));
    //        }

    //        if (windows == null || windows.Count == 0)
    //        {
    //            windows = AutomationElement.RootElement.FindAll(TreeScope.Children,
    //            new PropertyCondition(AutomationElement.NameProperty, ctls[1].AEText));
    //        }


    //        if (windows == null)
    //        {
    //            LogMessage(string.Format("GetElement {0} not found", ctls[1].UserName));
    //            return ae;
    //        }

    //        if (ctls[0].Path == "")
    //        {
    //            return windows[0];
    //        }

    //        foreach (AutomationElement window in windows)
    //        {
    //            if (ctls[0].AEAutomationId != "")
    //            {
    //                ae = window.FindFirst(TreeScope.Descendants,
    //                new PropertyCondition(AutomationElement.AutomationIdProperty, ctls[0].AEAutomationId));
    //                if (ae != null)
    //                {
    //                    LogMessage(string.Format("GetElement {0} automationid {1} found", Getctlname(details), ctls[0].AEAutomationId));
    //                    return ae;
    //                }
    //                LogMessage(string.Format("GetElement {0} automationid {1} not found", Getctlname(details), ctls[0].AEAutomationId));
    //            }

    //            string[] path = ctls[0].Path.Trim().Split(new char[] { ' ' });
    //            ae = window;
                
    //            for (int k=0; k<path.Length; ++k)
    //            {
    //                string s = path[k];
    //                if (ae == null)
    //                {
    //                    break;
    //                }

    //                ae = TreeWalker.RawViewWalker.GetFirstChild(ae);
    //                if (ae == null)
    //                {
    //                    LogMessage(string.Format("GetElement {0} Level {1} position 0 not found", Getctlname(details), k));
    //                    break;
    //                }
    //                for (int i = 1; i <= int.Parse(s); ++i)
    //                {
    //                    ae = TreeWalker.RawViewWalker.GetNextSibling(ae);
    //                    if (ae == null)
    //                    {
    //                        LogMessage(string.Format("GetElement {0} Level {1} position {2} not found", Getctlname(details), k, i));
    //                        break;
    //                    }
    //                }

    //            }


    //            if (ae != null && ctls[0].AEType == ae.Current.ControlType.LocalizedControlType && (ctls[0].AEText == ae.Current.Name || !busevalue))
    //            {
    //                LogMessage(string.Format("GetElement {0} path  {1} found", Getctlname(details), ctls[0].Path.Trim()));
    //                return ae;
    //            }

    //            ae = null;
    //            if (ae == null)
    //            {
    //                LogMessage(string.Format("SearchAEElement {0} {1}  {2}:", ctls[0].UserName, ctls[0].AEType, ctls[0].AEText));
    //                SearchAEElement(ctls[0], window, ref ae, busevalue);
    //            }
    //            if (ae != null)
    //            {
    //                return ae;
    //            }
    //        }

    //        return ae;
    //    }

    //    //TICS +6@201


    //    private void SearchAEElement(ControlInfo actl, AutomationElement ae, ref AutomationElement searchedele, bool busevalue)
    //    {

    //        if (searchedele != null)
    //        {
    //            return;
    //        }


    //        LogMessage(string.Format("current element {0} {1}", ae.Current.ControlType.LocalizedControlType, ae.Current.Name));
    //        if (actl.AEType == ae.Current.ControlType.LocalizedControlType && (actl.AEText == ae.Current.Name || !busevalue)) 
    //        {
    //            searchedele = ae;
    //            LogMessage(string.Format("current element {0} {1} FOUND", ae.Current.ControlType.LocalizedControlType, ae.Current.Name));
    //            return;
    //        }

    //        try
    //        {
    //            AutomationElement elementNode = TreeWalker.RawViewWalker.GetFirstChild(ae);

    //            while (elementNode != null)
    //            {
    //                SearchAEElement(actl, elementNode, ref searchedele, busevalue);
    //                if (searchedele != null)
    //                {
    //                    break;
    //                }
    //                else
    //                {
    //                    elementNode = TreeWalker.RawViewWalker.GetNextSibling(elementNode);
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            string s = ex.Message;
    //        }

    //    }

    //    bool GetBRPoint(string details, out Point pt)
    //    {
    //        pt = new Point();

    //        ControlInfo[] ctls = JSONPersister<ControlInfo[]>.SetJSON(details);
    //        string[] rect = ctls[0].AEBoundingRectangle.Split(new char[] { ' ' });
    //        pt = new Point(int.Parse(rect[0]) + int.Parse(rect[2]) / 2, int.Parse(rect[1]) + int.Parse(rect[3]) / 2);

    //        return true;
    //    }

    //    string Getctlname(string details)
    //    {
    //        ControlInfo[] ctls = JSONPersister<ControlInfo[]>.SetJSON(details);
    //        return ctls[0].UserName;
    //    }

    //     void LogMessage(string msg)
    //     {
    //         if (logfile == "")
    //         {
    //             return;
    //         }

    //         StreamWriter sw = new StreamWriter(logfile, true);
    //         sw.WriteLine(msg);
    //         sw.Close();

    //     }

    //    //*********************INTERFACE**************************
    //     /// <summary>SetLogFile</summary>
    //     public bool SetLogFile(string logfilename)
    //     {
    //         logfile = logfilename;
    //         return true;
    //     }

    //     /// <summary>MoveandClick</summary>
    //     public bool MoveandClick(int x, int y, bool brelative, bool bright, bool bdouble, int bwaitms)
    //    {
    //        bool bret=false;
    //        try
    //        {
    //            LogMessage(string.Format("MoveandClick({0},{1},{2},{3},{4},{5})", x, y, brelative, bright, bdouble, bwaitms));
    //            Point pt = curpt;
    //            if (brelative)
    //            {
    //                pt.X += x;
    //                pt.Y += y;
    //            }
    //            else
    //            {
    //                pt.X = x;
    //                pt.Y = y;
    //            }
    //            MouseClick(pt, bright, bdouble);
    //            Application.DoEvents();
    //            System.Threading.Thread.Sleep(bwaitms);
    //            bret =  true;
    //        }
    //        catch (Exception ex)
    //        {
    //            LogMessage("Exception occured:"+ex.Message);
    //        }

    //        return bret;
    //    }

    //     /// <summary>ClickGrid</summary>
    //    public bool ClickGrid(string details, bool bright, bool bdouble, int offset, int itemheight, int row, int bwaitms)
    //    {
    //        bool bret = false;
    //        try
    //        {
    //            LogMessage(string.Format("ClickGrid({0},{1},{2},{3},{4},{5},{6})",
    //                Getctlname(details), bright, bdouble, offset, itemheight, row, bwaitms));

    //            AutomationElement ae = GetElement(details,true);
    //            if (ae == null)
    //            {
    //                return false;
    //            }

    //            Rect br = (Rect)ae.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty);
    //            Point pt = new Point(br.Left + br.Width / 2, br.Top + (offset + (itemheight * row)));
    //            MouseClick(pt, bright, bdouble);

    //            Application.DoEvents();
    //            System.Threading.Thread.Sleep(bwaitms);
    //            bret = true;
    //        }
    //        catch (Exception ex)
    //        {
    //            LogMessage("Exception occured:"+ex.Message);

    //        }
    //        return bret;
    //    }

    //    /// <summary>ClickBoundRectangle</summary>
    //    public bool ClickBoundRectangle(string details, bool bright, bool bdouble, int bwaitms)
    //    {
    //        bool bret=false;
    //        try
    //        {
    //            LogMessage(string.Format("ClickBoundRectangle({0},{1},{2},{3})", Getctlname(details), bright, bdouble, bwaitms));

    //            Point pt;

    //            if (!GetBRPoint(details, out pt))
    //            {
    //                return false;
    //            }

    //            MouseClick(pt, bright, bdouble);
    //            Application.DoEvents();
    //            System.Threading.Thread.Sleep(bwaitms);

    //            bret = true;
    //        }
    //        catch (Exception ex)
    //        {
    //            LogMessage("Exception occured:"+ex.Message);
    //        }
    //        return bret;
    //    }

    //    /// <summary>Click</summary>
    //    public bool Click(string details, bool bright, bool bdouble, int bwaitms)
    //    {
    //        const int delay = 200;
    //        bool bret=false;
    //        try
    //        {
    //            LogMessage(string.Format("Click({0},{1},{2},{3})", Getctlname(details), bright, bdouble, bwaitms));
    //            AutomationElement ae = GetElement(details, true);
    //            if (ae == null)
    //            {
    //                return false;
    //            }
    //            Point pt;

    //            if (!ae.TryGetClickablePoint(out pt))
    //            {
    //                Rect br = (Rect)ae.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty);
    //                pt = new Point(br.Left + br.Width / 2, br.Top + br.Height / 2);
    //            }

    //            ae.SetFocus();
    //            Application.DoEvents();
    //            System.Threading.Thread.Sleep(delay);
    //            MouseClick(pt, bright, bdouble);
    //            Application.DoEvents();
    //            System.Threading.Thread.Sleep(bwaitms);
    //            bret = true;
    //        }
    //        catch (Exception ex)
    //        {
    //            LogMessage("Exception occured:" + ex.Message);
    //        }
    //        return bret;
    //    }

    //    /// <summary>ToggleCheckBox</summary>
    //    public bool ToggleCheckBox(string details, int bwaitms)
    //    {
    //        bool bret = false;
    //        try
    //        {
    //            LogMessage(string.Format("ToggleCheckBox({0},{1})", Getctlname(details), bwaitms));
    //            return Click(details, false, false, bwaitms);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogMessage("Exception occured:" + ex.Message);
    //        }
    //        return bret;
    //    }

    //    /// <summary>SelectItem</summary>
    //    public bool SelectItem(string details, int bwaitms)
    //    {
    //        bool bret = false;
    //        try
    //        {
    //            LogMessage(string.Format("SelectItem({0},{1})", Getctlname(details), bwaitms));
    //            AutomationElement ae = GetElement(details, true);
    //            if (ae == null)
    //            {
    //                return false;
    //            }

    //            SelectionItemPattern sipattern = ae.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
    //            if (sipattern == null)
    //            {
    //                return false;
    //            }

    //            sipattern.Select();
    //            Application.DoEvents();
    //            System.Threading.Thread.Sleep(bwaitms);
    //            bret = true;
    //        }
    //        catch (Exception ex)
    //        {
    //            LogMessage("Exception occured:" + ex.Message);
    //        }
    //        return bret;
    //    }

    //    /// <summary>ClickButton</summary>
    //    public bool ClickButton(string details, int bwaitms)
    //    {
    //        bool bret = false;
    //        try
    //        {
    //            LogMessage(string.Format("ClickButton({0},{1})", Getctlname(details), bwaitms));
    //            AutomationElement ae = GetElement(details, true);
    //            if (ae == null)
    //            {
    //                return false;
    //            }

    //            InvokePattern pattern = ae.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
    //            if (pattern == null)
    //            {
    //                return false;
    //            }

    //            pattern.Invoke();
    //            Application.DoEvents();
    //            System.Threading.Thread.Sleep(bwaitms);
    //            bret = true;
    //        }
    //        catch (Exception ex)
    //        {
    //            LogMessage("Exception occured:" + ex.Message);
    //        }
    //        return bret;
    //    }

    //    /// <summary>SetText</summary>
    //    public bool SetText(string details, string texttoset, int bwaitms)
    //    {
    //        bool bret = false;
    //        try
    //        {
    //            LogMessage(string.Format("SetText({0},{1})", Getctlname(details), bwaitms));
    //            AutomationElement ae = GetElement(details, false);
    //            if (ae == null)
    //            {
    //                return false;
    //            }
    //            ValuePattern pattern = ae.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
    //            if (pattern == null)
    //            {
    //                return false;
    //            }
    //            pattern.SetValue(texttoset);
    //            Application.DoEvents();
    //            System.Threading.Thread.Sleep(bwaitms);
    //            bret = true;
    //        }
    //        catch (Exception ex)
    //        {
    //            LogMessage("Exception occured:" + ex.Message);
    //        }
    //        return bret;
    //    }

    //    /// <summary>Setfocus</summary>
    //    public bool Setfocus(string details, int bwaitms)
    //    {
    //        bool bret = false;
    //        try
    //        {
    //            LogMessage(string.Format("Setfocus({0},{1})", Getctlname(details), bwaitms));
    //            AutomationElement ae = GetElement(details, true);
    //            if (ae == null)
    //            {
    //                return false;
    //            }
    //            ae.SetFocus();
    //            Application.DoEvents();
    //            System.Threading.Thread.Sleep(bwaitms);
    //            bret = true;

    //        }
    //        catch (Exception ex)
    //        {

    //            LogMessage("Exception occured:" + ex.Message);

    //        }
    //        return bret;
    //    }

    //    /// <summary>CheckControlVisiblity</summary>
    //    public bool CheckControlVisiblity(string details, bool bvisible, int maxwaitms)
    //    {
    //        bool bret = false;
    //        try
    //        {
    //            int startwait = 0;
    //            LogMessage(string.Format("CheckControlVisiblity({0},{1},{2})", Getctlname(details), bvisible, maxwaitms));
    //            while (startwait <= maxwaitms)
    //            {
    //                AutomationElement ae = GetElement(details, true);
    //                if ((ae != null) == bvisible)
    //                {
    //                    bret = true;
    //                    break;
    //                }
    //                Application.DoEvents();
    //                System.Threading.Thread.Sleep(1000);
    //                startwait += 1000;
    //            }
    //        }
    //        catch (Exception ex)
    //        {

    //            LogMessage("Exception occured:" + ex.Message);

    //        }
    //        return bret;
    //    }


    //    /// <summary>DragAndDrop</summary>
    //    public bool DragAndDrop(string details, int xofst, int yofst, int bwaitms)
    //    {
    //        bool bret = false;
    //        const int mofst = 50;
    //        try
    //        {
    //            LogMessage(string.Format("DragAndDrop({0},{1},{2},{3})", Getctlname(details), xofst, yofst, bwaitms));
    //            AutomationElement ae = GetElement(details, true);
    //            if (ae == null)
    //            {
    //                return false;
    //            }

    //            Point presentpos = curpt;
    //            Point destpos;
    //            if (!GetBRPoint(details, out destpos))
    //            {
    //                return false;
    //            }
    //            destpos.X += xofst;
    //            destpos.Y += yofst;
    //            ToScreen(ref destpos);

    //            mousesim.LeftButtonDown((int)presentpos.X, (int)presentpos.Y);
    //            Application.DoEvents();
    //            System.Threading.Thread.Sleep(1000);


    //            for (int i = 5; i >= 0; --i)
    //            {

    //                mousesim.MoveMouseTo(destpos.X, destpos.Y + i * mofst);
    //                Application.DoEvents();
    //                System.Threading.Thread.Sleep(1000);
    //            }

    //            presentpos = curpt;
    //            mousesim.LeftButtonUp((int)destpos.X, (int)destpos.Y);
    //            Application.DoEvents();
    //            System.Threading.Thread.Sleep(bwaitms);
    //            bret = true;

    //        }
    //        catch (Exception ex)
    //        {
    //            LogMessage("Exception occured:"+ex.Message);
    //        }
    //        return bret;
    //    }

    //}
}

