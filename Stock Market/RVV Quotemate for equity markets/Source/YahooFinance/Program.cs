using System;
using HtmlAgilityPack;
using System.Web.Script.Serialization;
using SimpleIPC;
using System.Diagnostics;

namespace YahooFinance
{
    public interface IQuotes
    {
        String QueryDateTime();
        String QueryIndicies();
        String QueryCloses(string ticker);
        String QueryQuote(string ticker);
    }

    public class Quotemate : IQuotes
    {
        public string QueryCloses(string ticker)
        {
            var knt = 0;
            var closes = new double[6];
            var closedays = new String[6];
            HtmlWeb web = new HtmlWeb();
            var doc = web.Load("https://finance.yahoo.com/quote/"+ticker+"/history");
            for (int i = 1; i < 10; ++i)
            {
                var nodes = doc.DocumentNode.SelectNodes("/html/body/div[2]/main/section/section/section/article/div[1]/div[3]/table/tbody/tr[" + i.ToString() + "]/td[5]");
                if (nodes != null)
                {
                    closes[knt] = double.Parse(nodes[0].InnerText);
                }
                else
                    continue;

                nodes = doc.DocumentNode.SelectNodes("/html/body/div[2]/main/section/section/section/article/div[1]/div[3]/table/tbody/tr[" + i.ToString() + "]/td[1]");
                if (nodes != null)
                {
                    DateTime dt = DateTime.Now;
                    dt -= new TimeSpan(9, 30, 0);
                    var dts = dt.ToString("MMM dd, yyyy");
                    closedays[knt] = nodes[0].InnerText;
                    if (closedays[knt] == dts)
                        continue;
                    closedays[knt] = DateTime.Parse(nodes[0].InnerText).ToString("yyyy MM dd");
                }
                if (++knt == 6)
                    break;
            }
            return new JavaScriptSerializer().Serialize(new { PrevCloses = new { close0 = closes[0], day0 = closedays[0],
                close1 = closes[1],day1 = closedays[1],close2 = closes[2],day2 = closedays[2], close3 = closes[3],day3 = closedays[3],
                close4 = closes[4],day4 = closedays[4],close5 = closes[5], day5 = closedays[5] }});
        }

        public string QueryIndiciesOld()
        {
            string dow = "";
            string nasdaq = "";

            HtmlWeb web = new HtmlWeb();
            var doc = web.Load("https://finance.yahoo.com/");
            var nodes = doc.DocumentNode.SelectNodes("/html/body/div[1]/div/div/div[1]/div/div[2]/div/div/div[4]/div/div/div/div[2]/div/div[1]/div[1]/ul/li[2]/h3/div/span[1]");
            if (nodes != null)
            {
                dow = nodes[0].InnerText;
            }

            nodes = doc.DocumentNode.SelectNodes("/html/body/div[1]/div/div/div[1]/div/div[2]/div/div/div[4]/div/div/div/div[2]/div/div[1]/div[1]/ul/li[3]/h3/div/span[1]");
            if (nodes != null)
            {
                nasdaq =  nodes[0].InnerText;
            }
            return new JavaScriptSerializer().Serialize(new { Indices = new { Dow = dow, Nasdaq = nasdaq } });
        }
        public string QueryIndicies()
        {
            string dow = "";
            string nasdaq = "";

            HtmlWeb web = new HtmlWeb();
            var doc = web.Load("https://finance.yahoo.com/quote/" + "^DJI" + "/");
            var nodes = doc.DocumentNode.SelectNodes("/html/body/div[2]/main/section/section/section/article/section[1]/div[2]/div[1]/section/div/section/div[1]/div[2]/span");
            if (nodes != null)
            {
                dow = nodes[0].InnerText.Split(new char['.'])[0];
            }

            doc = web.Load("https://finance.yahoo.com/quote/" + "^IXIC" + "/");
            nodes = doc.DocumentNode.SelectNodes("/html/body/div[2]/main/section/section/section/article/section[1]/div[2]/div[1]/section/div/section/div[1]/div[2]/span");
            if (nodes != null)
            {
                nasdaq = nodes[0].InnerText.Split(new char['.'])[0];
            }

           // System.IO.File.AppendAllText(@"d:\\yahoo.log", string.Format("{0} {1}\r\n", dow, nasdaq));
            return new JavaScriptSerializer().Serialize(new { Indices = new { Dow = dow, Nasdaq = nasdaq } });
        }

        public string QueryQuote(string ticker)
        {
            HtmlWeb web = new HtmlWeb();
            string lasttrade="", lasttradetime="", change = "", open = "", volume = "", ask = "", bid = "", dayrange = "", week52range = "";

            var doc = web.Load("https://finance.yahoo.com/quote/" + ticker + "/");
            //var nodes = doc.DocumentNode.SelectNodes("/html/body/div[1]/div/div/div[1]/div/div[2]/div/div/div[6]/div/div/div/div[3]/div[1]/div/fin-streamer[1]");
            var nodes = doc.DocumentNode.SelectNodes("/html/body/div[2]/main/section/section/section/article/section[1]/div[2]/div[1]/section/div/section/div[1]/div[1]");
            if (nodes != null)
            {
                lasttrade = nodes[0].InnerText;
            }
            nodes = doc.DocumentNode.SelectNodes("/html/body/div[2]/main/section/section/section/article/section[1]/div[2]/div[1]/section/div/section/div[2]/span/span/text()");
            if (nodes != null)
            {
                lasttradetime = nodes[0].InnerText;
            }

            nodes = doc.DocumentNode.SelectNodes("/html/body/div[2]/main/section/section/section/article/section[1]/div[2]/div[1]/section/div/section/div[1]/div[2]/span");
            if (nodes != null)
            {
                change = nodes[0].InnerText;
            }

            //nodes = doc.DocumentNode.SelectNodes("/html/body/div[2]/main/section/section/section/article/div[3]/ul/li[2]/span[2]/fin-streamer");
            nodes = doc.DocumentNode.SelectNodes("/html/body/div[2]/main/section/section/section/article/div[2]/ul/li[2]/span[2]/fin-streamer");
            if (nodes != null)
            {
                open = nodes[0].InnerText;
            }

            nodes = doc.DocumentNode.SelectNodes("/html/body/div[2]/main/section/section/section/article/div[2]/ul/li[7]/span[2]/fin-streamer");
            if (nodes != null)
            {
                volume = nodes[0].InnerText;
            }

            nodes = doc.DocumentNode.SelectNodes("/html/body/div[2]/main/section/section/section/article/div[2]/ul/li[4]/span[2]");
            if (nodes != null)
            {
                ask = nodes[0].InnerText;
            }

            nodes = doc.DocumentNode.SelectNodes("/html/body/div[2]/main/section/section/section/article/div[2]/ul/li[3]/span[2]");
            if (nodes != null)
            {
                bid = nodes[0].InnerText;
            }


            nodes = doc.DocumentNode.SelectNodes("/html/body/div[2]/main/section/section/section/article/div[2]/ul/li[5]/span[2]/fin-streamer");
            if (nodes != null)
            {
                dayrange = nodes[0].InnerText;
            }

            nodes = doc.DocumentNode.SelectNodes("/html/body/div[2]/main/section/section/section/article/div[2]/ul/li[6]/span[2]/fin-streamer");
            if (nodes != null)
            {
                week52range = nodes[0].InnerText;
            }

            return new JavaScriptSerializer().Serialize(new { quote = new { lasttrade = lasttrade, lasttradetime = lasttradetime, change = change, open= open, volume = volume , ask = ask, bid= bid, dayrange= dayrange, week52range= week52range } });
        }

        public string QueryDateTime()
        {
            string datetime = "";

            HtmlWeb web = new HtmlWeb();
            var doc = web.Load("https://time.is/New_York");
            var nodes = doc.DocumentNode.SelectNodes("/html/body/div[1]/div[2]/div[3]");
            string dt="", tm="";
            if (nodes != null)
            {
                var t = nodes[0].InnerText;
                dt = t.Substring(0, t.LastIndexOf(','));
            }

            nodes = doc.DocumentNode.SelectNodes("/html/body/div[1]/div[2]/div[2]/div/time");
            if (nodes != null)
            {
                tm += nodes[0].InnerText;
            }

            datetime = DateTime.Parse(dt + " "+ tm).ToString("yyyy-MM-dd HH:mm:ss");
            return new JavaScriptSerializer().Serialize(new  { DateTime= datetime } );
        }

    }

    class Program
    {
        static Quotemate t = null;
        static SimpleIPC.Windows.ServerContainer t1 = null;
        static SimpleIPC.NamedObject.SIPCServer t2 = null;

        static private void Testwindowsserver()
        {
            //instantiate server object
            t = new Quotemate();

            //create a windows container and start it
            t1 = new SimpleIPC.Windows.ServerContainer();
            t1.Start();

            //create a unique windows server and inject server object created above
            t1.CreateServer(new SimpleIPC.Windows.SIPCServer("YahooFinanceServer", t));
            Console.WriteLine("YahooFinanceServer is running");
            System.Threading.Thread.Sleep(new TimeSpan(1, 0, 0, 0));

            //stop container
            t1.Stop();
        }

        static private void TestNamedobjectserver()
        {
            //instantiate server object
            t = new Quotemate();

            //create a named container and add an unique named server
            t2 = new SimpleIPC.NamedObject.SIPCServer("YahooFinanceServer", t);

            //start
            t2.Start();
            Console.WriteLine("YahooFinanceServer is running");
            Console.ReadKey();
            //stop container
            t2.Stop();

        }

        static void Main(string[] args)
        {
            //var t = new Quotemate();
            //var s = t.QueryQuote("nvda");
            //Console.WriteLine(s);

            Testwindowsserver();
        }
    }
}
