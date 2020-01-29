using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnPDevicesInfoGrabber
{
    internal struct PnPEntityInfo
    {
        public string Name;
        public string DeviceID;
        public string PnPDeviceID;
        public string Description;
        /// <summary>
        /// Globally unique identifier (GUID) of this Plug and Play device.
        /// </summary>
        public string ClassGUID;
        //public int VID_PID;
    }
}
