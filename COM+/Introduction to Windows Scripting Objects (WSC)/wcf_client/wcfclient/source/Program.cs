using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wcfclient
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = new ServiceReference1.SillyCalc_WSC_1Client();
            Console.WriteLine("calling x.add(10, 2) Result:{0}", x.add(10, 2));
            Console.ReadKey();
        }
    }
}
