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
    public interface IUIAGridItemProvider 
    {

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


    class UIAGridItem : UIADriverPatternBase, IUIAGridItemProvider
    {
        internal static UIAGridItem GetInstance(UIAElement owner)
        {
            return GetPatternObject<UIAGridItem, GridItemPattern>(owner, GridItemPattern.Pattern);
        }
        
        public int Column
        {
            get
            {
                int ret = -1;
                Log("Column");
                try
                {
                    ret = ((GridItemPattern)pattern).Current.Column;
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
                    ret = ((GridItemPattern)pattern).Current.ColumnSpan;
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
                Log("GetContainingGrid");
                try
                {
                    AutomationElement ae = ((GridItemPattern)pattern).Current.ContainingGrid;
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
                    ret = ((GridItemPattern)pattern).Current.Row;
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
                    ret = ((GridItemPattern)pattern).Current.RowSpan;
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

