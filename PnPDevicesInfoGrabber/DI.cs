using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.DirectX.DirectInput;

namespace PnPDevicesInfoGrabber
{
    class DI
    {
        public static void SaveDirectInputDeviceInfos()
        {
            // gamepads are generally misidentified as Joysticks in DirectInput... get both

            DIDeviceInfo[] deviceInfosList =
                Enumerable.Concat(
                    Manager.GetDevices(DeviceType.Gamepad, EnumDevicesFlags.AttachedOnly)
                        .Cast<DeviceInstance>()
                        .Select(di => new DIDeviceInfo(di, GamingDeviceType.Gamepad) )
                    ,
                    Manager.GetDevices(DeviceType.Joystick, EnumDevicesFlags.AttachedOnly)
                        .Cast<DeviceInstance>()
                        .Select(di => new DIDeviceInfo(di, GamingDeviceType.Joystick) )
                )
                .ToArray();

            XmlRootAttribute rootAttribute = new XmlRootAttribute();
            rootAttribute.ElementName = "DIDevicesList";
            rootAttribute.IsNullable = true;

            XmlSerializer serializer = new XmlSerializer(typeof(DIDeviceInfo[]), rootAttribute);
            using (TextWriter writer = new StreamWriter("DIDevices.xml"))
            {
                serializer.Serialize(writer, deviceInfosList);
            }
        }
    }
}
