using ImageHandler;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace ImageHandler
{
    class MergeQuestionAnswers
    {
        string args = "1.0";
        int skipcount;

        public void getboundingrectangle(string srcbmpfile, string dstbmpfile, string args)
        {
            var parts = args.Split(',');            
            int stht = int.Parse(parts[0]), imgwd = int.Parse(parts[1]), imght = int.Parse(parts[2]);

            var rp = new ReplaceImage();
            using (Bitmap srcbmp = new Bitmap(srcbmpfile))
            {
                Rectangle dstRect = rp.getboundingrectangle(srcbmp, stht, imgwd, imght);
                var tempbmp = (Bitmap)srcbmp.Clone(dstRect, srcbmp.PixelFormat);
                tempbmp.Save(dstbmpfile);
            }
        }

        public void MergeAnswer(string imgdir, string filename)
        {
            skipcount = -1;
            var files = Directory.GetFiles(imgdir, "??.png").OrderBy(f => f);
            if (files.Count() == 0)
                return;
            var imgcmp = new ImageCompare();
            int skipknt = 0;
            int bigwd = 0;
            int bight = 0;
            var rp = new ReplaceImage();
            int minht = 48;
            int stht = 30, imght = 16, imgwd=80,minimght=10;

            string tempdir = Path.GetTempPath() + "imageprocessor";
            if (Directory.Exists(tempdir))
                Directory.Delete(tempdir, true);
            Directory.CreateDirectory(tempdir);

            string tempimgfile = tempdir + @"\MyAnswertemp.png";


            foreach (string f in files)
            {
                ++skipknt;
                using (Bitmap srcbmp = new Bitmap(f))
                {
                    if (srcbmp.Height < minht)
                        continue;
                    Rectangle dstRect = rp.getboundingrectangle(srcbmp, stht, imgwd, imght);
                    if (dstRect.Width < imgwd || dstRect.Height < minimght)
                        continue;
                    var tempbmp = (Bitmap)srcbmp.Clone(dstRect, srcbmp.PixelFormat);
                    tempbmp.Save(tempimgfile);
                    var myanswerimgfiles = Directory.GetFiles(Program.ImageProcessorloc + @"\ImageComparer\baseline\MyAnswer").OrderBy(af => af).ToArray();
                    bool bfound = false;
                    foreach (var aimgf in myanswerimgfiles)
                    {
                        if (imgcmp.Process(aimgf, tempimgfile, "", args))
                        {
                            bfound = true;
                            break;
                        }
                    }
                    if (bfound)
                        break;
                }
            }
            if (File.Exists(tempimgfile))
                File.Delete(tempimgfile);

            if (skipknt >= files.Count())
                return;

            skipknt--;
            skipcount = skipknt;

            int i = 0;
            foreach (string f in files)
            {
                if (i++ < skipknt)
                    continue;

                using (Bitmap smallBmp = new Bitmap(f))
                {
                    if (smallBmp.Width > bigwd)
                        bigwd = smallBmp.Width;
                    bight += (smallBmp.Height + 20);
                }
            }

            Bitmap bigBmp = new Bitmap(bigwd, bight);
            Graphics g = Graphics.FromImage(bigBmp);
            g.Clear(Color.White);
            int ht = 0;
            i = 0;
            foreach (string f in files)
            {
                if (i++ < skipknt)
                    continue;
                using (Bitmap smallBmp = new Bitmap(f))
                {
                    Rectangle smallRect = new Rectangle(0, 0, smallBmp.Width, smallBmp.Height);
                    Rectangle bigRect = new Rectangle(0, ht, smallBmp.Width, smallBmp.Height);
                    g.DrawImage(smallBmp, bigRect, smallRect, GraphicsUnit.Pixel);
                    ht += (smallBmp.Height + 20);
                }
            }
            bigBmp.Save(filename);
        }

        public void MergeQuestions(string imgdir, string filename)
        {
            var files = Directory.GetFiles(imgdir, "??.png").OrderBy(f => f).ToList();
            if (files.Count() == 0)
                return;
            files.RemoveRange(skipcount, files.Count - skipcount);
            int bigwd = 0;
            int bight = 0;

            foreach (string f in files)
            {
                using (Bitmap smallBmp = new Bitmap(f))
                {
                    if (smallBmp.Width > bigwd)
                        bigwd = smallBmp.Width;
                    bight += (smallBmp.Height + 20);
                }
            }

            Bitmap bigBmp = new Bitmap(bigwd, bight);
            Graphics g = Graphics.FromImage(bigBmp);
            g.Clear(Color.White);
            int ht = 0;
            var htfile = Path.GetDirectoryName(filename) + "\\output.txt";
            File.AppendAllText(htfile, filename + " ");
            foreach (string f in files)
            {
                using (Bitmap smallBmp = new Bitmap(f))
                {
                    File.AppendAllText(htfile, ht.ToString() + "," + smallBmp.Height.ToString() + " ");
                    Rectangle smallRect = new Rectangle(0, 0, smallBmp.Width, smallBmp.Height);
                    Rectangle bigRect = new Rectangle(0, ht, smallBmp.Width, smallBmp.Height);
                    g.DrawImage(smallBmp, bigRect, smallRect, GraphicsUnit.Pixel);
                    ht += (smallBmp.Height + 20);
                }
            }
            File.AppendAllText(htfile, "\n");
            bigBmp.Save(filename);
        }

        public void Preprocess(string imgdir)
        {
            var files = Directory.GetFiles(imgdir, "??_??.png").OrderBy(f => f).ToArray();
            if (files.Count() == 0)
            {
                return;
            }

            Bitmap dstBmp = new Bitmap(2000, 2000);
            Graphics g = Graphics.FromImage(dstBmp);
            g.Clear(Color.White);


            Rectangle destrect = new Rectangle(0, 0, 0, 0); ;
            int offset = 0;
            var tempf = Path.GetFileNameWithoutExtension(files[0]);
            var destf = Path.GetDirectoryName(files[0]) + "\\" + (int.Parse(tempf.Substring(0, 2))).ToString("00") + ".png";
            foreach (string f in files)
            {
                using (MemoryStream srcms = new MemoryStream(System.IO.File.ReadAllBytes(f)))
                {
                    using (Bitmap srcbmp = new Bitmap(srcms))
                    {
                        Rectangle srcRect = new Rectangle(0, 0, srcbmp.Width, srcbmp.Height);
                        destrect = new Rectangle(0, offset, srcbmp.Width, srcbmp.Height);
                        g.DrawImage(srcbmp, destrect, srcRect, GraphicsUnit.Pixel);
                        offset += srcbmp.Height;
                    }
                }
                System.IO.File.Delete(f);
            }
            destrect = new Rectangle(0, 0, destrect.Width, offset);
            var tempbmp = (Bitmap)dstBmp.Clone(destrect, dstBmp.PixelFormat);
            tempbmp.Save(destf);
        }

        public void Process(string rootdir, string targetdir, string item)
        {
            string[] pdirlst;
            if (string.IsNullOrEmpty(item))
                pdirlst = Directory.GetDirectories(rootdir, "???").OrderBy(f => f).ToArray();
            else
                pdirlst = Directory.GetDirectories(rootdir, "???").Where(f => f.Contains("\\" + item)).OrderBy(f => f).ToArray();

            foreach (var pdir in pdirlst)
            {
                int i = 1;
                var sdirlst = Directory.GetDirectories(pdir).OrderBy(f => f).ToArray();
                var tdir = targetdir + "\\" + Path.GetFileName(pdir);
                Directory.CreateDirectory(tdir + "\\Answers");
                Console.WriteLine("Processing {0}", pdir);
                foreach (var sdir in sdirlst)
                {
                    Preprocess(sdir);
                    var qfile = tdir + "\\" + i.ToString("00") + "_Question.png";
                    var ansfile = tdir + "\\Answers\\" + i.ToString("00") + "_Answer.png";
                    MergeAnswer(sdir, ansfile);
                    MergeQuestions(sdir, qfile);
                    i++;
                }
            }
        }
    }
}
