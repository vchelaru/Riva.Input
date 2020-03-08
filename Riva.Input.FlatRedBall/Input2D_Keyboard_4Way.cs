//using FlatRedBall.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using FlatRedBall;

using FRBInput = FlatRedBall.Input;

namespace Riva.Input.FlatRedBall
{
    public class Input2D_Keyboard_4Way : IR2DInput
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

        public Keys? KeyUp;
        public Keys? KeyDown;
        public Keys? KeyLeft;
        public Keys? KeyRight;

        private bool _GotInput;

        private Vector2 _Facing;

        private float _Length;

        // - For Settings (serialization / deserialization)
        public InputDeviceType DeviceType { get { return InputDeviceType.Keyboard; } }

        /*public Guid? DeviceID { get { return null; } }
        public PlayerIndex? XNAPlayerIndex { get { return null; } }*/



        // --- Methods
        // - IR2DInput
        public void Refresh() // _CalculateVector()
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

                _Facing += Input2D_Keyboard_8Way.VECTOR_UP;
            }
            if (KeyDown.HasValue && _Keyboard.KeyDown(KeyDown.Value))
            {
                _GotInput = true;

                _Facing += Input2D_Keyboard_8Way.VECTOR_DOWN;
            }
            if (KeyLeft.HasValue && _Keyboard.KeyDown(KeyLeft.Value))
            {
                _GotInput = true;

                _Facing += Input2D_Keyboard_8Way.VECTOR_LEFT;
            }
            if (KeyRight.HasValue && _Keyboard.KeyDown(KeyRight.Value))
            {
                _GotInput = true;

                _Facing += Input2D_Keyboard_8Way.VECTOR_RIGHT;
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
