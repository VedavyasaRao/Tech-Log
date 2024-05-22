using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Configuration;
using System.IO.MemoryMappedFiles;
using System.Security.Cryptography;
using System.Text;

namespace MigrationHelper
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    class Crc32
    {
        static uint[] crctab = new uint[] {
          0x00000000, 0x77073096, 0xee0e612c, 0x990951ba, 0x076dc419,
          0x706af48f, 0xe963a535, 0x9e6495a3, 0x0edb8832, 0x79dcb8a4,
          0xe0d5e91e, 0x97d2d988, 0x09b64c2b, 0x7eb17cbd, 0xe7b82d07,
          0x90bf1d91, 0x1db71064, 0x6ab020f2, 0xf3b97148, 0x84be41de,
          0x1adad47d, 0x6ddde4eb, 0xf4d4b551, 0x83d385c7, 0x136c9856,
          0x646ba8c0, 0xfd62f97a, 0x8a65c9ec, 0x14015c4f, 0x63066cd9,
          0xfa0f3d63, 0x8d080df5, 0x3b6e20c8, 0x4c69105e, 0xd56041e4,
          0xa2677172, 0x3c03e4d1, 0x4b04d447, 0xd20d85fd, 0xa50ab56b,
          0x35b5a8fa, 0x42b2986c, 0xdbbbc9d6, 0xacbcf940, 0x32d86ce3,
          0x45df5c75, 0xdcd60dcf, 0xabd13d59, 0x26d930ac, 0x51de003a,
          0xc8d75180, 0xbfd06116, 0x21b4f4b5, 0x56b3c423, 0xcfba9599,
          0xb8bda50f, 0x2802b89e, 0x5f058808, 0xc60cd9b2, 0xb10be924,
          0x2f6f7c87, 0x58684c11, 0xc1611dab, 0xb6662d3d, 0x76dc4190,
          0x01db7106, 0x98d220bc, 0xefd5102a, 0x71b18589, 0x06b6b51f,
          0x9fbfe4a5, 0xe8b8d433, 0x7807c9a2, 0x0f00f934, 0x9609a88e,
          0xe10e9818, 0x7f6a0dbb, 0x086d3d2d, 0x91646c97, 0xe6635c01,
          0x6b6b51f4, 0x1c6c6162, 0x856530d8, 0xf262004e, 0x6c0695ed,
          0x1b01a57b, 0x8208f4c1, 0xf50fc457, 0x65b0d9c6, 0x12b7e950,
          0x8bbeb8ea, 0xfcb9887c, 0x62dd1ddf, 0x15da2d49, 0x8cd37cf3,
          0xfbd44c65, 0x4db26158, 0x3ab551ce, 0xa3bc0074, 0xd4bb30e2,
          0x4adfa541, 0x3dd895d7, 0xa4d1c46d, 0xd3d6f4fb, 0x4369e96a,
          0x346ed9fc, 0xad678846, 0xda60b8d0, 0x44042d73, 0x33031de5,
          0xaa0a4c5f, 0xdd0d7cc9, 0x5005713c, 0x270241aa, 0xbe0b1010,
          0xc90c2086, 0x5768b525, 0x206f85b3, 0xb966d409, 0xce61e49f,
          0x5edef90e, 0x29d9c998, 0xb0d09822, 0xc7d7a8b4, 0x59b33d17,
          0x2eb40d81, 0xb7bd5c3b, 0xc0ba6cad, 0xedb88320, 0x9abfb3b6,
          0x03b6e20c, 0x74b1d29a, 0xead54739, 0x9dd277af, 0x04db2615,
          0x73dc1683, 0xe3630b12, 0x94643b84, 0x0d6d6a3e, 0x7a6a5aa8,
          0xe40ecf0b, 0x9309ff9d, 0x0a00ae27, 0x7d079eb1, 0xf00f9344,
          0x8708a3d2, 0x1e01f268, 0x6906c2fe, 0xf762575d, 0x806567cb,
          0x196c3671, 0x6e6b06e7, 0xfed41b76, 0x89d32be0, 0x10da7a5a,
          0x67dd4acc, 0xf9b9df6f, 0x8ebeeff9, 0x17b7be43, 0x60b08ed5,
          0xd6d6a3e8, 0xa1d1937e, 0x38d8c2c4, 0x4fdff252, 0xd1bb67f1,
          0xa6bc5767, 0x3fb506dd, 0x48b2364b, 0xd80d2bda, 0xaf0a1b4c,
          0x36034af6, 0x41047a60, 0xdf60efc3, 0xa867df55, 0x316e8eef,
          0x4669be79, 0xcb61b38c, 0xbc66831a, 0x256fd2a0, 0x5268e236,
          0xcc0c7795, 0xbb0b4703, 0x220216b9, 0x5505262f, 0xc5ba3bbe,
          0xb2bd0b28, 0x2bb45a92, 0x5cb36a04, 0xc2d7ffa7, 0xb5d0cf31,
          0x2cd99e8b, 0x5bdeae1d, 0x9b64c2b0, 0xec63f226, 0x756aa39c,
          0x026d930a, 0x9c0906a9, 0xeb0e363f, 0x72076785, 0x05005713,
          0x95bf4a82, 0xe2b87a14, 0x7bb12bae, 0x0cb61b38, 0x92d28e9b,
          0xe5d5be0d, 0x7cdcefb7, 0x0bdbdf21, 0x86d3d2d4, 0xf1d4e242,
          0x68ddb3f8, 0x1fda836e, 0x81be16cd, 0xf6b9265b, 0x6fb077e1,
          0x18b74777, 0x88085ae6, 0xff0f6a70, 0x66063bca, 0x11010b5c,
          0x8f659eff, 0xf862ae69, 0x616bffd3, 0x166ccf45, 0xa00ae278,
          0xd70dd2ee, 0x4e048354, 0x3903b3c2, 0xa7672661, 0xd06016f7,
          0x4969474d, 0x3e6e77db, 0xaed16a4a, 0xd9d65adc, 0x40df0b66,
          0x37d83bf0, 0xa9bcae53, 0xdebb9ec5, 0x47b2cf7f, 0x30b5ffe9,
          0xbdbdf21c, 0xcabac28a, 0x53b39330, 0x24b4a3a6, 0xbad03605,
          0xcdd70693, 0x54de5729, 0x23d967bf, 0xb3667a2e, 0xc4614ab8,
          0x5d681b02, 0x2a6f2b94, 0xb40bbe37, 0xc30c8ea1, 0x5a05df1b,
          0x2d02ef8d
        };

        static long HUNDEREDMB = 1024 * 1024 * 200;
        static byte[] temp = new byte[HUNDEREDMB];

        public static uint CountCrc(byte[] pBuf, int len)
        {
            // Table of CRC-32's of all single byte values
            uint c = 0xffffffff;  // begin at shift register contents 
            int i, n = len;
            for (i = 0; i < n; i++)
            {
                c = crctab[((int)c ^ pBuf[i]) & 0xff] ^ (c >> 8);
            }
            return c ^ 0xffffffff;
        }

        public static uint CountCrc(string fileName, uint offset, uint filelen)
        {
            var md5 = Encoding.Unicode.GetBytes(calucalatemd5optimized(fileName));

            return CountCrc(md5, md5.Length);
        }
        private static string calucalatemd5optimized(string _fullPath)
        {
            int ONEMB = 1024 * 1024;
            int HALFMB = 512 * 1024;
            byte[] data;
            long filelen = 0;
            string hash = "";
            System.IO.FileStream fs = null;
            try
            {
                var md5 = MD5.Create();
                fs = FileEx.OpenRead(_fullPath);
                filelen = fs.Length;
                if (filelen < ONEMB)
                {
                    data = FileEx.ReadAllBytes(_fullPath);
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
                driver.logtxt.AppendText("MD5 get failed   " + _fullPath);
                driver.logtxt.AppendText(ex.Message);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }

            }
            return "Khri$ha";
        }


        public static void WriteTo(System.IO.FileStream patchdata, string fileName)
        {
            long filelen =  FileInfoEx.FileInfo(fileName).Length;
            using (var mmf = MemoryMappedFile.CreateFromFile(fileName))
            {
                for (long i = 0; i < filelen; i += HUNDEREDMB)
                {
                    int len = (int)(((filelen - i) > HUNDEREDMB) ? HUNDEREDMB : (filelen - i));
                    using (var accessor = mmf.CreateViewAccessor(i, len))
                    {
                        accessor.ReadArray<byte>(0, temp, 0, len);
                        patchdata.Write(temp, 0, len);
                    }
                }
            }
        }

        public static void WriteFrom(string patchdata, string fileName, uint offset, uint filelen)
        {
            using (System.IO.FileStream fo =  FileStreamEx.FileStream(fileName, System.IO.FileMode.Create))
            {
                using (var mmf = MemoryMappedFile.CreateFromFile(patchdata))
                {
                    for (long i = 0; i < filelen; i += HUNDEREDMB)
                    {
                        int len = (int)(((filelen - i) > HUNDEREDMB) ? HUNDEREDMB : (filelen - i));
                        using (var accessor = mmf.CreateViewAccessor(i + offset, len))
                        {
                            accessor.ReadArray<byte>(0, temp, 0, len);
                            fo.Write(temp, 0, len);
                        }
                    }
                }
            }
        }

    }


    class driver
    {
        
        struct onelistitem
        {
            public string filename;
            public string extension;
            public string manualmerge;
            public string dir;
            public string isnew;
            public string[] pparts;
        }
        
        public static TextBox logtxt;
        public static TreeView previewtree;
        public static ProgressBar pbar;
        public static ListView previewlist;
        public static string outputpath="";

        [DllImport("CompressExpand.dll")]
        static extern int Compress(byte[] Dest, ref int Dest_len, byte[] Source, int source_len);
        [DllImport("CompressExpand.dll")]
        static extern int Expand(byte[] Dest, ref int Dest_len, byte[] Source, int source_len);

        static driver()
        {
            System.Configuration.Configuration config =
                          ConfigurationManager.OpenExeConfiguration(
                          ConfigurationUserLevel.None);
            outputpath = System.IO.Path.GetDirectoryName(config.AppSettings.CurrentConfiguration.FilePath) + "\\output";
        }

        public static void CreatePatch(string parentpath, string inoutdir, string srcpath, bool baddsrc)
        {
            logtxt.Text = "";
            Application.DoEvents();
            System.Threading.Thread.Sleep(0);

            string patchmetafile = inoutdir + "\\Patch_meta.csv";
            string patchdatafile = inoutdir + "\\Patch_data.dat";
            string patchsrcdatafile = inoutdir + "\\Patch_src_data.dat";
            List<string> files = new List<string>();

            logtxt.AppendText("Setting up for patch creation\r\n");
            files.AddRange(FileEx.ReadAllLines(driver.outputpath + @"\dir_diff.csv"));
            if (FileEx.Exists(inoutdir + @"\addtional_input.txt"))
            {
                files.AddRange((from  f in FileEx.ReadAllLines(inoutdir + @"\addtional_input.txt") where !files.Contains(f) select f));
            }

            if (FileEx.Exists(patchmetafile))
                FileEx.Delete(patchmetafile);

            if (FileEx.Exists(patchdatafile))
                FileEx.Delete(patchdatafile);

            if (FileEx.Exists(patchsrcdatafile))
                FileEx.Delete(patchsrcdatafile);

            System.IO.FileStream patchsrcdata = null;
            if (baddsrc)
                patchsrcdata = FileStreamEx.FileStream(patchsrcdatafile, System.IO.FileMode.CreateNew);
            System.IO.FileStream patchdata =  FileStreamEx.FileStream(patchdatafile, System.IO.FileMode.CreateNew);
            FileEx.AppendAllText(patchmetafile, "File,sCRC,Length,Offset,dCRC\r\n");
            uint writeofst = 0;
            uint writesrcofst = 0;
            pbar.Maximum = files.Count;
            pbar.Value = 0;
            logtxt.AppendText("Adding files to patch\r\n");
            foreach (string f in files)
            {
                string sf = srcpath + "\\" + f;
                string df = parentpath + "\\" + f;
                if (!FileEx.Exists(sf))
                    continue;
                uint crcf = Crc32.CountCrc(sf, 0, (uint) FileInfoEx.FileInfo(sf).Length);
                uint sflen = (uint) FileInfoEx.FileInfo(sf).Length;

                if (FileEx.Exists(df))
                {
                    uint crcdf = Crc32.CountCrc(df, 0, (uint) FileInfoEx.FileInfo(df).Length);
                    uint dflen = (uint) FileInfoEx.FileInfo(df).Length;

                    if (crcf != crcdf)
                    {
                        Crc32.WriteTo(patchdata, sf);
                        patchdata.Flush();
                        if (baddsrc)
                        {
                            Crc32.WriteTo(patchsrcdata, df);
                            patchsrcdata.Flush();

                        }

                        FileEx.AppendAllText(patchmetafile, string.Format("{0},{1},{2},{3},{4},{5},{6}\r\n", f, crcf, sflen, writeofst, crcdf, dflen, writesrcofst));
                        writeofst += sflen;
                        writesrcofst += dflen;
                    }
                }
                else
                {
                    Crc32.WriteTo(patchdata, sf);
                    patchdata.Flush();
                    FileEx.AppendAllText(patchmetafile, string.Format("{0},{1},{2},{3},{4},{5},{6}\r\n", f, crcf, sflen, writeofst, 0, 0, 0));
                    writeofst += sflen;
                }

                if (((pbar.Value++) % 100) == 0)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(0);
                }
            }
            patchdata.Close();
            if (baddsrc)
                patchsrcdata.Close();
            pbar.Value = 0;
            logtxt.AppendText("done.\r\n");
        }

        public static void RemoveReadonly(string filename)
        {
            if (!FileEx.Exists(filename))
                return;

            System.IO.FileAttributes attributes = FileEx.GetAttributes(filename);
            if ((attributes & System.IO.FileAttributes.ReadOnly) == System.IO.FileAttributes.ReadOnly)
            {
                attributes &= ~System.IO.FileAttributes.ReadOnly;
                FileEx.SetAttributes(filename, attributes);
            }

        }

        public static void ApplyPatch(string parentpath, string inoutdir, List<string> selectedfiles, bool bow, TextBox txtfname)
        {
            bool issrc = false;
            logtxt.Text = "";
            Application.DoEvents();
            System.Threading.Thread.Sleep(0);

            string patchmetafile = inoutdir + "\\Patch_meta.csv";
            string patchdatafile = inoutdir + "\\Patch_data.dat";

            issrc = (txtfname == null && parentpath.Contains("\\right"));
            if (issrc)
                patchdatafile = inoutdir + "\\Patch_src_data.dat";

            logtxt.AppendText("Applying patch\r\n");
            string[] lines = FileEx.ReadAllLines(patchmetafile);
            pbar.Maximum = lines.Length;
            pbar.Value = 0;
            foreach (string l in lines)
            {
                string[] parts = l.Split(new char[] { ',' });
                if (parts[0] == "File" || (!selectedfiles.Contains(parts[0]) && txtfname != null))
                    continue;


                string f = parentpath + "\\" + parts[0];
                if (txtfname != null)
                    txtfname.Text = parts[0];
                Application.DoEvents();
                if (FileEx.Exists(f))
                {
                    long crcdf = Crc32.CountCrc(f, 0, (uint) FileInfoEx.FileInfo(f).Length);
                    uint len = uint.Parse(parts[2]);
                    if (uint.Parse(parts[4]) == crcdf || bow)
                    {
                        RemoveReadonly(f);
                        Crc32.WriteFrom(patchdatafile,f,uint.Parse(parts[3]),len);
                    }
                    else if (uint.Parse(parts[4]) != crcdf && (uint.Parse(parts[4]) != 0 && uint.Parse(parts[5]) != 0) && uint.Parse(parts[6]) != 0)
                    {
                        string mf = System.IO.Path.GetDirectoryName(f) + System.IO.Path.GetFileNameWithoutExtension(f) + "+modified." + System.IO.Path.GetExtension(f);
                        Crc32.WriteFrom(patchdatafile, mf, uint.Parse(parts[3]), len);
                    }
                }
                else
                {
                    uint len = uint.Parse(parts[!issrc?2:5]);
                    if ((len != 0 && issrc) || !issrc)
                    {
                        System.IO.DirectoryInfo d =  DirectoryInfoEx.DirectoryInfo(System.IO.Path.GetDirectoryName(f));
                        if (!d.Exists)
                            d.Create();
                        Crc32.WriteFrom(patchdatafile, f, uint.Parse(parts[!issrc ? 3 : 6]), len);

                    }
                }
                if (((pbar.Value++) % 100) == 0)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(0);
                }
            }
            pbar.Value = 0;
            string delfiles = inoutdir + "\\removal_input.txt";
            if (FileEx.Exists(delfiles))
            {
                lines = FileEx.ReadAllLines(delfiles);
                pbar.Maximum = lines.Length;
                pbar.Value = 0;
                foreach (string l in lines)
                {
                    string f = parentpath + "\\" + l;
                    Application.DoEvents();
                    if (FileEx.Exists(f))
                    {
                        FileEx.Delete(f);
                    }
                }
            }
            logtxt.AppendText( "Completed.\r\n");
        }

        public static bool IsLink(string shortcutFilename)
        {
            string pathOnly = System.IO.Path.GetDirectoryName(shortcutFilename);
            string filenameOnly = System.IO.Path.GetFileName(shortcutFilename);

            Shell32.Shell shell = new Shell32.Shell();
            Shell32.Folder folder = shell.NameSpace(pathOnly);
            Shell32.FolderItem folderItem = folder.ParseName(filenameOnly);
            if (folderItem != null)
            {
                return folderItem.IsLink;
            }
            return false; // not found
        }

        private static TreeNode AddNodetotree(string f)
        {
            string[] fileparts = f.Split(new char[] { '\\' });
            TreeNodeCollection trn = previewtree.Nodes;
            string path = "";
            TreeNode tr = null;
            foreach (var fp in fileparts)
            {
                path = path + ((path.Length == 0) ? "":"\\") + fp;
                TreeNode[] trs = trn.Find(path, false);
                if (trs.Length == 0)
                {
                    tr = trn.Add(path, fp);
                }
                else
                {
                    tr = trs[0];
                }
                trn = tr.Nodes;
            }
            return tr;
        }

        public static void PreviewPatchTree(string parentpath, string inoutdir , bool bsingle, string showextensions, ref string extensions, bool bfilter = true, bool bskiporincludeflag = true, string filteroptions = "")
        {
            previewtree.Nodes.Clear();
            string patchmetafile = inoutdir + "\\Patch_meta.csv";
            string patchdatafile = inoutdir + "\\Patch_data.dat";
            List<string> skipfiles = filteroptions != "" ? filteroptions.Split(';').ToList() : new List<string>();

            foreach (string l in FileEx.ReadAllLines(patchmetafile))
            {
                string[] parts = l.Split(new char[] { ',' });
                if (parts[0] == "File")
                    continue;

                int kount = skipfiles.Count(sf => l.ToLower().Contains(sf.ToLower()));
                if ((kount > 0 && bskiporincludeflag) || (kount == 0 && !bskiporincludeflag))
                    continue;

                string f = parentpath + "\\" + parts[0];
                string extn = System.IO.Path.GetExtension(f);
                if ((showextensions == "*" || showextensions.Contains(extn)))
                {
                    AddNodetotree((!bsingle ? ((parts[4] == "0") ? "New" : "Diff") : "All") +"\\"+ parts[0]).Tag = parts;
                    if (!extensions.Contains(extn))
                        extensions += extn;
                }
            }
        }

        public static void PreviewPatchList(string parentpath, string inoutdir, int sortoption, bool asc, bool bfilter = true, bool bskiporincludeflag = true, string filteroptions = "")
        {
            string patchmetafile = inoutdir + "\\Patch_meta.csv";
            string patchdatafile = inoutdir + "\\Patch_data.dat";

            List<string> skipfiles = filteroptions != "" ? filteroptions.Split(';').ToList() : new List<string>();
            List<onelistitem> litems = new List<onelistitem>();
            foreach (string l in FileEx.ReadAllLines(patchmetafile))
            {
                string[] parts = l.Split(new char[] { ',' });
                if (parts[0] == "File")
                    continue;
                string f = parentpath + "\\" + parts[0];
                string isnew = "";
                onelistitem oli;
                string extn = System.IO.Path.GetExtension(f);
                if (FileEx.Exists(f))
                {
                    uint fcrc = Crc32.CountCrc(f, 0, (uint) FileInfoEx.FileInfo(f).Length);
                    isnew = (fcrc == uint.Parse(parts[1])) ? "Same" : "Diff";
                    oli = new onelistitem { filename = System.IO.Path.GetFileName(f), extension = System.IO.Path.GetExtension(f), manualmerge = (fcrc == uint.Parse(parts[4])).ToString(), dir = System.IO.Path.GetDirectoryName(f), isnew = isnew, pparts = parts };
                }
                else
                {
                    isnew = "New";
                    oli = new onelistitem { filename = f, extension = System.IO.Path.GetExtension(f), manualmerge = false.ToString(), dir = System.IO.Path.GetDirectoryName(f), isnew=isnew, pparts = parts };
                }
                
                if (bfilter)
                {
                    if (filteroptions =="Diff" || filteroptions =="Same" || filteroptions=="New") 
                    {
                        if (filteroptions !=isnew)
                            continue;
                    }
                    else
                    {
                        int kount = skipfiles.Count(sf => l.ToLower().Contains(sf.ToLower()));
                        if ((kount > 0 && bskiporincludeflag) || (kount == 0 && !bskiporincludeflag))
                            continue;
                    }
                }
                litems.Add(oli);

            }

            List<onelistitem> newlitems = litems;
            if (sortoption == 0 && asc)
                newlitems = (from li in litems orderby li.filename ascending select li).ToList();
            else if (sortoption == 0 && !asc)
                newlitems = (from li in litems orderby li.filename descending select li).ToList();
            else if (sortoption == 1 && asc)
                newlitems = (from li in litems orderby li.extension ascending select li).ToList();
            else if (sortoption == 1 && !asc)
                newlitems = (from li in litems orderby li.extension descending select li).ToList();
            else if (sortoption == 2 && asc)
                newlitems = (from li in litems orderby li.manualmerge ascending select li).ToList();
            else if (sortoption == 2 && !asc)
                newlitems = (from li in litems orderby li.manualmerge descending select li).ToList();
            else if (sortoption == 3 && asc)
                newlitems = (from li in litems orderby li.dir ascending select li).ToList();
            else if (sortoption == 3 && !asc)
                newlitems = (from li in litems orderby li.dir descending select li).ToList();
            else if (sortoption == 4 && asc)
                newlitems = (from li in litems orderby li.isnew ascending select li).ToList();
            else if (sortoption == 4 && !asc)
                newlitems = (from li in litems orderby li.isnew descending select li).ToList();

            previewlist.Items.Clear();
            foreach (var li in newlitems)
            {
                ListViewItem lvi = new ListViewItem(new string[]{li.filename , li.extension , li.manualmerge , li.dir, li.isnew});
                lvi.Tag = li.pparts;
                previewlist.Items.Add(lvi);
            }

        }
        
        public static void CreatePatchFile(string inoutdir, string[] parts)
        {
            string patchmetafile = inoutdir + "\\Patch_meta.csv";
            string patchdatafile = inoutdir + "\\Patch_data.dat";
            string patchsrcdatafile = inoutdir + "\\Patch_src_data.dat";

            if (!DirectoryEx.Exists(driver.outputpath + "\\ld"))
                DirectoryEx.CreateDirectory(driver.outputpath + "\\ld");
            
            if (!DirectoryEx.Exists(driver.outputpath + "\\rd"))
                DirectoryEx.CreateDirectory(driver.outputpath + "\\rd");

            string filename = driver.outputpath + @"\ld\"+ System.IO.Path.GetFileName(parts[0]);
            string srcfilename = driver.outputpath + @"\rd\" + System.IO.Path.GetFileName(parts[0]);

            if (FileEx.Exists(filename))
                FileEx.Delete(filename);

            if (FileEx.Exists(srcfilename))
                FileEx.Delete(srcfilename);


            uint len = uint.Parse(parts[2]);
            Crc32.WriteFrom(patchdatafile, filename, uint.Parse(parts[3]), len);

            if (FileEx.Exists(patchsrcdatafile))
            {
                len = uint.Parse(parts[5]);
                if (len == 0)
                    return;
                Crc32.WriteFrom(patchsrcdatafile, srcfilename, uint.Parse(parts[6]), len);
            }
        }
        public static bool isfile(string apath)
        {
            System.IO.FileAttributes attr = FileEx.GetAttributes(apath);

            return !(attr.HasFlag(System.IO.FileAttributes.Directory));
        }

        public static void Comparefolders(string leftdir, string rightdir, bool bcreatepatch)
        {
            logtxt.AppendText(String.Format("Fetching files from  folders: {0} and {1}\r\n",leftdir, rightdir));
            string filename = driver.outputpath + @"\dir_diff.csv";
            if (FileEx.Exists(filename))
                FileEx.Delete(filename);

            List<string> skipfiles = ConfigurationManager.AppSettings["exclude"].Split(';').ToList();
            FileEx.AppendAllText(filename, "File,remark\r\n");

            var ldir =  leftdir;
            var rdir = rightdir;

            logtxt.AppendText(String.Format("Applying filters\r\n"));
            List<string> srcfils, dstfils;
            if (isfile(ldir) && isfile(rdir))
            {
                var cmd = ConfigurationManager.AppSettings["filediffexe"];
                var args = ConfigurationManager.AppSettings["filediffargs"];
                args = args.Replace("<file 1>", leftdir);
                args = args.Replace("<file 2>", rightdir);

                Process p = Process.Start(cmd, args);
                p.WaitForExit();
                return;
            }
            else if (isfile(ldir) || isfile(rdir))
            {
                return;
            }
            var tempfiles = DirectoryEx.GetFiles(ldir, "*.*", System.IO.SearchOption.AllDirectories);
            srcfils = (from f in tempfiles where (skipfiles.Count(sf => f.ToLower().Contains(sf.ToLower())) == 0) select f.Substring(leftdir.Length + 1)).ToList();

            tempfiles = DirectoryEx.GetFiles(rdir, "*.*", System.IO.SearchOption.AllDirectories);
            dstfils = (from f in tempfiles where (skipfiles.Count(sf => f.ToLower().Contains(sf.ToLower())) == 0) select f.Substring(rightdir.Length + 1)).ToList();

            var samefiles = srcfils.Intersect(dstfils, StringComparer.OrdinalIgnoreCase).ToList();
            var srcunqfiles = srcfils.Except(samefiles, StringComparer.OrdinalIgnoreCase).ToList();
            var dstunqfiles = dstfils.Except(samefiles, StringComparer.OrdinalIgnoreCase).ToList();

            pbar.Maximum = samefiles.Count;
            pbar.Value = 0;
            logtxt.AppendText(String.Format("Comparing files\r\n"));
            foreach (string f in samefiles)
            {
                string fileleft = ldir + "\\" + f;
                string fileright = rdir + "\\" + f;
                var lfino =  FileInfoEx.FileInfo( fileleft);
                var rfinfo =  FileInfoEx.FileInfo(fileright);

                if ((Crc32.CountCrc(fileleft, 0, (uint)lfino.Length)) != Crc32.CountCrc(fileright, 0, (uint)rfinfo.Length))
                {
                    if (bcreatepatch)
                        FileEx.AppendAllText(filename, string.Format("{0}\r\n", f));
                    else
                        FileEx.AppendAllText(filename, string.Format("{0},{1}\r\n", f, "Diff"));

                }
                else
                {
                    if (!bcreatepatch)
                        FileEx.AppendAllText(filename, string.Format("{0},{1}\r\n", f, "Same"));
                }
                pbar.Value++;
                if (pbar.Value % 100 == 0)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(0);
                }
            }
            pbar.Value = 0;
            if (bcreatepatch)
                return;
            logtxt.AppendText(String.Format("Adding left unique files\r\n"));
            pbar.Maximum = srcunqfiles.Count;
            pbar.Value = 0;
            foreach (string f in srcunqfiles)
            {
                FileEx.AppendAllText(filename, string.Format("{0},{1}\r\n", f, "Left"));
                //logtxt.AppendText(f + "\r\n");
                if (pbar.Value % 100 == 0)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(0);
                }
                pbar.Value++;
            }

            logtxt.AppendText(String.Format("Adding right unique files\r\n"));
            pbar.Maximum = dstunqfiles.Count;
            pbar.Value = 0;
            foreach (string f in dstunqfiles)
            {
                FileEx.AppendAllText(filename, string.Format("{0},{1}\r\n", f, "Right"));
                //logtxt.AppendText(f + "\r\n");
                if (pbar.Value % 100 == 0)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(0);
                }
                pbar.Value++;
            }
            pbar.Value = 0;

        }

        public static void SortComparefiles(int sortoption, bool asc,bool bfilter=true, bool bskiporincludeflag = true, string filteroptions="")
        {
            try
            {
                List<string> skipfiles = filteroptions != ""?filteroptions.Split(';').ToList():new List<string>(){ "Same" };
                string difffile = driver.outputpath + "\\dir_diff.csv";
                List<onelistitem> litems = new List<onelistitem>();

                string[] lines = FileEx.ReadAllLines(difffile);
                pbar.Maximum = lines.Length;
                pbar.Value = 0;
                foreach (string l in lines)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    string[] parts = new string[2];
                    parts[0] = l.Substring(0, l.LastIndexOf(','));
                    parts[1] = l.Substring(l.LastIndexOf(',') + 1);
                    if (parts[0] == "File")
                        continue;
                    string f = parts[0];
                    string extn = System.IO.Path.GetExtension(f);
                    if (bfilter)
                    {
                        int kount = skipfiles.Count(sf => l.ToLower().Contains(sf.ToLower()));
                        if ((kount > 0 && bskiporincludeflag) || (kount == 0 && !bskiporincludeflag))
                            continue;
                    }
                    onelistitem oli = new onelistitem { filename = System.IO.Path.GetFileName(f), extension = System.IO.Path.GetExtension(f), manualmerge = parts[1], dir = System.IO.Path.GetDirectoryName(f), pparts = parts };
                    litems.Add(oli);
                    if (pbar.Value % 100 == 0)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(0);
                    }

                }

                List<onelistitem> newlitems = litems;
                if (sortoption == 0 && asc)
                    newlitems = (from li in litems orderby li.filename ascending select li).ToList();
                else if (sortoption == 0 && !asc)
                    newlitems = (from li in litems orderby li.filename descending select li).ToList();
                else if (sortoption == 1 && asc)
                    newlitems = (from li in litems orderby li.extension ascending select li).ToList();
                else if (sortoption == 1 && !asc)
                    newlitems = (from li in litems orderby li.extension descending select li).ToList();
                else if (sortoption == 2 && asc)
                    newlitems = (from li in litems orderby li.manualmerge ascending select li).ToList();
                else if (sortoption == 2 && !asc)
                    newlitems = (from li in litems orderby li.manualmerge descending select li).ToList();
                else if (sortoption == 3 && asc)
                    newlitems = (from li in litems orderby li.dir ascending select li).ToList();
                else if (sortoption == 3 && !asc)
                    newlitems = (from li in litems orderby li.dir descending select li).ToList();

                previewlist.Items.Clear();
                foreach (var li in newlitems)
                {
                    ListViewItem lvi = new ListViewItem(new string[] { li.filename, li.extension, li.manualmerge, li.dir });
                    lvi.Tag = li.pparts;
                    previewlist.Items.Add(lvi);
                }
            }
            catch 
            {

            }
        }

        public static void PreviewPatchTree(string showextensions, ref string extensions, bool bsingle, bool bfilter = true, bool bskiporincludeflag = true, string filteroptions = "")
        {
            try
            {
                previewtree.Nodes.Clear();
                List<string> skipfiles = filteroptions != "" ? filteroptions.Split(';').ToList() : new List<string>();
                string difffile = driver.outputpath + "\\dir_diff.csv";
                List<onelistitem> litems = new List<onelistitem>();

                string[] lines = FileEx.ReadAllLines(difffile);
                pbar.Maximum = lines.Length;
                pbar.Value = 0;
                foreach (string l in lines)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    string[] parts = new string[2];
                    parts[0] = l.Substring(0, l.LastIndexOf(','));
                    parts[1] = l.Substring(l.LastIndexOf(',') + 1);
                    if (parts[0] == "File")
                        continue;
                    string f = parts[0];
                    string extn = System.IO.Path.GetExtension(f);
                    if (bfilter)
                    {
                        int kount = skipfiles.Count(sf => l.ToLower().Contains(sf.ToLower()));
                        if ((kount > 0 && bskiporincludeflag) || (kount == 0 && !bskiporincludeflag))
                            continue;
                    }

                    if ((showextensions == "*" || showextensions.Contains(extn)))
                    {
                        AddNodetotree((bsingle?"All":parts[1]) + "\\" + f).Tag = parts;
                        if (!extensions.Contains(extn))
                            extensions += extn;
                    }

                    if (pbar.Value % 100 == 0)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(0);
                    }

                }

            }
            catch 
            {

            }
        }

        public static void ExtractFiles(string inoutdir)
        {
            string parentpath = inoutdir +"\\left";
            if (DirectoryEx.Exists(parentpath))
                DirectoryEx.Delete(parentpath,true);
            ApplyPatch(parentpath, inoutdir, new List<string>(), false, null);

            if (FileEx.Exists(inoutdir + "\\Patch_src_data.dat"))
            {
                parentpath = inoutdir + "\\right";
                if (DirectoryEx.Exists(parentpath))
                    DirectoryEx.Delete(parentpath, true);

                ApplyPatch(parentpath, inoutdir, new List<string>(), false, null);
            }
        }


    }
}
