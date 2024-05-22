using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using CefSharp.Example;
using CefSharp.Example.Handlers;
using System.Threading;
using CefSharp;
using CefSharp.OffScreen;
using System.Web.Script.Serialization;
using System.Linq;
using System.Text.RegularExpressions;

namespace GooleNews
{
    public interface INews
    {
        String QueryNews(string ticker);
    }

    public class Quotemate : INews,IDisposable
    {
        private static string searchtext = "";
        private static AutoResetEvent oSignalEvent = new AutoResetEvent(false);

        public Quotemate()
        {
            CefExample.Init(new CefSettings(), browserProcessHandler: new BrowserProcessHandler());
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

            MainAsync(TestUrl);
            oSignalEvent.WaitOne();
            var lines = searchtext.Split(new char[] { '\n' }).ToList();
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

        public string OldQueryNews(string ticker)
        {
            DateTime dt = DateTime.Now;
            dt -= new TimeSpan(9, 30, 0);
            int hour = 0, min = 0, day = 0, mon = 0, year = 0;
            string TestUrl = String.Format("https://www.google.com/search?q={0}&tbm=nws&tbs=sbd:1", ticker);
            string lastnewstitle = "";
            string lastnewssource = "";
            string timestamp = "";
            string temps = "";

            MainAsync(TestUrl);
            oSignalEvent.WaitOne();
            var lines = searchtext.Split(new char[] { '\n' }).ToList();
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
                        if (Regex.Matches(lastnewssource, @"-\d\d").Count > 0)
                        {
                            temps = Regex.Matches(lastnewssource, @"-\d\d")[0].Value;
                            min = int.Parse(temps.Substring(1));
                        }
                        else if (Regex.Matches(lastnewssource, @"-\d").Count > 0)
                        {
                            temps = Regex.Matches(lastnewssource, @"-\d")[0].Value;
                            min = int.Parse(temps.Substring(1));
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
                        if (Regex.Matches(lastnewssource, @"-\d\d").Count > 0)
                        {
                            temps = Regex.Matches(lastnewssource, @"-\d\d")[0].Value;
                            hour = int.Parse(temps.Substring(1));
                        }
                        else if (Regex.Matches(lastnewssource, @"-\d").Count > 0)
                        {
                            temps = Regex.Matches(lastnewssource, @"-\d")[0].Value;
                            hour = int.Parse(temps.Substring(1));
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
                    lastnewstitle = lines[idx - 1];
                }

            }

            return new JavaScriptSerializer().Serialize(new { news = new { title = lastnewstitle, source = lastnewssource, timestamp = timestamp } });
        }

        private async void MainAsync(string TestUrl)
        {
            using (var browser = new ChromiumWebBrowser(TestUrl))
            {
                await LoadPageAsync(browser);
                searchtext = await browser.GetMainFrame().GetTextAsync();
                oSignalEvent.Set();
            }
        }

        public Task LoadPageAsync(IWebBrowser browser, string address = null)
        {
            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            EventHandler<LoadingStateChangedEventArgs> handler = null;
            handler = (sender, args) =>
            {
                //Wait for while page to finish loading not just the first frame
                if (!args.IsLoading)
                {
                    browser.LoadingStateChanged -= handler;
                    //Important that the continuation runs async using TaskCreationOptions.RunContinuationsAsynchronously
                    tcs.TrySetResult(true);
                }
            };

            browser.LoadingStateChanged += handler;

            if (!string.IsNullOrEmpty(address))
            {
                browser.Load(address);
            }
            return tcs.Task;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Cef.Shutdown();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Quotemate() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion


    }
    public class Program
    {
        static private void Testwindowsserver()
        {
            //instantiate server object
            var t = new Quotemate();

            //create a windows container and start it
            var t1 = new SimpleIPC.Windows.ServerContainer();
            t1.Start();

            //create a unique windows server and inject server object created above
            t1.CreateServer(new SimpleIPC.Windows.SIPCServer("GoogleNewsServer", t));
            Console.WriteLine("GoogleNewsServer is running");
            System.Threading.Thread.Sleep(new TimeSpan(1, 0, 0, 0));
            //stop container
            t1.Stop();
        }

        static private void TestNamedobjectserver()
        {
            //instantiate server object
            var t = new Quotemate();

            //create a named container and add an unique named server
            var t1 = new SimpleIPC.NamedObject.SIPCServer("GoogleNewsServer", t);

            //start
            t1.Start();
            Console.WriteLine("GoogleNewsServer is running");
            Console.ReadKey();
            //stop container
            t1.Stop();

        }

        public static void Main(string[] args)
        {
            Testwindowsserver();
        }

    }
}
