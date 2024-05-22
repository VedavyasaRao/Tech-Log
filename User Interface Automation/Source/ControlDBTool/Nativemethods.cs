using System;
//TICS -3@102  -- not relevant here
//TICS -3@107  -- not relevant here

namespace UITesting.Automated.ControlDBTool
{
    /// <summary>Nativemethods</summary>
    internal static class Nativemethods
    {
        /// <summary>ShowWindow</summary>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        /// <summary>SetWindowPos</summary>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool SetWindowPos(
            IntPtr hWnd, IntPtr hwndAfter, int x, int y, 
            int width, int height, int flags);

        /// <summary>GetWindowLong</summary>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        /// <summary>SetWindowLong</summary>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, 
            int dwNewLong);
        /// <summary>SetProcessDPIAware</summary>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool SetProcessDPIAware();

        /// <summary>GWL_EXSTYLE</summary>
        internal const int GWL_EXSTYLE = -20;

        /// <summary>SW_SHOWNA</summary>
        internal const int SW_SHOWNA = 8;
        /// <summary>WS_EX_TOOLWINDOW</summary>
        internal const int WS_EX_TOOLWINDOW = 0x00000080;


        // SetWindowPos constants (used by highlight rect)
        /// <summary>SWP_NOACTIVATE</summary>
        internal const int SWP_NOACTIVATE = 0x0010;
        /// <summary>HWND_TOPMOST</summary>
        internal static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    
    
    }
}

//TICS +3@107
//TICS +3@102
