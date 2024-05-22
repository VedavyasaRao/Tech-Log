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
    public interface IUIATransformProvider
    {
        bool CanMove
        {
            get;
        }

        bool CanResize
        {
            get;
        }

        bool CanRotate
        {
            get;
        }

        void Move(double x, double y);

        void Resize(double width, double height);

        void Rotate(double degrees);
    }

    class UIATransform : UIADriverPatternBase, IUIATransformProvider
    {
        internal static UIATransform GetInstance(UIAElement owner)
        {
            return GetPatternObject<UIATransform, TransformPattern>(owner, TransformPattern.Pattern);
        }

        public bool CanMove
        {
            get
            {
                bool ret = false;
                Log("CanMove");
                try
                {
                    ret = ((TransformPattern)pattern).Current.CanMove;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public bool CanResize
        {
            get
            {
                bool ret = false;
                Log("CanResize");
                try
                {
                    ret = ((TransformPattern)pattern).Current.CanResize;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public bool CanRotate
        {
            get
            {
                bool ret = false;
                Log("CanRotate");
                try
                {
                    ret = ((TransformPattern)pattern).Current.CanRotate;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public void Move(double x, double y)
        {
            Log(string.Format("Move({0},{1})", x, y));
            try
            {
                ((TransformPattern)pattern).Move(x, y);
                CauseDelay();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

        public void Resize(double width, double height)
        {
            Log(string.Format("Rotate({0},{1})", width, height));
            try
            {
                ((TransformPattern)pattern).Resize(width, height);
                CauseDelay();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

        public void Rotate(double degrees)
        {
            Log(string.Format("Rotate({0})", degrees));
            try
            {
                ((TransformPattern)pattern).Rotate(degrees);
                CauseDelay();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }
    }
}

