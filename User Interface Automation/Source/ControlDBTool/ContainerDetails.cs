using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UITesting.Automated.ControlDBTool
{
    public struct ContainerDetails
    {
        public string id;
        public int ver;
        public string desc;

        public ContainerDetails(string id, string desc)
        {
            this.id=id;
            ver=1;
            this.desc = desc;
        }

    }
}
