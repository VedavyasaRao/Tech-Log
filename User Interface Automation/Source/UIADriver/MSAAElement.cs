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
using Accessibility;
using AccCheck;

namespace UITesting.Automated.UIADriver
{
    public interface IMSAAAccessibleProvider
    {
        void DoDefaultAction();

        void MakeSelection(int flagsSelect);

        string DefaultAction
        {
            get;
        }

        string Description
        {
            get;
        }

        string Name
        {
            get;
        }

        int Role
        {
            get;
        }

        int State
        {
            get;
        }

        string Value
        {
            get;
            set;
        }

        IMSAAAccessibleProvider Focus
        {
            get;
        }

        string KeyboardShortcut
        {
            get;
        }

        IMSAAAccessibleProvider Parent
        {
            get;
        }
    }


    class MSAAElement : UIADriverPatternBase, IMSAAAccessibleProvider
    {

        private Accessible accobj;

        private void GetAccessibleObject()
        {
            accobj = null;

            try
            {
                int hwnd =  (int)owner.GetAE.GetCurrentPropertyValue(AutomationElement.NativeWindowHandleProperty);
                if (hwnd != 0)
                {
                    Accessible.FromWindow(new IntPtr(hwnd), out accobj);
                }
                else
                {
                    System.Windows.Rect boundingRect = (System.Windows.Rect)owner.GetAE.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty);
                    Point pt = new Point((boundingRect.Left + boundingRect.Width / 2), (boundingRect.Top + boundingRect.Height / 2));
                    Accessible.FromPoint(new System.Drawing.Point((int)pt.X,(int)pt.Y), out accobj);

                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }


        internal static MSAAElement GetInstance(UIAElement owner)
        {
            MSAAElement ret = GetPatternObject<MSAAElement>(owner);
            ret.GetAccessibleObject();

            return ret;
        }

        public void DoDefaultAction()
        {
            Log("DoDefaultAction()");
            try
            {
                accobj.DoDefaultAction();
                CauseDelay();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

        public void MakeSelection(int flagsSelect)
        {
            Log(string.Format("{0}({1})", "MakeSelection", flagsSelect));
            try
            {
                accobj.Select(flagsSelect);
                CauseDelay();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

        public string DefaultAction
        {
            get
            {
                string ret = "";
                Log("DefaultAction");
                try
                {
                    ret = accobj.DefaultAction;
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }

                return ret;
            }
        }


        public string Description
        {
            get
            {
                string ret = "";
                Log("Description");
                try
                {
                    ret = accobj.Description;
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }

                return ret;
            }
        }

        public string Name
        {
            get
            {
                string ret = "";
                Log("Name");
                try
                {
                    ret = accobj.Name;
                }

                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;
            }
        }


        public int Role
        {
            get
            {
                int ret = 0;
                Log("Role");
                try
                {
                    ret = accobj.Role;
                }

                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;
            }
        }


        public int State
        {
            get
            {
                int ret = 0;
                Log("State");
                try
                {
                    ret = accobj.State;
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;
            }
        }


        public string Value
        {
            get
            {
                Log("GetValue");
                try
                {
                    return accobj.Value;
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return "";
            }
            set
            {
                Log("SetValue");
                try
                {
                    accobj.Value = value;
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
            }
        }

        public IMSAAAccessibleProvider Parent
        {
            get
            {
                Log("Parent");
                try
                {
                    MSAAElement ret = GetPatternObject<MSAAElement>(owner);
                    ret.accobj = accobj.Parent;
                    return ret;
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return null;
            }
        }

        public string KeyboardShortcut
        {
            get
            {
                Log("KeyboardShortcut");
                try
                {
                    return KeyboardShortcut;
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return "";
            }
        }

        public IMSAAAccessibleProvider Focus
        {
            get
            {
                Log("Focus");
                try
                {
                    MSAAElement ret = GetPatternObject<MSAAElement>(owner);
                    ret.accobj = accobj.Focus;
                    return ret;
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return null;
            }
        }
    }
}

