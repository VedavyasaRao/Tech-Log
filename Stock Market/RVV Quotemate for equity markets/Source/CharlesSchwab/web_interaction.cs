using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;


namespace CharlesSchwab
{
    class web_interaction
    {

        public web_interaction()
        {
            System.Net.ServicePointManager.Expect100Continue = true;
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
        }
        public async Task<string> retrivetokens(cschwabsettings mysettings)
        {
            string url = "https://api.schwabapi.com/v1/oauth/authorize?response_type=code&client_id=";
            url += mysettings.appid();
            url += "&scope=readonly&redirect_uri=https://127.0.0.1";
            System.Diagnostics.Process.Start(url);

            url = Microsoft.VisualBasic.Interaction.InputBox("enter url");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + mysettings.appidsecret());

            var p = url.IndexOf("code=") + 5;
            var len = url.IndexOf("%40") - p;
            var response_code = url.Substring(p, len) + "@";
            var values = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", response_code },
                { "redirect_uri", "https://127.0.0.1" }
            };

            var content = new FormUrlEncodedContent(values);
            string tokenbuffer = "";
            HttpResponseMessage response = null;
            byte[] responseString = null;
            try
            {
                response = await client.PostAsync("https://api.schwabapi.com/v1/oauth/token", content);
                responseString = await response.Content.ReadAsByteArrayAsync();
            }
            catch (Exception)
            {

            }
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                tokenbuffer = Encoding.UTF8.GetString(responseString);
            }
            else
            {
                try
                {
                    using (Stream fs = new MemoryStream(responseString))
                    {
                        using (Stream csStream = new GZipStream(fs, CompressionMode.Decompress))
                        {
                            byte[] buffer = new byte[1024];
                            var nRead = csStream.Read(buffer, 0, buffer.Length);
                            var s = Encoding.UTF8.GetString(buffer, 0, nRead);
                        }
                    }
                }
                catch (Exception)
                {

                }
            }

            return tokenbuffer;
        }

        public   async Task<string> updaterefreshtoken(cschwabsettings mysettings)
        {
            string ret = "";

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + mysettings.appidsecret());

            var values = new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", mysettings.refreshtoken() }
            };
            var content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = null;
            byte[] responseString = null;
            try
            {
                response = await client.PostAsync("https://api.schwabapi.com/v1/oauth/token", content);
                responseString = await response.Content.ReadAsByteArrayAsync();
            }
            catch (Exception)
            {

            }

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ret = Encoding.UTF8.GetString(responseString);
            }
            else
            {
                try
                {
                    using (Stream fs = new MemoryStream(responseString))
                    {
                        using (Stream csStream = new GZipStream(fs, CompressionMode.Decompress))
                        {
                            byte[] buffer = new byte[1024];
                            var nRead = csStream.Read(buffer, 0, buffer.Length);
                            var s = Encoding.UTF8.GetString(buffer, 0, nRead);
                        }
                    }
                }
                catch (Exception)
                {

                }
            }

            return ret;
        }

        public async Task<string> getquote(cschwabsettings mysettings, string symbol)
        {
            string ret = "";

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + mysettings.accesstoken());
            string url = "https://api.schwabapi.com/marketdata/v1/" + symbol + "/quotes?fields=quote";
            HttpResponseMessage response = null;
            byte[] responseString = null;
            try
            {
                response = await client.GetAsync(url);
                responseString = await response.Content.ReadAsByteArrayAsync();
            }
            catch (Exception)
            {

            }

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ret = Encoding.UTF8.GetString(responseString);
            }
            else
            {
                ret = Encoding.UTF8.GetString(responseString);
            }

            return ret;
        }

        public async Task<string> gethistoricaldata(cschwabsettings mysettings, string symbol)
        {
            string ret = "";

            HttpClient client = new HttpClient();
            var ed = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var st = DateTimeOffset.UtcNow.AddDays(-20).ToUnixTimeMilliseconds();
            var url = "https://api.schwabapi.com/marketdata/v1/pricehistory?symbol=";
            url += symbol.ToUpper();
            url += "&periodType=month&period=1&frequencyType=daily&frequency=1&startDate=";
            url += st.ToString();
            url += "&endDate=" + ed.ToString();
            url += "&needExtendedHoursData=false&needPreviousClose=false";
            //https://api.schwabapi.com/marketdata/v1/pricehistory?symbol=NVDA&periodType=month&period=1&frequencyType=daily&frequency=1&startDate=1758326400000&endDate=1759352324000&needExtendedHoursData=false&needPreviousClose=false
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + mysettings.accesstoken());
            HttpResponseMessage response = null;
            byte[] responseString = null;
            try
            {
                response = await client.GetAsync(url);
                responseString = await response.Content.ReadAsByteArrayAsync();
            }
            catch (Exception)
            {

            }

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ret = Encoding.UTF8.GetString(responseString);
            }
            else
            {
                ret = Encoding.UTF8.GetString(responseString);
            }

            return ret;
        }

        public async Task<string> getquotes(cschwabsettings mysettings, string symbols)
        {
            string ret = "";

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + mysettings.accesstoken());
            string url = "https://api.schwabapi.com/marketdata/v1/quotes?symbols=" + symbols + "&fields=quote&indicative=false";
            HttpResponseMessage response = null;
            byte[] responseString = null;
            try
            {
                response = await client.GetAsync(url);
                responseString = await response.Content.ReadAsByteArrayAsync();
            }
            catch (Exception)
            {

            }

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ret = Encoding.UTF8.GetString(responseString);
            }
            else
            {
                ret = Encoding.UTF8.GetString(responseString);
            }

            return ret;
        }

        public async Task<string> gettime()
        {
            string ret = "";

            HttpClient client = new HttpClient();

            string url = "https://www.timeanddate.com/worldclock/usa/new-york";
            HttpResponseMessage response = null;
            byte[] responseString = null;
            try
            {
                response = await client.GetAsync(url);
                responseString = await response.Content.ReadAsByteArrayAsync();
            }
            catch (Exception)
            {

            }

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ret = Encoding.UTF8.GetString(responseString);
            }
            else
            {
                ret = Encoding.UTF8.GetString(responseString);
            }


            return ret;
        }

        public async Task<string> getbalances(cschwabsettings mysettings)
        {
            string ret = "";

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + mysettings.accesstoken());
            string url = "https://api.schwabapi.com/trader/v1/accounts?fields=positions";
            HttpResponseMessage response = null;
            byte[] responseString = null;
            try
            {
                response = await client.GetAsync(url);
                responseString = await response.Content.ReadAsByteArrayAsync();
            }
            catch (Exception)
            {

            }

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ret = Encoding.UTF8.GetString(responseString);
            }
            else
            {
                ret = Encoding.UTF8.GetString(responseString);
            }

            return ret;
        }

    }
}
