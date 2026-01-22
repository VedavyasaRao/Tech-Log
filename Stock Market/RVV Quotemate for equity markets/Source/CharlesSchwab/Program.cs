using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleIPC;

namespace CharlesSchwab
{

    class Program
    {
        static cschwabsettings mysettings = new cschwabsettings();
        static Quotemate t = null;
        static SimpleIPC.Windows.ServerContainer t1 = null;
        static SimpleIPC.NamedObject.SIPCServer t2 = null;

        static private void Testwindowsserver()
        {
            //instantiate server object
            t = new Quotemate(mysettings);

            //create a windows container and start it
            t1 = new SimpleIPC.Windows.ServerContainer();
            t1.Start();

            //create a unique windows server and inject server object created above
            t1.CreateServer(new SimpleIPC.Windows.SIPCServer("CharlesSchwabServer", t, SIPCEncoding.json));
            Console.WriteLine("CharlesSchwabServer is running");
            System.Threading.Thread.Sleep(new TimeSpan(1, 0, 0, 0));

            //stop container
            t1.Stop();
        }

        static private void TestNamedobjectserver()
        {
            //instantiate server object
            t = new Quotemate(mysettings);

            //create a named container and add an unique named server
            t2 = new SimpleIPC.NamedObject.SIPCServer("CharlesSchwabServer", t, SIPCEncoding.json);

            //start
            t2.Start();
            Console.WriteLine("CharlesSchwabServer is running");
            Console.ReadKey();
            //stop container
            t2.Stop();

        }

        static void Main(string[] args)
        {
             if (!mysettings.deserialize())
                return;
            
            if (args[0] == "t")
            {
                Quotemate qm = new Quotemate(mysettings);
                string s;

                s = qm.QueryDateTime();
                Console.WriteLine(s);
                Console.WriteLine();

                s = qm.QueryIndicies();
                Console.WriteLine(s);
                Console.WriteLine();

                s = qm.QueryQuote("msft");
                Console.WriteLine(s);
                Console.WriteLine();

                s = qm.QueryCloses("nvda");
                Console.WriteLine(s);
                Console.WriteLine();

                s = qm.QueryBalances();
                Console.WriteLine(s);
                Console.WriteLine();

            }

            if (args[0] == "r")
            {
                Testwindowsserver();
            }
        }
    }

}
