using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleIPC;

namespace Reader
{
    class IOWriter : CacheStream.IIOHelper
    {

        public void WroteBytes(int pid, IntPtr handle, int offset, int len)
        {
            Console.WriteLine("******System Cache Read Write Demo*******");
            Console.WriteLine("Received IPC call from the writer");
            Console.WriteLine("opened d:\\test1.dat on system cache");
            CacheStream.IOHelper iop = new CacheStream.IOHelper(pid, handle);

            byte[] buffer = new Byte[len];
            iop.Read(buffer, offset, len);
            Console.WriteLine("read {0} bytes with {1}", len, buffer[offset]);

            Console.WriteLine("write {0} bytes with {1}", len, 2);
            for (int i = 0; i < buffer.Length; ++i)
            {
                buffer[i] = 2;
            }
            bool bret = iop.Write(buffer, offset, buffer.Length);
            Console.WriteLine("press any key to stop");
        }


        public void WroteMemBytes(int pid, IntPtr address, int len)
        {
            Console.WriteLine("\n\n******ReadProcessMemory WriteProcessMeory Demo*******");
            Console.WriteLine("Received IPC call from the writer");
            CacheStream.IOHelper iop = new CacheStream.IOHelper();
            byte[] buffer = new byte[len];

            iop.ReadMemory(pid, address, buffer, len);
            Console.WriteLine("remote read {0} bytes with {1}", len, buffer[0]);

            Console.WriteLine("remote write {0} bytes with {1}", len, 4);
            for (int i = 0; i < buffer.Length; ++i)
            {
                buffer[i] = 4;
            }
            iop.WriteMemory(pid, address, buffer, len);
            Console.WriteLine("press any key to stop");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Reader starts");

            var t1 = new SimpleIPC.Windows.ServerContainer();
            t1.Start();
            t1.CreateServer(new SimpleIPC.Windows.SIPCServer("cachestreamreader", new IOWriter()));
            Console.ReadKey();
            t1.Stop();
            Console.WriteLine("Reader stops");
        }
    }
}
