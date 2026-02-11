using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace RVVPM
{
    public interface IQuotes : IDisposable    {
        String QueryDateTime();
        String QueryIndicies();
        String QueryCloses(string ticker);
        String QueryQuote(string ticker);
        string QueryBalances();

    }

    class Reportgenerator
    {
        public class record
        {
            public string ticker;
            public short qty;
            public float currentprice;
            public float marketvalue;
            public float daysgainloss;
            public float totalgainloss;
            
            public string print()
            {
                string fmt = "$#,###,###.00;$(#,###,###.00)";
                return $"{ticker.PadRight(8, ' ')}\t{qty.ToString().PadRight(8, ' ')}\t{currentprice.ToString(fmt).PadRight(8,' ')}\t{marketvalue.ToString(fmt).PadRight(8, ' ')}\t{daysgainloss.ToString(fmt).PadRight(8, ' ')}\t{totalgainloss.ToString(fmt)}\n";
            }
        };

        SimpleIPC.GenericProxy<IQuotes> quotesp = null;
        List<record> records = new List<record>();
        string outputfile;

        public void Init()
        {
            var p = Process.GetProcessesByName("CharlesSchwab");
            if (p.Length == 0)
            {
                string curpath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                curpath = Path.GetDirectoryName(curpath) + "\\CharlesSchwab";
                ProcessStartInfo pi = new ProcessStartInfo();
                pi.FileName= curpath + "\\CharlesSchwab.exe";
                pi.Arguments = "t";
                pi.WorkingDirectory = curpath;
                Process.Start(pi).WaitForExit();
                System.Threading.Thread.Sleep(3000);

                pi.Arguments = "r";
                Process.Start(pi);
                System.Threading.Thread.Sleep(5000);
            }
            quotesp = new SimpleIPC.GenericProxy<IQuotes>(new SimpleIPC.Windows.SIPCProxy("CharlesSchwabServer"), true, true, true);
            System.Threading.Thread.Sleep(5000);
        }

        object getavalue(object data, string key)
        {
            var dic = data as Dictionary<string, object>;
            if (dic.ContainsKey(key))
                return dic[key];
            return null;
        }

        object getvalue(object data, string[] keys)
        {
            object obj = data;
            foreach (var key in keys)
            {
                obj = getavalue(obj, key);
                if (obj == null)
                    break;
            }

            return obj;
        }

        record createrecord(object position)
        {
            var rec = new record();
            rec.ticker = (string)getvalue(position, new string[] { "instrument", "symbol" });
            rec.qty = Convert.ToInt16(getvalue(position, new string[] { "longQuantity" }));
            rec.marketvalue = Convert.ToSingle(getvalue(position, new string[] { "marketValue" }));
            rec.daysgainloss = Convert.ToSingle(getvalue(position, new string[] { "currentDayProfitLoss" }));
            rec.totalgainloss = Convert.ToSingle(getvalue(position, new string[] { "longOpenProfitLoss" }));
            var quote = quotesp.Proxy.QueryQuote(rec.ticker);
            System.Threading.Thread.Sleep(2000);
            var tokens = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(quote);
            rec.currentprice = Convert.ToSingle(getvalue(tokens, new string[] { "quote", "lasttrade" }));

            return rec;

        }

        private void process(object[]positions)
        {
            foreach (var p in positions)
            {
                var rec = createrecord(p);
                records.Add(rec);
            }
        }

        void logit(string msg)
        {
            System.Console.Write(msg);
            File.AppendAllText(outputfile, msg);
        }

        public void createreport()
        {
            var cswab = quotesp.Proxy.QueryDateTime();
            System.Threading.Thread.Sleep(2000);
            var datetime = new JavaScriptSerializer().Deserialize<Dictionary<string,object>>(cswab);
            var dttm = (string)getvalue(datetime, new string[] { "DateTime"});

            outputfile = $"c:\\temp\\{dttm.Replace(':',' ')}.txt";
            var savdlg = new SaveFileDialog();

            savdlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            savdlg.FilterIndex = 2;
            savdlg.RestoreDirectory = true;
            savdlg.FileName= $"{dttm.Replace(':', ' ')}.txt";
            if (savdlg.ShowDialog() != DialogResult.OK)
                return;

            outputfile = savdlg.FileName;
            logit($"{dttm}\n");

            cswab = quotesp.Proxy.QueryIndicies();
            System.Threading.Thread.Sleep(2000);
            var indices = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(cswab);
            var dow = (string)getvalue(indices, new string[] { "Indices","Dow" });
            var nasdaq = (string)getvalue(indices, new string[] { "Indices", "Nasdaq" });
            logit($"Dow: {dow}\t\t\tNasdaq: {nasdaq}\n\n");
            cswab = quotesp.Proxy.QueryBalances();
            System.Threading.Thread.Sleep(2000);

            var tokens = new JavaScriptSerializer().Deserialize<List<object>>(cswab);

            foreach (var token in tokens)
            {
                var acno = getvalue(token, new string[] { "securitiesAccount", "accountNumber" });
                logit($"Account No: {(string)acno}\n");

                var positions = getvalue(token, new string[] { "securitiesAccount", "positions" });
                if (positions != null)
                {
                    process((object[])positions);
                    float dailylossgain = 0;
                    logit($"Positions:\n\n");
                    logit($"{"Name".PadRight(8, ' ')}\t{"Qty".PadRight(8, ' ')}\t{"Price".PadRight(8, ' ')}\t{"Market Value".PadRight(8, ' ')}\t{"Days G/L".PadRight(8, ' ')}\t{"Total G/L"}\n");
                    foreach (var rec in records)
                    {
                        logit(rec.print());
                        dailylossgain += rec.daysgainloss;
                    }
                    string fmt = "$#,###,###.00;$(#,###,###.00)";
                    var curbalance = getvalue(token, new string[] { "securitiesAccount", "currentBalances" }) as Dictionary<string, object>;
                    var cashbalance = Convert.ToSingle(curbalance["totalCash"]);
                    var mktvalue = Convert.ToSingle(curbalance["longMarketValue"]);
                    logit($"{"Cash".PadRight(8, ' ')}\t\t\t\t\t\t\t\t\t{cashbalance.ToString(fmt).PadRight(8, ' ')}\n");
                    logit($"{"Total".PadRight(8, ' ')}\t\t\t\t\t\t\t{dailylossgain.ToString(fmt).PadRight(8, ' ')}\t{mktvalue.ToString(fmt).PadRight(8, ' ')}\n");
                }
                logit($"****************************************************************************************\n\n");
            }
        }

        void cleanup()
        {
            if (quotesp != null)
                quotesp.Dispose();
        }


        [STAThread]
        static void Main(string[] args)
        {
            var rg = new Reportgenerator();

            rg.Init();
            rg.createreport();
            rg.cleanup();
            
        }
    }
}
