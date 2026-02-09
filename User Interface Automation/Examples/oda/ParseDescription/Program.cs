using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

namespace ParseDescription
{
    internal class Program
    {


        static void Main(string[] args)
        {
            if (args.Length  < 1)
                return;
            var fname = args[0];
            var headerregex = new Regex(@"^\d{3}$");
            var footerregex = new Regex(@"[""](Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)[ ]\d{1,2}.*");
            //"Apr 28, 2025, 7:00 PM"
            var lines = File.ReadLines(fname);
            foreach(var line in lines)
            {
                if (headerregex.Match(line).Success)
                {
                    System.Console.WriteLine(line);
                }

                if (footerregex.Match(line).Success)
                {
                    var dttms = line.Replace("\"", "");
                    var idx = dttms.IndexOf(" AM");
                    if (idx == -1)

                    idx = dttms.IndexOf(" PM");
                    if (idx == -1)
                        continue;
                    dttms = dttms.Substring(0, idx + 3);
                    //System.Console.WriteLine(line);
                    dttms = DateTime.Parse(dttms).ToString("s");
                    dttms = dttms.Substring(0, dttms .Length - 3);
                    System.Console.WriteLine(dttms);
                    
                }


            }



        }
    }
}
