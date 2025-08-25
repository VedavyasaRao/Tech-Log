using ImageHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace ImageHandler
{
    class CopySection
    {
        public void process(string srcfilename, string dstfilename, int x, int y, int wd, int ht)
        {
            Bitmap dstBmp = new Bitmap(2000, 2000);
            Graphics g = Graphics.FromImage(dstBmp);
            g.Clear(Color.White);

            Bitmap srcbmp = new Bitmap(srcfilename);
            Rectangle srcRect = new Rectangle(x, y, wd, ht);
            Rectangle dstRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);
            g.DrawImage(srcbmp, dstRect, srcRect, GraphicsUnit.Pixel);

            var tempbmp = (Bitmap)dstBmp.Clone(dstRect, srcbmp.PixelFormat);
            tempbmp.Save(dstfilename);
        }
    }

    internal class Program
    {
        public static string ImageProcessorloc = Path.GetDirectoryName(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));
        static void showsyntax()
        {
            Console.WriteLine("Syntax:ImageProcessor <option> args");
            Console.WriteLine("Option 1: Copy Cross section");
            Console.WriteLine("Arguments: <src file> <target file> <x> <y> <width> <height>");
            Console.WriteLine("Example:");
            Console.WriteLine("ImageProcessor.exe 1 \"D:\\temp3\\oda\\013\\02_Question.png\" \"D:\\temp\\temp.png\" 80 960 550 460");
            Console.WriteLine();

            Console.WriteLine("Option 2: ImageCompare");
            Console.WriteLine("Arguments: <src file> <target file> <output file> <args>");
            Console.WriteLine("Example:");
            Console.WriteLine("ImageProcessor.exe 2  \"D:\\temp\\oda\\Homework\\ImageProcessor\\baseline\\white\\a.png\" \"D:\\temp\\oda\\Homework\\ImageProcessor\\baseline\\New folder\\white\\a.png\"  \"d:\\temp\\output.txt\" \"0.90 0 60 50 50\"");
            Console.WriteLine();

            Console.WriteLine("Option 3: Merge Question and Answers");
            Console.WriteLine("Arguments: <image dir>");
            Console.WriteLine("Example:");
            Console.WriteLine("ImageProcessor.exe 3  \"D:\\temp\\oda\"");
            Console.WriteLine();

            Console.WriteLine("Option 4: Unselect Options");
            Console.WriteLine("Arguments: <image dir>");
            Console.WriteLine("Example:");
            Console.WriteLine("ImageProcessor.exe 4  \"D:\\temp\\oda\"");
            Console.ReadKey();

        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                showsyntax();
                return;
            }

            if (args[0] == "1")
            {
                if (args.Length < 7)
                {
                    showsyntax();
                    return;
                }

                new CopySection().process(args[1], args[2], int.Parse(args[3]), int.Parse(args[4]), int.Parse(args[5]), int.Parse(args[6]));
            }

            if (args[0] == "2")
            {
                if (args.Length < 5)
                {
                    showsyntax();
                    return;
                }
                new ImageCompare().Process(args[1], args[2], args[3], args[4]);
            }

            if (args[0] == "3")
            {
                if (args.Length < 1)
                {
                    showsyntax();
                    return;
                }
                new MergeQuestionAnswers().Process(args[1]);
            }

            if (args[0] == "4")
            {
                if (args.Length < 1)
                {
                    showsyntax();
                    return;
                }
                new ReplaceImage().Process(args[1]);
            }
        }
    }
}
