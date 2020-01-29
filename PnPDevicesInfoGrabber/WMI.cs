using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PnPDevicesInfoGrabber
{
    class WMI
    {
        private const string Namespace = @"\\.\root\cimv2";
        private const string ClassName = @"Win32_PNPEntity"; // does include all the USB devices, and hundreds more non-USB devices

        private const string XBoxControllerDeviceIDMarker = "IG_";
        const string VIDprefix = "VID_";
        const string PIDprefix = "PID_";



        // --- System.Management (old, but works on Win7)
        public static void SavePnPDeviceInfos_ManagementObjectSearcher()
        {
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PNPEntity");

            ManagementObjectCollection collection = searcher.Get();
            searcher.Dispose();

            /*// ? Needs assebly System.Management.XmlSerializers, that does not exist ?
            XmlSerializer serializer = new XmlSerializer(typeof(ManagementObjectCollection));
            using (TextWriter writer = new StreamWriter("PnPDevices.xml"))
            {
                serializer.Serialize(writer, collection);
            }*/


            List<PnPEntityInfo> deviceInfosList = new List<PnPEntityInfo>();

            foreach (ManagementObject obj in collection)
            {
                deviceInfosList.Add(
                    new PnPEntityInfo
                    {
                        Name = (string)obj.GetPropertyValue("Name"),
                        DeviceID = (string)obj.GetPropertyValue("DeviceID"),
                        PnPDeviceID = (string)obj.GetPropertyValue("PNPDeviceID"),
                        ClassGUID = (string)obj.GetPropertyValue("ClassGuid"),
                        Description = (string)obj.GetPropertyValue("Description"),
                    }
                );
            }

            collection.Dispose();

            XmlRootAttribute rootAttribute = new XmlRootAttribute();
            rootAttribute.ElementName = "PnPDevicesList";
            rootAttribute.IsNullable = true;

            XmlSerializer serializer = new XmlSerializer(typeof(List<PnPEntityInfo>), rootAttribute);
            using (TextWriter writer = new StreamWriter("PnPDevices.xml"))
            {
                serializer.Serialize(writer, deviceInfosList);
            }
        }

        public static int Get_VID_PID(string deviceID)
        {
            //"VID_%4X"

            const string ZERO_ID = "0000";

            string VID;
            string PID;

            int VIDsubstringIndex = deviceID.IndexOf(VIDprefix);
            if (VIDsubstringIndex == -1)
            {
                VID = ZERO_ID;
            }
            else
            {
                VID = deviceID.Substring(VIDsubstringIndex + 4, 4);
            }

            int PIDsubstringIndex = deviceID.IndexOf(PIDprefix);
            if (PIDsubstringIndex == -1)
            {
                PID = ZERO_ID;
            }
            else
            {
                PID = deviceID.Substring(PIDsubstringIndex + 4, 4);
            }

            return int.Parse(PID + VID, System.Globalization.NumberStyles.HexNumber);
        }

        public static int MakeLong(short lowPart, short highPart)
        {
            return (int)(((ushort)lowPart) | (uint)(highPart << 16));
        }

        public static string Get_PID_VID(string deviceID)
        {
            //"VID_%4X"

            const string zeroID = "0000";

            string VID;
            string PID;

            int VIDsubstringIndex = deviceID.IndexOf(VIDprefix);
            if (VIDsubstringIndex == -1)
            {
                VID = zeroID;
            }
            else
            {
                VID = deviceID.Substring(VIDsubstringIndex + 4, 4);
            }

            int PIDsubstringIndex = deviceID.IndexOf(PIDprefix);
            if (PIDsubstringIndex == -1)
            {
                PID = zeroID;
            }
            else
            {
                PID = deviceID.Substring(PIDsubstringIndex + 4, 4);
            }

            return PID + VID;
        }
    }
}
