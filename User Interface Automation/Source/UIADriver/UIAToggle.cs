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
    public interface IUIAToggleProvider
    {
        void Toggle();

        int ToggleState
        {
            get;
        }
    }

    class UIAToggle : UIADriverPatternBase, IUIAToggleProvider
    {

        internal static UIAToggle GetInstance(UIAElement owner)
        {
            return GetPatternObject<UIAToggle, TogglePattern>(owner, TogglePattern.Pattern);
        }


        public void Toggle()
        {
            Log("Toggle()");
            try
            {
                ((TogglePattern)pattern).Toggle();
                CauseDelay();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

        public int ToggleState
        {
            get
            {
                ToggleState ret = System.Windows.Automation.ToggleState.Indeterminate;
                Log("ToggleState");
                try
                {
                    ret = ((TogglePattern)pattern).Current.ToggleState;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return (int)ret;
            }
        }
    }

}

