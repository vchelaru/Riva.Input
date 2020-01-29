using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericControllersTest.XBoxIdentify
{
    public class PnPEntityInfo
    {
        public string Name;
        public string DeviceID;
        public string PnPDeviceID;
        public string Description;
        /// <summary>
        /// Globally unique identifier (GUID) of this Plug and Play device.
        /// </summary>
        public string ClassGUID;


        public uint VID_PID;
        public string PID_VIDstring;
        public bool IsXBoxDevice;

        public object RTag_FW;
        public object RTag { get { return RTag_FW; } }

        public override string ToString()
        {
            return
$@"Name: {Name}

VID_PID: {VID_PID}
PID_VIDstring: {PID_VIDstring}
DeviceID: {DeviceID}
PnPDeviceID {PnPDeviceID}
ClassGUID: {ClassGUID}

Description: {Description}";
        }
    }
}
