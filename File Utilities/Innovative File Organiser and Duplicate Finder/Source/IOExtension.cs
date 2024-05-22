using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrganiser
{
    public static class PathEx
    {
        public const uint MAX_FILE_LEN = 260;
        public const uint MAX_DIR_LEN = 248;
        public static string RemoveUNC(this System.String  path)
        {
            return path.Replace("\\\\?\\", "");
        }
        public static string AddUNC(this System.String path)
        {
            if (path.Length < MAX_DIR_LEN)
                return path;
            return "\\\\?\\" + path;
        }
    }
    public static class DirectoryEx
        {

        public static System.IO.DirectoryInfo CreateDirectory(string path)
        {
            return System.IO.Directory.CreateDirectory(path.AddUNC());
        }

        public static bool Exists(string path)
        {
            return System.IO.Directory.Exists(path.AddUNC());
        }

        public static void Delete(string path,bool recursive)
        {
            System.IO.Directory.Delete(path.AddUNC(), recursive);
        }

        public static void Delete(string path)
        {
            System.IO.Directory.Delete(path.AddUNC());
        }
        public static string[] GetFiles(string path)
        {
            var temp = System.IO.Directory.GetFiles(path.AddUNC());
            return temp.Select(l => l.RemoveUNC()).ToArray();
        }

        public static string[] GetFiles(string path, string searchPattern, System.IO.SearchOption searchOption)
        {
            var temp = System.IO.Directory.GetFiles(path.AddUNC(), searchPattern, searchOption);
            return temp.Select(l => l.RemoveUNC()).ToArray();
        }
        public static System.IO.DirectoryInfo GetParent(string path)
        {
            return System.IO.Directory.GetParent(path.AddUNC());
        }
        public static string[] GetDirectories(string path)
        {
            var temp = System.IO.Directory.GetDirectories(path.AddUNC());
            return temp.Select(l => l.RemoveUNC()).ToArray();
        }
    }

    public static class FileEx
    {
        public static void Delete(string path)
        {
            System.IO.File.Delete(path.AddUNC());
        }

        public static bool Exists(string path)
        {
            return System.IO.File.Exists(path.AddUNC());
        }

        public static string ReadAllText(string path)
        {
            return System.IO.File.ReadAllText(path.AddUNC());
        }

        public static void WriteAllText(string path, string contents)
        {
            System.IO.File.WriteAllText(path.AddUNC(), contents);
        }

        public static void AppendAllText(string path, string contents)
        {
            System.IO.File.AppendAllText(path.AddUNC(), contents);
        }

        public static string[] ReadAllLines(string path)
        {
            return System.IO.File.ReadAllLines(path.AddUNC());
        }

        public static string[] ReadAllLines(string path,Encoding encoding)
        {
            return System.IO.File.ReadAllLines(path.AddUNC(), encoding);
        }

        public static void WriteAllLines(string path, System.Collections.Generic.IEnumerable<string> contents)
        {
            System.IO.File.WriteAllLines(path.AddUNC(), contents);
        }

        public static void Move(string sourceFileName, string destFileName)
        {
            System.IO.File.Move(sourceFileName.AddUNC(),  destFileName.AddUNC());
        }

        public static System.IO.FileAttributes GetAttributes(string path)
        {
            return System.IO.File.GetAttributes(path.AddUNC());
        }

        public static void AppendAllLines(string path, System.Collections.Generic.IEnumerable<string> contents)
        {
            System.IO.File.AppendAllLines(path.AddUNC(), contents);
        }

        public static System.IO.FileStream OpenRead(string path)
        {
            return System.IO.File.OpenRead(path.AddUNC());
        }

        public static byte[] ReadAllBytes(string path)
        {
            return System.IO.File.ReadAllBytes(path.AddUNC());
        }

        public static void SetAttributes(string path, System.IO.FileAttributes fileAttributes)
        {
            System.IO.File.SetAttributes(path.AddUNC(), fileAttributes);
        }

        public static void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            System.IO.File.Copy(sourceFileName.AddUNC(), destFileName.AddUNC(), overwrite);
        }
        public static void WriteAllBytes(string path, byte[] bytes)
        {
            System.IO.File.WriteAllBytes(path.AddUNC(), bytes);
        }
    }

    public static class FileStreamEx
    {
        public static System.IO.FileStream FileStream(string path, System.IO.FileMode mode)
        {
            return new System.IO.FileStream(path.AddUNC(), mode);
        }
    }

    public static class FileInfoEx
    {
        public static System.IO.FileInfo FileInfo(string path)
        {
            return new System.IO.FileInfo(path.AddUNC());
        }
    }

    public static class DirectoryInfoEx
    {
        public static System.IO.DirectoryInfo DirectoryInfo(string path)
        {
            return  new System.IO.DirectoryInfo(path.AddUNC());
        }
    }
}

