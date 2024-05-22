using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileOrganiser
{
    public class MD5Util
    {
        int dupidx;
        bool bfast = true;
        List<fileitem> ficlist = new List<fileitem>();
        private string calucalatemd5certutil(string filename)
        {
            try
            {
                ProcessStartInfo p = new ProcessStartInfo();
                p.UseShellExecute = false;
                p.RedirectStandardOutput = true;
                p.FileName = @"certutil.exe";
                p.Arguments = string.Format("-hashfile \"{0}\" MD5", filename);
                p.CreateNoWindow = true;

                string md5 = "";
                using (Process pp = Process.Start(p))
                {
                    md5 = pp.StandardOutput.ReadToEnd();
                }
                return md5.Split(new string[] { "\r\n" }, StringSplitOptions.None)[1];
            }
            catch (Exception ex)
            {
                driver.logit("MD5 ge failed   " + filename);
                driver.logit(ex.Message);
            }
            return "";
        }
        private string calucalatemd5full(string filename)
        {
            try
            {
                var fs = FileEx.OpenRead(filename);
                var md5 = MD5.Create();
                return BitConverter.ToString((md5.ComputeHash(fs)));
            }
            catch (Exception ex)
            {
                driver.logit("MD5 get failed   " + filename);
                driver.logit(ex.Message);
            }
            return "";
        }

        private string calucalatemd5optimized(string filename)
        {
            const int ONEMB = 1024 * 1024;
            byte[] data;
            long filelen = 0;
            try
            {
                var fs = FileEx.OpenRead(filename);
                filelen = fs.Length;
                if (filelen < ONEMB)
                {
                    data = FileEx.ReadAllBytes(filename);
                }
                else
                {
                    data = new byte[ONEMB];
                    fs.Seek(-ONEMB, System.IO.SeekOrigin.End);
                    fs.Read(data, 0, ONEMB);
                }
                var md5 = MD5.Create();
                return BitConverter.ToString((md5.ComputeHash(data)));
            }
            catch (Exception ex)
            {
                driver.logit("MD5 get failed   " + filename);
                driver.logit(ex.Message);
            }
            return "";
        }

        private string calucalatemd5fast(fileitem fi)
        {
            return String.Format("{0}{1}",fi._dateupdated, fi._size); ;
        }

        public void CalculateMD5(object data)
        {
            List<fileitem> ficlist = (List<fileitem>)data;
            while (true)
            {
                int idx = Interlocked.Increment(ref dupidx);
                if (idx >= ficlist.Count)
                    break;
                var fi = ficlist[idx];
                string temp = bfast ? calucalatemd5fast(fi) : calucalatemd5full(fi._fullPath);
                if (temp != "")
                    fi._md5 = temp;
                driver.pmon.updatepbar();
            }
            driver.pmon.closebar();

        }

        public void CalculateMD52(object data)
        {
            List<fileitem> ficlist = (List<fileitem>)data;
            while (true)
            {
                int idx = Interlocked.Increment(ref dupidx);
                if (idx >= ficlist.Count)
                    break;
                var fi = ficlist[idx];
                string temp = calucalatemd5optimized(fi._fullPath);
                if (temp != "")
                    fi._md5 = temp;
                driver.pmon.updatepbar();
            }
            driver.pmon.closebar();

        }


        public void md5threadpool2(List<fileitem> leaves)
        {
            int nthreads = 15;
            dupidx = -1;
            Thread[] tpool = new Thread[nthreads];
            for (int i = 0; i < nthreads; ++i)
            {
                tpool[i] = new Thread(CalculateMD52);
                tpool[i].Start(leaves);
            }
            for (int i = 0; i < nthreads; ++i)
            {
                tpool[i].Join();
            }
            driver.pmon.closebar();
        }

        public void md5threadpool(IEnumerable<fileitem>[] duplicates)
        {
            ficlist.Clear();

            for (int idx = 0; idx < duplicates.Length;++idx)
            {
                foreach (var fi in duplicates[idx])
                {
                    ficlist.Add(fi);
                }
            }

            int nthreads = 15;
            dupidx = -1;
            Thread[] tpool = new Thread[nthreads];
            for (int i = 0; i < nthreads; ++i)
            {
                tpool[i] = new Thread(CalculateMD5);
                tpool[i].Start(ficlist);
            }
            for (int i = 0; i < nthreads; ++i)
            {
                tpool[i].Join();
            }
            driver.pmon.closebar();

            driver.pmon.initpbar(ficlist.Count);
            foreach (fileitem fic in ficlist)
            {
                driver.pmon.updatepbar();
                if (fic.hasdup)
                    continue;

                var temp = (from ad in ficlist
                            where (fic._md5 == ad._md5)
                            select ad).ToList();
                if (temp.Count > 1)
                {
                    fic.hasdup = true;
                    foreach (fileitem fic2 in temp)
                    {
                        fic2.hasdup = true;
                    }
                }
            }
            driver.pmon.closebar();


            List<fileitem> duplist = ficlist.FindAll(fi => (fi.hasdup));
            driver.pmon.initpbar(duplist.Count);
            bfast = false;
            dupidx = -1;
            for (int i = 0; i < nthreads; ++i)
            {
                tpool[i] = new Thread(CalculateMD5);
                tpool[i].Start(duplist);
            }
            for (int i = 0; i < nthreads; ++i)
            {
                tpool[i].Join();
            }

            driver.pmon.initpbar(ficlist.Count);
            foreach (fileitem fic in ficlist)
            {
                fic.hasdup = false;
            }

            foreach (fileitem fic in ficlist)
            {
                driver.pmon.updatepbar();
                if (fic.hasdup)
                    continue;

                var temp = (from ad in ficlist
                            where (fic._md5 == ad._md5)
                            select ad).ToList();
                if (temp.Count > 1)
                {
                    fic.hasdup = true;
                    foreach (fileitem fic2 in temp)
                    {
                        fic2.hasdup = true;
                    }
                }
            }
            driver.pmon.closebar();
            duplist = ficlist.FindAll(fi => (fi.hasdup));
        }

    }
}
