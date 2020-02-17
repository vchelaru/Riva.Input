using System;
using System.Collections.Generic;
using System.Linq;

namespace Riva.Input.FlatRedBall
{
    public static class InputManager
    {
        public static void UpdateAllDirectInputDevices()
        {
            foreach (var device in Riva.Input.DirectInputManager.Devices)
            {
                UpdateDirectInputDevice(device);
            }
        }

        public static void UpdateDirectInputDevice(DirectInputGamepad device)
        {
            device.Poll();
            device.Buttons.Refresh();
            device.ThumbSticks.Refresh();
        }
    }
}