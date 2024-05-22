using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Provider;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;
using UITesting.Automated.WindowsInput;
using UITesting.Automated.ControlInf;
using UITesting.Automated.JSonSerializer;
using UITesting.Automated.WindowsInput.Native;

namespace UITesting.Automated.UIADriver
{
    public interface IUIAGenericProvider
    {
        void Setfocus();
        void Click(bool bright, bool bdouble);
        void ClickGrid(bool bright, bool bdouble, int offset, int itemheight, int row);
        void MoveandClick(int x, int y, bool brelative, bool bright, bool bdouble);
        void SendKeyStrokes(string keys);
        string WindowText
        {
            get;
            set;
        }
        string GetAutomationProperty(int id);
        void CaptureBitmap(string filename);
        string GetMousePosition();
        void DragandDrop(int startx, int starty, int endx, int endy);
        void SendVirtualKeyStrokes(int key, int Modifier);
    }

    class UIAGeneric : UIADriverPatternBase, IUIAGenericProvider
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        private const int WM_SETTEXT = 0x000C;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, string sendstr);


        private const int DelayMS = 500;
        private MouseSimulator mousesim = new MouseSimulator();
        private Point curpt = new Point();


        private void ToScreen(ref Point pt)
        {
            pt.X = (pt.X * 65535.0) / System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width; ;
            pt.Y = (pt.Y * 65535.0) / System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
        }


        private void MouseMove(Point pt, bool bright, bool bdouble)
        {
            curpt = pt;
            ToScreen(ref pt);
            mousesim.MoveMouseTo(pt.X, pt.Y);
            Application.DoEvents();
            System.Threading.Thread.Sleep(DelayMS);

            if (bright)
            {
                if (bdouble)
                {
                    mousesim.RightButtonDoubleClick();
                }
                else
                {
                    mousesim.RightButtonClick();
                }
            }
            else
            {
                if (bdouble)
                {
                    mousesim.LeftButtonDoubleClick();
                }
                else
                {
                    mousesim.LeftButtonClick();
                }
            }
            Application.DoEvents();
        }


        private void MouseClick(Point pt, bool bright, bool bdouble)
        {
            curpt = pt;
            ToScreen(ref pt);
            mousesim.MoveMouseTo(pt.X, pt.Y);
            Application.DoEvents();
            System.Threading.Thread.Sleep(DelayMS);

            if (bright)
            {
                if (bdouble)
                {
                    mousesim.RightButtonDoubleClick();
                }
                else
                {
                    mousesim.RightButtonClick();
                }
            }
            else
            {
                if (bdouble)
                {
                    mousesim.LeftButtonDoubleClick();
                }
                else
                {
                    mousesim.LeftButtonClick();
                }
            }
            Application.DoEvents();
        }

        internal static UIAGeneric GetInstance(UIAElement owner)
        {
            return GetPatternObject<UIAGeneric>(owner);
        }

        public void Setfocus()
        {
            try
            {
                Log("Setfocus()");
                owner.GetAE.SetFocus();
                CauseDelay();

            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

        public void Click(bool bright, bool bdouble)
        {
            const int delay = 200;
            try
            {
                Log(string.Format("Click({0}.{1})", bright, bdouble));
                Point pt;

                if (!owner.GetAE.TryGetClickablePoint(out pt))
                {
                    Rect br = (Rect)owner.GetAE.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty);
                    pt = new Point(br.Left + br.Width / 2, br.Top + br.Height / 2);
                }

                Setfocus();
                int temp = owner.Waitms;
                owner.Waitms = delay;
                CauseDelay();
                owner.Waitms = temp;
                MouseClick(pt, bright, bdouble);
                CauseDelay();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

        public void ClickGrid(bool bright, bool bdouble, int offset, int itemheight, int row)
        {
            try
            {
                Log(string.Format("ClickGrid({0},{1},{2},{3},{4})", bright, bdouble, offset, itemheight, row));
                Rect br = (Rect)owner.GetAE.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty);
                Point pt = new Point(br.Left + br.Width / 2, br.Top + (offset + (itemheight * row)));
                MouseClick(pt, bright, bdouble);
                CauseDelay();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);

            }
        }

        public void MoveandClick(int x, int y, bool brelative, bool bright, bool bdouble)
        {
            try
            {
                Log(string.Format("MoveandClick({0},{1},{2},{3},{4})", x, y, brelative, bright, bdouble));
                Log(string.Format("postion {0},{1}", GetMousePosition(),  curpt));
                Point pt = curpt;
                if (brelative)
                {
                    pt.X += x;
                    pt.Y += y;
                }
                else
                {
                    pt.X = x;
                    pt.Y = y;
                }
                MouseClick(pt, bright, bdouble);
                CauseDelay();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

        public string WindowText
        {
            get
            {
                string ret = "";
                try
                {
                    Log("WindowText");
                    IntPtr hWnd = new IntPtr((int)owner.GetAE.GetCurrentPropertyValue(AutomationElement.NativeWindowHandleProperty));
                    if (hWnd != IntPtr.Zero)
                    {
                        int length = GetWindowTextLength(hWnd);
                        StringBuilder sb = new StringBuilder(length + 1);
                        GetWindowText(hWnd, sb, sb.Capacity);
                        ret = sb.ToString();
                        CauseDelay();
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }

                return ret;
            }
            set
            {
                try
                {
                    Log("WindowText");
                    IntPtr hWnd = new IntPtr((int)owner.GetAE.GetCurrentPropertyValue(AutomationElement.NativeWindowHandleProperty));
                    if (hWnd != IntPtr.Zero)
                    {
                        SendMessage(hWnd, WM_SETTEXT, 0, value);
                        CauseDelay();
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }

            }
        }

        public void SendKeyStrokes(string keys)
        {
            Log(string.Format("SendKeyStrokes({0})", keys));
            try
            {
                SendKeys.SendWait(keys);
                CauseDelay();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }

        }

        public void SendVirtualKeyStrokes(int key, int Modifier)
        {
            Log(string.Format("SendVirtualKeyStrokes({0})", key.ToString(), Convert.ToInt32(Modifier)));

            try
            {
                KeyboardSimulator kbdsim = new KeyboardSimulator();
                if ((Keys)Modifier == Keys.None)
                {
                    kbdsim.KeyDown((VirtualKeyCode)key);
                }
                else
                {
                    List<VirtualKeyCode> modifierKeyCodes = new List<VirtualKeyCode>();
                    if (((Keys)Modifier & Keys.Alt) == Keys.Alt)
                        modifierKeyCodes.Add((VirtualKeyCode)Keys.Alt);
                    else if (((Keys)Modifier & Keys.Control) == Keys.Control)
                        modifierKeyCodes.Add((VirtualKeyCode)Keys.LControlKey);
                    else if (((Keys)Modifier & Keys.Shift) == Keys.Shift)
                        modifierKeyCodes.Add((VirtualKeyCode)Keys.LShiftKey);
                    kbdsim.ModifiedKeyStroke(modifierKeyCodes, (VirtualKeyCode)key);
                }
                CauseDelay();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }

        }

        public string GetAutomationProperty(int id)
        {
            string ret = "";
            Log(string.Format("GetAutomationProperty({0})", id));
            try
            {
                AutomationProperty p = AutomationProperty.LookupById(id);
                ret = JSonSerializer.JSONPersister<object>.GetRawJSON(owner.GetAE.GetCurrentPropertyValue(p));
                CauseDelay();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
            return ret;
        }

        public void CaptureBitmap(string filename)
        {
            Log(string.Format("CaptureBitmap({0})", filename));
            try
            {
                System.Windows.Rect rect = owner.GetAE.Current.BoundingRectangle;
                System.Drawing.Bitmap printscreen = new System.Drawing.Bitmap((int)rect.Width, (int)rect.Height);
                System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(printscreen as System.Drawing.Image);
                graphics.CopyFromScreen((int)rect.Left, (int)rect.Top, 0, 0, new System.Drawing.Size((int)rect.Width, (int)rect.Height));
                printscreen.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
                CauseDelay();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }

        }

        public string GetMousePosition()
        {
            string ret = "";
            Log("GetMousePosition");
            try
            {
                ret = JSonSerializer.JSONPersister<object>.GetRawJSON(System.Windows.Forms.Cursor.Position);
                CauseDelay();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
            return ret;
        }

        public void DragandDrop(int startx, int starty, int endx, int endy)
        {
            Log(string.Format("DragandDrop({0},{1},{2},{3})", startx, starty, endx, endy));
            try
            {
                Point pt = curpt;
                pt.X = startx;
                pt.Y = starty;
                ToScreen(ref pt);
                mousesim.MoveMouseTo((int)pt.X, (int)pt.Y);
                mousesim.LeftButtonDown();

                pt.X = endx;
                pt.Y = endy;
                ToScreen(ref pt);
                mousesim.MoveMouseTo((int)pt.X, (int)pt.Y);
                mousesim.LeftButtonUp();

                Application.DoEvents();
                System.Threading.Thread.Sleep(DelayMS);
                CauseDelay();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }

        }




    }

}

