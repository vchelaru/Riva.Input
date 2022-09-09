//using FlatRedBall.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

using FRBInput = FlatRedBall.Input;

namespace Riva.Input.FlatRedBall
{
    public class Input2D_Keyboard_8Way_DebugFixed : IR2DInput
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

            if (_Keyboard.KeyDown(Keys.Up) || _Keyboard.KeyDown(Keys.NumPad8))
            {
                _GotInput = true;
                _Facing +=  Riva.Input.FlatRedBall.Input2D_Keyboard_8Way.VECTOR_UP;
            }
            if (_Keyboard.KeyDown(Keys.Down) || _Keyboard.KeyDown(Keys.NumPad2))
            {
                _GotInput = true;

                _Facing += Input2D_Keyboard_8Way.VECTOR_DOWN;
            }
            if (_Keyboard.KeyDown(Keys.Left) || _Keyboard.KeyDown(Keys.NumPad4))
            {
                _GotInput = true;

                _Facing += Input2D_Keyboard_8Way.VECTOR_LEFT;
            }
            if (_Keyboard.KeyDown(Keys.Right) || _Keyboard.KeyDown(Keys.NumPad6))
            {
                _GotInput = true;

                _Facing += Input2D_Keyboard_8Way.VECTOR_RIGHT;
            }

            if (_Keyboard.KeyDown(Keys.NumPad1))
            {
                _GotInput = true;

                _Facing += Input2D_Keyboard_8Way.VECTOR_LEFT_DOWN;
            }
            if (_Keyboard.KeyDown(Keys.NumPad3))
            {
                _GotInput = true;

                _Facing += Input2D_Keyboard_8Way.VECTOR_RIGHT_DOWN;
            }
            if (_Keyboard.KeyDown(Keys.NumPad9))
            {
                _GotInput = true;

                _Facing += Input2D_Keyboard_8Way.VECTOR_RIGHT_UP;
            }
            if (_Keyboard.KeyDown(Keys.NumPad7))
            {
                _GotInput = true;

                _Facing += Input2D_Keyboard_8Way.VECTOR_LEFT_UP;
            }

            if (_GotInput)
            {
                // Does resulting vector have lenght ?
                _Facing.Normalize();

                if (Single.IsNaN(_Facing.X) || Single.IsNaN(_Facing.Y) || Single.IsInfinity(_Facing.X) || Single.IsInfinity(_Facing.Y))
                {
                    // No (is not valid) = Character is not really moving => replace it with Zero

                    // keep facing vector as it was
                    //_FacingVectorN = Vector2.Zero;

                    _Facing.X = 0f;
                    _Facing.Y = 0f;
                    _Length = 0f;

                    _GotInput = false;
                    // facingChanged remains false
                }
                else
                {
                    _Length = _Facing.Length();
                }
            }
        }
    }
}
