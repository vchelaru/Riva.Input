using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericControllersTest.XBoxIdentify
{
    public class Match<T1, T2>
    {
        public T1 Item1_FW;
        public T1 Item1 { get { return Item1_FW; } }
        public T2 Item2_FW;
        public T2 Item2 { get { return Item2_FW; } }
    }

    public class MatchDevInfs : Match<
                                    IGrouping<uint, GenericControllersTest.XBoxIdentify.PnPEntityInfo>,
                                    IGrouping<uint, GenericControllersTest.XBoxIdentify.DIDeviceInfo>
                                >
    {
    }
}
