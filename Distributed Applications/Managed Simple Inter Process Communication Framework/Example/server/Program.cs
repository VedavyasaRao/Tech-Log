using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Example;
using SimpleIPC;

namespace ConsoleApplication2
{
    public class testobj : Example.ICallInterface
    {
        string current_name="";
        public string current
        {
            get
            {
                return current_name;
            }
        }

        bool winmode;

        public testobj(bool winmode)
        {
            this.winmode = winmode;
        }
        
        public string register(string name, string cbservername, out int ticket)
        {
            Console.WriteLine("Received register call from client with name {0}",name);
            Console.WriteLine("result: success. name {0} ticket {1}",name,100);
            current_name = name;
            ticket = 100;
            System.Threading.Thread t = new System.Threading.Thread(publish);
            t.Start(cbservername);
            return "success";
        }

        
        public void publish(object o)
        {
            string cbservername  =  o as string;
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("publishing to client for ticket#  {0}, name:{1}", 100, current_name);
            GenericProxy<Example.ICallbackInterface> p = null;
            if (winmode)
                p = new SimpleIPC.GenericProxy<Example.ICallbackInterface>(new SimpleIPC.Windows.SIPCProxy(cbservername));
            else
                p = new SimpleIPC.GenericProxy<Example.ICallbackInterface>(new SimpleIPC.NamedObject.SIPCProxy(cbservername));

            p.Proxy.update(100, new Example.regdata { name = current_name, regid = Guid.NewGuid().ToString() });
            p.Dispose();
        }
       
    }


    class Program
    {
        static private void Testwindowsserver()
        {
            //instantiate server object
            var t = new testobj(true);

            //create a windows container and start it
            var t1 = new SimpleIPC.Windows.ServerContainer();
            t1.Start();

            //create a unique windows server and inject server object created above
            t1.CreateServer(new SimpleIPC.Windows.SIPCServer("winserver", t));
            Console.WriteLine("winserver is running");
            Console.ReadKey();
            t1.Stop();
        }

        static private void TestNamedobjectserver()
        {
            //instantiate server object
            var t = new testobj(false);

            //create a named container and add an unique named server
            var t1 = new SimpleIPC.NamedObject.SIPCServer("namedserver", t);

            //start
            t1.Start();
            Console.WriteLine("namedserver is running");
            Console.ReadKey();
            //stop container
            t1.Stop();
        }
       
        static void Main(string[] args)
        {
            if (args.Length != 1)
                return;
            if (args[0] == "w")
                Testwindowsserver();
            else
                TestNamedobjectserver();
        }
    }
}
