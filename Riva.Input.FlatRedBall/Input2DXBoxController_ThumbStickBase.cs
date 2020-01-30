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
    public abstract class Input2DXBoxController_ThumbStickBase 
    {
        // - IR2DInput
        public bool GotInput { get { return _GotInput; } }

        public PlayerIndex? XNAPlayerIndex { get { return _XNAPlayerIndex; } }

        // -- Mine
        protected readonly PlayerIndex _XNAPlayerIndex;

        public readonly FRBInput.Xbox360GamePad InputDevice;

        protected bool _GotInput;

        public readonly eThumbStick ThumbStickKind;

        // - For Settings (serialization / deserialization)
        public eInputDeviceType DeviceType { get { return eInputDeviceType.XBoxController; } }
        //public Guid? DeviceID { get { return null; } }



        public Input2DXBoxController_ThumbStickBase(PlayerIndex xnaPlayerIndex, eThumbStick thumbStickKind)
        {
            _XNAPlayerIndex = xnaPlayerIndex;

            InputDevice = FRBInput.InputManager.Xbox360GamePads[(int)xnaPlayerIndex];

            if (thumbStickKind == eThumbStick.Third)
                throw new NotSupportedException("XBox controllers do not have third thumb stick.");

            ThumbStickKind = thumbStickKind;
        }

        public Input2DXBoxController_ThumbStickBase(PlayerIndex xnaPlayerIndex, eThumbStick thumbStickKind, bool onlyForDebug)
        {
            _XNAPlayerIndex = xnaPlayerIndex;

            if (thumbStickKind == eThumbStick.Third)
                throw new NotSupportedException("XBox controllers do not have third thumb stick.");

            ThumbStickKind = thumbStickKind;
        }


        // --- Methods
        // - IR2DInput
        public abstract void Poll();
    }
}
