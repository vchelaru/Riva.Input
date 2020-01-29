using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DirectX.DirectInput;
using Riva.Input;

//using GamingDeviceType = Riva.Input.GamingDeviceType;

namespace GenericControllersTest.XBoxIdentify
{
    public class DIDeviceInfo
    {
        //
        // Summary:
        //     Device type specifier. The least-significant byte of the device type description
        //     code specifies the device type. The next-significant byte specifies the device
        //     subtype. This value can also be combined with DIDEVTYPE_HID, which specifies
        //     a Human Interface Device (TermHid).
        public int DeviceSubType;
        //
        // Summary:
        //     Device type specifier. The least-significant byte of the device type description
        //     code specifies the device type. The next-significant byte specifies the device
        //     subtype. This value can also be combined with DIDEVTYPE_HID, which specifies
        //     a Human Interface Device (TermHid).
        public DeviceType DeviceType;
        //
        // Summary:
        //     Unique identifier for the driver being used for force feedback. The driver's
        //     manufacturer establishes this identifier.
        public Guid FFDriverGuid;
        //
        // Summary:
        //     Unique identifier for the instance of the device. An application can save the
        //     instance GUID into a configuration file and use it at a later time. Instance
        //     GUIDs are specific to a particular computer. An instance GUID obtained from one
        //     computer is unrelated to instance GUIDs on another.
        public Guid InstanceGuid;
        //
        // Summary:
        //     Friendly name for the instance, for example, 'Joystick 1.'
        public string InstanceName;
        //
        // Summary:
        //     Unique identifier for the product. This identifier is established by the manufacturer
        //     of the device.
        public Guid ProductGuid;
        //
        // Summary:
        //     Friendly name for the product.
        public string ProductName;
        //
        // Summary:
        //     If the device is a Human Interface Device (HID), this member contains the HID
        //     usage code.
        public short Usage;
        //
        // Summary:
        //     If the device is a Human Interface Device (HID), this member contains the HID
        //     usage page code.
        public short UsagePage;

        public GamingDeviceType RGamingDeviceType;

        public uint VID_PID;
        public string PID_VIDstring;

        public object RTag_FW;
        public object RTag { get { return RTag_FW; } }

        public DIDeviceInfo() { }

        public DIDeviceInfo(DeviceInstance deviceInstance, GamingDeviceType gamingDeviceType)
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

            RGamingDeviceType = gamingDeviceType;
        }




        /*public IEnumerable<string> PropsToStrings()
        {
            yield return DeviceSubType.ToString();
            yield return DeviceType.ToString();
            FFDriverGuid;
            InstanceGuid;
            InstanceName;
            ProductGuid;
            ProductName;
            Usage;
            UsagePage;
            GamingDeviceType;
            VID_PID;
        }*/
        public override string ToString()
        {
            return
$@"ProductName {ProductName}
InstanceName {InstanceName}

VID_PID: {VID_PID}
PID_VIDstring: {PID_VIDstring}
ProductGuid: {ProductGuid}
InstanceGuid: {InstanceGuid}
FFDriverGuid: {FFDriverGuid}

DeviceType: {DeviceType}
DeviceSubType: {DeviceSubType}
GamingDeviceType {RGamingDeviceType}

Usage: {Usage}
UsagePage {UsagePage}";
        }
    }
}
