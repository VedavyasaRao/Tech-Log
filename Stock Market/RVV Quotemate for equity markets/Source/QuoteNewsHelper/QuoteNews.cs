using RGiesecke.DllExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoteNewsHelper
{
    public interface INews
    {
        String QueryNews(string ticker);
    }

    public interface IQuotes : IDisposable
    {
        String QueryDateTime();
        String QueryIndicies();
        String QueryCloses(string ticker);
        String QueryQuote(string ticker);
    }

    public class QuoteNews
    {
        static SimpleIPC.GenericProxy<IQuotes> quotesp = null;
        static SimpleIPC.GenericProxy<INews> newsp = null;

        public static  void Main()
        {
            var t = QuoteNews.QueryDateTime();
        }
        static QuoteNews()
        {
            System.Threading.Thread.Sleep(3000);
            quotesp = new SimpleIPC.GenericProxy<IQuotes>(new SimpleIPC.Windows.SIPCProxy("YahooFinanceServer"), true, true, true);
            System.Threading.Thread.Sleep(3000);
            newsp = new SimpleIPC.GenericProxy<INews>(new SimpleIPC.Windows.SIPCProxy("GoogleNewsServer"), false, false, false);
            System.Threading.Thread.Sleep(3000);
        }

        [DllExport(CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        static public string QueryNews(string ticker)
        {
            return newsp.Proxy.QueryNews(ticker);
        }

        [DllExport(CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        static public string QueryIndicies()
        {
            return  quotesp.Proxy.QueryIndicies();
        }

        [DllExport(CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        static public string QueryDateTime()
        {
            return quotesp.Proxy.QueryDateTime();
        }

        [DllExport(CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        static public string QueryCloses(string ticker)
        {
            return quotesp.Proxy.QueryCloses(ticker);
        }

        [DllExport(CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        static public string QueryQuote(string ticker)
        {
            return quotesp.Proxy.QueryQuote(ticker);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    quotesp.Dispose();
                    newsp.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Program() {
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

