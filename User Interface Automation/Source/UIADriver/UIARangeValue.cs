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
    public interface IUIARangeValueProvider
    {
        bool IsReadOnly
        {
            get;
        }

        double LargeChange
        {
            get;
        }

        double Maximum
        {
            get;
        }

        double Minimum
        {
            get;
        }

        double SmallChange
        {
            get;
        }

        double Value
        {
            get;
            set;
        }
    }

    class UIARangeValue : UIADriverPatternBase, IUIARangeValueProvider
    {
        internal static UIARangeValue GetInstance(UIAElement owner)
        {
            return GetPatternObject<UIARangeValue, RangeValuePattern>(owner, RangeValuePattern.Pattern);
        }


        public bool IsReadOnly
        {
            get
            {
                bool ret = false;
                Log("IsReadOnly");
                try
                {
                    ret = ((RangeValuePattern)pattern).Current.IsReadOnly;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public double LargeChange
        {
            get
            {
                double ret = 0;
                Log("LargeChange");
                try
                {
                    ret = ((RangeValuePattern)pattern).Current.LargeChange;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public double Maximum
        {
            get
            {
                double ret = 0;
                Log("Maximum");
                try
                {
                    ret = ((RangeValuePattern)pattern).Current.Maximum;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public double Minimum
        {
            get
            {
                double ret = 0;
                Log("Minimum");
                try
                {
                    ret = ((RangeValuePattern)pattern).Current.Minimum;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public double SmallChange
        {
            get
            {
                double ret = 0;
                Log("SmallChange");
                try
                {
                    ret = ((RangeValuePattern)pattern).Current.SmallChange;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public double Value
        {
            get
            {
                double ret = 0;
                Log("get_Value");
                try
                {
                    ret = ((RangeValuePattern)pattern).Current.Value;
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
                    ((RangeValuePattern)pattern).SetValue(value);
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

