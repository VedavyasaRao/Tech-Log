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
    public interface IUIATableProvider
    {
        IUIAElement GetColumnHeaders(int index);
        IUIAElement GetRowHeaders(int index);
        int RowColumnMajor
        {
            get;
        }
        int ColumnCount
        {
            get;
        }
        IUIAElement GetItem(int row, int column);
        int RowCount
        {
            get;
        }
    }

  
    class UIATable : UIADriverPatternBase, IUIATableProvider
    {

        internal static UIATable GetInstance(UIAElement owner)
        {
            return GetPatternObject<UIATable, TablePattern>(owner, TablePattern.Pattern);
        }


        public IUIAElement GetColumnHeaders(int index)
        {
            UIAElement ret = null;
            Log(string.Format("GetColumnHeaders({0})", index));
            try
            {
                AutomationElement[] aes = ((TablePattern)pattern).Current.GetColumnHeaders();
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

        public IUIAElement GetRowHeaders(int index)
        {
            UIAElement ret = null;
            Log(string.Format("GetRowHeaders({0})", index));
            try
            {
                AutomationElement[] aes = ((TablePattern)pattern).Current.GetRowHeaders();
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

        public int RowColumnMajor
        {
            get
            {
                RowOrColumnMajor ret = RowOrColumnMajor.ColumnMajor;
                Log("RowOrColumnMajor");
                try
                {
                    ret = ((TablePattern)pattern).Current.RowOrColumnMajor;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return (int)ret;
            }
        }

        public int ColumnCount
        {
            get
            {
                int ret = -1;
                Log("ColumnCount");
                try
                {
                    ret = ((TablePattern)pattern).Current.ColumnCount;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                return ret;
            }
        }

        public IUIAElement GetItem(int row, int column)
        {
            UIAElement ret = null;
            Log(string.Format("GetItem({0},{1})", row, column));
            try
            {
                AutomationElement ae = ((TablePattern)pattern).GetItem(row, column);
                ret = UIAElement.GetInstance(ae, owner);
                CauseDelay();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
            return ret;
        }

        public int RowCount
        {
            get
            {
                int ret = -1;
                Log("RowCount");
                try
                {
                    ret = ((TablePattern)pattern).Current.RowCount;
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

