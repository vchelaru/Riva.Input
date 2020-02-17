using FlatRedBall.Input;
using System;
using System.Collections.Generic;
using XnaInput = Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using Riva.Input;


namespace Riva.Input.FlatRedBall
{
    public class InputButton_GenericController : IRPressableInput
    {
        // * GenericController Poll and Buttons Refresh are done by ControlsInputManager (once per frame)

        // --IPressableInput
        public bool IsDown { get { return ParentDevice.Buttons[ _ButtonNumber ] == XnaInput.ButtonState.Pressed; } }

        public bool WasJustPressed { get { throw new NotSupportedException(); } }

        public bool WasJustReleased { get { throw new NotSupportedException(); } }

        // -- IRPressableInput
        protected int _ButtonNumber;
        public int ButtonNumber { get { return _ButtonNumber; } }

        // For Settings (serialization / deserialization)
        public InputDeviceType DeviceType { get { return InputDeviceType.DirectInputDevice; } }
        /*public Guid? DeviceID { get { return InputDevice.Guid; } }
        public PlayerIndex? XNAPlayerIndex { get { return null; } }*/

        // -- Mine
        public readonly DirectInputGamepad ParentDevice;


        /*public InputButtonGenericController(DirectInputDevice inputDevice)
        {
            InputDevice = inputDevice;
        }*/
        public InputButton_GenericController(DirectInputGamepad parentDevice, int buttonNumber)
        {
            ParentDevice = parentDevice;
            _ButtonNumber = buttonNumber;
        }


    }
}
