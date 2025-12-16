using System;

namespace BackupRestoreTool
{
    public class ProgressMonitor
    {
        int i;
        int ticks = 0;
        public System.Windows.Controls.ProgressBar pbar;
        public void initpbar(double max)
        {
            i = 0;
            ticks = (int)max / 100;
            if (ticks == 0)
                ticks = 100;
            App.disp.Invoke(new Action(() => { pbar.Minimum = 0; pbar.Maximum = max+1; pbar.Value = 0; }));
            busycursor();
        }

        public void updatepbar()
        {
            if ((++i % ticks) == 0)
                App.disp.Invoke(new Action(() => { pbar.Value = i; }));
        }
        public void closebar()
        {
            App.disp.Invoke(new Action(() => { pbar.Value = 0;  }));
            normalcursor();
        }
        public void busycursor()
        {
            App.disp.Invoke(new Action(() => { System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait; }));
        }

        public void normalcursor()
        {
            App.disp.Invoke(new Action(() => { System.Windows.Input.Mouse.OverrideCursor = null; }));
        }

    }
}



