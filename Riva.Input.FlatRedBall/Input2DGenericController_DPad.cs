using FlatRedBall.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using FlatRedBall;

using Riva.Input;

namespace Riva.Input.FlatRedBall
{
    public class Input2DGenericController_DPad //: Input2DGenericController_ThumbStickBase
    {
        //public override eThumbStick ThumbStick { get { return eThumbStick.Third; } }




        public Input2DGenericController_DPad(DirectInputDevice inputDevice) : base(inputDevice) {}



        // - IR2DInput
        public override void Poll()
        {
            // Will be called by ControlsInputManager - so it's not called more than once per frame
            //InputDevice.Poll();
            //InputDevice.ThumbSticks.Refresh();

            _Facing = InputDevice.ThumbSticks.Third;

            base.Poll();
        }
    }
}
