using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using CefSharp;
using CefSharp.OffScreen;
using System.Web.Script.Serialization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace CefLoadPage
{
    public class CefLoadPager : IDisposable
    {
        private static string searchtext = "";
        private static AutoResetEvent oSignalEvent = new AutoResetEvent(false);

        public CefLoadPager(string cachedir)
        {
            var settings = new CefSettings()
            {
                //By default CefSharp will use an in-memory cache, you need to specify a Cache Folder to persist data
                CachePath = cachedir
            };

            //Perform dependency check to make sure all relevant resources are in our output directory.
            //var success = Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
            var success = Cef.Initialize(settings);
            if (!success)
            {
                var exitCode = Cef.GetExitCode();

                throw new Exception($"Cef.Initialize failed with {exitCode}, check the log file for more details.");
            }
        }
        public List<string> LoadPage(string url)
        {
            MainAsync(url);
            oSignalEvent.WaitOne();
            return searchtext.Split(new char[] { '\n' }).ToList();
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
}
