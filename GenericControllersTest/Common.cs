using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.DirectX.DirectInput;

namespace GenericControllersTest
{
    static class Common
    {
        public static Visibility BoolToVisibility(bool value)
        {
            if (value)
                return Visibility.Visible;

            return Visibility.Collapsed;
        }

        /*public static string DeviceTypeToString(DeviceType type)
        {
            switch (type)
            {
                case DeviceType.Supplemental:
                    return "Supplemental";
                case DeviceType.Remote:
                    return "Remote";
                case DeviceType.ScreenPointer:
                    return "ScreenPointer";
                case DeviceType.DeviceControl:
                    return "DeviceControl";
                case DeviceType.FirstPerson:
                    return "v";
                case DeviceType.Flight:
                    return "Flight";
                case DeviceType.Driving:
                    return "Driving";
                case DeviceType.Gamepad:
                    return "Gamepad";
                case DeviceType.Joystick:
                    return "Joystick";
                case DeviceType.Keyboard:
                    return "Keyboard";
                case DeviceType.Mouse:
                    return "Mouse";
                case DeviceType.Device:
                    return "Device";
                case DeviceType.LimitedGameSubtype:
                    return "LimitedGameSubtype";
                default:
                    return "[out of range " + type + " ]";
            }
        }*/

        public static IEnumerable<Enum> GetUniqueFlags(this Enum flags)
        {
            ulong flag = 1;
            foreach (var value in Enum.GetValues(flags.GetType()).Cast<Enum>())
            {
                ulong bits = Convert.ToUInt64(value);
                while (flag < bits)
                {
                    flag <<= 1;
                }

                if (flag == bits && flags.HasFlag(value))
                {
                    yield return value;
                }
            }
        }
    }
}
