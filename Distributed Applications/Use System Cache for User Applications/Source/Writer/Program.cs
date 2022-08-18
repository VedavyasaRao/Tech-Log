using SimpleIPC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Writer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
                return;

            Console.WriteLine("writer starts");
            byte[] one = new byte[100000];
            GenericProxy<CacheStream.IIOHelper> p = null;
            p = new SimpleIPC.GenericProxy<CacheStream.IIOHelper>(new SimpleIPC.Windows.SIPCProxy("cachestreamreader"));
            if (args[0] == "1")
            {
                Console.WriteLine("******System Cache Read Write Demo*******");
                CacheStream.IOHelper ioh = new CacheStream.IOHelper(@"test1.dat");
                Console.WriteLine("created d:\\test1.dat on system cache");
                for (int i = 0; i < one.Length; ++i)
                {
                    one[i] = 1;
                }

                bool bret = ioh.Write(one, 0, one.Length);
                Console.WriteLine("Wrote 100000 bytes with 1");
                Console.WriteLine("Sending IPC call to the Reader");
                p.Proxy.WroteBytes(Process.GetCurrentProcess().Id, ioh.hImageFile, 0, one.Length);
                Console.WriteLine("Sleeping for 5 seconds");
                System.Threading.Thread.Sleep(5000);
                ioh.Read(one, 0, one.Length);
                Console.WriteLine("read {0} bytes with {1}", one.Length, one[0]);

            }
            else
            {
                Console.WriteLine("\n\n******ReadProcessMemory WriteProcessMeory Demo*******");
                for (int i = 0; i < one.Length; ++i)
                {
                    one[i] = 3;
                }
                GCHandle handle = GCHandle.Alloc(one, GCHandleType.Pinned);
                Console.WriteLine("Wrote 100000 bytes with 3");
                Console.WriteLine("Sending IPC call to the Reader");
                p.Proxy.WroteMemBytes(Process.GetCurrentProcess().Id, handle.AddrOfPinnedObject(), one.Length);
                Console.WriteLine("Sleeping for 5 seconds");
                System.Threading.Thread.Sleep(5000);
                Console.WriteLine("Remote Read 100000 bytes with {0}", one[0]);
                handle.Free();
                p.Dispose();
            }
            Console.WriteLine("press any key to stop");
            Console.ReadKey();
            Console.WriteLine("writer stops");
        }
    }
}
