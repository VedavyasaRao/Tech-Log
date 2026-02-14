using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace UITesting.Automated.ControlDBTool
{
    /// <summary>Program</summary>
    static class Program
    {
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SystemParametersInfo(uint uiAction, uint uiParam, IntPtr pvParam, uint fWinIni);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool PostMessage(IntPtr  hWnd, uint Msg, uint wParam, uint lParam);

        static void startinspect()
        {
            var p = Process.GetProcessesByName("inspect");
            if (p.Length == 1)
                p[0].Kill();
            System.Threading.Thread.Sleep(3000);
            ProcessStartInfo ps = new ProcessStartInfo();
            ps.CreateNoWindow = true;
            ps.WindowStyle = ProcessWindowStyle.Hidden;
            ps.FileName = "inspect.exe";
            Process.Start(ps);
            System.Threading.Thread.Sleep(3000);
        }


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //SystemParametersInfo(SPI_SETSCREENREADER, TRUE, NULL, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
            //PostMessage(HWND_BROADCAST, WM_WININICHANGE, SPI_SETSCREENREADER, 0);

            //SystemParametersInfo(0x0046, 1, IntPtr.Zero, 3);
            //PostMessage(new IntPtr(0xffff), 0x1A, 0x0047, 0);

            //// The key should be wrapped in a 'using' statement to ensure it is closed properly.
            //using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Control Panel\Accessibility\Blind Access"))
            //{
            //    key.SetValue("On", "1");
            //}

            startinspect();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWnd());
        }
    }
}
