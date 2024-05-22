using UITesting.Automated.WindowsInput.Native;

namespace UITesting.Automated.WindowsInput
{
    /// <summary>MouseButton</summary>
    internal enum MouseButton
    {
        /// <summary>LeftButton</summary>
        LeftButton,
        /// <summary>MiddleButton</summary>
        MiddleButton,
        /// <summary>RightButton</summary>
        RightButton,
    }

    /// <summary>MouseButtonExtensions</summary>
    internal static class MouseButtonExtensions
    {
        /// <summary>MouseFlag</summary>
        internal static MouseFlag ToMouseButtonDownFlag(this MouseButton button)
        {
            switch (button)
            {
                case MouseButton.LeftButton:
                    return MouseFlag.LeftDown;

                case MouseButton.MiddleButton:
                    return MouseFlag.MiddleDown;

                case MouseButton.RightButton:
                    return MouseFlag.RightDown;

                default:
                    return MouseFlag.LeftDown;
            }
        }

        /// <summary>ToMouseButtonUpFlag</summary>
        internal static MouseFlag ToMouseButtonUpFlag(this MouseButton button)
        {
            switch (button)
            {
                case MouseButton.LeftButton:
                    return MouseFlag.LeftUp;

                case MouseButton.MiddleButton:
                    return MouseFlag.MiddleUp;

                case MouseButton.RightButton:
                    return MouseFlag.RightUp;

                default:
                    return MouseFlag.LeftUp;
            }
        }
    }
}