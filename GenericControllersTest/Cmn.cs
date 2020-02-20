using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GenericControllersTest
{
    static class Cmn
    {
        public static float RangeConversion(float sourceRangeMin, float sourceRangeMax, float targetRangeMin, float targetRangeMax, float sourceRangeValue)
        {
            var sourceRangeLength = sourceRangeMax - sourceRangeMin;
            if (sourceRangeLength == 0)
                //var rangeTargetValue = rangeTargetMin;
                throw new NotSupportedException("Lenght of source range is 0. Value of 'Source range value' in Target range can not be determined.");

            var targetRangeLength = targetRangeMax - targetRangeMin;
            return (( (sourceRangeValue - sourceRangeMin) * targetRangeLength ) / sourceRangeLength) + targetRangeMin;
        }

        public static float RangeConversion(float aRangeMin, float bRangeMin, float ratioBA, float sourceRangeValue)
        {
            return ((sourceRangeValue - aRangeMin) * ratioBA) + bRangeMin;
        }

        public static string ToStringF4(this Vector2 vector)
        {
            return $"{vector.X.ToString("F4")} {vector.Y.ToString("F4")}";
        }
    }

}
