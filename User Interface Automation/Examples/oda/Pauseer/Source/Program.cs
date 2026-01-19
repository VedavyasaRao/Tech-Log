using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pauseer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var evt = EventWaitHandle.OpenExisting("Global\\OdaCapture");
            if (evt.WaitOne(100))
                evt.Reset();
            else
                evt.Set();
            return;

        }
    }
}
