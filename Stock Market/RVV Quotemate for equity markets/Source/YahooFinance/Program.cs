using CefLoadPage;
using CefSharp;
using CefSharp.OffScreen;
using SimpleIPC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace YahooFinance
{
    public interface IQuotes
    {
        String QueryDateTime();
        String QueryIndicies();
        String QueryCloses(string ticker);
        String QueryQuote(string ticker);
    }

    public class Quotemate : CefLoadPager, IQuotes
    {
        private static string searchtext = "";
        private static AutoResetEvent oSignalEvent = new AutoResetEvent(false);
        string logfile = "";
        public Quotemate(string cachedir):base(cachedir)
        {
            //string assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //logfile = System.IO.Path.GetDirectoryName(assemblyLocation) + "\\output.txt";
            //if (File.Exists(logfile))
            //    File.Delete(logfile);

        }
        void logit(string s)
        {
            if (!string.IsNullOrEmpty(logfile))
            {
                File.AppendAllText(logfile, s + "\n");
            }
        }
        public string QueryCloses(string ticker)
        {
            int magicnum = 40;
            var knt = 0;
            var closes = new double[6];
            var closedays = new String[6];
            var lines = LoadPage("https://finance.yahoo.com/quote/" + ticker + "/history/");
            var j = magicnum;
            for (;j<lines.Count;++j)
            {
                if (lines[j].Contains("Volume"))
                    break;
            }
            ++j;
            for (var i=0; i < 10; ++i)
            {
                if (lines[i+j].Contains("Dividend"))
                    continue;
                var closetexts = lines[j + i].Split(new char[] { '\t' }).ToList();
                DateTime dt = DateTime.Now;
                dt -= new TimeSpan(9, 30, 0);
                var dts = dt.ToString("MMM dd, yyyy");
                closedays[knt] = closetexts[0];
                if (closedays[knt] == dts)
                    continue;
                closedays[knt] = DateTime.Parse(closetexts[0]).ToString("yyyy MM dd");

                closes[knt] = double.Parse(closetexts[5]);

                if (++knt == 6)
                    break;
            }
            
            var ret =  new JavaScriptSerializer().Serialize(new { PrevCloses = new { close0 = closes[0], day0 = closedays[0],
                close1 = closes[1],day1 = closedays[1],close2 = closes[2],day2 = closedays[2], close3 = closes[3],day3 = closedays[3],
                close4 = closes[4],day4 = closedays[4],close5 = closes[5], day5 = closedays[5] }});

            logit(ret);
            return ret;
        }

        string getindex(string ind, List<string> lines)
        {
            string ret = "";
            int i = 29;
            if (lines[i].EndsWith(ind))
                ret = lines[i + 3];
            else
            {
                for (i = 0; i < lines.Count(); ++i)
                {
                    if (lines[i].EndsWith(ind))
                    {
                        ret = lines[i + 3];
                        Console.WriteLine(i);
                        break;
                    }
                }
            }
            return ret;
        }
        public string QueryIndicies()
        {
            string dow = "";
            string nasdaq = "";

            var lines = LoadPage("https://finance.yahoo.com/quote/" + "^DJI" + "/");
            dow = getindex("(^DJI)",lines);

            lines = LoadPage("https://finance.yahoo.com/quote/" + "^IXIC" + "/");
            nasdaq = getindex("(^IXIC)", lines);

            var ret = new JavaScriptSerializer().Serialize(new { Indices = new { Dow = dow, Nasdaq = nasdaq } });

            logit(ret);
            return ret;
        }


        public string QueryQuote(string ticker)
        {
            string lasttrade="", lasttradetime="", change = "", open = "", volume = "", ask = "", bid = "", dayrange = "", week52range = "";

            var lines = LoadPage("https://finance.yahoo.com/quote/" + ticker + "/");
            int p = 40;
            for (; p < lines.Count; ++p)
            {
                if (lines[p].StartsWith("As of ") || lines[p].StartsWith("At close: "))
                    break;
            }
            p -= 3;
            lasttrade = lines[p];
            change = lines[p+1];
            lasttradetime = lines[p+3];

            p = 40;
            for (; p < lines.Count; ++p)
            {
                if (lines[p] =="Previous Close")
                    break;
            }
            p += 3;

            open = lines[p];

            p += 2;
            bid = lines[p];

            p += 2;
            ask = lines[p];

            p += 2;
            dayrange = lines[p];

            p += 2;
            week52range = lines[p];

            p += 2;
            volume = lines[p];


            var ret = new JavaScriptSerializer().Serialize(new { quote = new { lasttrade = lasttrade, lasttradetime = lasttradetime, change = change, open= open, volume = volume , ask = ask, bid= bid, dayrange= dayrange, week52range= week52range } });

            logit(ret);
            return ret;
        }

        public string QueryDateTime()
        {
            string datetime = "";
            var lines = LoadPage("https://time.is/New_York");
            var tm = lines[2];
            var dt = lines[3];
            dt = dt.Substring(0, dt.LastIndexOf(','));

            datetime = DateTime.Parse(dt + " "+ tm).ToString("yyyy-MM-dd HH:mm:ss");
            var ret = new JavaScriptSerializer().Serialize(new  { DateTime= datetime } );
            logit(ret);
            return ret;
        }

    }

    class Program
    {
        static Quotemate t = null;
        static SimpleIPC.Windows.ServerContainer t1 = null;
        static SimpleIPC.NamedObject.SIPCServer t2 = null;

        static private void Testwindowsserver()
        {
            string assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string appPath = System.IO.Path.GetDirectoryName(assemblyLocation) + "\\cache";

            //instantiate server object
            t = new Quotemate(appPath);

            //create a windows container and start it
            t1 = new SimpleIPC.Windows.ServerContainer();
            t1.Start();

            //create a unique windows server and inject server object created above
            t1.CreateServer(new SimpleIPC.Windows.SIPCServer("YahooFinanceServer", t, SIPCEncoding.json ));
            Console.WriteLine("YahooFinanceServer is running");
            System.Threading.Thread.Sleep(new TimeSpan(1, 0, 0, 0));

            //stop container
            t1.Stop();
        }

        static private void TestNamedobjectserver()
        {
            string assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string appPath = System.IO.Path.GetDirectoryName(assemblyLocation) + "\\cache";

            //instantiate server object
            t = new Quotemate(appPath);

            //create a named container and add an unique named server
            t2 = new SimpleIPC.NamedObject.SIPCServer("YahooFinanceServer", t, SIPCEncoding.json);

            //start
            t2.Start();
            Console.WriteLine("YahooFinanceServer is running");
            Console.ReadKey();
            //stop container
            t2.Stop();

        }

        static void Main(string[] args)
        {
            //string assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //string appPath = System.IO.Path.GetDirectoryName(assemblyLocation) + "\\cache";
            //string[] s = new string[5];
            //t = new Quotemate(appPath);
            //s[0] = t.QueryDateTime();
            //s[1] = t.QueryIndicies();
            //s[2] = t.QueryCloses("msft");
            //s[3] = t.QueryQuote("tsla");

            //foreach(var ss in s)
            //    Console.WriteLine(ss);

            Testwindowsserver();
        }
    }
}
