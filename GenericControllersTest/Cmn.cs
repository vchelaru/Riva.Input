using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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


        private static readonly Thickness _GUI_BottonIndicatorMargin = new Thickness(4);
        private static readonly Thickness _GUI_BottonIndicatorBorderThickness = new Thickness(1);

        public static Border CreateBottonIndicator(string text)
        {
            return new Border
            {
                BorderThickness = _GUI_BottonIndicatorBorderThickness,
                BorderBrush = Brushes.White,
                Margin = _GUI_BottonIndicatorMargin,
                MinWidth = 24d,
                MinHeight = 24d,

                Child = new TextBlock
                {
                    Foreground = Brushes.White,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Text = text
                }
            };
        }
    }

}
