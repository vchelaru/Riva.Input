//using FlatRedBall.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using FlatRedBall;

using FRBInput = FlatRedBall.Input;

namespace Riva.Input.FlatRedBall
{
    public class Input2DKeyboard8Way : IR2DInput
    {
        // -- I2DInput
        public float Magnitude { get { return _Length; } }

        public float X { get { return _Facing.X; } }

        public float XVelocity { get { return 0f; } }

        public float Y { get { return _Facing.Y; } }

        public float YVelocity { get { return 0f; } }

        // - IR2DInput
        public Vector2 Vector { get { return _Facing; } }

        public bool GotInput { get { return _GotInput; } }

        // -- Mine
        private FRBInput.Keyboard _Keyboard;

        /*private readonly Dictionary<Keys, Vector2> _INPUTKeysVelocityVectors = new Dictionary<Keys, Vector2>(8)
        {
            Keys.Up,        new Vector2(0, 1)
            Keys.Down,      new Vector2(0, -1)
            Keys.Left,      new Vector2(-1, 0)
            Keys.Right,     new Vector2(1, 0)

            Keys.NumPad1,   new Vector2(-1, -1) ); // left - down
            Keys.NumPad9,   new Vector2(1, 1)  ); // right - up
            Keys.NumPad3,   new Vector2(1, -1) ); // right - down
            Keys.NumPad7,   new Vector2(-1, 1) ); // left - up
            
        };*/
        public static readonly Vector2 VECTOR_UP = new Vector2(0, 1);
        public static readonly Vector2 VECTOR_DOWN = new Vector2(0, -1);
        public static readonly Vector2 VECTOR_LEFT = new Vector2(-1, 0);
        public static readonly Vector2 VECTOR_RIGHT = new Vector2(1, 0);

        public static readonly Vector2 VECTOR_LEFT_DOWN = new Vector2(-1, -1);
        public static readonly Vector2 VECTOR_RIGHT_UP = new Vector2(1, 1);
        public static readonly Vector2 VECTOR_RIGHT_DOWN = new Vector2(1, -1);
        public static readonly Vector2 VECTOR_LEFT_UP = new Vector2(-1, 1);

        public Keys? KeyUp;
        public Keys? KeyDown;
        public Keys? KeyLeft;
        public Keys? KeyRight;

        public Keys? KeyLeftDown;
        public Keys? KeyRightUp;
        public Keys? KeyRightDown;
        public Keys? KeyLeftUp;

        private bool _GotInput;

        private Vector2 _Facing;

        private float _Length;

        // - For Settings (serialization / deserialization)
        public eInputDeviceType DeviceType { get { return eInputDeviceType.Keyboard; } }

        /*public Guid? DeviceID { get { return null; } }
        public PlayerIndex? XNAPlayerIndex { get { return null; } }*/



        // --- Methods
        // - IR2DInput
        public void Poll() // _CalculateVector()
        {
            _Keyboard = FRBInput.InputManager.Keyboard;

            // ** vector Length = √( x2 + y2 ) = √(x*x + y*y)
            //    vector x y from length = 

            //MathFunctions.AngleToVector()

            //Vector2 facingVector = Vector2.Zero;
            _Facing.X = 0f;
            _Facing.Y = 0f;
            _Length = 0f;

            if (KeyUp.HasValue && _Keyboard.KeyDown(KeyUp.Value))
            {
                _GotInput = true;

                _Facing += VECTOR_UP;
            }
            if (KeyDown.HasValue && _Keyboard.KeyDown(KeyDown.Value))
            {
                _GotInput = true;

                _Facing += VECTOR_DOWN;
            }
            if (KeyLeft.HasValue && _Keyboard.KeyDown(KeyLeft.Value))
            {
                _GotInput = true;

                _Facing += VECTOR_LEFT;
            }
            if (KeyRight.HasValue && _Keyboard.KeyDown(KeyRight.Value))
            {
                _GotInput = true;

                _Facing += VECTOR_RIGHT;
            }

            if (KeyLeftDown.HasValue && _Keyboard.KeyDown(KeyLeftDown.Value))
            {
                _GotInput = true;

                _Facing += VECTOR_LEFT_DOWN;
            }
            if (KeyRightDown.HasValue && _Keyboard.KeyDown(KeyRightDown.Value))
            {
                _GotInput = true;

                _Facing += VECTOR_RIGHT_DOWN;
            }
            if (KeyRightUp.HasValue && _Keyboard.KeyDown(KeyRightUp.Value))
            {
                _GotInput = true;

                _Facing += VECTOR_RIGHT_UP;
            }
            if (KeyLeftUp.HasValue && _Keyboard.KeyDown(KeyLeftUp.Value))
            {
                _GotInput = true;

                _Facing += VECTOR_LEFT_UP;
            }

            if (_GotInput)
            {
                // Does resulting vector have lenght ?
                _Facing.Normalize();

                if (Single.IsNaN(_Facing.X) || Single.IsNaN(_Facing.Y) || Single.IsInfinity(_Facing.X) || Single.IsInfinity(_Facing.Y))
                {
                    // _Facing is not valid = Character is not really moving => replace it with Zero

                    _Facing.X = 0f;
                    _Facing.Y = 0f;
                    _Length = 0f;

                    _GotInput = false;
                }
                else
                {
                    _Length = _Facing.Length();
                }
            }
        }
    }
}
