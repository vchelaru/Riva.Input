using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnPDevicesInfoGrabber
{
    class Program
    {
        static void Main(string[] args)
        {
            WMI.SavePnPDeviceInfos_ManagementObjectSearcher();

            DI.SaveDirectInputDeviceInfos();
        }
    }
}
