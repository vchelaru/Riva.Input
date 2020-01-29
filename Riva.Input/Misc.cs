using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riva.Input
{
    public static class Misc
    {
        public const string XBoxOneControllerProductName = "Controller (XBOX One For Windows)";

        public static uint PartA(this Guid guid)
        {
            /*string guidString = guid.ToString();    
                                // x ToString("N"); = ad9532e7ade044bcb645d26f866409d0

            string[] guidParts = guidString.Split('-');

            string guidPart1 = guidParts[0];

            return guidPart1;*/

            byte[] guidBytes = guid.ToByteArray();

            uint guidPartA = BitConverter.ToUInt32(guidBytes, 0);

            return guidPartA;
        }
    }
}
