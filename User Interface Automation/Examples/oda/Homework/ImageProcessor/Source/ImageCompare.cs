using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ImageHandler
{
    public class ImageCompare
    {
        string impcompexe = Program.ImageProcessorloc + @"\ImageComparer\ImageComparer.exe";
        public bool Process(string srcfile, string tgtfile, string outputfile, string args)
        {

            string cmdline = string.Format("\"{1}\"  \"{0}\" {2}", srcfile, tgtfile, args);
            var p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = impcompexe;
            p.StartInfo.Arguments = cmdline;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.WorkingDirectory =  Path.GetTempPath() + "imageprocessor";
            p.Start();
            if (!string.IsNullOrEmpty(outputfile) )
                File.WriteAllText(outputfile, p.StandardOutput.ReadToEnd());
            else
                File.WriteAllText(p.StartInfo.WorkingDirectory + "\\" + Path.GetFileNameWithoutExtension(srcfile) + ".txt", p.StandardOutput.ReadToEnd());
            p.WaitForExit();


            var dir = System.IO.Path.GetDirectoryName(tgtfile);
            cmdline = string.Format("/c \"cd/d \"{0}\" &  del  *.txt cropped*.* diff*.* histogram*.*\"", dir);
            var p2 = new Process();
            p2.StartInfo.UseShellExecute = false;
            p2.StartInfo.FileName = "cmd";
            p2.StartInfo.Arguments = cmdline;
            p2.StartInfo.CreateNoWindow = true;
            p2.Start();
            p2.WaitForExit();

            return (p.ExitCode == 0);
        }
    }
}
