using Microsoft.DirectX.DirectInput;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Riva.Input
{
    public class DirectInputManager
    {
        protected static List<DirectInputGamepad> _Devices;
        /// <summary>List of currently connected HID devices of Microsoft.DirectX.DirectInput.DeviceType Gamepad and Joystick.</summary>
		public static List<DirectInputGamepad> Devices
		{
			get
			{
				if (_Devices == null)
					ReloadDevices();

				return _Devices;
			}
		}

        /// <summary>
        /// This does not automaticaly update Devices. ReloadDevices() needs to be called after changing this variable, to update Devices list.
        /// Setting this to true makes getting Devices = calling ReloadDevices() a lot more expensive operation !
        /// </summary>
        /// <remarks>
        /// When set to true, ReloadDevices() will go thru whole system PlugNPlay devices list via WMI. <br>
        /// That is a slow operation.
        /// </remarks>
        public static bool ExcludeXInputDevices = false;


        /// <summary>
        /// Normally for internal use only. Call if user has attached new Gamepads,
        /// or detached Gamepads you want discarded.
        /// Otherwise, loaded once on first Devices request and does not reflect changes in gamepads attachment.
        /// TODO: Do this better ?
        /// </summary>
        public static void ReloadDevices()
		{
            if (_Devices == null)
                _Devices = new List<DirectInputGamepad>(16);
            else
                _Devices.Clear();

            // gamepads are generally misidentified as Joysticks in DirectInput... get both

            var devices =
                Enumerable.Concat(
                    Manager.GetDevices(DeviceType.Gamepad, EnumDevicesFlags.AttachedOnly)
                    .Cast<DeviceInstance>()
                    .Select(di => new { DeviceInstance = di, GamingDeviceType = GamingDeviceType.Gamepad })
                    ,

                    Manager.GetDevices(DeviceType.Joystick, EnumDevicesFlags.AttachedOnly)
                    .Cast<DeviceInstance>()
                    .Select(di => new { DeviceInstance = di, GamingDeviceType = GamingDeviceType.Joystick })
                );

            if (ExcludeXInputDevices)
            {
                uint[] xInputDevices_VIDPIDs = WMI.GetAllXInputDevicesVIDPIDsFromPnPDevices();

                devices = devices
                    .Where(dii => xInputDevices_VIDPIDs.Contains( dii.DeviceInstance.ProductGuid.PartA() ) == false);
            }

            var comparerOrdNoCase = new StringEqualityComparerOrdinalNoCase();

            var groups = devices
                //.GroupBy(di => di.ProductName, stringEqualityComparerOrdinal);
                .ToLookup(a => a.DeviceInstance.ProductName, comparerOrdNoCase);
            
            foreach (var group in groups)
            {
                if (group.Count() == 1)
                {
                    var deviceInfo = group.First();
                    _Devices.Add(
                        new DirectInputGamepad(
                            deviceInfo.DeviceInstance, 
                            deviceInfo.DeviceInstance.ProductName, 
                            deviceInfo.GamingDeviceType
                        )
                    );
                }
                else
                {
                    int i = 1;
                    foreach (var deviceInfo in group)
                    {
                        _Devices.Add(
                            new DirectInputGamepad(
                                deviceInfo.DeviceInstance,
                                $"{deviceInfo.DeviceInstance.ProductName} [{i}]",
                                deviceInfo.GamingDeviceType
                            )
                        );
                        i++;
                    }
                }
            }
		}
    }
}
