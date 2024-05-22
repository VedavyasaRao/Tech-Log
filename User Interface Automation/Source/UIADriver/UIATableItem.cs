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
    public interface IUIATableItemProvider
    {
        IUIAElement GetColumnHeaderItems(int index);
        IUIAElement GetRowHeaderItems(int index);
        int Column
        {
            get;
        }
        int ColumnSpan
        {
            get;
        }
        IUIAElement ContainingGrid
        {
            get;
        }
        int Row
        {
            get;
        }
        int RowSpan
        {
            get;
        }
    }

    class UIATableItem : UIADriverPatternBase, IUIATableItemProvider
    {
        internal static UIATableItem GetInstance(UIAElement owner)
        {
            return GetPatternObject<UIATableItem, TableItemPattern>(owner, TableItemPattern.Pattern);
        }

        public IUIAElement GetColumnHeaderItems(int index)
        {
            UIAElement ret=null;
            Log(string.Format("GetColumnHeaderItems({0})", index));
            try
            {
                AutomationElement[] aes = ((TableItemPattern)pattern).Current.GetColumnHeaderItems();
                if (aes != null && aes.Length > index)
                {
                    ret = UIAElement.GetInstance(aes[index], owner);
                    CauseDelay();
                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
            return ret;
        }

        public IUIAElement GetRowHeaderItems(int index)
        {
            UIAElement ret = null;
            Log(string.Format("GetRowHeaderItems({0})", index));
            try
            {
                AutomationElement[] aes = ((TableItemPattern)pattern).Current.GetRowHeaderItems();
                if (aes != null && aes.Length > index)
                {
                    ret = UIAElement.GetInstance(aes[index], owner);
                    CauseDelay();
                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
            return ret;
        }

        public int Column
        {
            get
            {
                int ret = -1;
                Log("Column");
                try
                {
                    ret = ((TableItemPattern)pattern).Current.Column;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public int ColumnSpan
        {
            get
            {
                int ret = -1;
                Log("ColumnSpan");
                try
                {
                    ret = ((TableItemPattern)pattern).Current.ColumnSpan;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public IUIAElement ContainingGrid
        {
            get
            {
                UIAElement ret = null;
                Log("ContainingGrid");
                try
                {
                    AutomationElement ae = ((TableItemPattern)pattern).Current.ContainingGrid;
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

        public int Row
        {
            get
            {
                int ret = -1;
                Log("Row");
                try
                {
                    ret = ((TableItemPattern)pattern).Current.Row;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public int RowSpan
        {
            get
            {
                int ret = -1;
                Log("RowSpan");
                try
                {
                    ret = ((TableItemPattern)pattern).Current.RowSpan;
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

