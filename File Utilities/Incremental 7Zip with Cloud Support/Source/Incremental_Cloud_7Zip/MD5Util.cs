using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BackupRestoreTool
{
    public class MD5Util
    {
        object test = new object();
        public int dupidx;
        public int stidx;
        public void CalculateMD5(object data)
        {
            List<fileitem> ficlist = (List<fileitem>)((object[])data)[0];
            while (true)
            {
                int idx = Interlocked.Increment(ref dupidx);
                if (idx >= ficlist.Count)
                    break;
                var fi = ficlist[idx];
                fi._crc =  fi.calucalatemd5full();
                App.pmon.updatepbar();
            }
        }
        public void updatelave(object data)
        {
            List<fileitem> ficlist = (List<fileitem>)((object[])data)[0];
            List<fileitem> archivedlist = (List<fileitem>)((object[])data)[1]; ;
            List<BackupInfo> backups=(List<BackupInfo>)((object[])data)[2]; ;

            while (true)
            {
                int idx = Interlocked.Increment(ref dupidx);
                if (idx >= ficlist.Count)
                    break;
                var fic = ficlist[idx];
                try
                {
                    foreach (var bkup in backups)
                    {
                        //List<fileitem> ad;
                        //var temp = bkup.md2ficdic.TryGetValue(fic._crc, out ad);
                        //if (temp && bkup.md2ficdic.ContainsKey(fic._crc))
                        if (bkup.md2ficdic.Contains(fic._crc))
                        {
                            var ad = bkup.md2ficdic[fic._crc].ElementAt(0);
                            fic.duplicatefile = (string.IsNullOrEmpty(ad.duplicatefile) ? ad._fullPath : ad.duplicatefile);
                            fic.archive = ad.archive;
                            fic._status = "Archived";
                            int idx2 = Interlocked.Increment(ref stidx);
                            archivedlist[idx2] = fic;
                        }
                    }
                }
                catch (Exception ex)
                {
                    App.logit(ex.Message);
                    App.logit(fic._fullPath);
                }
                App.pmon.updatepbar();
            }
        }

        public void updatefolder(object data)
        {
            List<fileitem> archivedlist = (List<fileitem>)((object[])data)[0]; ;
            ConcurrentDictionary<string,byte> processedlist = (ConcurrentDictionary<string, byte>)((object[])data)[1]; ;
            while (true)
            {
                int idx = Interlocked.Increment(ref dupidx);
                if (idx >= archivedlist.Count)
                    break;
                var fi = archivedlist[idx];

                try
                {
                    fileitem fic = fi._parent;
                    while (fic != null)
                    {
                        if (!processedlist.ContainsKey(fic._fullPath) && (fic._status != "Archived"))
                        {
                            List<fileitem> archivedleaves = new List<fileitem>();
                            fic.getleaves(ref archivedleaves);
                            var cnt = (from fiarch in archivedleaves where fiarch._status != "Archived" select fiarch).Count();
                            fic._status = (cnt == 0) ? "Archived" : "Partial";
                            processedlist[fic._fullPath] = 1;
                        }
                        fic = fic._parent;
                    }
                }
                catch (Exception ex)
                {
                    App.logit(ex.Message);
                    App.logit(fi._fullPath);
                }
                App.pmon.updatepbar();
            }
        }

        public void threadpool(object[] data, Action<object> threadproc)
        {
            int nthreads = 4;
            dupidx = -1;
            stidx = -1;

            Thread[] tpool = new Thread[nthreads];
            for (int i = 0; i < nthreads; ++i)
            {
                tpool[i] = new Thread( new ParameterizedThreadStart(threadproc));
                tpool[i].Start(data);
            }
            for (int i = 0; i < nthreads; ++i)
            {
                tpool[i].Join();
            }
            App.pmon.closebar();
            
        }

    }
}
