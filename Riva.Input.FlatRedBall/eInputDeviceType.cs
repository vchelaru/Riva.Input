using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riva.Input.FlatRedBall
{
    public enum InputDeviceType : byte
    {
        Unset = 0,
        Keyboard,
        Mouse,
        DirectInputDevice,
        XInputDevice,
    }
}
