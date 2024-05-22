using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SetupLayout;

namespace InstallerCreator
{
    class ConfigHelper
    {
        [DllImport("shell32.dll")]
        public static extern Int32 SHGetPathFromIDList(IntPtr pidl, StringBuilder pszPath);

        [DllImport("shell32.dll")]
        public static extern IntPtr ILCombine(IntPtr pidl1, IntPtr pidl2);

        [DllImport("shell32.dll")]
        public static extern void ILFree(IntPtr pidl);

        public List<Nodedata> GetDDSelections(IDataObject dragdropdata)
        {
            // Copy clipboard data into unmanaged memory.
            MemoryStream data = (MemoryStream)(dragdropdata).GetData("Shell IDList Array");
            byte[] b = data.ToArray();
            IntPtr p = Marshal.AllocHGlobal(b.Length);
            Marshal.Copy(b, 0, p, b.Length);

            // Get number of items.
            UInt32 cidl = (UInt32)Marshal.ReadInt32(p);

            // Get parent folder.
            int offset = sizeof(UInt32);
            IntPtr parentpidl = (IntPtr)((int)p + (UInt32)Marshal.ReadInt32(p, offset));
            StringBuilder path = new StringBuilder(256);
            SHGetPathFromIDList(parentpidl, path);

            // Get subitems.
            List<Nodedata> filestoadd = new List<Nodedata>();
            for (int i = 1; i <= cidl; ++i)
            {
                offset += sizeof(UInt32);
                IntPtr relpidl = (IntPtr)((int)p + (UInt32)Marshal.ReadInt32(p, offset));
                IntPtr abspidl = ILCombine(parentpidl, relpidl);
                SHGetPathFromIDList(abspidl, path);
                ILFree(abspidl);
                filestoadd.Add(new Nodedata(path.ToString(),true));
            }
            return filestoadd;
        }


        public List<Nodedata> AddDir(string fullpath)
        {
            List<Nodedata> newfiles = new List<Nodedata>();
            DirectoryInfo rootdi = new DirectoryInfo(fullpath);
            foreach (FileInfo fi in rootdi.GetFiles())
            {
                newfiles.Add(new Nodedata(fi.FullName, true));
            }
            
            foreach (DirectoryInfo di in rootdi.GetDirectories())
            {
                newfiles.Add(new Nodedata(di.FullName, true));
            }
            
            return newfiles;
        }
    }
}
