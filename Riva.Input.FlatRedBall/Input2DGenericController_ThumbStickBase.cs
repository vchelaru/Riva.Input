using FlatRedBall.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using FlatRedBall;

using Riva.Input;

namespace Riva.Input.FlatRedBall
{
    public abstract class Input2DGenericController_ThumbStickBase : IR2DInput
    {
        // -- I2DInput
        public float Magnitude { get { return _Length; } }

        public float X { get { return _Facing.X; } }

        /// <summary>Not supported. (Throws NotSupportedException.)</summary>
        public float XVelocity { get { throw new NotSupportedException(); /*return 0f;*/ } }

        public float Y { get { return _Facing.Y; } }

        /// <summary>Not supported. (Throws NotSupportedException.)</summary>
        public float YVelocity { get { throw new NotSupportedException(); /*return 0f;*/ } }

        // - IR2DInput
        /// <summary>
        /// Range -1 to 1
        /// Is not normalized vector !
        /// </summary>
        public Vector2 Vector { get { return _Facing; } }
        public bool GotInput { get { return _GotInput; } }

        // -- Mine
        public readonly DirectInputDevice InputDevice;

        protected Vector2 _Facing;

        protected float _Length;

        protected bool _GotInput;

        // - For Settings (serialization / deserialization)
        public eInputDeviceType DeviceType { get { return eInputDeviceType.GenericController; } }
        /*public Guid? DeviceID { get { return InputDevice.Guid; } }
        public PlayerIndex? XNAPlayerIndex { get { return null; } }*/

        //public abstract eThumbStick ThumbStick { get; } 


        public Input2DGenericController_ThumbStickBase(DirectInputDevice inputDevice)
        {
            InputDevice = inputDevice;
        }



        // --- Methods
        // - IR2DInput
        public virtual void Poll()
        {
            // ** vector Length = √( x2 + y2 ) = √(x*x + y*y)

            // Will be called by ControlsInputManager - so it's not called more than once per frame
            //InputDevice.Poll();
            //InputDevice.ThumbSticks.Refresh();

            // !! This must be implenented on derived class !!
            //_Facing = InputDevice.ThumbSticks.Left;

            if (_Facing.X != 0f || _Facing.Y != 0f) // is not Vector2.Zero
            {
                _GotInput = true;
                _Length = _Facing.Length();
            }
            else
            {
                _GotInput = false;
                _Length = 0f;
            }
        }
    }
}
