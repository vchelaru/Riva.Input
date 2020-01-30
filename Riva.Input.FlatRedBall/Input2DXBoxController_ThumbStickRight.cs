using FlatRedBall.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using FlatRedBall;

using Riva.Input;

namespace Riva.Input.FlatRedBall
{
    public class Input2DXBoxController_ThumbStickRight : Input2DXBoxController_ThumbStickBase, IR2DInput
    {
        // -- I2DInput
        public float Magnitude { get { return (float)InputDevice.RightStick.Magnitude; } }

        public float X { get { return InputDevice.RightStick.Horizontal.Value; } }

        /// <summary>Not supported. (Throws NotSupportedException.)</summary>
        public float XVelocity { get { throw new NotSupportedException(); /*return 0f;*/ } }

        public float Y { get { return InputDevice.RightStick.Vertical.Value; } }

        /// <summary>Not supported. (Throws NotSupportedException.)</summary>
        public float YVelocity { get { throw new NotSupportedException(); /*return 0f;*/ } }

        // - IR2DInput
        /// <summary>
        /// Range -1 to 1
        /// Is not normalized vector !
        /// </summary>
        public Vector2 Vector { get { return InputDevice.RightStick.Position; } }



        public Input2DXBoxController_ThumbStickRight(PlayerIndex xnaPlayerIndex) : base(xnaPlayerIndex, eThumbStick.Right) {}

        public Input2DXBoxController_ThumbStickRight(PlayerIndex xnaPlayerIndex, bool onlyForDebug) 
            : base(xnaPlayerIndex, eThumbStick.Right, onlyForDebug) { }



        // - IR2DInput
        public override void Poll()
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
            _GotInput = InputDevice.RightStick.Horizontal.Value != 0f || InputDevice.RightStick.Vertical.Value != 0f;
        }
    }
}
