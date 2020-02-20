using FlatRedBall.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using FlatRedBall;

using Riva.Input;

namespace Riva.Input.FlatRedBall
{
    public class Input2D_GenericController_DPad : IR2DInput
    {
        // -- I2DInput
        public float Magnitude
        {
            // Input is unit vector. It's magnitude is always 1 or 0.

            get
            {
                if (ParentDPad.GotInput) return 1f;
                else                     return 0f;
            }
        } 

        public float X { get { return ParentDPad.Vector.X; } }

        /// <summary>Not supported. (Throws NotSupportedException.)</summary>
        public float XVelocity { get { throw new NotSupportedException(); /*return 0f;*/ } }

        public float Y { get { return ParentDPad.Vector.Y; } }

        /// <summary>Not supported. (Throws NotSupportedException.)</summary>
        public float YVelocity { get { throw new NotSupportedException(); /*return 0f;*/ } }

        // - IR2DInput
        /// <summary>
        /// Range -1 to 1
        /// IS normalized vector.
        /// </summary>
        public Vector2 Vector { get { return ParentDPad.Vector; } }
        public bool GotInput { get { return ParentDPad.GotInput; } }

        // -- Mine
        public readonly DirectInputDPad ParentDPad;

        // - For Settings (serialization / deserialization)
        public InputDeviceType DeviceType { get { return InputDeviceType.DirectInputDevice; } }

        public readonly int DPadIndex;



        public Input2D_GenericController_DPad(DirectInputGamepad parentDevice, int dPadIndex)
        {
            ParentDPad = parentDevice.DPads[dPadIndex];
            DPadIndex = dPadIndex;
        }



        // - IR2DInput
        /// <summary>Is empty.</summary>
        public void Refresh()
        {
            // Will be called by ControlsInputManager - so it's not called more than once per frame
            //InputDevice.Poll();
            //InputDevice.ThumbSticks.Refresh();

            // Is calucalted by DirectInputDPad
            //_Facing = ParentDevice.ThumbSticks.Third;
            //_GotInput = ParentDPad.Vector.X != 0f || ParentDPad.Vector.Y != 0f;
        }
    }
}
