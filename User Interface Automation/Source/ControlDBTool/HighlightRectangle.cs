using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace UITesting.Automated.ControlDBTool
{
    /// <summary>HighlightRectangle</summary>
    internal class HighlightRectangle 
    {
        #region Private Fields

        private bool highlightShown;
        private int highlightLineWidth;
        private Rectangle highlightLocation;

        private Form leftForm;
        private Form topForm;
        private Form rightForm;
        private Form bottomForm;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <remarks>
        /// Creates each side of the highlight rectangle as a form, so that
        /// drawing and erasing are handled automatically.
        /// </remarks>
        public HighlightRectangle()
        {
            // Construct the rectangle and set some values.
            highlightShown = false;
            highlightLineWidth = 3;
            leftForm = new Form();
            topForm = new Form();
            rightForm = new Form();
            bottomForm = new Form();
            Form[] forms = { leftForm, topForm, rightForm, bottomForm };
            foreach (Form form in forms)
            {
                form.FormBorderStyle = FormBorderStyle.None;
                form.ShowInTaskbar = false;
                form.TopMost = true;
                form.Visible = false;
                form.Left = 0;
                form.Top = 0;
                form.Width = 1;
                form.Height = 1;
                form.BackColor = Color.Red;

                // Make it a tool window so it doesn't show up with Alt+Tab.
                int style = Nativemethods.GetWindowLong(
                    form.Handle, Nativemethods.GWL_EXSTYLE);
                Nativemethods.SetWindowLong(
                    form.Handle, Nativemethods.GWL_EXSTYLE,
                    (int)(style | Nativemethods.WS_EX_TOOLWINDOW));
            }
        }
        #endregion


        #region Public Properties

        /// <summary>
        /// Sets the visible state of the rectangle.
        /// </summary>
        /// <remarks>
        /// The Layout method is called by using BeginInvoke, to prevent
        /// cross-thread updates to the UI. This method can be called on
        /// any form that belongs to the UI thread.
        /// </remarks>
        public bool Visible
        {
            set
            {
                if (highlightShown != value)
                {
                    highlightShown = value;
                    if (highlightShown)
                    {
                        MethodInvoker mi = new MethodInvoker(Layout);
                        leftForm.BeginInvoke(mi);
                        mi = new MethodInvoker(ShowRectangle);
                        leftForm.BeginInvoke(mi);
                    }
                    else
                    {
                        MethodInvoker mi = new MethodInvoker(HideRectangle);
                        leftForm.BeginInvoke(mi);
                    }
                }
            }
        }

        /// <summary>
        /// Sets the location of the highlight.
        /// </summary>
        public Rectangle Location
        {
            set
            {
                highlightLocation = value;
                MethodInvoker mi = new MethodInvoker(Layout);
                leftForm.BeginInvoke(mi);
            }
        }

        #endregion


        #region Private Methods

        /// <summary>
        /// Shows or hides the rectangle.
        /// </summary>
        /// <param name="show">true to show, false to hide.</param>
        private void Show(bool show)
        {
            if (show)
            {
                Nativemethods.ShowWindow(leftForm.Handle, Nativemethods.SW_SHOWNA);
                Nativemethods.ShowWindow(topForm.Handle, Nativemethods.SW_SHOWNA);
                Nativemethods.ShowWindow(rightForm.Handle, Nativemethods.SW_SHOWNA);
                Nativemethods.ShowWindow(bottomForm.Handle, Nativemethods.SW_SHOWNA);
            }
            else
            {
                leftForm.Hide();
                topForm.Hide();
                rightForm.Hide();
                bottomForm.Hide();
            }
        }

        /// <summary>
        /// Shows the highlight.
        /// </summary>
        /// <remarks> Parameterless method for MethodInvoker.</remarks>
        void ShowRectangle()
        {
            Show(true);
        }

        /// <summary>
        /// Hides the highlight.
        /// </summary>
        /// <remarks> Parameterless method for MethodInvoker.</remarks>
        void HideRectangle()
        {
            Show(false);
        }

        /// <summary>
        /// Sets the position and size of the four forms that make up the rectangle.
        /// </summary>
        /// <remarks>
        /// Use the Win32 SetWindowPosfunction so that SWP_NOACTIVATE can be set. 
        /// This ensures that the windows are shown without receiving the focus.
        /// </remarks>
        private void Layout()
        {
            // Use SetWindowPos instead of changing the location via form properties: 
            // this allows us to also specify HWND_TOPMOST. 
            // Using Form.TopMost = true to do this has the side-effect
            // of activating the rectangle windows, causing them to gain the focus.
            Nativemethods.SetWindowPos(leftForm.Handle, Nativemethods.HWND_TOPMOST,
                        highlightLocation.Left - highlightLineWidth, 
                        highlightLocation.Top, 
                        highlightLineWidth, highlightLocation.Height, 
                        Nativemethods.SWP_NOACTIVATE);
            Nativemethods.SetWindowPos(topForm.Handle, Nativemethods.HWND_TOPMOST,
                        highlightLocation.Left - highlightLineWidth, 
                        highlightLocation.Top - highlightLineWidth, 
                        highlightLocation.Width + 2 * highlightLineWidth, 
                        highlightLineWidth, 
                        Nativemethods.SWP_NOACTIVATE);
            Nativemethods.SetWindowPos(rightForm.Handle, Nativemethods.HWND_TOPMOST,
                        highlightLocation.Left + highlightLocation.Width, 
                        highlightLocation.Top, highlightLineWidth, 
                        highlightLocation.Height, 
                        Nativemethods.SWP_NOACTIVATE);
            Nativemethods.SetWindowPos(bottomForm.Handle, Nativemethods.HWND_TOPMOST,
                        highlightLocation.Left - highlightLineWidth, 
                        highlightLocation.Top + highlightLocation.Height, 
                        highlightLocation.Width + 2 * highlightLineWidth, 
                        highlightLineWidth, 
                        Nativemethods.SWP_NOACTIVATE);
        }


        #endregion

    }  // class
}  // namespace
