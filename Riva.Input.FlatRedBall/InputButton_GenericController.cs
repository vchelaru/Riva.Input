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
        private bool _IsDown;
        public bool IsDown { get { return _IsDown; } }

        bool _WasJustPressed;
        public bool WasJustPressed 
        {
            get { return _WasJustPressed; }
        }

        bool _WasJustReleased;
        public bool WasJustReleased
        {
            get { return _WasJustReleased; }
        }

        // -- IRPressableInput
        protected int _ButtonNumber;
        public int ButtonNumber { get { return _ButtonNumber; } }

        // For Settings (serialization / deserialization)
        public InputDeviceType DeviceType { get { return InputDeviceType.DirectInputDevice; } }

        /*public Guid? DeviceID { get { return InputDevice.Guid; } }
public PlayerIndex? XNAPlayerIndex { get { return null; } }*/

        // -- Mine
        public readonly DirectInputGamepad ParentDevice;

        private bool _WasLastTimeDown;
        public bool WasLastTimeDown { get { return _WasLastTimeDown; } }

        /*public InputButtonGenericController(DirectInputDevice inputDevice)
        {
            InputDevice = inputDevice;
        }*/
        public InputButton_GenericController(DirectInputGamepad parentDevice, int buttonNumber)
        {
            ParentDevice = parentDevice;
            _ButtonNumber = buttonNumber;
        }

        public void Refresh()
        {
            _WasLastTimeDown = _IsDown;
            _IsDown = ParentDevice.Buttons[_ButtonNumber] == XnaInput.ButtonState.Pressed;

            _WasJustPressed = _WasLastTimeDown == false && IsDown;
            _WasJustReleased = _WasLastTimeDown == true && IsDown == false;
        }
    }
}
