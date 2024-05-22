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
    public interface IUIAValueProvider 
    {
        bool IsReadOnly
        {
            get;
        }

        string Value
        {
            get;
            set;
        }

    }
    

    class UIAValue : UIADriverPatternBase, IUIAValueProvider
    {
        internal static UIAValue GetInstance(UIAElement owner)
        {
            return GetPatternObject<UIAValue, ValuePattern>(owner, ValuePattern.Pattern);
        }

        public bool IsReadOnly
        {
            get
            {
                bool ret = false;
                Log("IsReadOnly");
                try
                {
                    ret = ((ValuePattern)pattern).Current.IsReadOnly;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public void SetValue(string value)
        {
            Log(string.Format("SetValue({0})", value));
            try
            {
                ((ValuePattern)pattern).SetValue(value);
                CauseDelay();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

        public string Value
        {
            get
            {
                string ret = "";
                Log("get_Value");
                try
                {
                    ret = ((ValuePattern)pattern).Current.Value;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;
            }
            set
            {
                Log(string.Format("set_Value({0})", value));
                try
                {
                    ((ValuePattern)pattern).SetValue(value);
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
            }
        }
    }
}

