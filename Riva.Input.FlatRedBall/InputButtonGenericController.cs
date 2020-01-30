using FlatRedBall.Input;
using System;
using System.Collections.Generic;
using XnaInput = Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using Riva.Input;


namespace Riva.Input.FlatRedBall
{
    public class InputButtonGenericController : IRPressableInput
    {
        // * GenericController Poll and Buttons Refresh are done by ControlsInputManager (once per frame)

        // --IPressableInput
        public bool IsDown { get { return InputDevice.Buttons[ _ButtonNumber ] == XnaInput.ButtonState.Pressed; } }

        public bool WasJustPressed { get { throw new NotSupportedException(); } }

        public bool WasJustReleased { get { throw new NotSupportedException(); } }

        // -- IRPressableInput
        protected int _ButtonNumber;
        public int ButtonNumber { get { return _ButtonNumber; } }

        // For Settings (serialization / deserialization)
        public eInputDeviceType DeviceType { get { return eInputDeviceType.GenericController; } }
        /*public Guid? DeviceID { get { return InputDevice.Guid; } }
        public PlayerIndex? XNAPlayerIndex { get { return null; } }*/

        // -- Mine
        public readonly DirectInputDevice InputDevice;


        /*public InputButtonGenericController(DirectInputDevice inputDevice)
        {
            InputDevice = inputDevice;
        }*/
        public InputButtonGenericController(DirectInputDevice inputDevice, int buttonNumber)
        {
            InputDevice = inputDevice;
            _ButtonNumber = buttonNumber;
        }


    }
}
