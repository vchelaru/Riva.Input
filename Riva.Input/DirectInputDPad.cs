using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Riva.Input
{
    public class DirectInputDPad
	{
        //private float PI_OVER_180 = 0.0174532925f;

		protected ButtonState _Up;
        public ButtonState Up { get { return _Up; } }
		protected ButtonState _Right;
        public ButtonState Right { get { return _Right; } }
		protected ButtonState _Down;
        public ButtonState Down { get { return _Down; } }
		protected ButtonState _Left;
        public ButtonState Left { get { return _Left; } }

        protected Vector2 _Vector;
        /// <summary>Unit vector2 of DPad's direction. [-1 .. 0 .. 1]</summary>
        public Vector2 Vector { get { return _Vector; } }

        protected float _AngleRad;
        /// <summary>Angle of DPad's direction in positive radians. [0 .. 6.28]</summary>
        public float AngleRad { get { return _AngleRad; } }

        protected bool _GotInput;
        public bool GotInput { get { return _GotInput; } }

        public readonly DirectInputGamepad ParentDevice;
        public readonly byte Index;




        public DirectInputDPad(DirectInputGamepad parentDevice, byte index)
		{
            ParentDevice = parentDevice;

            Index = index;
        }



		public void Refresh()
		{
            int directionRaw = ParentDevice.RawDPadsStates[Index];

            //  Direction controllers, such as point-of-view hats. 
            //  The position is indicated in 
            //  hundredths of a degree clockwise from north (away from the user). 
            //  Center position is normally reported as -1; but see Remarks. 

            //   For indicators that have only five positions, the value for a controller is 
            //  -1, 0, 9000, 18000, or 27000.
            //      0  90    180       270
            //      Up Right Down      Left

            //  Diagonal directions 4500, 13500, 22500, 31500

            _Up = ButtonState.Released;
            _Right = ButtonState.Released;
            _Down = ButtonState.Released;
            _Left = ButtonState.Released;

            if (directionRaw == -1)
            {
                _GotInput = false;

                _AngleRad = 0f;

                _Vector.X = 0f;
                _Vector.Y = 0f;
            }
            else
            {
                _GotInput = true;

                // As angle in radians

                int angleDeg = directionRaw / 100;
                _AngleRad = MathHelper.ToRadians(angleDeg);

                // As Vector
                _Vector.X = (float)Math.Sin(_AngleRad);
                _Vector.Y = (float)Math.Cos(_AngleRad);

                // As 4 buttons

                /*if (directionRaw > 31500 || directionRaw < 4500)
                    _Up = ButtonState.Pressed;
                else
                    _Up = ButtonState.Released;

                if (0 < directionRaw && directionRaw < 18000)
                    _Right = ButtonState.Pressed;
                else
                    _Right = ButtonState.Released;

                if (9000 < directionRaw && directionRaw < 27000)
                    _Down = ButtonState.Pressed;
                else
                    _Down = ButtonState.Released;

                if (18000 < directionRaw)
                    _Left = ButtonState.Pressed;
                else
                    _Left = ButtonState.Released;*/

                //  -1
                //  0, 9000, 18000, or 27000
                //  Diagonal directions 4500, 13500, 22500, 31500

                if (directionRaw < 2251)
                {
                    _Up = ButtonState.Pressed; 
                }
                else if (directionRaw <= 6751)
                {
                    _Up = ButtonState.Pressed;
                    _Right = ButtonState.Pressed;
                }
                else if (directionRaw < 11251)      
                {
                    _Right = ButtonState.Pressed;
                }
                else if (directionRaw < 15751)      
                {
                    _Right = ButtonState.Pressed;
                    _Down = ButtonState.Pressed;
                }
                else if (directionRaw < 20251)      
                {
                    _Down = ButtonState.Pressed;
                }
                else if (directionRaw < 24751)      
                {
                    _Down = ButtonState.Pressed;
                    _Left = ButtonState.Pressed;
                }
                else if (directionRaw < 29251)
                {
                    _Left = ButtonState.Pressed;
                }
                else if (directionRaw < 33751)
                {
                    _Left = ButtonState.Pressed;
                    _Up = ButtonState.Pressed;
                }
                else
                {
                    _Up = ButtonState.Pressed;
                }
            }
        }


        public override string ToString()
        {
            return $"{base.ToString()}{{ParentDevice: {ParentDevice.Name}}}";
        }
    }
}
