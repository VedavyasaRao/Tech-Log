using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace managed_client
{
    class Program
    {
        static void Main(string[] args)
        {
            dynamic remoteobj = Activator.CreateInstance(Type.GetTypeFromProgID("SillyCalc.WSC.1"));
            Console.WriteLine("40+6={0}", remoteobj.add(40, 6));
        }
    }
}
