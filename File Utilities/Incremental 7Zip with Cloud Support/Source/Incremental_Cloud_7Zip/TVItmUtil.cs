using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BackupRestoreTool
{
    public class TVItmUtil
    {
        public int maxshow = 500;
        public fileitem dummy = new fileitem { _title = "dummy" };
        public Dictionary<fileitem, double> fiitmsmap = new Dictionary<fileitem, double>();
        public Slider slider;
        public StackPanel pnl;
        public fileitemComp.fileitemsort sortby;

        public void clear(StackPanel pnl, Slider slider, fileitemComp.fileitemsort sortby)
        {
            fiitmsmap.Clear();
            this.sortby = sortby;
            this.slider = slider;
            this.pnl = pnl;
            this.slider.TickFrequency = 1;
        }

        public void closenode(fileitem fi)
        {
            pnl.Visibility = Visibility.Hidden;
        }

        public void showitems(fileitem fi)
        {
            fi.Items.Clear();
            int p = (int)fiitmsmap[fi];
            int q = p + ((fi._Items.Count > maxshow) ? maxshow : fi._Items.Count);
            //q = p + fi._Items.Count;
            if (!fi._sorted)
            {
                fi._Items.Sort(new fileitemComp(sortby));
                fi._sorted = true;
            }
            for (; p < q; ++p)
            {
                fi.Items.Add(fi._Items[p]);
            }
        }

        public void expandnode(fileitem fi)
        {
            pnl.Visibility = Visibility.Hidden;
            if (fiitmsmap.ContainsKey(fi))
                return;
            fiitmsmap.Add(fi, 0);
            showitems(fi);
        }

        public void selectnode(fileitem fi)
        {
            if (fiitmsmap.ContainsKey(fi) && fi._Items.Count > maxshow)
            {
                pnl.Visibility = Visibility.Visible;
                slider.Value = fiitmsmap[fi];
                slider.Maximum = fi._Items.Count - maxshow;

            }
        }

        public void dragslider(fileitem fi)
        {
            if (!fiitmsmap.ContainsKey(fi))
                return;
            if (slider.Value == fiitmsmap[fi])
                return;
            fiitmsmap[fi] = (int)slider.Value;
            showitems(fi);
        }
        public void lostfocus()
        {
            pnl.Visibility = Visibility.Hidden;
        }

    }
}
