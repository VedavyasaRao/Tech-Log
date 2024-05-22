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
    public interface IUIASelectionItemProvider
    {
        void AddToSelection();
        bool IsSelected
        {
            get;
        }
        void RemoveFromSelection();
        void MakeSelection();
        IUIAElement SelectionContainer
        {
            get;
        }
    }


    class UIASelectionItem : UIADriverPatternBase, IUIASelectionItemProvider
    {

        internal static UIASelectionItem GetInstance(UIAElement owner)
        {
            return GetPatternObject<UIASelectionItem, SelectionItemPattern>(owner, SelectionItemPattern.Pattern);
        }

        public void AddToSelection()
        {
            Log("AddToSelection()");
            try
            {
                ((SelectionItemPattern)pattern).AddToSelection();
                CauseDelay();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

        public bool IsSelected
        {
            get
            {
                bool ret = false;
                Log("IsSelected");
                try
                {
                    ret = ((SelectionItemPattern)pattern).Current.IsSelected;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public void RemoveFromSelection()
        {
            Log("RemoveFromSelection()");
            try
            {
                ((SelectionItemPattern)pattern).RemoveFromSelection();
                CauseDelay();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

        public void MakeSelection()
        {
            Log("MakeSelection()");
            try
            {
                ((SelectionItemPattern)pattern).Select();
                CauseDelay();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

        public IUIAElement SelectionContainer
        {
            get
            {
                UIAElement ret = null;
                Log("SelectionContainer");
                try
                {
                    AutomationElement ae = ((SelectionItemPattern)pattern).Current.SelectionContainer;
                    ret = UIAElement.GetInstance(ae, owner);
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
}

