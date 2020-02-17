using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Riva.Input
{
    /// <summary>
    /// The buttons on a PC Gamepad
    /// </summary>
    /// <remarks>
    /// The comparable X, Y, A, B etc are not currently mapped because the position of buttons
    /// indexed 0, 1, 2, etc varies widely from PC gamepad to PC gamepad
    /// Instead, you should provide an interface for the user to visually map the buttons on their
    /// gamepad to what you expect for X, Y, etc.
    /// Tools to support this mapping may be added to this framework in the future
    /// </remarks>
    public class DirectInputButtons : IEnumerable<ButtonState>
    {
        /*
		public ButtonState X;
		public ButtonState Y;
		public ButtonState A;
		public ButtonState B;
		public ButtonState Back;
		public ButtonState Start;
		public ButtonState LeftShoulder;
		public ButtonState RightShoulder;
		public ButtonState LeftStick;
		public ButtonState RightStick;
		*/

        public DirectInputGamepad ParentDevice;


		protected ButtonState[] _ButtonsStates;
        public ButtonState[] ButtonsStates { get { return _ButtonsStates; } }
        
        protected byte[] _RawButtonsStates;

        public ButtonState this[int index]    // Indexer declaration  
        {  
            get { return _ButtonsStates[index]; }
        }


        protected int _NumberOfButtons;
        public int NumberOfButtons { get { return _NumberOfButtons; } }
        public int Count { get { return _NumberOfButtons; } }

        



		public DirectInputButtons(DirectInputGamepad parentDevice)
		{
            ParentDevice = parentDevice;

            _NumberOfButtons = ParentDevice.RawDevice.Caps.NumberButtons;

            _ButtonsStates = new ButtonState[_NumberOfButtons];
		}



        public void Refresh()
        {
            _RawButtonsStates = ParentDevice.RawDeviceState.GetButtons();

			/*
			X = (buttons[0] == 0 ? ButtonState.Released : ButtonState.Pressed);
			Y = (buttons[1] == 0 ? ButtonState.Released : ButtonState.Pressed);
			A = (buttons[2] == 0 ? ButtonState.Released : ButtonState.Pressed);
			B = (buttons[3] == 0 ? ButtonState.Released : ButtonState.Pressed);
			Back = (buttons[4] == 0 ? ButtonState.Released : ButtonState.Pressed);
			Start = (buttons[5] == 0 ? ButtonState.Released : ButtonState.Pressed);
			LeftShoulder = (buttons[6] == 0 ? ButtonState.Released : ButtonState.Pressed);
			RightShoulder = (buttons[7] == 0 ? ButtonState.Released : ButtonState.Pressed);
			LeftStick = (buttons[8] == 0 ? ButtonState.Released : ButtonState.Pressed);
			RightStick = (buttons[9] == 0 ? ButtonState.Released : ButtonState.Pressed);
			*/

			for (int i = 0; i < _NumberOfButtons; i++)
			{
				_ButtonsStates[i] = (_RawButtonsStates[i] == 0 ? ButtonState.Released : ButtonState.Pressed);
			}
        }



        public IEnumerator<ButtonState> GetEnumerator()
        {
            // wrong: (IEnumerator<ButtonState>)_ButtonStates.GetEnumerator();
            return ((IEnumerable<ButtonState>)_ButtonsStates).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _ButtonsStates.GetEnumerator();
        }



        public override string ToString()
        {
            return $"{base.ToString()}{{ParentDevice: {ParentDevice.Name}}}";
        }
    }
}
