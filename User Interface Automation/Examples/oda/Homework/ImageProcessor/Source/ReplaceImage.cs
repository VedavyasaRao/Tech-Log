using ImageHandler;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ImageHandler
{
    class ReplaceImage
    {
        string whiteimgbaselinedir = Program.ImageProcessorloc + @"\bin\ImageComparer\baseline\OptionA";
        string whiteimgdir = Program.ImageProcessorloc + @"\bin\ImageComparer\white";
        string questionfile = "";
        string tempdir = "";
        string matchedcolredimgfile = "";
        string matchedtempimgfile = "";


        public void CreateImages(string imgdir)
        {
            var files = Directory.GetFiles(imgdir, "??_Question.png").OrderBy(f => f);
            foreach (var f in files)
            {
                questionfile = f;
                tempdir = imgdir +  "\\temp\\" + Path.GetFileNameWithoutExtension(questionfile);
                if (Directory.Exists(tempdir))
                    Directory.Delete(tempdir, true);
                Directory.CreateDirectory(tempdir);
                var lines = File.ReadAllLines(Path.GetDirectoryName(questionfile) + "\\output.txt");
                var hts = lines.Where(l => l.StartsWith(questionfile)).ToList();
                Createsubimages(hts[0]);
            }
        }

        bool  replaceimage(string colredimgfile, string[] loc)
        {
            int imgwdht = 35;
            string tempbmp = tempdir + "\\temp.png";
            Brush b = new SolidBrush(Color.FromArgb(255, 255, 255));

            using (Bitmap dstBmp = new Bitmap(questionfile))
            {
                using (Graphics g = Graphics.FromImage(dstBmp))
                {

                    using (Bitmap srcbmp = new Bitmap(colredimgfile))
                    {
                        Rectangle srcRect = new Rectangle(0, 0, srcbmp.Width, srcbmp.Height);
                        Rectangle dstRect = new Rectangle(int.Parse(loc[1]), int.Parse(loc[0]), imgwdht, imgwdht);
                        g.FillRectangle(b, dstRect);
                        dstRect = new Rectangle(dstRect.X, int.Parse(loc[0]), srcbmp.Width, srcbmp.Height);
                        g.DrawImage(srcbmp, dstRect, srcRect, GraphicsUnit.Pixel);
                    }
                }
                dstBmp.Save(tempbmp);
            }
            File.Delete(questionfile);
            File.Move(tempbmp, questionfile);
            return true;
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


        bool matchoption(string optionAFile,string arg)
        {
            var tempfiles = Directory.GetFiles(tempdir, "*.png").OrderBy(f => f);
            var imgcmp = new ImageCompare();
            matchedcolredimgfile = "";
            matchedtempimgfile = "";
            foreach (var tf in tempfiles)
            {
                if (imgcmp.Process(optionAFile, tf,"", arg))
                {
                    Console.WriteLine("{0}  -> {1}", optionAFile, tf);
                    matchedcolredimgfile = optionAFile;
                    matchedtempimgfile = tf;
                    return true;
                }
            }
            return false;
        }

        public void Createsubimages(string hts)
        {
            var parts = hts.Split(new char[] { ' ' }).ToList();
            parts.RemoveRange(0, 1);
            parts.RemoveRange(parts.Count - 1, 1);
            int imgwdht = 35;
            int minimght = 30;
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
                    if (ht > imgwdht*2)
                        rows.Add(y + ht - imgwdht);

                    foreach (var row in rows)
                    {
                        Rectangle dstRect;
                        if (row == y)
                            dstRect = getboundingrectangle(srcbmp, row, imgwdht, imgwdht);
                        else
                            dstRect = getboundingrectangle(srcbmp, row, srcbmp.Width, imgwdht);
                        //if (dstRect.X >= srcbmp.Width || dstRect.Width == 0 || dstRect.Height < minimght)
                        if (dstRect.Width  > imgwdht || dstRect.Height < minimght)
                            continue;
                        using (var dstBmp = (Bitmap)srcbmp.Clone(dstRect, srcbmp.PixelFormat))
                        {
                            dstBmp.Save(string.Format("{0}\\{1}_{2}x{3}.png", tempdir, (++i).ToString("00"), row, dstRect.X));
                        }
                    }
                }
            }
        }
        public void ProcessOne(string f, string pcnt)
        {
            string args = "1.0";

            if (!string.IsNullOrEmpty(pcnt))
                args = pcnt;

            questionfile = f;
            tempdir = Path.GetDirectoryName(questionfile) + "\\temp";
            if (Directory.Exists(tempdir))
                Directory.Delete(tempdir, true);
            Directory.CreateDirectory(tempdir);
            var lines = File.ReadAllLines(Path.GetDirectoryName(questionfile) + "\\output.txt");
            var hts = lines.Where(l => l.StartsWith(questionfile)).ToList();
            Createsubimages(hts[0]);
            var whitebaselinefiles = Directory.GetFiles(whiteimgbaselinedir, "*.png").OrderBy(wf => wf).ToArray();
            foreach (var optA in whitebaselinefiles)
            {
                matchedtempimgfile = "";
                if (matchoption(optA, args))
                    break;
            }

            if (!string.IsNullOrEmpty(matchedtempimgfile))
            {
                var whiteimgfiles = Directory.GetFiles(whiteimgdir, "*.png").OrderBy(cf=> cf).ToArray();
                var tempfiles = Directory.GetFiles(tempdir, "*.png").OrderBy(tf => tf).ToList();

                int i = tempfiles.IndexOf(matchedtempimgfile);

                for (int j=0; i < tempfiles.Count; ++i)
                {
                    var matchfile = Path.GetFileNameWithoutExtension(tempfiles[i]);
                    if (replaceimage(whiteimgfiles[j], matchfile.Substring(3).Split(new char[] { 'x' })))
                        j++;
                }
            }

            if (Directory.Exists(tempdir))
                Directory.Delete(tempdir, true);
        }

        public void Process(string rootdir, string dir, string qno, string pcnt)
        {
            string[] pdirlst;
            if (string.IsNullOrEmpty(dir))
                pdirlst = Directory.GetDirectories(rootdir, "???").OrderBy(f => f).ToArray();
            else
                pdirlst = Directory.GetDirectories(rootdir, "???").Where(f=>f.Contains(dir)).OrderBy(f => f).ToArray();

            foreach (var pdir in pdirlst)
            {
                string[] files;
                if (string.IsNullOrEmpty(qno))
                    files = Directory.GetFiles(pdir, "??_Question.png").OrderBy(f => f).ToArray();
                else
                    files = Directory.GetFiles(pdir, "??_Question.png").Where(f => f.Contains(qno)).OrderBy(f => f).ToArray();

                Console.WriteLine("Processing {0}", pdir);
                foreach (var f in files)
                {
                    Console.WriteLine("Processing {0}", f);
                    ProcessOne(f,pcnt);
                }
            }
        }
    }
}
