using FlatRedBall.Input;
using System;
using System.Collections.Generic;
using XnaInput = Microsoft.Xna.Framework.Input;

using Riva.Input;


namespace Riva.Input.FlatRedBall
{
    public class InputButton_GenericController_DebugFixed : IPressableInput
    {
        // --IPressableInput
        public bool IsDown { get { return InputDevice.Buttons[2] == XnaInput.ButtonState.Pressed; } }

        public bool WasJustPressed { get { throw new NotSupportedException(); } }

        public bool WasJustReleased { get { throw new NotSupportedException(); } }

        // -- Mine
        public DirectInputGamepad InputDevice;

        // - For Settings (serialization / deserialization)
        public eInputDeviceType DeviceType { get { return eInputDeviceType.DirectInputDevice; } }
        //public Guid? DeviceID { get { return InputDevice.Guid; } }




        public InputButton_GenericController_DebugFixed(DirectInputGamepad inputDevice)
        {
            InputDevice = inputDevice;
        }



        /*public void Poll()
        {
            InputDevice.Buttons.Refresh();
        }*/
    }
}
