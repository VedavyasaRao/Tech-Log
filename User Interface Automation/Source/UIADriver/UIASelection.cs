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

    public interface IUIASelectionProvider
    {
        bool CanSelectMultiple
        {
            get;
        }

        IUIAElement GetSelection(int index);

        bool IsSelectionRequired
        {
            get;
        }
    }


    class UIASelection : UIADriverPatternBase, IUIASelectionProvider
    {

        internal static UIASelection GetInstance(UIAElement owner)
        {
            return GetPatternObject<UIASelection, SelectionPattern>(owner, SelectionPattern.Pattern);
        }

        public bool CanSelectMultiple
        {
            get
            {
                bool ret = false;
                Log("CanSelectMultiple");
                try
                {
                    ret = ((SelectionPattern)pattern).Current.CanSelectMultiple;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public IUIAElement GetSelection(int index)
        {
            AutomationElement[] selitms;
            UIAElement ret = null;
            Log(string.Format("GetSelection({0})", index));
            try
            {
                selitms = ((SelectionPattern)pattern).Current.GetSelection();
                if (selitms != null && index < selitms.Length)
                {
                    ret = UIAElement.GetInstance(selitms[0], owner);
                    CauseDelay();
                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
            return ret;
        }

        public bool IsSelectionRequired
        {
            get
            {
                bool ret = false;
                Log("IsSelectionRequired");
                try
                {
                    ret = ((SelectionPattern)pattern).Current.IsSelectionRequired;
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

