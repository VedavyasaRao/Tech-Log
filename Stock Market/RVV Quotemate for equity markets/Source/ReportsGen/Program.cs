using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace RVVPM
{
    public interface IQuotes : IDisposable    {
        String QueryTime();
        String QueryIndicies();
        String QueryCloses(string ticker);
        String QueryQuote(string ticker);
    }

    class Reportgenerator
    {
        static ulong tidcounter = 50000000000;
        public class record
        {
            public enum BS { bought, sold };
            public ulong tid;
            public DateTime dt;
            public BS bs;
            public short qty;
            public string ticker;
            public float price;
            public float dvalue;
            public record(string aline)
            {
                string[] parts = aline.Split(new char[] { ',' });
                dt = DateTime.Parse(parts[0]);
                tid = Reportgenerator.tidcounter++;
                bs = parts[2].Contains("Bought") ? BS.bought : BS.sold;
                qty = short.Parse(parts[3]);
                ticker = parts[4];
                price = float.Parse(parts[5]);
                dvalue = qty * price;
            }
            public void deductqty (short dedqty)
            {
                qty -= dedqty;

            }
            public void deductvalue(float dedvalue)
            {
                dvalue -= dedvalue;

            }
        };

        SimpleIPC.GenericProxy<IQuotes> quotesp = null;

        public List<record> transactions = new List<record>();
        public void Init()
        {
            var p = Process.GetProcessesByName("YahooFinance");
            if (p.Length == 0)
            {
                string curpath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string yahoofinexe = curpath + "..\\..\\YahooFinance\\YahooFinance.exe";
                Process.Start(yahoofinexe);
                System.Threading.Thread.Sleep(3000);
            }
            quotesp = new SimpleIPC.GenericProxy<IQuotes>(new SimpleIPC.Windows.SIPCProxy("YahooFinanceServer"), true, true, true);
            System.Threading.Thread.Sleep(3000);
        }

        public void ReadTranansctions(string filename)
        {
            string[] lines = null;
            lines = File.ReadAllLines(filename);
            if (lines == null)
                return;
            for (int i = 1; i < lines.Length; ++i)
            {
                if (!lines[i].Contains("Bought") && !lines[i].Contains("Sold"))
                    continue;
                transactions.Add(new record(lines[i]));
            }
        }

        private void process()
        {
            var solds = (from trn in transactions where trn.bs == record.BS.sold select trn).ToList();
            for (int i=0; i<solds.Count(); ++i)
            {
                var boughts = (from trn in transactions where (trn.ticker== solds[i].ticker && trn.bs == record.BS.bought && trn.tid < solds[i].tid  && trn.qty > 0) select trn).ToList();
                var sqty = solds[i].qty;
                int j = 0;
                while (sqty > 0 && j < boughts.Count())
                {
                    var tqty = Math.Min(sqty, boughts[j].qty);
                    solds[i].deductvalue(tqty * boughts[j].price);
                    boughts[j].deductqty(tqty);
                    sqty -= tqty;
                    if (boughts[j].qty == 0)
                        j++;
                }
            }
        }
        public void createreport(string outputfile)
        {
            var results = (from trn in transactions
                           group trn by trn.ticker into recs
                           orderby recs.Key
                           select new { ticker = recs.Key, records = recs.ToList() }).ToList();
            if (File.Exists(outputfile))
                File.Delete(outputfile);
            jsonparser jspobj = new jsonparser();

            File.AppendAllText(outputfile, "id,ticker,date,type,qty,price,actgain,curprice,potgain" + Environment.NewLine);
            foreach (var kp in results)
            {
                foreach (var rec in kp.records)
                {
                    if (rec.qty == 0)
                        continue;
                    var curpricejson = quotesp.Proxy.QueryQuote(rec.ticker);
                    var curprice = float.Parse(jspobj.ParseObj(curpricejson, "quote.lasttrade").ToString());
                    var curvalue = (curprice * rec.qty);
                    if (rec.bs == record.BS.bought)
                        rec.dvalue = 0;
                    curvalue -= ((rec.qty*rec.price) - rec.dvalue);
                    File.AppendAllText(outputfile, String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}{9}", rec.tid,kp.ticker, rec.dt.ToShortDateString(), rec.bs, rec.qty, rec.price.ToString("0.00"), rec.dvalue.ToString("0.00"), curprice.ToString("0.00"), curvalue.ToString("0.00"), Environment.NewLine));
                }
            }
        }
        public void createreport2(string outputfile)
        {
            if (File.Exists(outputfile))
                File.Delete(outputfile);
            File.AppendAllText(outputfile, "id,ticker,date,type,qty,price,value" + Environment.NewLine);
            foreach (var rec in transactions)
            {
                File.AppendAllText(outputfile, String.Format("{0},{1},{2},{3},{4},{5},{6}{7}", rec.tid, rec.ticker, rec.dt.ToShortDateString(), rec.bs, rec.qty, rec.price.ToString("0.00"), rec.dvalue.ToString("0.00"), Environment.NewLine));
            }
        }

        public void createreport3(string outputfile)
        {
            var results = (from trn in transactions
                           group trn by trn.ticker into recs
                           orderby recs.Key
                           select new { ticker = recs.Key, records = recs.OrderBy(x=>x.tid).ToList() }).ToList();
            if (File.Exists(outputfile))
                File.Delete(outputfile);
            File.AppendAllText(outputfile, "id,ticker,date,type,qty,price,value" + Environment.NewLine);
            foreach (var kp in results)
            {
                foreach (var rec in kp.records)
                {
                    File.AppendAllText(outputfile, String.Format("{0},{1},{2},{3},{4},{5},{6}{7}", rec.tid, rec.ticker, rec.dt.ToShortDateString(), rec.bs, rec.qty, rec.price.ToString("0.00"), (rec.qty*rec.price).ToString("0.00"), Environment.NewLine));
                }
            }
        }


        [STAThread]
        static void Main(string[] args)
        {
            string trnfilename = "";
            string rptfilename = "";
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                trnfilename = openFileDialog.FileName;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                rptfilename = saveFileDialog.FileName;
            }
            
            var rg = new Reportgenerator();
            rg.Init();
            var curpricejson = rg.quotesp.Proxy.QueryTime();

            rg.ReadTranansctions(trnfilename);
            rg.process();
            rg.createreport(rptfilename);
            if (rg.quotesp != null)
                rg.quotesp.Dispose();
        }
    }
}
