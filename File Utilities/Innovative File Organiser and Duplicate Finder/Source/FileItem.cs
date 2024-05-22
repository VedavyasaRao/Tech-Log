using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FileOrganiser
{

    public class fileitem: INotifyPropertyChanged
    {
        public static int n = 50;
        public static double sel = 1;
        public fileitem()
        {
            _Items = new List<FileOrganiser.fileitem>();
            Items = new ObservableCollection<fileitem>();
            Color = Brushes.Black;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public bool isdup { get; set; }
        public bool isfile { get; set; }
        public string _title { get; set; }
        public string _fullPath { get; set; }
        public double _size { get; set; }
        public int _count { get; set; }
        public int _dupcount { get; set; }
        public bool _selected { get; set; }
        public string _md5 = "";
        public fileitem _parent;
        public ObservableCollection<fileitem> Items { get; set; }
        public List<fileitem> _Items { get; set; }
        internal long _dateupdated;
        internal bool _sorted;
        public bool hasdup = false;
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

        public Brush Color { get; set; }

        public string Count
        {
            get
            {
                if (_count == -1) return "File Count";
                return ((_count == 0) ? "" : _count.ToString());
            }
        }

        public string Size
        {
            get
            {
                if (_size == -1) return "Folder Size";
                return ((_size == 0 ) ? "" : Math.Round(((double)_size / sel), 2).ToString("F2"));
            }
        }

        public string DupSize
        {
           
            get
            {
                if (_size == -1) return "Duplicate Size";
                return ((_size == 0 || _Items.Count == 0 ) ? "" : Math.Round(((double)_size / sel), 2).ToString("F2"));
            }
        }

        public string DupCount
        {
            get
            {
                if (_dupcount == -1) return "Duplicate Count";
                return ((_dupcount == 0) ? "" : _dupcount.ToString());
            }
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

        public void toggleselected(bool? bsel)
        {
            if (isdup )
            {
                Selected = (bsel??false);
            }
            else
            {
                foreach (var fic in _Items)
                    fic.toggleselected(bsel);
            }
        }

        public void toggleselected2(bool? bsel)
        {
            foreach (var fic in _Items)
            {
                if (fic.isdup)
                {
                    fic.Selected = (bsel ?? false);
                }
            }
        }

        public void getselected(ref List<string> fo)
        {
            if (isdup && _selected)
            {
                fo.Add(_fullPath);
            }
            else
            {
                foreach (var fic in _Items)
                    fic.getselected(ref fo);
            }
        }

        public void getselected2(ref List<string> fo)
        {
            foreach (var fic in _Items)
            {
                if (fic.isdup && fic._selected)
                {
                    fo.Add(fic._fullPath);
                }
            }
        }

        public void getselected3(ref List<fileitem> fo)
        {
            if (isdup && _selected)
            {
                fo.Add(this);
            }
            else
            {
                foreach (var fic in _Items)
                    fic.getselected3(ref fo);
            }
        }

        public void removenondupnodes()
        {
            int i = 0;
            while (i < _Items.Count)
            {
                var fi = _Items[i];
                if (fi._dupcount == 0 && !fi.isdup)
                    _Items.Remove(fi);
                else
                    ++i;
            }

            foreach (var fi in _Items)
                fi.removenondupnodes();
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
                return y._size.CompareTo(x._size);
            }
            else if (sortby == fileitemsort.dupcount)
            {
                return y._dupcount.CompareTo(x._dupcount);
            }
            else
            {
                return 0;
            }
        }
    }


}