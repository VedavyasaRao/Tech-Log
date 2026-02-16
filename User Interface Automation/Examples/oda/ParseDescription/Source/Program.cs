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


        static Dictionary<string, string> getsubject(Dictionary<string, string> d, Dictionary<string, string> d2)
        {
            var subx = new Regex("^(\\w+)[ ]-[ ]");
            var ret = new Dictionary<string, string>();
            foreach (var kv in d)
            {
                var s = kv.Value;
                Match m = subx.Match(s);
                if (m.Success)
                {
                    var subj = m.Groups[1].Captures[0];
                    ret.Add(d2[kv.Key], subj.Value);
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

            var cmkvts = generatetimestamps(cm_descf+"\\description.txt");
            var vikvts = generatetimestamps(vid_descf + "\\description.txt");

            var hwkv = generatedesc(hw_descf + "\\description.txt", "h");
            var vikv = generatedesc(vid_descf + "\\description.txt", "v");

            Func<KeyValuePair<string, string>, string> vidlookup = ((kv =>  vikv.Where(vkv => vkv.Value.Contains(kv.Value)).First().Key + "," + kv.Key));
            var vidhwkv = hwkv.Where(hkv => vikv.Values.Count(v => v.Contains(hkv.Value)) != 0).Select(vidlookup).ToLookup(kv => kv.Substring(0, 3), kv => kv.Substring(4, 3));
            var subjects = getsubject(vikv, vikvts);

            var regx = new Regex(@"[\\/:*?""<>|]");


            if (Directory.Exists(finaldir)) 
                Directory.Delete(finaldir, true);
            Directory.CreateDirectory(finaldir);


             var prevdate = "";
            var keys = subjects.Keys.ToList();
            keys.Sort();

            foreach (var k in keys)
            {
                var dstfldr = finaldir + subjects[k];
                if (!Directory.Exists(dstfldr))
                    Directory.CreateDirectory(dstfldr);

                var dt = k;
                if (dt == prevdate)
                    continue;
                prevdate = dt;
                dstfldr = dstfldr + "\\" + dt.Substring(0, 10); 
                var vid = vikvts.Where(vkv => vkv.Value.Contains(dt)).First();
                dstfldr = dstfldr  + regx.Replace(vikv[vid.Key].Replace(subjects[k],""), "").Trim();
                Directory.CreateDirectory(dstfldr);


                int m = 0;
                //home work
                foreach (string hw in vidhwkv[vid.Key])
                {
                    var srcdir = hw_descf + "\\" + hw;
                    var dstdir = dstfldr + "\\";
                    if (vidhwkv[vid.Key].Count() > 1)
                        dstdir = dstdir + (++m).ToString("00") +"_";
                    dstdir = dstdir + "HomeWork";
                    Directory.Move(srcdir, dstdir);
                }

                //course material
                var ck = cmkvts.Where(ckv => ckv.Value.Contains(dt));
                foreach (var fname in ck)
                {
                    var pdfs = Directory.GetFiles(cm_descf, fname.Key + "*.pdf");
                    if (pdfs.Length != 0)
                        File.Move(pdfs[0], dstfldr + "\\" + Path.GetFileName(pdfs[0]).Substring(4));
                }

                var mp4s = Directory.GetFiles(vid_descf, vid.Key + "*.mp4");
                if (mp4s.Length != 0)
                    File.Move(mp4s[0], dstfldr + "\\" + Path.GetFileName(mp4s[0]).Substring(4));
            }




        }
    }
}
