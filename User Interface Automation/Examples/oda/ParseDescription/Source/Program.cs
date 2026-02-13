using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace ParseDescription
{
    internal class Program
    {

        static Dictionary<string, string> generatetimestamps(string fname)
        {
            var ret = new Dictionary<string, string>();

            var headerregex = new Regex(@"^\d{3}$");
            var footerregex = new Regex(@"[""](Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)[ ]\d{1,2},[ ]\d{4}.*");
            var footerregex2 = new Regex(@"[""](Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)[ ]\d{1,2}[ ].*");
            var lines = File.ReadLines(fname);
            var key = "";
            foreach (var line in lines)
            {
                if (headerregex.Match(line).Success)
                {
                    key = line;

                }

                if ((footerregex.Match(line).Success) || (footerregex2.Match(line).Success))
                {
                    var dttms = line.Replace("\"", "");
                    var idx = dttms.IndexOf(" AM");
                    if (idx == -1)
                        idx = dttms.IndexOf(" PM");
                    if (idx == -1)
                        continue;
                    dttms = dttms.Substring(0, idx + 3);
                    if (footerregex2.IsMatch(line))
                    {
                        idx = dttms.IndexOf(' ',4);
                        dttms = dttms.Insert(idx, ", 2026,");
                    }
                    dttms = DateTime.Parse(dttms).ToString("s");
                    dttms = dttms.Substring(0, dttms.Length - 3);
                    ret.Add(key, dttms);
                }
                
            }

            return ret;
        }

        static Dictionary<string,string> generatedesc(string fname,string op)
        {
            var ret = new Dictionary<string, string>();

            var headerregex = new Regex(@"^\d{3}$");
            string[] descx = new string[]{ "\\\"\\d+\\.[ ]", "\\\".+\\-[ ]", "\\\"\\w+[ ]-[ ]" };
            var removeln = new Regex(descx[0]);
            var removeln2 = new Regex(descx[1]);
            var removeln3 = new Regex(descx[2]);

            var lines = File.ReadLines(fname).ToArray();
            int i = 0;
            foreach (var line in lines)
            {
                if (headerregex.Match(line).Success)
                {
                    var l2 = removeln.Replace(lines[i + 1], "\"");
                    if (op == "h")
                        l2 = removeln3.Replace(l2, "\"");
                    ret.Add(line.Replace("\"", ""), l2.Replace("\"", ""));
                }
                i++;
            }

            return ret;
        }

        static Dictionary<string, string> makedict(string fname)
        {
            return File.ReadAllLines(fname).Select(f => { var parts = f.Split(',');  return new KeyValuePair<string, string>(parts[0].Replace("\"", ""), parts[1].Replace("\"", "")); }).ToDictionary(f => f.Key, f => f.Value);
        }

        static Dictionary<string, string> getsubject(List<KeyValuePair<string, string>> d, Dictionary<string, string> d2, Dictionary<string, string> d3)
        {
            var subx = new Regex("^(\\w+)[ ]-[ ]");
            var ret = new Dictionary<string, string>();
            foreach (var kv in d)
            {
                var s = d2[kv.Key];
                Match m = subx.Match(s);
                if (m.Success)
                {
                    var subj = m.Groups[1].Captures[0];
                    ret.Add(d3[kv.Key], subj.Value);
                }
            }

            return ret;
        }


        static void Main(string[] args)
        {
            if (args.Length  < 3)
                return;

            var cm_descf = args[0];
            var hw_descf = args[1];
            var vid_descf = args[2];
            var finaldir = args[3]+"\\";

            var cmkv = generatetimestamps(cm_descf+"\\description.txt");
            var hwkv = generatedesc(hw_descf + "\\description.txt", "h");
            var vikv = generatedesc(vid_descf + "\\description.txt", "v");
            var vikv2 = generatetimestamps(vid_descf + "\\description.txt");

            var vikv3 = vikv.Where(kv => hwkv.Values.Count(v => kv.Value.Contains(v)) != 0).ToDictionary(f => f.Key, f => f.Value);
            var vikv4 = vikv2.Where(kv => vikv3.Keys.Contains(kv.Key)).ToList();
            var vikv5 = getsubject(vikv4, vikv, vikv2);

            if (Directory.Exists(finaldir)) 
                Directory.Delete(finaldir, true);
            Directory.CreateDirectory(finaldir);

            foreach (var k in vikv5.Keys)
            {
                var dstfldr = finaldir + vikv5[k];
                if (!Directory.Exists(dstfldr))
                    Directory.CreateDirectory(dstfldr);
                
                var dt = k.Substring(0, 10);
                dstfldr = dstfldr + "\\" + dt;
                Directory.CreateDirectory(dstfldr);
                var ck = cmkv.Where(ckv => ckv.Value.Contains(dt));
                foreach (var fname in ck)
                {
                    var pdfs = Directory.GetFiles(cm_descf, fname.Key + "*.pdf");
                    File.Move(pdfs[0], dstfldr + "\\" + Path.GetFileName(pdfs[0]).Substring(4));
                }
            }




        }
    }
}
