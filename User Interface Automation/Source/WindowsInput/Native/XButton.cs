﻿//TICS -3@102  -- not relevant here
namespace UITesting.Automated.WindowsInput.Native
{
    /// <summary>
    /// XButton definitions for use in the MouseData property of the <see cref="MOUSEINPUT"/> structure. (See: http://msdn.microsoft.com/en-us/library/ms646273(VS.85).aspx)
    /// </summary>
    public enum XButton : int
    {
        /// <summary>
        /// Set if the first X button is pressed or released.
        /// </summary>
        XButton1 = 0x0001,

        /// <summary>
        /// Set if the second X button is pressed or released.
        /// </summary>
        XButton2 = 0x0002,
    }
}
//TICS +3@102
