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
    public interface IUIAExpandCollapseProvider
    {
        void Collapse();
        void Expand();
        int ExpandCollapseState
        {
            get;
        }
    }


    class UIAExpandCollapse : UIADriverPatternBase, IUIAExpandCollapseProvider
    {
        internal static UIAExpandCollapse GetInstance(UIAElement owner)
        {
            return GetPatternObject<UIAExpandCollapse, ExpandCollapsePattern>(owner, ExpandCollapsePattern.Pattern);
        }

        public void Collapse()
        {
            Log("Collapse()");
            try
            {
                ((ExpandCollapsePattern)pattern).Collapse();
                CauseDelay();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);

            }
        }

        public void Expand()
        {
            Log("Expand()");
            try
            {
                ((ExpandCollapsePattern)pattern).Expand();
                CauseDelay();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);

            }
        }

        public int ExpandCollapseState
        {
            get
            {
                ExpandCollapseState ret = System.Windows.Automation.ExpandCollapseState.LeafNode;  
                Log("ExpandCollapseState");
                try
                {
                    ret = ((ExpandCollapsePattern)pattern).Current.ExpandCollapseState;
                    CauseDelay();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);

                }
                return (int)ret;
            }

        }

    }
}

