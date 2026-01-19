using ImageHandler;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ImageHandler
{
    class ReplaceImage
    {
        string greenimgdir = Program.ImageProcessorloc + @"\baseline\green";
        string whiteimgdir = Program.ImageProcessorloc + @"\baseline\white";
        string redimgdir = Program.ImageProcessorloc + @"\baseline\red";
        string questionfile = "";
        string tempdir = "";
        string matchedcolredimgfile = "";
        string matchedtempimgfile = "";

        void replaceimage(string colredimgfile, string[] loc)
        {
            string tempbmp = tempdir + "\\temp.png";
            Brush b = new SolidBrush(Color.FromArgb(255,255,255));

            using (Bitmap dstBmp = new Bitmap(questionfile))
            {
                using (Graphics g = Graphics.FromImage(dstBmp))
                {

                    using (Bitmap srcbmp = new Bitmap(colredimgfile))
                    {
                        Rectangle srcRect = new Rectangle(0, 0, 60, srcbmp.Height);
                        Rectangle dstRect = new Rectangle(int.Parse(loc[1]), int.Parse(loc[0]), 60, 60);
                        g.FillRectangle(b, dstRect);
                        dstRect = new Rectangle(dstRect.X, int.Parse(loc[0]), 60, srcbmp.Height);
                        g.DrawImage(srcbmp, dstRect, srcRect, GraphicsUnit.Pixel);
                    }
                }
                dstBmp.Save(tempbmp);
            }
            File.Delete(questionfile);
            File.Move(tempbmp, questionfile);
        }


        public unsafe Rectangle getboundingrectangle(Bitmap bmp, int y, int wd, int ht)
        {
            Rectangle rect = new Rectangle(0, y, bmp.Width, ht);
            System.Drawing.Imaging.BitmapData bmpData =
                bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);


            byte bitsPerPixel = 32;

            int i = 0, j = 0;

            int topx = bmpData.Width, topy = bmpData.Height;
            int botx = 0, boty = 0;

            byte* scan0 = (byte*)bmpData.Scan0.ToPointer();

            //for (i = 0; i < ht; ++i)
            //{
            //    for (j = 0; j < wd; ++j)
            //    {
            //        byte* data = scan0 + i * bmpData.Stride + j * bitsPerPixel / 8;
            //        var l = string.Format("{0}, {1}: {2} {3} {4} {5}\n", i, j, *data, *(data+1), *(data+2), *(data+3));
            //        File.AppendAllText(@"d:\temp\dump.txt", l);
            //    }
            //}

            bool btopdone = false;
            bool bfound = false;
            for (i = 0; i < ht; ++i)
            {
                bfound = false;
                for (j = 0; j < wd; ++j)
                {
                    byte* data = scan0 + i * bmpData.Stride + j * bitsPerPixel / 8;
                    if (*data !=255 || data[1] != 255 || data[2] != 255)
                    {
                        btopdone = true;
                        bfound = true;
                        if (j < topx)
                            topx = j;

                        if (i < topy)
                            topy = i;

                        if (i > boty)
                            boty = i;
                    }
                }

                if (btopdone && !bfound) 
                    break;
            }

            for (j = topx; j < bmpData.Width; ++j)
            {
                bfound = false;
                for (i = topy; i < bmpData.Height; ++i)
                {
                    byte* data = scan0 + i * bmpData.Stride + j * bitsPerPixel / 8;
                    if (*data != 255 || data[1] != 255 || data[2] != 255)
                    {
                        bfound = true;
                        if (j > botx)
                            botx = j;
                    }
                }

                if (!bfound & j >= wd )
                    break;
            }


            // Unlock the bits.
            bmp.UnlockBits(bmpData);
            return new Rectangle(topx, topy+y, botx - topx, boty - topy);

        }


        bool matchoption(string coloredimgdir,string arg)
        {
            var coloredfiles = Directory.GetFiles(coloredimgdir, "*.png").OrderBy(f => f);
            var tempfiles = Directory.GetFiles(tempdir, "*.png").OrderByDescending(f => f);
            var imgcmp = new ImageCompare();
            matchedcolredimgfile = "";
            matchedtempimgfile = "";
            foreach (var gf in coloredfiles)
            {
                foreach (var tf in tempfiles)
                {
                    if (imgcmp.Process(gf, tf,"", arg))
                    {
                        Console.WriteLine("{0}  -> {1}", gf, tf);
                        matchedcolredimgfile = gf;
                        matchedtempimgfile = tf;
                        return true;
                    }
                }
            }
            return false;
        }

        public void Createsubimages(string hts)
        {
            var parts = hts.Split(new char[] { ' ' }).ToList();
            parts.RemoveRange(0, 1);
            parts.RemoveRange(parts.Count - 1, 1);
            using (Bitmap srcbmp = new Bitmap(questionfile))
            {
                int i = 0;
                foreach (var part in parts)
                {
                    var subparts = part.Split(new char[] { ',' }).ToList();
                    var y = int.Parse(subparts[0]);
                    var ht = int.Parse(subparts[1]);
                    if (ht < 28)
                        continue;

                    var rows = new List<int>();
                    rows.Add(y);
                    if (ht > 110)
                        rows.Add(y + ht - 55);

                    foreach (var row in rows)
                    {
                        Rectangle dstRect;
                        if (row == y)
                            dstRect = getboundingrectangle(srcbmp, row,55,55);
                        else
                            dstRect = getboundingrectangle(srcbmp, row, srcbmp.Width, 55);
                        if (dstRect.X >= srcbmp.Width || dstRect.Width == 0 || dstRect.Height == 0)
                            continue;
                        using (var dstBmp = (Bitmap)srcbmp.Clone(dstRect, srcbmp.PixelFormat))
                        {
                            dstBmp.Save(string.Format("{0}\\{1}_{2}x{3}.png", tempdir, (++i).ToString("00"), row, dstRect.X));
                        }
                    }
                }
            }
        }
        public void ProcessOne(string f)
        {
            string[] args = { "0.75 17 15 20 24", "0.85 17 15 20 24" };

            questionfile = f;
            tempdir = Path.GetDirectoryName(questionfile) + "\\temp";
            if (Directory.Exists(tempdir))
                Directory.Delete(tempdir, true);
            Directory.CreateDirectory(tempdir);
            var lines = File.ReadAllLines(Path.GetDirectoryName(questionfile) + "\\output.txt");
            var hts = lines.Where(l => l.StartsWith(questionfile)).ToList();
            Createsubimages(hts[0]);
            int i = 0;
            foreach (var dir in new string[] { greenimgdir, redimgdir })
            {
                if (matchoption(dir,args[i++]))
                {
                    var matchfile = Path.GetFileNameWithoutExtension(matchedtempimgfile);
                    replaceimage(Path.Combine(whiteimgdir, Path.GetFileName(matchedcolredimgfile)), matchfile.Substring(3).Split(new char[] { 'x' }));
                }
            }
            if (Directory.Exists(tempdir))
                Directory.Delete(tempdir, true);
        }

        public void Process(string rootdir)
        {
            var pdirlst = Directory.GetDirectories(rootdir, "???").OrderBy(f => f);
            foreach (var pdir in pdirlst)
            {
                var files = Directory.GetFiles(pdir, "??_Question.png").OrderBy(f => f);
                Console.WriteLine("Processing {0}", pdir);
                foreach (var f in files)
                {
                    Console.WriteLine("Processing {0}", f);
                    ProcessOne(f);
                }
            }
        }
    }
}
