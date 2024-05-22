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
    public interface IUIAScrollProvider
    {
        bool HorizontallyScrollable { get; }
        double HorizontalScrollPercent { get; }
        double HorizontalViewSize { get; }
        bool VerticallyScrollable { get; }
        double VerticalScrollPercent { get; }
        double VerticalViewSize { get; }
        void Scroll(int horizontalAmount, int verticalAmount);
        void SetScrollPercent(double horizontalPercent, double verticalPercent);
    }


    class UIAScroll : UIADriverPatternBase, IUIAScrollProvider
    {
        internal static UIAScroll GetInstance(UIAElement owner)
        {
            return GetPatternObject<UIAScroll, ScrollPattern>(owner, ScrollPattern.Pattern);
        }


        public bool HorizontallyScrollable
        {
            get
            {
                bool ret = false;
                Log("HorizontallyScrollable");
                try
                {
                    ret = ((ScrollPattern)pattern).Current.HorizontallyScrollable;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public double HorizontalScrollPercent
        {
            get
            {
                double ret = 0;
                Log("HorizontalScrollPercent");
                try
                {
                    ret = ((ScrollPattern)pattern).Current.HorizontalScrollPercent;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public double HorizontalViewSize
        {
            get
            {
                double ret = 0;
                Log("HorizontalViewSize");
                try
                {
                    ret = ((ScrollPattern)pattern).Current.HorizontalViewSize;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public bool VerticallyScrollable
        {
            get
            {
                bool ret = false;
                Log("VerticallyScrollable");
                try
                {
                    ret = ((ScrollPattern)pattern).Current.VerticallyScrollable;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public double VerticalScrollPercent
        {
            get
            {
                double ret = 0;
                Log("VerticalScrollPercent");
                try
                {
                    ret = ((ScrollPattern)pattern).Current.VerticalScrollPercent;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public double VerticalViewSize
        {
            get
            {
                double ret = 0;
                Log("VerticalViewSize");
                try
                {
                    ret = ((ScrollPattern)pattern).Current.VerticalViewSize;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public void Scroll(int horizontalAmount, int verticalAmount)
        {
            Log(string.Format("Scroll({0}.{1})", horizontalAmount, verticalAmount));
            try
            {
                ((ScrollPattern)pattern).Scroll((ScrollAmount)horizontalAmount, (ScrollAmount)verticalAmount);
                CauseDelay();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

        public void SetScrollPercent(double horizontalPercent, double verticalPercent)
        {
            Log(string.Format("SetScrollPercent({0}.{1})", horizontalPercent, verticalPercent));
            try
            {
                ((ScrollPattern)pattern).SetScrollPercent(horizontalPercent, verticalPercent);
                CauseDelay();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

    }
}

