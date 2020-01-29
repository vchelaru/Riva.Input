using Microsoft.Xna.Framework.Input;

namespace Riva.Input
{
    public class DirectInputDPad
	{
		protected ButtonState _Up;
        public ButtonState Up { get { return _Up; } }
		protected ButtonState _Right;
        public ButtonState Right { get { return _Right; } }
		protected ButtonState _Down;
        public ButtonState Down { get { return _Down; } }
		protected ButtonState _Left;
        public ButtonState Left { get { return _Left; } }

        public readonly DirectInputDevice ParentDevice;
        public readonly int Index;




        public DirectInputDPad(DirectInputDevice parentDevice, int index)
		{
            ParentDevice = parentDevice;

            Index = index;
        }



		public void Refresh()
		{
            int direction = ParentDevice.RawDPadsStates[Index];

            if (direction == -1)
            {
                _Up = ButtonState.Released;
                _Right = ButtonState.Released;
                _Down = ButtonState.Released;
                _Left = ButtonState.Released;
            }
            else
            {
                if (direction > 27000 || direction < 9000)
                    _Up = ButtonState.Pressed;
                else
                    _Up = ButtonState.Released;

                if (0 < direction && direction < 18000)
                    _Right = ButtonState.Pressed;
                else
                    _Right = ButtonState.Released;

                if (9000 < direction && direction < 27000)
                    _Down = ButtonState.Pressed;
                else
                    _Down = ButtonState.Released;

                if (18000 < direction)
                    _Left = ButtonState.Pressed;
                else
                    _Left = ButtonState.Released;
            }
		}
	}
}
