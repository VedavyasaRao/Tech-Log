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
    public interface IUIAWindowProvider
    {

        void Close();

        int InteractionState
        {
            get;
        }

        bool IsModal
        {
            get;
        }

        bool IsTopmost
        {
            get;
        }

        bool Maximizable
        {
            get;
        }

        bool Minimizable
        {
            get;
        }

        void SetVisualState(int state);

        int VisualState
        {
            get;
        }

         bool WaitForInputIdle(int milliseconds);

    }

    class UIAWindow : UIADriverPatternBase, IUIAWindowProvider
    {
        internal static UIAWindow GetInstance(UIAElement owner)
        {
            return GetPatternObject<UIAWindow, WindowPattern>(owner, WindowPattern.Pattern);
        }

        public void Close()
        {
            Log("Close()");
            try
            {
                ((WindowPattern)pattern).Close();
                CauseDelay();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

        public int InteractionState
        {
            get
            {
                WindowInteractionState ret = WindowInteractionState.NotResponding;
                Log("InteractionState");
                try
                {
                    ret = (WindowInteractionState)((WindowPattern)pattern).Current.WindowInteractionState;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return (int)ret;
            }

        }

        public bool IsModal
        {
            get
            {
                bool ret = false;
                Log("IsModal");
                try
                {
                    ret = ((WindowPattern)pattern).Current.IsModal;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public bool IsTopmost
        {
            get
            {
                bool ret = false;
                Log("IsTopmost");
                try
                {
                    ret = ((WindowPattern)pattern).Current.IsTopmost;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public bool Maximizable
        {
            get
            {
                bool ret = false;
                Log("Maximizable");
                try
                {
                    ret = ((WindowPattern)pattern).Current.CanMaximize;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public bool Minimizable
        {
            get
            {
                bool ret = false;
                Log("Minimizable");
                try
                {
                    ret = ((WindowPattern)pattern).Current.CanMinimize;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;
            }
        }


        public void SetVisualState(int state)
        {
            Log(string.Format("SetVisualState({0})", state));
            try
            {
                ((WindowPattern)pattern).SetWindowVisualState((WindowVisualState)state);
                CauseDelay();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

        public int VisualState
        {
            get
            {
                WindowVisualState ret = WindowVisualState.Normal;
                Log("VisualState");
                try
                {
                    ret = ((WindowPattern)pattern).Current.WindowVisualState;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return (int)ret;
            }
        }

        public bool WaitForInputIdle(int milliseconds)
        {

            bool ret = false;
            Log(string.Format("WaitForInputIdle({0})", milliseconds));
            try
            {
                ret = ((WindowPattern)pattern).WaitForInputIdle(milliseconds);
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

