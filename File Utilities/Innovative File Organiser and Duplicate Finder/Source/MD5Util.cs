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
        public MD5Util(bool bfast)
        {
            this.bfast = bfast;
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

        private string calucalatemd5optimized(string fname)
        {
            int ONEMB = 1024 * 1024;
            int HALFMB = 512 * 1024;
            byte[] data;
            long filelen = 0;
            string hash = "";
            string filename = driver.sourcedir + fname;
            System.IO.FileStream fs = null;
            try
            {
                var md5 = MD5.Create();
                fs = FileEx.OpenRead(filename);
                filelen = fs.Length;
                if (filelen < ONEMB)
                {
                    data = FileEx.ReadAllBytes(filename);
                    hash = BitConverter.ToString((md5.ComputeHash(data)));
                }
                else
                {
                    data = new byte[HALFMB];
                    fs.Seek(0, System.IO.SeekOrigin.Begin);
                    fs.Read(data, 0, HALFMB);
                    hash = BitConverter.ToString((md5.ComputeHash(data)));
                    fs.Seek(-HALFMB, System.IO.SeekOrigin.End);
                    fs.Read(data, 0, HALFMB);
                    hash += BitConverter.ToString((md5.ComputeHash(data)));
                }
                return hash;
            }
            catch (Exception ex)
            {
                driver.logit("MD5 get failed   " + filename);
                driver.logit(ex.Message);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }

            }
            return "";
        }


        private string calucalatemd5fast(fileitem fi)
        {
            return String.Format("{0}*{1}", fi._dateupdated, fi._size); ;
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
                string temp = bfast ? calucalatemd5fast(fi) : calucalatemd5optimized(fi._fullPath);
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


    }
}