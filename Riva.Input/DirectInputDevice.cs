using System;
using Microsoft.DirectX.DirectInput;
using Microsoft.Xna.Framework.Input;


namespace Riva.Input
{
    public class DirectInputDevice
    {
        //public DirectInputDevice(Guid gamepadInstanceGuid, string name, GamingDeviceType type)
        public DirectInputDevice(DeviceInstance deviceInstance, string uniqueName, GamingDeviceType type)
        {
            Name = uniqueName;
            //InstanceGuid = gamepadInstanceGuid;

            RawDevice = new Device(deviceInstance.InstanceGuid);
			RawDevice.SetDataFormat(DeviceDataFormat.Joystick);
			RawDevice.Acquire();

            ThumbSticks = new DirectInputThumbSticks(this);

            // DPads
            int dPadsCount = RawDevice.Caps.NumberPointOfViews;
            DPads = new DirectInputDPad[dPadsCount];
            for (int i = 0; i < dPadsCount; i++)
            {
                DPads[i] = new DirectInputDPad(this, i);
            }

            Buttons = new DirectInputButtons(this);
		}


        /// <summary>
        /// Contains lot of informations about the device. 
        /// <para> System? InstanceName. </para>
        /// <para> Manufacturer? ProductName. </para>
        /// <para> System InstanceGuid. - A device instance ID is persistent across system restarts. </para>
        /// <para> Manufacturer ProductGuid. </para>
        /// <para> MS: Describes an instance of a Microsoft® DirectInput® device. </para>
        /// </summary>
        public DeviceInstance DeviceInfo { get { return RawDevice.DeviceInformation; } }

        public readonly Device RawDevice;


        public readonly DirectInputThumbSticks ThumbSticks;

        public readonly DirectInputDPad[] DPads;

        public readonly DirectInputButtons Buttons;

        /// <summary>
        /// Friendly device name that is unique. Based on device's ProductName.
        /// </summary>
        public readonly string Name = null;
        //public readonly Guid InstanceGuid;

        public readonly GamingDeviceType Type;


        protected JoystickState _DeviceState;
        public JoystickState DeviceState { get { return _DeviceState; } }

        protected bool _DPadsStatesStale = true;
        protected int[] _RawDPadsStates;
        public int[] RawDPadsStates
        {
            get
            {
                if (_DPadsStatesStale)
                {
                    _RawDPadsStates = _DeviceState.GetPointOfView();
                    _DPadsStatesStale = false;
                }

                return _RawDPadsStates;
            }
        }


        /// <summary>Get fresh info of all controlls on this gaming device.</summary>
        public void Poll()
        {
            _DeviceState = RawDevice.CurrentJoystickState;

            _DPadsStatesStale = true;
        }


		#region Diagnostics
		public string DiagnosticsThumbSticks
		{
			get
			{
				return
					"X" + Math.Round(ThumbSticks.Left.X, 4) +
					" Y" + Math.Round(ThumbSticks.Left.Y, 4) +
					" X" + Math.Round(ThumbSticks.Right.X, 4) +
					" Y" + Math.Round(ThumbSticks.Right.Y, 4);
			}
		}

		public string DiagnosticsRawGamepadData
		{
			get
			{
				return
					"X" + RawDevice.CurrentJoystickState.X +
					" Y" + RawDevice.CurrentJoystickState.Y +
					" Z" + RawDevice.CurrentJoystickState.Z +
					" Rx" + RawDevice.CurrentJoystickState.Rx +
					" Ry" + RawDevice.CurrentJoystickState.Ry +
					" Rz" + RawDevice.CurrentJoystickState.Rz +
					" pov[0]" + RawDevice.CurrentJoystickState.GetPointOfView()[0];
			}
		}

		public string DiagnosticsButtons
		{
			get
			{
				System.Text.StringBuilder sb = new System.Text.StringBuilder();

				int i = 0;
				foreach (ButtonState bs in Buttons)
				{
					sb.Append(i);
					sb.Append("=");
					sb.Append((bs == ButtonState.Pressed ? "1" : "0"));
					sb.Append(" ");
					i++;
				}

				return sb.ToString();
			}
		}
		#endregion
	}
}
