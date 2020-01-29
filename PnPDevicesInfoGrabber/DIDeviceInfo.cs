using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DirectX.DirectInput;

namespace PnPDevicesInfoGrabber
{
    internal struct DIDeviceInfo
    {
        //
        // Summary:
        //     Device type specifier. The least-significant byte of the device type description
        //     code specifies the device type. The next-significant byte specifies the device
        //     subtype. This value can also be combined with DIDEVTYPE_HID, which specifies
        //     a Human Interface Device (TermHid).
        internal int DeviceSubType;
        //
        // Summary:
        //     Device type specifier. The least-significant byte of the device type description
        //     code specifies the device type. The next-significant byte specifies the device
        //     subtype. This value can also be combined with DIDEVTYPE_HID, which specifies
        //     a Human Interface Device (TermHid).
        internal DeviceType DeviceType;
        //
        // Summary:
        //     Unique identifier for the driver being used for force feedback. The driver's
        //     manufacturer establishes this identifier.
        internal Guid FFDriverGuid;
        //
        // Summary:
        //     Unique identifier for the instance of the device. An application can save the
        //     instance GUID into a configuration file and use it at a later time. Instance
        //     GUIDs are specific to a particular computer. An instance GUID obtained from one
        //     computer is unrelated to instance GUIDs on another.
        internal Guid InstanceGuid;
        //
        // Summary:
        //     Friendly name for the instance, for example, 'Joystick 1.'
        internal string InstanceName;
        //
        // Summary:
        //     Unique identifier for the product. This identifier is established by the manufacturer
        //     of the device.
        internal Guid ProductGuid;
        //
        // Summary:
        //     Friendly name for the product.
        internal string ProductName;
        //
        // Summary:
        //     If the device is a Human Interface Device (HID), this member contains the HID
        //     usage code.
        internal short Usage;
        //
        // Summary:
        //     If the device is a Human Interface Device (HID), this member contains the HID
        //     usage page code.
        internal short UsagePage;

        internal GamingDeviceType GamingDeviceType;


        internal DIDeviceInfo(DeviceInstance deviceInstance, GamingDeviceType gamingDeviceType)
        {
            DeviceSubType = deviceInstance.DeviceSubType;
            DeviceType = deviceInstance.DeviceType;
            FFDriverGuid = deviceInstance.FFDriverGuid;
            InstanceGuid = deviceInstance.InstanceGuid;
            InstanceName = deviceInstance.InstanceName;
            ProductGuid = deviceInstance.ProductGuid;
            ProductName = deviceInstance.ProductName;
            Usage = deviceInstance.Usage;
            UsagePage = deviceInstance.UsagePage;

            GamingDeviceType = gamingDeviceType;
        }

    }
}
