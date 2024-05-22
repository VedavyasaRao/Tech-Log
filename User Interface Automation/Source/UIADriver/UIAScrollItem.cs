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
    public interface IUIAScrollItemProvider
    {
        void ScrollIntoView();
    }

    class UIAScrollItem : UIADriverPatternBase, IUIAScrollItemProvider
    {
        internal static UIAScrollItem GetInstance(UIAElement owner)
        {
            return GetPatternObject<UIAScrollItem, ScrollItemPattern>(owner, ScrollItemPattern.Pattern);
        }


        public void ScrollIntoView()
        {
            Log(string.Format("ScrollIntoView()"));
            try
            {
                ((ScrollItemPattern)pattern).ScrollIntoView();
                CauseDelay();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

    }
}

