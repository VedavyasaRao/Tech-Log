using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Windows.Media;

namespace BackupRestoreTool
{
    [Serializable]
    public class basicfileitem 
    {
        public Guid _id = Guid.NewGuid();
        public string _crc = "";
        public string duplicatefile = "";
        public string archive = "";
        public string _fullPath;
        public double _size;
        internal long _dateupdated;
        public basicfileitem() {}
        public basicfileitem(fileitem fi)
        {
            _id = fi._id;
            _crc = fi._crc;
            duplicatefile = fi.duplicatefile;
            archive = fi.archive;
            _fullPath = fi._fullPath;
            _size = fi._size;
            _dateupdated = fi._dateupdated;
        }

    }

    public class basicfileitemComp :  IEqualityComparer<basicfileitem>
    {
        public bool Equals(basicfileitem x, basicfileitem y)
        {
            if (x == null || y == null)
                return false;

            return (x._id.CompareTo(y._id) == 0);
        }

        public int GetHashCode(basicfileitem obj)
        {
            return obj.GetHashCode();
        }
    }

    [Serializable]
    public class fileitem: basicfileitem, INotifyPropertyChanged
    {
        public static int n = 50;
        public static double sel = 1;
        public fileitem()
        {
            _Items = new List<BackupRestoreTool.fileitem>();
            Items = new ObservableCollection<fileitem>();
        }
        public fileitem(basicfileitem bfi)
        {
            _id = bfi._id;
            _crc = bfi._crc;
            duplicatefile = bfi.duplicatefile;
            archive = bfi.archive;
            _fullPath = bfi._fullPath;
            _size= bfi._size;
            _dateupdated= bfi._dateupdated;
            isfile = true;
            _title = System.IO.Path.GetFileName(_fullPath);
            Items = new ObservableCollection<fileitem>();
            _Items = new List<fileitem>();
        }
        [field: NonSerializedAttribute]
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public bool isfile { get; set; }
        public string _title { get; set; }
        public int _count { get; set; }
        public bool _selected { get; set; }
        public string _status { get; set; }
        public fileitem _parent;
        public ObservableCollection<fileitem> Items { get; set; }
        public List<fileitem> _Items { get; set; }
        internal bool _sorted;
        internal bool barchived=false;

        public string Title
        {
            get
            {
                return String.Format("{0," + (n * -1) + "}", _title.Substring(0, ((n < _title.Length) ? n : _title.Length)));
            }
        }

        public string FullPath 
        {
            get { return (_fullPath[_fullPath.Length - 1] == '\\') ? _fullPath.Substring(0, _fullPath.Length - 1) : _fullPath; }
        }

        public string Count { get { return ((_count == -1) ? "File Count" : ((_count == 0) ? "" : _count.ToString())); } }

        public string Size
        {
            get { return ((_size == -1) ? "File Size" : ((_size == 0 ) ? "" : Math.Round(((double)_size / sel), 2).ToString("F2"))); }
        }

        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                NotifyPropertyChanged("Selected");
            } 
        }

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                NotifyPropertyChanged("Status");
            }
        }

        public string DateModified
        {
            get
            {
                if (!isfile)
                    return "";
                return DateTime.FromFileTime(_dateupdated).ToString();
            }
        }

        public Brush Foreground
        {
            get {return ((_status == "Status") ? Brushes.Coral : Brushes.Black); } }

        public void getleaves(ref List<fileitem> fo)
        {
            if (isfile)
            {
                fo.Add(this);
            }
            else
            {
                foreach (var fic in _Items)
                    fic.getleaves(ref fo);
            }
        }

        public void updatesorted()
        {
            _sorted = false;
            foreach (var fic in _Items)
                fic.updatesorted();
        }

        public void toggleselected(bool? bsel, bool bskip)
        {
            if ((_status != "Archived" && _status != "Same") || (bskip))
                Selected = ((bsel) ?? false);
            foreach (var fic in _Items)
            {
                fic.toggleselected(bsel,bskip);
            }
        }
        public string calucalatemd5fast()
        {
            return String.Format("{0}{1}", _dateupdated, _size);
        }

        public string calucalatemd5full()
        {

            if (_crc != "")
                return _crc;
            return calucalatemd5optimized();
        }

        private string calucalatemd5optimized()
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
                App.logit("MD5 get failed   " + _fullPath);
                App.logit(ex.Message);
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
        public int GetHashCode(basicfileitem product)
        {
            return 0;
        }
    }

    public class fileitemComp : IComparer<fileitem>
    {
        public enum fileitemsort { title, count, size, dupcount };

        fileitemsort sortby ;
        public fileitemComp(fileitemsort sortby)
        {
            this.sortby = sortby;
        }

        public int Compare(fileitem x, fileitem y)
        {
            if ( x == null || y == null)
                return 0;
            if (sortby == fileitemsort.title)
            {
                return x._title.CompareTo(y._title);
            }
            else if (sortby == fileitemsort.count)
            {
                return y._count.CompareTo(x._count);
            }
            else if (sortby == fileitemsort.size)
            {
                int t = 0;
                if (x.isfile != y.isfile)
                    t = -1;
                else
                    t = (x._size).CompareTo(y._size);
                //App.logit(String.Format("{0},{1}-->{2},{3}-->{4}", x._fullPath, x._size, y._fullPath,y._size,t));
                return t;
            }
            else
            {
                return 0;
            }
        }
    }


}