using FlatRedBall.Input;
using System;
using System.Collections.Generic;
using XnaInput = Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using Riva.Input;


namespace Riva.Input.FlatRedBall
{
    public class InputButtonXBoxController : IRPressableInput
    {
        // * GenericController Poll and Buttons Refresh are done by ControlsInputManager (once per frame)

        // --IPressableInput
        public bool IsDown { get { return _ButtonReference.IsDown; } }

        public bool WasJustPressed { get { return _ButtonReference.WasJustPressed; } }

        public bool WasJustReleased { get { return _ButtonReference.WasJustReleased; } }

        // -- IRPressableInput
        protected int _ButtonNumber;
        public int ButtonNumber { get { return _ButtonNumber; } }

        // For Settings (serialization / deserialization)
        public eInputDeviceType DeviceType { get { return eInputDeviceType.XBoxController; } }
        //public Guid? DeviceID { get { return null; } }
        //public PlayerIndex? XNAPlayerIndex { get { return _XNAPlayerIndex; } }

        // -- Mine
        //protected readonly PlayerIndex _XNAPlayerIndex;
        protected readonly IPressableInput _ButtonReference;

        /*public InputButtonXBoxController(Xbox360GamePad inputDevice)
        {

        }*/
        public InputButtonXBoxController(Xbox360GamePad inputDevice, /*PlayerIndex xnaPlayerIndex,*/ int buttonNumber)
        {
            //_XNAPlayerIndex = xnaPlayerIndex;

            _ButtonReference = inputDevice.GetButton((Xbox360GamePad.Button)buttonNumber);
            _ButtonNumber = buttonNumber;
        }

        public InputButtonXBoxController(Xbox360GamePad inputDevice, int buttonNumber, bool onlyForDebug)
        {
            //_XNAPlayerIndex = xnaPlayerIndex;

            _ButtonNumber = buttonNumber;
        }
    }
}
