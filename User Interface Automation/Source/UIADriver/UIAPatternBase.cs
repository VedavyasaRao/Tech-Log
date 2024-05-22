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
    class UIADriverPatternBase
    {
        protected UIAElement owner;
        protected BasePattern pattern;
        protected string typename;

        protected string Getctlname()
        {
            return owner.Getctlname;
        }

        protected void CauseDelay( )
        {
            owner.CauseDelay();

        }

        protected void Log(string msg)
        {
            Logger.LogMessage(string.Format("{0},{1},{2}", typename, Getctlname(),msg));
        }

        protected void LogError(string msg)
        {
            Logger.LogMessage(string.Format("Exception occured:{0},{1},{2}", typename, Getctlname(), msg));
        }

        protected static T GetPatternObject<T>(UIAElement owner)
            where T : UIADriverPatternBase, new()
        {
            T ret = null;
            ret = new T();
            ret.owner = owner;
            ret.typename = ret.GetType().Name;
            return ret;
        }


        protected static T GetPatternObject<T, U>(UIAElement owner, AutomationPattern match)
            where T : UIADriverPatternBase, new()
            where U : BasePattern
        {
            T ret = GetPatternObject<T>(owner);

            if (ret != null)
            {
                ret.typename = ret.GetType().Name;
                ret.pattern = ret.owner.GetPattern<U>(match);
                if (ret.pattern == null)
                {
                    ret = null;
                }
            }

            return ret;
        }

    }


    
}

