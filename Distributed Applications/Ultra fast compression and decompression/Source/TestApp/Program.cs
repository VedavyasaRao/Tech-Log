using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TestApp
{
    class Program
    {
        [DllImport("CompressExpand.dll")]
        [return: MarshalAs(UnmanagedType.I1)]
        extern static bool Initialize([MarshalAs(UnmanagedType.I1)]bool bfast, ref short token);

        [DllImport("CompressExpand.dll")]
        [return: MarshalAs(UnmanagedType.I1)]
        extern static bool Compress(short token, IntPtr pDest, ref UInt32 pDest_len, IntPtr pSource, UInt32 source_len);

        [DllImport("CompressExpand.dll")]
        [return: MarshalAs(UnmanagedType.I1)]
        extern static bool Expand(short token, IntPtr pDest, ref UInt32 pDest_len, IntPtr pSource, UInt32 source_len);

        
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Syntax: TestApp <filename>");
                return;
            }
            string filename = args[0];
            if (!File.Exists(filename))
            {
                Console.WriteLine("bad file");
                return;
            }

            for (int i = 0; i < 2; ++i)
            {

                //init
                short token = 0;
                bool bret = Initialize((i==0), ref token);

                byte[] sourcestream = File.ReadAllBytes(filename);
                UInt32 source_len = (UInt32)sourcestream.Length;
                UInt32 dest_len = source_len;
                byte[] deststream = new byte[dest_len];
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch(); ;
                //compress
                unsafe
                {
                    fixed (byte* sourceptr = sourcestream)
                    {
                        fixed (byte* destptr = deststream)
                        {
                            sw.Start();
                            bret = Compress(token, new IntPtr(destptr), ref dest_len, new IntPtr(sourceptr), source_len);
                            sw.Stop();
                        }
                    }
                }

                Console.WriteLine("\nCompression [{3}]\nuncompressed size:\t{0}\tcompressed size:\t{1}\ttime in ms:{2}\n", source_len, dest_len, sw.ElapsedMilliseconds, (i == 0) ? "fast" : "slow");

                unsafe
                {
                    fixed (byte* sourceptr = sourcestream)
                    {
                        fixed (byte* destptr = deststream)
                        {
                            sw.Restart();
                            bret = Expand(token, new IntPtr(sourceptr), ref source_len, new IntPtr(destptr), dest_len);
                            sw.Stop();
                        }
                    }
                }

                Console.WriteLine("Decompression [{3}]\ncompressed size:\t{0}\tuncompressed size:\t{1}\ttime in ms:{2}\n", dest_len, source_len, sw.ElapsedMilliseconds, (i == 0) ? "fast" : "slow");
                System.Threading.Thread.Sleep(5000);

            }

        }
    }
}
