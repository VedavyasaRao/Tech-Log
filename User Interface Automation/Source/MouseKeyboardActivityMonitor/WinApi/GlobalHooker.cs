using System.Diagnostics;
//TICS -3@102  -- not relevant here
//TICS -3@107  -- not relevant here

namespace UITesting.Automated.MouseKeyboardActivityMonitor.WinApi
{
    /// <summary>
    /// Provides methods for subscription and unsubscription to global mouse and keyboard hooks.
    /// </summary>
    public class GlobalHooker : Hooker
    {
        /// <summary>Subscribe</summary>
        internal override int Subscribe(int hookId, HookCallback hookCallback)
        {
            int hookHandle = SetWindowsHookEx(
                hookId,
                hookCallback,
                Process.GetCurrentProcess().MainModule.BaseAddress,
                0);

            if (hookHandle == 0)
            {
                ThrowLastUnmanagedErrorAsException();
            }

            return hookHandle;
        }

        /// <summary>IsGlobal</summary>
        internal override bool IsGlobal
        {
            get { return true; }
        }

        /// <summary>
        /// Windows NT/2000/XP/Vista/7: Installs a hook procedure that monitors low-level mouse input events.
        /// </summary>
        internal const int WH_MOUSE_LL = 14;

        /// <summary>
        /// Windows NT/2000/XP/Vista/7: Installs a hook procedure that monitors low-level keyboard  input events.
        /// </summary>
        internal const int WH_KEYBOARD_LL = 13;
    }
}
//TICS +3@107
//TICS +3@102
