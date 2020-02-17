//using FlatRedBall.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using FlatRedBall;
using FRBInput = FlatRedBall.Input;

using Riva.Input;


namespace Riva.Input.FlatRedBall
{
    public class Input2D_XBoxController_ThumbStick : IR2DInput
    {
        // -- I2DInput
        public float Magnitude { get { return (float)ParentDevice.LeftStick.Magnitude; } }

        public float X { get { return ParentDevice.LeftStick.Horizontal.Value; } }

        /// <summary>Not supported. (Throws NotSupportedException.)</summary>
        public float XVelocity { get { throw new NotSupportedException(); /*return 0f;*/ } }

        public float Y { get { return ParentDevice.LeftStick.Vertical.Value; } }

        /// <summary>Not supported. (Throws NotSupportedException.)</summary>
        public float YVelocity { get { throw new NotSupportedException(); /*return 0f;*/ } }

        // - IR2DInput
        public bool GotInput { get { return _GotInput; } }

        public PlayerIndex? XNAPlayerIndex { get { return _XNAPlayerIndex; } }

        /// <summary>
        /// Range -1 to 1
        /// Is not normalized vector.
        /// </summary>
        public Vector2 Vector { get { return ParentDevice.LeftStick.Position; } }

        // -- Mine
        protected readonly PlayerIndex _XNAPlayerIndex;

        public readonly FRBInput.Xbox360GamePad ParentDevice;

        protected bool _GotInput;

        //public readonly eThumbStick ThumbStickKind;

        // - For Settings (serialization / deserialization)
        public InputDeviceType DeviceType { get { return InputDeviceType.XInputDevice; } }
        //public Guid? DeviceID { get { return null; } }
        public readonly int ThumbStickIndex;
        


        public Input2D_XBoxController_ThumbStick(PlayerIndex xnaPlayerIndex, int thumbStickIndex)
        {
            _XNAPlayerIndex = xnaPlayerIndex;
            ParentDevice = FRBInput.InputManager.Xbox360GamePads[(int)xnaPlayerIndex];

            if (thumbStickIndex > 1)
                throw new NotSupportedException("XBox controllers do not have more than 2 thumb sticks.");
            ThumbStickIndex = thumbStickIndex;
        }
#if DEBUG
        public Input2D_XBoxController_ThumbStick(PlayerIndex xnaPlayerIndex, int thumbStickIndex, bool onlyForDebug)
        {
            _XNAPlayerIndex = xnaPlayerIndex;

            if (thumbStickIndex > 1)
                throw new NotSupportedException("XBox controllers do not have more than 2 thumb sticks.");
            ThumbStickIndex = thumbStickIndex;
        }
#endif

        // --- Methods
        // - IR2DInput
        public void Refresh()
        {
            // Will be called by ControlsInputManager - so it's not called more than once per frame
            //InputDevice.Poll();
            //InputDevice.ThumbSticks.Refresh();

            /*if (_Facing.X != 0f || _Facing.Y != 0f) // is not Vector2.Zero
            {
                _GotInput = true;
                //_Length = _Facing.Length();
            }
            else
            {
                _GotInput = false;
                //_Length = 0f;
            }*/
            _GotInput = ParentDevice.LeftStick.Horizontal.Value != 0f || ParentDevice.LeftStick.Vertical.Value != 0f;
        }
    }
}
