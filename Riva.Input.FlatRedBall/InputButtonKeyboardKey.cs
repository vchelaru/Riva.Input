//using FlatRedBall.Input;
using System;
using System.Collections.Generic;
using XnaInput = Microsoft.Xna.Framework.Input;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Microsoft.Xna.Framework;
using FRBInput = FlatRedBall.Input;

namespace Riva.Input.FlatRedBall
{
    public class InputButtonKeyboardKey : IRPressableInput
    {
        // * GenericController Poll and Buttons Refresh are done by ControlsInputManager (once per frame)

        // --IPressableInput
        public bool IsDown { get { return KeyReference.IsDown; } }

        public bool WasJustPressed { get { return KeyReference.WasJustPressed; } }

        public bool WasJustReleased { get { return KeyReference.WasJustReleased; } }

        // -- Mine
        public FRBInput.KeyReference KeyReference;

        public int ButtonNumber { get { return (int)KeyReference.Key; } }

        /*// - For Settings (serialization / deserialization)
        public Guid? DeviceID { get { return null; } }
        public PlayerIndex? XNAPlayerIndex { get { return null; } }*/

        public eInputDeviceType DeviceType { get { return eInputDeviceType.Keyboard; } }

        


        public InputButtonKeyboardKey(FRBInput.KeyReference keyReference)
        {
            KeyReference = keyReference;
        }
        public InputButtonKeyboardKey(Keys key)
        {
            KeyReference = FRBInput.InputManager.Keyboard.GetKey(key);
        }
        public InputButtonKeyboardKey(Keys key, bool onlyForDebug)
        {
            KeyReference = new FRBInput.KeyReference();
            KeyReference.Key = key;
        }

    }
}
