using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

//using PnPDevicesInfoGrabber;
using Riva.Input;

// Debug
using System.Diagnostics;

namespace GenericControllersTest.XBoxIdentify
{
    class XInputRecognition
    {
        public List<MatchDevInfs> DeviceInfosPaired;
        public List<IGrouping<uint, GenericControllersTest.XBoxIdentify.PnPEntityInfo>> PnPGroupsUnpaired;
        public List<IGrouping<uint, GenericControllersTest.XBoxIdentify.DIDeviceInfo>> DInputGroupsListUnpaired;

        public void LoadAndCrunchFromGrabberFiles(string pnpDevicesFileNamePath, string dinputDevicesFileNamePath)
        {
            List<GenericControllersTest.XBoxIdentify.PnPEntityInfo> PnPList;
            List<GenericControllersTest.XBoxIdentify.DIDeviceInfo> DInputList;

            // DeSerialize data from files
            LoadFromGrabberFiles(pnpDevicesFileNamePath, dinputDevicesFileNamePath,
                                 out PnPList, out DInputList);

            CrunchData(PnPList, DInputList);
        }

        public void LoadFromGrabberFiles(string pnpDevicesFileNamePath, string dinputDevicesFileNamePath,
                                         out List<GenericControllersTest.XBoxIdentify.PnPEntityInfo> PnPList,
                                         out List<GenericControllersTest.XBoxIdentify.DIDeviceInfo> DInputList)
        {
            // --- DeSerialize data from files
            PnPList = null;
            DInputList = null;

            if (TryDeserializeDataFromXMLFile(
                    pnpDevicesFileNamePath,
                    "PnPDevicesList",
                    out PnPList) == false)
            {
                Debug.WriteLine("PnP devices file doesn't exist.");
                return;
            }

            if (TryDeserializeDataFromXMLFile(
                    dinputDevicesFileNamePath,
                    "DIDevicesList",
                    out DInputList) == false)
            {
                Debug.WriteLine("DInput devices file doesn't exist.");
                return;
            }
        }

        public void CrunchData(List<GenericControllersTest.XBoxIdentify.PnPEntityInfo> PnPList,
                               List<GenericControllersTest.XBoxIdentify.DIDeviceInfo> DInputList)
        {
            // --- Exctract VID-PID and sore them in device infos
            //int[] xBoxDevicesVIDPIDs = GetAllXBoxControlersVIDPIDsFromPnPDevices(PnPList);
            AddXBoxControlersVIDPIDsToPnPDevices(PnPList);
            AddVIDPIDsToDIDevices(DInputList);

            // --- Combine the data
            var PnPDevicesGroupedByVIDPID = PnPList
                .Where(pnpd => pnpd.IsXBoxDevice)
                .GroupBy(pnpd => pnpd.VID_PID).ToList();

            var DInputDevicesGroupedByVIDPID = DInputList
                .GroupBy(did => did.VID_PID).ToList();
         
            DeviceInfosPaired = PairedAndRemaining(
                    PnPDevicesGroupedByVIDPID, DInputDevicesGroupedByVIDPID,
                    out PnPGroupsUnpaired, out DInputGroupsListUnpaired
                );
        }

        private bool TryDeserializeDataFromXMLFile<T>(string xmlFilePathName, string rootElementName, out T data)
        {
            var settingsFile = new FileInfo(xmlFilePathName);
            if (!settingsFile.Exists)
            {
                data = default(T);
                return false;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootElementName));

            using (var stream = settingsFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                data = (T)serializer.Deserialize(stream);
            }

            return true;
        }

        private void AddXBoxControlersVIDPIDsToPnPDevices(List<PnPEntityInfo> collection)
        {
            foreach (var pnpEntityInfo in collection)
            {
                if (pnpEntityInfo.DeviceID.Contains(WMI.XInputDeviceIDMarker))
                {
                    pnpEntityInfo.IsXBoxDevice = true;
                    pnpEntityInfo.VID_PID = WMI.Get_VID_PID(pnpEntityInfo.DeviceID);
                    pnpEntityInfo.PID_VIDstring = WMI.Get_PID_VID(pnpEntityInfo.DeviceID);
                }
            }
        }

        private void AddVIDPIDsToDIDevices(List<DIDeviceInfo> collection)
        {
            foreach (var diDevice in collection)
            {
                diDevice.VID_PID = diDevice.ProductGuid.PartA();
                diDevice.PID_VIDstring = diDevice.ProductGuid.ToString().Split('-')[0];
            }
        }

        private List<MatchDevInfs> PairedAndRemaining(
            List<IGrouping<uint, GenericControllersTest.XBoxIdentify.PnPEntityInfo>> pnpGroupsList, 
            List<IGrouping<uint, GenericControllersTest.XBoxIdentify.DIDeviceInfo>> dinputGroupsList,
            out List<IGrouping<uint, GenericControllersTest.XBoxIdentify.PnPEntityInfo>> pnpGroupsUnpaired,
            out List<IGrouping<uint, GenericControllersTest.XBoxIdentify.DIDeviceInfo>> dinputGroupsListUnpaired
        )
        {
            pnpGroupsUnpaired = new List<IGrouping<uint, GenericControllersTest.XBoxIdentify.PnPEntityInfo>>(pnpGroupsList);
            dinputGroupsListUnpaired = new List<IGrouping<uint, GenericControllersTest.XBoxIdentify.DIDeviceInfo>>(dinputGroupsList);

            var pairsList = new List<MatchDevInfs>();

            IGrouping<uint, GenericControllersTest.XBoxIdentify.PnPEntityInfo> pnpGroup;
            IGrouping<uint, GenericControllersTest.XBoxIdentify.DIDeviceInfo> dinputGroup;
            for (int a = pnpGroupsUnpaired.Count - 1; a > -1; a--)
            {
                pnpGroup = pnpGroupsUnpaired[a];

                for (int b = dinputGroupsListUnpaired.Count - 1; b > -1; b--)
                {
                    dinputGroup = dinputGroupsListUnpaired[b];

                    if (pnpGroup.Key == dinputGroup.Key)
                    {
                        pnpGroupsUnpaired.RemoveAt(a);
                        dinputGroupsListUnpaired.RemoveAt(b);

                        pairsList.Add(
                            new MatchDevInfs { Item1_FW = pnpGroup, Item2_FW = dinputGroup }
                        );
                        break;
                    }
                }
            }

            return pairsList;
        }
        /*private IEnumerable<Match<T1,T2>> PairBy<T1,T2>(IList<T1> collectionA, IList<T2> collectionB)
        {
            T1 itemA;
            T2 itemB;
            for (int a = collectionA.Count - 1; a > -1; a--)
            {
                itemA = collectionA[a];

                for (int b = collectionB.Count - 1; b > -1; b--)
                {
                    itemB = collectionB[b];

                    if (itemA)
                    {

                    }
                }
            }
        }*/
    }
}