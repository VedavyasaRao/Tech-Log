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
    public interface IUIAElementProperties
    {
        string  AcceleratorKeyProperty {get;}
        string  AccessKeyProperty {get;}
        string  AutomationIdProperty {get;}
        string  BoundingRectangleProperty {get;}
        string  ClassNameProperty {get;}
        string  ClickablePointProperty {get;}
        string  ControlTypeProperty {get;}
        string  CultureProperty {get;}
        string  FrameworkIdProperty {get;}
        string  HasKeyboardFocusProperty {get;}
        string  HelpTextProperty {get;}
        string  IsContentElementProperty {get;}
        string  IsControlElementProperty {get;}
        string  IsDockPatternAvailableProperty {get;}
        string  IsEnabledProperty {get;}
        string  IsExpandCollapsePatternAvailableProperty {get;}
        string  IsGridItemPatternAvailableProperty {get;}
        string  IsGridPatternAvailableProperty {get;}
        string  IsKeyboardFocusableProperty {get;}
        string  IsMultipleViewPatternAvailableProperty {get;}
        string  IsOffscreenProperty {get;}
        string  IsPasswordProperty {get;}
        string  IsRangeValuePatternAvailableProperty {get;}
        string  IsRequiredForFormProperty {get;}
        string  IsScrollItemPatternAvailableProperty {get;}
        string  IsScrollPatternAvailableProperty {get;}
        string  IsSelectionItemPatternAvailableProperty {get;}
        string  IsSelectionPatternAvailableProperty {get;}
        string  IsTableItemPatternAvailableProperty {get;}
        string  IsTablePatternAvailableProperty {get;}
        string  IsTextPatternAvailableProperty {get;}
        string  IsTogglePatternAvailableProperty {get;}
        string  IsTransformPatternAvailableProperty {get;}
        string  IsValuePatternAvailableProperty {get;}
        string  IsWindowPatternAvailableProperty {get;}
        string  ItemStatusProperty {get;}
        string  ItemTypeProperty {get;}
        string  LabeledByProperty {get;}
        string  LocalizedControlTypeProperty {get;}
        string  NameProperty {get;}
        string  NativeWindowHandleProperty {get;}
        string  OrientationProperty {get;}
        string  ProcessIdProperty {get;}
        string  RuntimeIdProperty {get;}
    }

    class UIAElementProperties: UIADriverPatternBase, IUIAElementProperties
    {

        internal static UIAElementProperties GetInstance(UIAElement owner)
        {
            return GetPatternObject<UIAElementProperties>(owner);
        }



        public string AcceleratorKeyProperty
        {
            get 
            {
                string ret = "";
                Log("AcceleratorKeyProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.AcceleratorKeyProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string AccessKeyProperty
        {
            get 
            {
                string ret = "";
                Log("AccessKeyProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.AccessKeyProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string AutomationIdProperty
        {
            get 
            {
                string ret = "";
                Log("AutomationIdProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.AutomationIdProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }

        }

        public string BoundingRectangleProperty
        {
            get 
            {
                string ret = "";
                Log("BoundingRectangleProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty).ToString();
                }
                catch (Exception ex)
                {

                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public string ClassNameProperty
        {
            get 
            {
                string ret = "";
                Log("ClassNameProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.ClassNameProperty).ToString();
                }
                catch (Exception ex)
                {

                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public string ClickablePointProperty
        {
            get 
            {
                string ret = "";
                Log("ClickablePointProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.ClickablePointProperty).ToString();
                }
                catch (Exception ex)
                {

                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public string ControlTypeProperty
        {
            get 
            {
                string ret = "";
                Log("ControlTypeProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.ControlTypeProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string CultureProperty
        {
            get 
            {
                string ret = "";
                Log("CultureProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.CultureProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string FrameworkIdProperty
        {
            get 
            {
                string ret = "";
                Log("FrameworkIdProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.FrameworkIdProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string HasKeyboardFocusProperty
        {
            get 
            {
                string ret = "";
                Log("AcceleratorKeyProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.AcceleratorKeyProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string HelpTextProperty
        {
            get 
            {
                string ret = "";
                Log("HelpTextProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.HelpTextProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string IsContentElementProperty
        {
            get 
            {
                string ret = "";
                Log("IsContentElementProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.IsContentElementProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string IsControlElementProperty
        {
            get 
            {
                string ret = "";
                Log("IsControlElementProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.IsControlElementProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string IsDockPatternAvailableProperty
        {
            get 
            {
                string ret = "";
                Log("IsDockPatternAvailableProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.IsDockPatternAvailableProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string IsEnabledProperty
        {
            get 
            {
                string ret = "";
                Log("IsEnabledProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.IsEnabledProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string IsExpandCollapsePatternAvailableProperty
        {
            get 
            {
                string ret = "";
                Log("IsExpandCollapsePatternAvailableProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.IsExpandCollapsePatternAvailableProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string IsGridItemPatternAvailableProperty
        {
            get 
            {
                string ret = "";
                Log("IsGridItemPatternAvailableProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.IsGridItemPatternAvailableProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string IsGridPatternAvailableProperty
        {
            get 
            {
                string ret = "";
                Log("IsGridPatternAvailableProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.IsGridPatternAvailableProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string IsKeyboardFocusableProperty
        {
            get 
            {
                string ret = "";
                Log("IsKeyboardFocusableProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.IsKeyboardFocusableProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string IsMultipleViewPatternAvailableProperty
        {
            get 
            {
                string ret = "";
                Log("IsMultipleViewPatternAvailableProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.IsMultipleViewPatternAvailableProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string IsOffscreenProperty
        {
            get 
            {
                string ret = "";
                Log("IsOffscreenProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.IsOffscreenProperty).ToString();
                }
                catch (Exception ex)
                {

                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public string IsPasswordProperty
        {
            get 
            {
                string ret = "";
                Log("IsPasswordProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.IsPasswordProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string IsRangeValuePatternAvailableProperty
        {
            get 
            {
                string ret = "";
                Log("IsRangeValuePatternAvailableProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.IsRangeValuePatternAvailableProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string IsRequiredForFormProperty
        {
            get 
            {
                string ret = "";
                Log("IsRequiredForFormProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.IsRequiredForFormProperty).ToString();
                }
                catch (Exception ex)
                {

                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public string IsScrollItemPatternAvailableProperty
        {
            get 
            {
                string ret = "";
                Log("IsScrollItemPatternAvailableProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.IsScrollItemPatternAvailableProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string IsScrollPatternAvailableProperty
        {
            get 
            {
                string ret = "";
                Log("IsScrollPatternAvailableProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.IsScrollPatternAvailableProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string IsSelectionItemPatternAvailableProperty
        {
            get 
            {
                string ret = "";
                Log("IsSelectionItemPatternAvailableProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.IsSelectionItemPatternAvailableProperty).ToString();
                }
                catch (Exception ex)
                {

                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public string IsSelectionPatternAvailableProperty
        {
            get 
            {
                string ret = "";
                Log("IsSelectionPatternAvailableProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.IsSelectionPatternAvailableProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string IsTableItemPatternAvailableProperty
        {
            get 
            {
                string ret = "";
                Log("IsTableItemPatternAvailableProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.IsTableItemPatternAvailableProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string IsTablePatternAvailableProperty
        {
            get 
            {
                string ret = "";
                Log("IsTablePatternAvailableProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.IsTablePatternAvailableProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string IsTextPatternAvailableProperty
        {
            get 
            {
                string ret = "";
                Log("IsTextPatternAvailableProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.IsTextPatternAvailableProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string IsTogglePatternAvailableProperty
        {
            get 
            {
                string ret = "";
                Log("IsTogglePatternAvailableProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.IsTogglePatternAvailableProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string IsTransformPatternAvailableProperty
        {
            get 
            {
                string ret = "";
                Log("IsTransformPatternAvailableProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.IsTransformPatternAvailableProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string IsValuePatternAvailableProperty
        {
            get 
            {
                string ret = "";
                Log("IsValuePatternAvailableProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.IsValuePatternAvailableProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string IsWindowPatternAvailableProperty
        {
            get 
            {
                string ret = "";
                Log("IsWindowPatternAvailableProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.IsWindowPatternAvailableProperty).ToString();
                }
                catch (Exception ex)
                {

                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public string ItemStatusProperty
        {
            get 
            {
                string ret = "";
                Log("ItemStatusProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.ItemStatusProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string ItemTypeProperty
        {
            get 
            {
                string ret = "";
                Log("ItemTypeProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.ItemTypeProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string LabeledByProperty
        {
            get 
            {
                string ret = "";
                Log("LabeledByProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.LabeledByProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string LocalizedControlTypeProperty
        {
            get 
            {
                string ret = "";
                Log("LocalizedControlTypeProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.LocalizedControlTypeProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string NameProperty
        {
            get 
            {
                string ret = "";
                Log("NameProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.NameProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string NativeWindowHandleProperty
        {
            get 
            {
                string ret = "";
                Log("NativeWindowHandleProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.NativeWindowHandleProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string OrientationProperty
        {
            get 
            {
                string ret = "";
                Log("OrientationProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.OrientationProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string ProcessIdProperty
        {
            get 
            {
                string ret = "";
                Log("ProcessIdProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.ProcessIdProperty).ToString();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return ret;
            }
        }

        public string RuntimeIdProperty
        {
            get 
            {
                string ret = "";
                Log("RuntimeIdProperty");
                try
                {
                    ret = owner.GetAE.GetCurrentPropertyValue(AutomationElement.RuntimeIdProperty).ToString();
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

