using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using System.Threading;

namespace CharlesSchwab
{
    public interface IQuotes
    {
        String QueryDateTime();
        String QueryIndicies();
        String QueryCloses(string ticker);
        String QueryQuote(string ticker);
        string QueryBalances();
    }

    public class Quotemate : IQuotes
    {
        string logfile = "";
        cschwabsettings mysettings;
        web_interaction pi = new web_interaction();

        public Quotemate(cschwabsettings mysettings)
        {
            this.mysettings = mysettings;
            //string assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //logfile = System.IO.Path.GetDirectoryName(assemblyLocation) + "\\output.txt";
            //if (File.Exists(logfile))
            //    File.Delete(logfile);
        }

        private void refreshtokens()
        {
            if (mysettings.accesstoken_datetime() < (DateTime.Now.AddDays(-7)))
            {
                var buffer = pi.retrivetokens(mysettings).GetAwaiter().GetResult();
                if (string.IsNullOrEmpty(buffer))
                    return;
                mysettings.updatetokens(buffer, false);
            }

            if (mysettings.refreshtoken_datetime() < (DateTime.Now - TimeSpan.Parse("00:30:00")))
            {
                var buffer = pi.updaterefreshtoken(mysettings).GetAwaiter().GetResult();
                if (string.IsNullOrEmpty(buffer))
                    return;
                mysettings.updatetokens(buffer,true);
            }
        }

        void logit(string s)
        {
            if (!string.IsNullOrEmpty(logfile))
            {
                File.AppendAllText(logfile, s + "\n");
            }
        }

        object getvalsobj(Dictionary<string, object> tokens, string key, string key2)
        {
            object vals;
            object vals2;
            tokens.TryGetValue(key, out vals);
            ((Dictionary<string, object>)vals).TryGetValue(key2, out vals2);
            return vals2;
        }


        Dictionary<string,object> getvals(Dictionary<string, object> tokens, string key, string key2)
        {
            return getvalsobj(tokens,key,key2) as Dictionary<string, object>;
        }

        public string QueryCloses(string ticker)
        {
            var closes = new double[6];
            var closedays = new String[6];
            refreshtokens();


            var cswab = pi.gethistoricaldata(mysettings, ticker).GetAwaiter().GetResult();
            var tokens = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(cswab);
            var closesarr = tokens["candles"] as System.Collections.ArrayList;
            int idx = closesarr.Count;
            for (int i=0; i < 6; ++i,idx--)
            {
                closes[i] = (double)((decimal)((Dictionary<string, object>)closesarr[idx - 1])["close"]);
                closedays[i] = DateTimeOffset.FromUnixTimeMilliseconds(((long)((Dictionary<string, object>)closesarr[idx - 1])["datetime"])).ToString("yyyy-MM-dd");
            }

            var ret = new JavaScriptSerializer().Serialize(new
            {
                PrevCloses = new
                {
                    close0 = closes[0],
                    day0 = closedays[0],
                    close1 = closes[1],
                    day1 = closedays[1],
                    close2 = closes[2],
                    day2 = closedays[2],
                    close3 = closes[3],
                    day3 = closedays[3],
                    close4 = closes[4],
                    day4 = closedays[4],
                    close5 = closes[5],
                    day5 = closedays[5]
                }
            });

            return ret;
        }

        public string QueryIndicies()
        {
            string dow = "";
            string nasdaq = "";
            refreshtokens();

            var cswab = pi.getquotes(mysettings, "$DJI,$COMPX").GetAwaiter().GetResult();
            var tokens = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(cswab);

            var vals = getvals(tokens, "$DJI","quote");
            if ((string)vals["securityStatus"] != "Closed")
            {
                dow = ((decimal)vals["netChange"]).ToString("+#.##;-#.##;0");

                vals = getvals(tokens, "$COMPX", "quote");
                nasdaq = ((decimal)vals["netChange"]).ToString("+#.##;-#.##;0");
            }
            else
            {
                var closes = QueryCloses("$DJI");
                tokens = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(closes);
                dow = (((decimal)getvalsobj(tokens, "PrevCloses", "close0")) - ((decimal)getvalsobj(tokens, "PrevCloses", "close1"))).ToString("+#.##;-#.##;0");

                closes = QueryCloses("$COMPX");
                tokens = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(closes);
                nasdaq = (((decimal)getvalsobj(tokens, "PrevCloses", "close0")) - ((decimal)getvalsobj(tokens, "PrevCloses", "close1"))).ToString("+#.##;-#.##;0");
            }

            var ret = new JavaScriptSerializer().Serialize(new { Indices = new { Dow = dow, Nasdaq = nasdaq } });

            return ret;
        }


        public string QueryQuote(string ticker)
        {
            refreshtokens();
            var cswab = pi.getquote(mysettings, ticker).GetAwaiter().GetResult();
            var tokens = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(cswab);

            string lasttrade = "", lasttradetime = "", change = "", open = "", volume = "", ask = "", bid = "", dayrange = "", week52range = "";
            {
                var vals = getvals(tokens, ticker.ToUpper(),"quote");
                week52range = vals["52WeekLow"] + " - " + vals["52WeekHigh"];
                dayrange = vals["lowPrice"] + " - " + vals["highPrice"];
                bid = vals["bidPrice"] + " x " + vals["bidSize"];
                ask = vals["askPrice"] + " x " + vals["askSize"];
                volume = vals["totalVolume"].ToString();
                open = vals["openPrice"].ToString();
                change = vals["netChange"].ToString();
                var tmpdttm = (new DateTime(1970, 1, 1)).AddMilliseconds((long)vals["quoteTime"]);
                TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                lasttradetime = "As of " + TimeZoneInfo.ConvertTimeFromUtc(tmpdttm, targetTimeZone).ToString("HH:mm:ss tt") + "EDT.";
                lasttrade = vals["lastPrice"].ToString();
            }

            var ret = new JavaScriptSerializer().Serialize(new { quote = new { lasttrade = lasttrade, lasttradetime = lasttradetime, change = change, open = open, volume = volume, ask = ask, bid = bid, dayrange = dayrange, week52range = week52range } });
            return ret;
        }

        public string QueryDateTime()
        {

            /*
            var s = pi.gettime().GetAwaiter().GetResult();
            var srch = "<span id=ct class=h1>";
            var t = s.Substring(s.IndexOf(srch) + srch.Length, 8);
             */

            TimeSpan ts = TimeSpan.Parse("9:30:00");
            DateTime convertedTime = DateTime.Now - ts;
            try
            {
                TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                convertedTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, targetTimeZone);
            }
            catch (Exception)
            {
            }

            var ret = new JavaScriptSerializer().Serialize(new { DateTime = convertedTime.ToString("yyyy-MM-dd HH:mm:ss") } );
            return ret;
        }

        public string QueryBalances()
        {
            refreshtokens();

            var cswab = pi.getbalances(mysettings).GetAwaiter().GetResult();
            return cswab;
        }



    }
}
