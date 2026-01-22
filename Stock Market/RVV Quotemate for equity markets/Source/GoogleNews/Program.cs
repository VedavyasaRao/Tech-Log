using CefLoadPage;
using CefSharp;
using CefSharp.OffScreen;
using SimpleIPC;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace GooleNews
{
    public interface INews
    {
        String QueryNews(string ticker);
    }

    public class Quotemate : CefLoadPager, INews, IDisposable
    {
        public Quotemate(string cachedir) : base(cachedir)
        {

        }

        public string QueryNews(string ticker)
        {
            DateTime dt = DateTime.Now;
            dt -= new TimeSpan(9, 30, 0);
            int hour = 0, min = 0, day = 0, mon = 0, year = 0;
            string TestUrl = String.Format("https://www.google.com/search?q={0}&tbm=nws&tbs=sbd:1", ticker);
            string lastnewstitle = "";
            string lastnewssource = "";
            string timestamp = "";
            string temps = "";
            var lines = LoadPage(TestUrl);
            var idx = lines.IndexOf("Search Results");
            if (idx != -1)
            {
                idx += 2;
                while (idx < lines.Count)
                {
                    lastnewssource = lines[idx];
                    if (lastnewssource.Contains("minute ago"))
                    {
                        min = 1;
                        dt = dt - new TimeSpan(0, min, 0);
                        break;
                    }
                    else if (lastnewssource.Contains("mins ago"))
                    {
                        min = 0;
                        if (Regex.Matches(lastnewssource, @"\d+").Count > 0)
                        {
                            temps = Regex.Matches(lastnewssource, @"\d+")[0].Value;
                            min = int.Parse(temps);
                        }
                        dt = dt - new TimeSpan(0, min, 0);
                        break;
                    }
                    if (lastnewssource.Contains("hour ago"))
                    {
                        hour = 1;
                        dt = dt - new TimeSpan(hour, 0, 0);
                        break;
                    }
                    else if (lastnewssource.Contains("hours ago"))
                    {
                        hour = 0;
                        if (Regex.Matches(lastnewssource, @"\d+").Count > 0)
                        {
                            temps = Regex.Matches(lastnewssource, @"\d+")[0].Value;
                            hour = int.Parse(temps);
                        }
                        dt = dt - new TimeSpan(hour, 0, 0);
                        break;
                    }
                    else if (Regex.Matches(lastnewssource, @"\d\d-...-\d\d\d\d").Count == 1)
                    {
                        temps = Regex.Matches(lastnewssource, @"\d\d-...-\d\d\d\d")[0].Value;
                        dt = DateTime.Parse(temps);
                        break;
                    }
                    idx++;
                }
                if (idx == lines.Count)
                {
                    lastnewssource = "";
                    timestamp = "";
                    lastnewstitle = "";
                }
                else
                {
                    timestamp = dt.ToString("yyyy MM dd hh:mm");
                    lastnewstitle = lines[idx - 3];
                }

            }

            return new JavaScriptSerializer().Serialize(new { news = new { title = lastnewstitle, source = lastnewssource, timestamp = timestamp } });
        }
    }

    public class Program
    {
        static private void Testwindowsserver()
        {
            string assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string appPath = System.IO.Path.GetDirectoryName(assemblyLocation) + "\\cache";

            //instantiate server object
            var t = new Quotemate(appPath);

            //create a windows container and start it
            var t1 = new SimpleIPC.Windows.ServerContainer();
            t1.Start();

            //create a unique windows server and inject server object created above
            t1.CreateServer(new SimpleIPC.Windows.SIPCServer("GoogleNewsServer", t, SIPCEncoding.json));
            Console.WriteLine("GoogleNewsServer is running");
            System.Threading.Thread.Sleep(new TimeSpan(1, 0, 0, 0));
            //stop container
            t1.Stop();
        }

        static private void TestNamedobjectserver()
        {
            string assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string appPath = System.IO.Path.GetDirectoryName(assemblyLocation) + "\\cache";


            //instantiate server object
            var t = new Quotemate(appPath);

            //create a named container and add an unique named server
            var t1 = new SimpleIPC.NamedObject.SIPCServer("GoogleNewsServer", t, SIPCEncoding.json);

            //start
            t1.Start();
            Console.WriteLine("GoogleNewsServer is running");
            Console.ReadKey();
            //stop container
            t1.Stop();

        }

        public static void Main(string[] args)
        {
            //string assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //string appPath = System.IO.Path.GetDirectoryName(assemblyLocation) + "\\cache";
            //var t = new Quotemate(appPath);
            //var n = t.QueryNews("nvda");
            //Console.WriteLine(n);


            Testwindowsserver();
        }

    }
}
