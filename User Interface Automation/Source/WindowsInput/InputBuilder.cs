﻿using System;
using System.Collections;
using System.Collections.Generic;
using UITesting.Automated.WindowsInput.Native;
//TICS -3@102  -- not relevant here
//TICS -3@104  -- not relevant here
//TICS -3@107  -- not relevant here
//TICS -10@301  -- not relevant here

namespace UITesting.Automated.WindowsInput
{
    /// <summary>
    /// A helper class for building a list of <see cref="INPUT"/> messages ready to be sent to the native Windows API.
    /// </summary>
    internal class InputBuilder : IEnumerable<INPUT>
    {
        /// <summary>
        /// The internal list of <see cref="INPUT"/> messages being built by this instance.
        /// </summary>
        private readonly List<INPUT> _inputList;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputBuilder"/> class.
        /// </summary>
        public InputBuilder()
        {
            _inputList = new List<INPUT>();
        }

        /// <summary>
        /// Returns the list of <see cref="INPUT"/> messages as a <see cref="System.Array"/> of <see cref="INPUT"/> messages.
        /// </summary>
        /// <returns>The <see cref="System.Array"/> of <see cref="INPUT"/> messages.</returns>
        public INPUT[] ToArray()
        {
            return _inputList.ToArray();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the list of <see cref="INPUT"/> messages.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the list of <see cref="INPUT"/> messages.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<INPUT> GetEnumerator()
        {
            return _inputList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the list of <see cref="INPUT"/> messages.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the list of <see cref="INPUT"/> messages.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets the <see cref="INPUT"/> at the specified position.
        /// </summary>
        /// <value>The <see cref="INPUT"/> message at the specified position.</value>
        public INPUT this[int position]
        {
            get
            {
                return _inputList[position];
            }
        }

        /// <summary>
        /// Adds a key down to the list of <see cref="INPUT"/> messages.
        /// </summary>
        /// <param name="keyCode">The <see cref="VirtualKeyCode"/>.</param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddKeyDown(VirtualKeyCode keyCode)
        {
            var down = new INPUT();
            down.Type = (UInt32)InputType.Keyboard;
            down.Data.Keyboard = new KEYBDINPUT();
            down.Data.Keyboard.KeyCode = (UInt16)keyCode;
            down.Data.Keyboard.Scan = 0;
            down.Data.Keyboard.Flags = 0;
            down.Data.Keyboard.Time = 0;
            down.Data.Keyboard.ExtraInfo = IntPtr.Zero;

            _inputList.Add(down);
            return this;
        }

        /// <summary>
        /// Adds a key up to the list of <see cref="INPUT"/> messages.
        /// </summary>
        /// <param name="keyCode">The <see cref="VirtualKeyCode"/>.</param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddKeyUp(VirtualKeyCode keyCode)
        {
            var up = new INPUT();
            up.Type = (UInt32)InputType.Keyboard;
            up.Data.Keyboard = new KEYBDINPUT();
            up.Data.Keyboard.KeyCode = (UInt16)keyCode;
            up.Data.Keyboard.Scan = 0;
            up.Data.Keyboard.Flags = (UInt32)KeyboardFlag.KeyUp;
            up.Data.Keyboard.Time = 0;
            up.Data.Keyboard.ExtraInfo = IntPtr.Zero;

            _inputList.Add(up);
            return this;
        }

        /// <summary>
        /// Adds a key press to the list of <see cref="INPUT"/> messages which is equivalent to a key down followed by a key up.
        /// </summary>
        /// <param name="keyCode">The <see cref="VirtualKeyCode"/>.</param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddKeyPress(VirtualKeyCode keyCode)
        {
            AddKeyDown(keyCode);
            AddKeyUp(keyCode);
            return this;
        }

        /// <summary>
        /// Adds the character to the list of <see cref="INPUT"/> messages.
        /// </summary>
        /// <param name="character">The <see cref="System.Char"/> to be added to the list of <see cref="INPUT"/> messages.</param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddCharacter(char character)
        {
            UInt16 scanCode = character;

            var down = new INPUT();
            down.Type = (UInt32)InputType.Keyboard;
            down.Data.Keyboard = new KEYBDINPUT();
            down.Data.Keyboard.KeyCode = 0;
            down.Data.Keyboard.Scan = scanCode;
            down.Data.Keyboard.Flags = (UInt32)KeyboardFlag.Unicode;
            down.Data.Keyboard.Time = 0;
            down.Data.Keyboard.ExtraInfo = IntPtr.Zero;

            var up = new INPUT();
            up.Type = (UInt32)InputType.Keyboard;
            up.Data.Keyboard = new KEYBDINPUT();
            up.Data.Keyboard.KeyCode = 0;
            up.Data.Keyboard.Scan = scanCode;
            up.Data.Keyboard.Flags = (UInt32)(KeyboardFlag.KeyUp | KeyboardFlag.Unicode);
            up.Data.Keyboard.Time = 0;
            up.Data.Keyboard.ExtraInfo = IntPtr.Zero;

            // Handle extended keys:
            // If the scan code is preceded by a prefix byte that has the value 0xE0 (224),
            // we need to include the KEYEVENTF_EXTENDEDKEY flag in the Flags property. 
            if ((scanCode & 0xFF00) == 0xE000)
            {
                down.Data.Keyboard.Flags |= (UInt32)KeyboardFlag.ExtendedKey;
                up.Data.Keyboard.Flags |= (UInt32)KeyboardFlag.ExtendedKey;
            }

            _inputList.Add(down);
            _inputList.Add(up);
            return this;
        }

        /// <summary>
        /// Adds all of the characters in the specified <see cref="IEnumerable{T}"/> of <see cref="char"/>.
        /// </summary>
        /// <param name="characters">The characters to add.</param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddCharacters(IEnumerable<char> characters)
        {
            foreach (var character in characters)
            {
                AddCharacter(character);
            }
            return this;
        }

        /// <summary>
        /// Adds the characters in the specified <see cref="string"/>.
        /// </summary>
        /// <param name="characters">The string of <see cref="char"/> to add.</param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddCharacters(string characters)
        {
            return AddCharacters(characters.ToCharArray());
        }

        #region Mouse
        /// <summary>
        /// AddRelativeMouseMovement
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public InputBuilder AddRelativeMouseMovement(int x, int y)
        {
            var movement = new INPUT();
            movement.Type = (UInt32)InputType.Mouse;
            movement.Data.Mouse.Flags = (UInt32)MouseFlag.Move;
            movement.Data.Mouse.X = x;
            movement.Data.Mouse.Y = y;

            _inputList.Add(movement);

            return this;
        }

        /// <summary>
        /// AddAbsoluteMouseMovement
        /// </summary>
        /// <param name="absoluteX"></param>
        /// <param name="absoluteY"></param>
        /// <returns></returns>
        public InputBuilder AddAbsoluteMouseMovement(int absoluteX, int absoluteY)
        {
            var movement = new INPUT();
            movement.Type = (UInt32)InputType.Mouse;
            movement.Data.Mouse.Flags = (UInt32)(MouseFlag.Move | MouseFlag.Absolute);
            movement.Data.Mouse.X = absoluteX;
            movement.Data.Mouse.Y = absoluteY;

            _inputList.Add(movement);

            return this;
        }

        /// <summary>
        /// AddAbsoluteMouseMovementOnVirtualDesktop
        /// </summary>
        /// <param name="absoluteX"></param>
        /// <param name="absoluteY"></param>
        /// <returns></returns>
        public InputBuilder AddAbsoluteMouseMovementOnVirtualDesktop(int absoluteX, int absoluteY)
        {
            var movement = new INPUT();
            movement.Type = (UInt32)InputType.Mouse;
            movement.Data.Mouse.Flags = (UInt32)(MouseFlag.Move | MouseFlag.Absolute | MouseFlag.VirtualDesk);
            movement.Data.Mouse.X = absoluteX;
            movement.Data.Mouse.Y = absoluteY;

            _inputList.Add(movement);

            return this;
        }

        /// <summary>
        /// AddMouseButtonDown
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public InputBuilder AddMouseButtonDown(MouseButton button)
        {
            var buttonDown = new INPUT();
            buttonDown.Type = (UInt32)InputType.Mouse;
            buttonDown.Data.Mouse.Flags = (UInt32)button.ToMouseButtonDownFlag();

            _inputList.Add(buttonDown);

            return this;
        }

        /// <summary>
        /// AddMouseButtonDown
        /// </summary>
        /// <param name="button"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public InputBuilder AddMouseButtonDown(MouseButton button, int x, int y)
        {
            var buttonDown = new INPUT();
            buttonDown.Type = (UInt32)InputType.Mouse;
            buttonDown.Data.Mouse.Flags = (UInt32)button.ToMouseButtonDownFlag();
            buttonDown.Data.Mouse.X = x;
            buttonDown.Data.Mouse.Y = y;

            _inputList.Add(buttonDown);

            return this;
        }

        /// <summary>
        /// AddMouseXButtonDown
        /// </summary>
        /// <param name="xButtonId"></param>
        /// <returns></returns>
        public InputBuilder AddMouseXButtonDown(int xButtonId)
        {
            var buttonDown = new INPUT();
            buttonDown.Type = (UInt32)InputType.Mouse;
            buttonDown.Data.Mouse.Flags = (UInt32)MouseFlag.XDown;
            buttonDown.Data.Mouse.MouseData = (UInt32)xButtonId;
            _inputList.Add(buttonDown);

            return this;
        }

        /// <summary>
        /// AddMouseButtonUp
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public InputBuilder AddMouseButtonUp(MouseButton button)
        {
            var buttonUp = new INPUT();
            buttonUp.Type = (UInt32)InputType.Mouse;
            buttonUp.Data.Mouse.Flags = (UInt32)button.ToMouseButtonUpFlag();
            _inputList.Add(buttonUp);

            return this;
        }

        /// <summary>
        /// AddMouseButtonUp
        /// </summary>
        /// <param name="button"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public InputBuilder AddMouseButtonUp(MouseButton button, int x, int y)
        {
            var buttonUp = new INPUT();
            buttonUp.Type = (UInt32)InputType.Mouse;
            buttonUp.Data.Mouse.Flags = (UInt32)button.ToMouseButtonUpFlag();
            buttonUp.Data.Mouse.X = x;
            buttonUp.Data.Mouse.Y = y;
            _inputList.Add(buttonUp);

            return this;
        }

        /// <summary>
        /// AddMouseXButtonUp
        /// </summary>
        /// <param name="xButtonId"></param>
        /// <returns></returns>
        public InputBuilder AddMouseXButtonUp(int xButtonId)
        {
            var buttonUp = new INPUT();
            buttonUp.Type = (UInt32)InputType.Mouse;
            buttonUp.Data.Mouse.Flags = (UInt32)MouseFlag.XUp;
            buttonUp.Data.Mouse.MouseData = (UInt32)xButtonId;
            _inputList.Add(buttonUp);

            return this;
        }

        /// <summary>
        /// AddMouseButtonClick
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public InputBuilder AddMouseButtonClick(MouseButton button)
        {
            return AddMouseButtonDown(button).AddMouseButtonUp(button);
        }

        /// <summary>
        /// AddMouseXButtonClick
        /// </summary>
        /// <param name="xButtonId"></param>
        /// <returns></returns>
        public InputBuilder AddMouseXButtonClick(int xButtonId)
        {
            return AddMouseXButtonDown(xButtonId).AddMouseXButtonUp(xButtonId);
        }

        /// <summary>
        /// AddMouseButtonDoubleClick
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public InputBuilder AddMouseButtonDoubleClick(MouseButton button)
        {
            return AddMouseButtonClick(button).AddMouseButtonClick(button);
        }

        /// <summary>
        /// AddMouseXButtonDoubleClick
        /// </summary>
        /// <param name="xButtonId"></param>
        /// <returns></returns>
        public InputBuilder AddMouseXButtonDoubleClick(int xButtonId)
        {
            return AddMouseXButtonClick(xButtonId).AddMouseXButtonClick(xButtonId);
        }

        /// <summary>
        /// AddMouseVerticalWheelScroll
        /// </summary>
        /// <param name="scrollAmount"></param>
        /// <returns></returns>
        public InputBuilder AddMouseVerticalWheelScroll(int scrollAmount)
        {
            var scroll = new INPUT();
            scroll.Type = (UInt32) InputType.Mouse;
            scroll.Data.Mouse.Flags = (UInt32) MouseFlag.VerticalWheel;
            scroll.Data.Mouse.MouseData = (UInt32) scrollAmount;
            
            _inputList.Add(scroll);

            return this;
        }

        /// <summary>
        /// AddMouseHorizontalWheelScroll
        /// </summary>
        /// <param name="scrollAmount"></param>
        /// <returns></returns>
        public InputBuilder AddMouseHorizontalWheelScroll(int scrollAmount)
        {
            var scroll = new INPUT();
            scroll.Type = (UInt32) InputType.Mouse;
            scroll.Data.Mouse.Flags = (UInt32) MouseFlag.HorizontalWheel;
            scroll.Data.Mouse.MouseData = (UInt32) scrollAmount;
            
            _inputList.Add(scroll);

            return this;
        }

        #endregion
    }
}
//TICS +10@301
//TICS +3@107
//TICS +3@104
//TICS +3@102
