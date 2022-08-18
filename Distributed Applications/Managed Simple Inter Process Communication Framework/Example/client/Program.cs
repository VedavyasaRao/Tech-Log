using System;
using System.Collections.Generic;
using System.Text;

using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CSharp;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using SimpleIPC;

namespace test
{
    class testclient:Example.ICallbackInterface
    {
        public void update(int ticket, Example.regdata data)
        {
            Console.WriteLine("received callback from server: ticket {0}, {1}, Reg Id:{2}", ticket, data.name, data.regid);
        }
    }


    class Program
    {
        static void windowsclienttest()
        {
            testclient tc = new testclient();
            var t1 = new SimpleIPC.Windows.ServerContainer();
            t1.Start();
            t1.CreateServer(new SimpleIPC.Windows.SIPCServer("winclient", tc));
            Console.WriteLine("winclient running");
            var p = new SimpleIPC.GenericProxy<Example.ICallInterface>(new SimpleIPC.Windows.SIPCProxy("winserver"));

            int ticket = 0;
            Console.WriteLine("calling server : register(veda)");
            string s = p.Proxy.register("veda", "winclient", out ticket);
            Console.WriteLine("result of register call:{0}, Ticket:{1}", s, ticket);
            Console.ReadKey();
            p.Dispose();

            t1.Stop();
        }

        static void namedobjectclienttest()
        {
            testclient tc = new testclient();
            SimpleIPC.NamedObject.SIPCServer t1 = new SimpleIPC.NamedObject.SIPCServer("namedclient", tc);
            t1.Start();
            Console.WriteLine("namedclient running");
            var p = new SimpleIPC.GenericProxy<Example.ICallInterface>( new SimpleIPC.NamedObject.SIPCProxy("namedserver"));
            int ticket = 0;
            Console.WriteLine("calling server : register(veda)");
            string s = p.Proxy.register("veda", "namedclient", out ticket);
            Console.WriteLine("result of register call:{0}, Ticket:{1}", s, ticket);
            Console.ReadKey();
            p.Dispose();
            t1.Stop();
        }

        static void Main(string[] args)
        {
            if (args.Length != 1)
                return;
            System.Threading.Thread.Sleep(500);
            if (args[0] == "w")
                windowsclienttest();
            else
                namedobjectclienttest();
        }
    }

}
