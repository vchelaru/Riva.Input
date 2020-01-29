using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// C:\Program Files (x86)\Reference Assemblies\Microsoft\WMI\v1.0\Microsoft.Management.Infrastructure.dll
//using Microsoft.Management.Infrastructure;

// original .NET namespace. APIs in this namespace generally are slower and do not scale as well.
using System.Management; // for getting data from system
//using System.Management.Instrumentation; // for controling system and programs

// Debug
using System.Diagnostics;

namespace Riva.Input
{
    public static class WMI
    {
        private const string Namespace = @"\\.\root\cimv2";
        private const string ClassName = @"Win32_PNPEntity"; // does include all the USB devices, and hundreds more non-USB devices
        private const string DeviceID  = @"DeviceID";

        public const string XBoxControllerDeviceIDMarker = "IG_";
        public const string VIDprefix = "VID_";
        public const string PIDprefix = "PID_";

        /*  MI Win32_PnPEntity class description
        https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/win32-pnpentity

        --- IDs

        ! DeviceID
        Identifier of the Plug and Play device
        I have seen "USB\%", "USBSTOR\%", "USBPRINT\%", "BTH\%", "SWD\%", and "HID\%".

        PNPDeviceID
        Windows Plug and Play device identifier of the logical device

        CompatibleID []
        A vendor-defined identification string 
        that Setup uses to match a device to an INF file. 
        A device can have a list of compatible IDs associated with it.

        HardwareID []
        A vendor-defined identification string 
        that Setup uses to match a device to an INF file.
        Normally, a device has an associated list of hardware IDs.

        ! ClassGuid
        Globally unique identifier (GUID) of this Plug and Play device

        --- Names

        Name
        Label by which the object is known.

        x SystemName
        Name of the scoping system

        ? CreationClassName
        Name of the first concrete class to appear in the inheritance chain used in the creation of an instance. 
        When used with the other key properties of the class, 
        the property allows all instances of this class and its subclasses to be uniquely identified.
        
        ? SystemCreationClassName
        Value of the scoping computer's CreationClassName property

        --- Descriptions

        Description
        Description of the object (yea, thanks a lot for explanation MS)
            HID-compliant game controller
            ACPI Fan
            NVIDIA Virtual Audio Device (Wave Extensible) (WDM)

        Caption
        Short description of the object

        x PNPClass
        The name of the type of this Plug and Play device
        doesn't really exist in Win32_PnPEntity
        */

        /* A GUID is a 128-bit value consisting of one group of. is 128-bit integer (16 bytes).
			    8 hexadecimal digits, 
			    followed by three groups of 4 hexadecimal digits each, 
			    followed by one group of 12 hexadecimal digits. 
		        = total of 32 hexa digits

			    C++: Data1 - Specifies the first 8 hexadecimal digits of the GUID
		        .Net: a (_a) - The first 4 bytes of the GUID. = int32

		        https://docs.microsoft.com/en-us/windows/win32/api/guiddef/ns-guiddef-guid
        */

        /*public static int VID_PID(string deviceID)
        {
            //"VID_%4X"

            const string VIDprefix = "VID_";
            const string PIDprefix = "PID_";

            string VIDstring;
            string PIDstring;

            short VID;
            short PID;

            int VIDsubstringIndex = deviceID.IndexOf(VIDprefix);
            if (VIDsubstringIndex == -1)
            {
                VID = 0;
            }
            else
            {
                VIDstring = deviceID.Substring(VIDsubstringIndex + 4, 4);
                VID = short.Parse(VIDstring, System.Globalization.NumberStyles.HexNumber);
            }

            int PIDsubstringIndex = deviceID.IndexOf(PIDprefix);
            if (PIDsubstringIndex == -1)
            {
                PID = 0;
            }
            else
            {
                PIDstring = deviceID.Substring(PIDsubstringIndex + 4, 4);
                PID = short.Parse(PIDstring, System.Globalization.NumberStyles.HexNumber);
            }

            return MakeLong(VID, PID);
        }*/
        public static uint Get_VID_PID(string deviceID)
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

            return uint.Parse(PID + VID, System.Globalization.NumberStyles.HexNumber);
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


        // --- System.Management (old)
        private static ManagementObjectCollection GetAllPnPDevices()
        {
            using (var searcher = new ManagementObjectSearcher("SELECT DeviceID FROM Win32_PNPEntity"))
                return searcher.Get();
        }

        /// <summary>
        /// Enumerates list of PlugNPlay devices connected to the system.
        /// Finds ones that match XBox Controller signature.
        /// Extracts and returns number that matches first 4 bytes of devices' DirectInput ProductGuid.
        /// </summary>
        /// <param name="collection">ManagementObjectCollection retrieved from WMI.</param>
        /// <returns>
        /// For each XBox controller found returns: 
        /// Number that matches first 4 bytes of that device's DirectInput ProductGuid.
        /// </returns>
        public static uint[] GetAllXBoxControlersVIDPIDsFromPnPDevices(ManagementObjectCollection collection)
        {
            List<uint> xb_VIDPIDs = new List<uint>();

            string deviceID;
            foreach (ManagementObject obj in collection)
            {
                deviceID = (string)obj.GetPropertyValue("DeviceID");
                if ( deviceID.Contains(XBoxControllerDeviceIDMarker) )
                {
                    xb_VIDPIDs.Add( Get_VID_PID(deviceID) );
                }
            }

            return xb_VIDPIDs.Distinct().ToArray();
        }

        /// <summary>
        /// Enumerates list of PlugNPlay devices connected to the system.
        /// Finds ones that match XBox Controller signature.
        /// Extracts and returns number that matches first 4 bytes of devices' DirectInput ProductGuid.
        /// </summary>
        /// <param name="collection">ManagementObjectCollection retrieved from WMI.</param>
        /// <returns>
        /// For each XBox controller found returns: 
        /// string representation of 8 digits hexa number 
        /// that matches first 4 bytes of that device's DirectInput ProductGuid.
        /// </returns>
        public static string[] GetAllXBoxControlersVIDPIDsFromPnPDevices_AsStrings(ManagementObjectCollection collection)
        {
            List<string> xb_VIDPIDs = new List<string>();

            string deviceID;
            foreach (ManagementObject obj in collection)
            {
                deviceID = (string)obj.GetPropertyValue("DeviceID");
                if ( deviceID.Contains(XBoxControllerDeviceIDMarker) )
                {
                    xb_VIDPIDs.Add( Get_PID_VID(deviceID) );
                }
            }

            var comparerOrdNoCase = new StringEqualityComparerOrdinalNoCase();

            return xb_VIDPIDs.Distinct(comparerOrdNoCase).ToArray();
        }

        /// <summary>
        /// Enumerates all PlugNPlay devices connected to the system.
        /// Finds ones that match XBox Controller signature.
        /// Extracts and returns number that matches first 4 bytes of devices' DirectInput ProductGuid.
        /// </summary>
        /// <param name="collection">ManagementObjectCollection retrieved from WMI.</param>
        /// <returns>
        /// For each XBox controller found returns: 
        /// Number that matches first 4 bytes of that device's DirectInput ProductGuid.
        /// </returns>
        public static uint[] GetAllXBoxControlersVIDPIDsFromPnPDevices()
        {
            return GetAllXBoxControlersVIDPIDsFromPnPDevices( GetAllPnPDevices() );
        }

        /// <summary>
        /// Enumerates all PlugNPlay devices connected to the system.
        /// Finds ones that match XBox Controller signature.
        /// Extracts and returns number that matches first 4 bytes of devices' DirectInput ProductGuid.
        /// </summary>
        /// <param name="collection">ManagementObjectCollection retrieved from WMI.</param>
        /// <returns>
        /// For each XBox controller found returns: 
        /// string representation of 8 digits hexa number 
        /// that matches first 4 bytes of that device's DirectInput ProductGuid.
        /// </returns>
        public static string[] GetAllXBoxControlersVIDPIDsFromPnPDevices_AsStrings()
        {
            return GetAllXBoxControlersVIDPIDsFromPnPDevices_AsStrings( GetAllPnPDevices() );
        }

        #region    --- Testing
        private struct PnPEntityInfo
        {
            public string Name;
            public string DeviceID;
            public string PnPDeviceID;
            public string Description;
            /// <summary>
            /// Globally unique identifier (GUID) of this Plug and Play device.
            /// </summary>
            public string GUID;
            //public int VID_PID;
        }

        // --- System.Management (old)
        public static void DbgGetPnPDeviceInfos_ManagementObjectSearcher()
        {
            //System.Management.
            //ConnectServer

            /*//var options = new ConnectionOptions();

            // var myScope = new ManagementScope($@"\{remoteDevice}rootCIMV2", options);
            var myScope = new ManagementScope(Namespace);

            // var query = new ObjectQuery("SELECT Caption,Description,LastBootUpTime,Version,ProductType FROM Win32_OperatingSystem");
            //var query = new ObjectQuery("SELECT DeviceID FROM Win32_PNPEntity");
            var query = new ObjectQuery("SELECT * FROM Win32_PNPEntity");

            var searcher = new ManagementObjectSearcher(myScope, query);

            // option 1
            foreach (ManagementObject obj in searcher.Get())
            {
                obj.
            }*/

            // option 2
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PNPEntity");

            ManagementObjectCollection collection = searcher.Get();
            searcher.Dispose();

            List<PnPEntityInfo> xBoxControllersList = new List<PnPEntityInfo>();
            StringBuilder sb = new StringBuilder();

            string name;
            string deviceID;
            string pnpDeviceID;
            string description;
            string classGUID;
            foreach (ManagementObject obj in collection)
            {
                name = (string)obj.GetPropertyValue("Name");

                // Identifier of the Plug and Play device
                deviceID = (string)obj.GetPropertyValue("DeviceID");

                // Windows Plug and Play device identifier of the logical device.
                pnpDeviceID = (string)obj.GetPropertyValue("PNPDeviceID");

                description = (string)obj.GetPropertyValue("Description");

                // Globally unique identifier(GUID) of this Plug and Play device
                classGUID = (string)obj.GetPropertyValue("ClassGuid");

                sb
                    .AppendLine()
                    .Append("Name: ").AppendLine(name)
                    .Append("DeviceID: ").AppendLine(deviceID)
                    .Append("PNPDeviceID: ").AppendLine(pnpDeviceID)
                    .Append("Description: ").AppendLine(description);

                if (deviceID.Contains(XBoxControllerDeviceIDMarker))
                {
                    xBoxControllersList.Add(
                        new PnPEntityInfo
                        {
                            Name = name,
                            DeviceID = deviceID,
                            PnPDeviceID = pnpDeviceID,
                            Description = description,
                            GUID = classGUID
                        }
                    );
                }
            }

            collection.Dispose();

            Debug.Write(sb);

            _DbgOutputFoundXBoxControllers(xBoxControllersList, sb);
        }

        // --- Microsoft.Management.Infrastructure (new better)
        /*public static void GetPnPDeviceInfos_Cim()
        {
            //Microsoft.Management.Infrastructure.
            // CimSession mySession = CimSession.Create("Computer_B");
            CimSession session = CimSession.Create(Environment.MachineName);

            IEnumerable<CimInstance> queryInstances = session.QueryInstances(Namespace, "WQL", "SELECT * FROM Win32_PNPEntity");

            List<PnPEntityInfo> xBoxControllersList = new List<PnPEntityInfo>();
            StringBuilder sb = new StringBuilder();

            string name;
            string deviceID;
            string pnpDeviceID;
            string description;
            //Guid classGUID;
            string classGUID;

            foreach (CimInstance cimInstance in queryInstances)
            {
                // Show volume information

                name = (string)cimInstance.CimInstanceProperties["Name"].Value;

                // Identifier of the Plug and Play device
                deviceID = (string)cimInstance.CimInstanceProperties["DeviceID"].Value;

                // Windows Plug and Play device identifier of the logical device.
                pnpDeviceID = (string)cimInstance.CimInstanceProperties["PNPDeviceID"].Value;

                description = (string)cimInstance.CimInstanceProperties["Description"].Value;

                // Globally unique identifier(GUID) of this Plug and Play device
                classGUID = (string)cimInstance.CimInstanceProperties["ClassGuid"].Value;

                sb
                    .AppendLine()
                    .Append("Name: ").AppendLine(name)
                    .Append("DeviceID: ").AppendLine(deviceID)
                    .Append("PNPDeviceID: ").AppendLine(pnpDeviceID)
                    .Append("Description: ").AppendLine(description);

                if (deviceID.Contains(XBoxControllerDeviceIDMarker))
                {
                    xBoxControllersList.Add(
                        new PnPEntityInfo
                        {
                            Name = name, DeviceID = deviceID, PnPDeviceID = pnpDeviceID,
                            Description = description, GUID = classGUID
                        }
                    );
                }
            }
            session.Close();
            session.Dispose();

            Debug.Write(sb);

            _OutputFoundXBoxControllers(xBoxControllersList, sb);
        }*/

        private static void _DbgOutputFoundXBoxControllers(List<PnPEntityInfo> xBoxControllersList, StringBuilder sb)
        {
            if (xBoxControllersList.Count > 0)
            {
                Debug.WriteLine("\n\n------------------------\n  xBoxControllers  \n------------------------");

                sb.Clear();

                foreach (var item in xBoxControllersList)
                {
                    sb
                        .AppendLine()
                        .Append("Name: ").AppendLine(item.Name)
                        .Append("DeviceID: ").AppendLine(item.DeviceID)
                        .Append("PNPDeviceID: ").AppendLine(item.PnPDeviceID)
                        .Append("GUID: ").AppendLine(item.GUID)
                        .Append("VID+PID: ").AppendLine(Get_VID_PID(item.DeviceID).ToString())
                        .Append("PID+VID: ").AppendLine(Get_PID_VID(item.DeviceID))
                        //.Append("Description: ").AppendLine(item.Description)
                        ;
                }

                Debug.Write(sb);
            }
        }

        /*public static short HiWord(int dword)
        {
            return (short)(dword >> 16);
        }

        public static short LoWord(int dword)
        {
            return (short)dword;
        }*/

        private static void DbgTests()
        {
            var guid = new Guid();
            guid.ToByteArray(); // Returns a 16-element byte array that contains the value of this instance
        } 
        #endregion --- Testing END
    }
}
