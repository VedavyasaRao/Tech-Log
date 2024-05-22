using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace UITesting.Automated.ControlDBTool
{
    class PatternHelper
    {
        Dictionary<string, string> patuiamap = null;


        public PatternHelper()
        {
            patuiamap = new Dictionary<string, string>();
            patuiamap.Add("MSAAAccessible", "MSAAAccessible");
            patuiamap.Add("ExpandCollapse", "UIAExpandCollapse");
            patuiamap.Add("Generic", "UIAGeneric");
            patuiamap.Add("Grid", "UIAGrid");
            patuiamap.Add("GridItem", "UIAGridItem");
            patuiamap.Add("Invoke", "UIAInvoke");
            patuiamap.Add("RangeValue", "UIARangeValue");
            patuiamap.Add("Selection", "UIASelection");
            patuiamap.Add("SelectionItem", "UIASelectionItem");
            patuiamap.Add("Table", "UIATable");
            patuiamap.Add("TableItem", "UIATableItem");
            patuiamap.Add("Toggle", "UIAToggle");
            patuiamap.Add("Transform", "UIATransform");
            patuiamap.Add("Value", "UIAValue");
            patuiamap.Add("Window", "UIAWindow");
            patuiamap.Add("Constants", "UIAConstants");
            patuiamap.Add("Navigation", "UIANavigation");
            patuiamap.Add("Scroll", "UIAScroll");
            patuiamap.Add("ScrollItem", "UIAScrollItem");

        }
        
        public string[] SupportedPatterns
        {
            get
            {
                return patuiamap.Keys.ToArray();
            }

        }

        public string GetPattrenclass(string patname)
        {
            string ret;

            if (!patuiamap.TryGetValue(patname, out ret))
            {
                ret = "";
            }
            
            return ret;
        }

        public string GetPattrenprop(string patname)
        {
            string ret = patname;


            return ret;
        }

    }
}
