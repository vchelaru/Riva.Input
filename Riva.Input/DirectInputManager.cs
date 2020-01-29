using Microsoft.DirectX.DirectInput;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Riva.Input
{
    public class DirectInputManager
    {
        protected static List<DirectInputDevice> _Devices;
        /// <summary>List of currently connected HID devices of type Gamepad and Joystick.</summary>
		public static List<DirectInputDevice> Devices
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
        /// Setting this to true makes getting Devices / calling ReloadDevices() a lot more expensive operation !
        /// </summary>
        /// <remarks>
        /// When set to true, ReloadDevices() will go thru whole system PlugNPlay devices list via WMI. 
        /// That is a slow operation.
        /// </remarks>
        public static bool ExcludeXInputDevices = false;


		/// <summary>
		/// Normally for internal use only; call if user has attached new Gamepads,
		/// or detached Gamepads you want discarded
		/// Otherwise, loaded once on first Gamepad request and does not reflect changes in gamepad attachment.
		/// TODO: Do this better
		/// </summary>
		public static void ReloadDevices()
		{
            // gamepads are generally misidentified as Joysticks in DirectInput... get both

            /*DeviceList gamepadInstanceList = Manager.GetDevices(DeviceType.Gamepad, EnumDevicesFlags.AttachedOnly);
			DeviceList joystickInstanceList = Manager.GetDevices(DeviceType.Joystick, EnumDevicesFlags.AttachedOnly);

            if (_Devices == null)
                _Devices = new List<DirectInputDevice>(gamepadInstanceList.Count + joystickInstanceList.Count);
            else
                _Devices.Clear();

            foreach (DeviceInstance deviceInstance in gamepadInstanceList)
            {
                DirectInputDevice device = new DirectInputDevice(deviceInstance, GamingDeviceType.Gamepad);
                _Devices.Add(device);
            }
            foreach (DeviceInstance deviceInstance in joystickInstanceList)
            {
                DirectInputDevice device = new DirectInputDevice(deviceInstance, GamingDeviceType.Joystick);
                _Devices.Add(device);
            }*/

            if (_Devices == null)
                _Devices = new List<DirectInputDevice>(16);
            else
                _Devices.Clear();

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
                uint[] xBoxes_VIDPIDs = WMI.GetAllXBoxControlersVIDPIDsFromPnPDevices();

                devices = devices
                    .Where(dii => xBoxes_VIDPIDs.Contains( dii.DeviceInstance.ProductGuid.PartA() ) == false);
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
                        new DirectInputDevice(
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
                            new DirectInputDevice(
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
