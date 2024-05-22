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
    public interface IUIAGridProvider 
    {
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

    class UIAGrid : UIADriverPatternBase, IUIAGridProvider
    {
        internal static UIAGrid GetInstance(UIAElement owner)
        {
            return GetPatternObject<UIAGrid, GridPattern>(owner, GridPattern.Pattern);
        }


        public int ColumnCount
        {
            get
            {
                int ret = -1;
                Log("ColumnCount");
                try
                {
                    ret = ((GridPattern)pattern).Current.ColumnCount;
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
            Log(string.Format("GetItem({0}.{1})", row, column));
            try
            {
                AutomationElement ae = ((GridPattern)pattern).GetItem(row, column);
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
                    ret = ((GridPattern)pattern).Current.RowCount;
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

