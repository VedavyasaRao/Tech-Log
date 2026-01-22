using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Runtime;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;


namespace CharlesSchwab
{
    public class cschwabsettings
    {
        public enum items { accesstoken, refreshtoken, accesstoken_datetime, refreshtoken_datetime, app_id, secret_code };
        public Dictionary<items, object> data = new Dictionary<items, object>(6);

        public void serialize()
        {
            var filename = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            filename = filename + "\\mysettings.dat";
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, data);
            }
        }

        public string appid()
        {
            return (string)data[items.app_id];

        }

        public string appidsecret()
        {
            var code = (string)data[items.app_id] + ":" + (string)data[items.secret_code];
            return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(code));
        }

        public string refreshtoken()
        {
            return (string)data[items.refreshtoken];
        }

        public string accesstoken()
        {
            return (string)data[items.accesstoken];
        }

        public DateTime accesstoken_datetime()
        {
            return (DateTime)data[items.accesstoken_datetime];
        }

        public DateTime refreshtoken_datetime()
        {
            return (DateTime)data[items.refreshtoken_datetime];
        }

        public bool deserialize()
        {
            var filename = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            filename = filename + "\\mysettings.dat";
            if (!File.Exists(filename))
            {
                string appid, secret;
                appid = Microsoft.VisualBasic.Interaction.InputBox("App Key");
                if (string.IsNullOrEmpty(appid))
                    return false;

                secret = Microsoft.VisualBasic.Interaction.InputBox("Secret");
                if (string.IsNullOrEmpty(secret))
                    return false;

                updateappid(appid, secret);
                return true;
            }

            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                data = (Dictionary<items, object>)formatter.Deserialize(fs);
            }

            return true;
        }

        public void updatetokens(string buffer, bool brefresh)
        {
            var tokens = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(buffer);
            var dttm = DateTime.Now;
            if (!brefresh)
            {
                data[items.accesstoken_datetime] = dttm;
            }
            data[items.accesstoken] = tokens["access_token"];
            data[items.refreshtoken] = tokens["refresh_token"];
            data[items.refreshtoken_datetime] = dttm;

            serialize();
        }

        public void updateappid(string appid, string secret)
        {
            data[items.app_id] = appid;
            data[items.secret_code] = secret;
            var dttm = DateTime.Now - TimeSpan.Parse("240:00:00");
            data[items.accesstoken_datetime] = dttm;
            data[items.refreshtoken_datetime] = dttm;
            serialize();
        }

        public void updaterefreshtoken(DateTime dttm)
        {
            data[items.refreshtoken_datetime] = dttm;
            serialize();
        }

    }

}
