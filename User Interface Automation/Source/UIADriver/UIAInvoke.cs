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
    public interface IUIAInvokeProvider
    {
        void Click();
    }

    class UIAInvoke : UIADriverPatternBase, IUIAInvokeProvider
    {
        internal static UIAInvoke GetInstance(UIAElement owner)
        {
            return GetPatternObject<UIAInvoke, InvokePattern>(owner, InvokePattern.Pattern);
        }


        public void Click()
        {
            Log("Invoke");
            try
            {
                ((InvokePattern)pattern).Invoke();
                CauseDelay();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

    }

}

