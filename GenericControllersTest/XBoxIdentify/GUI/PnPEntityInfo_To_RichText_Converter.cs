using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

using System.Windows.Data; // mine - IValueConverter
using System.Globalization; // mine - IValueConverter > CultureInfo

using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;
using System.Collections.Generic;

//using RCommon;

using Riva.Input;
using System.Windows.Media;

// Debug
//using System.Diagnostics;


namespace GenericControllersTest.XBoxIdentify
{
    class PnPEntityInfo_To_RichText_Converter : IMultiValueConverter
    {
        public static readonly FontWeight ImportantFontWeight = FontWeights.Bold;
        public static readonly Brush ImportantBrush = Brushes.White;
        //private static readonly Brush _UnimportantBrush = ; // Gray;


        // from Data to UI
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // - Check input data

            if (values.Length < 2)
                return null;

            // Check right Type for reference type (class, string, array)
            var pnpEntityInfo = values[0] as PnPEntityInfo;
            if (pnpEntityInfo == null)
                return null;

            var textBlock = values[1] as TextBlock;
            if (textBlock == null)
                return null;

            // - Do conversion

            var inlines = textBlock.Inlines;
            inlines.Clear();

            FormatDataIntoRichText(inlines, pnpEntityInfo);

            return null;
        }

        public static void FormatDataIntoRichText(InlineCollection inlines, PnPEntityInfo pnpEntityInfo)
        {
            //Run run;
            inlines.Add(
                new Run($"Name: {pnpEntityInfo.Name}")
                { FontWeight = ImportantFontWeight, Foreground = ImportantBrush }
            );
            inlines.Add(new LineBreak());
            inlines.Add(new LineBreak());
            inlines.Add(new Run($"VID_PID: ") { FontWeight = ImportantFontWeight });
            inlines.Add(
                new Run( pnpEntityInfo.VID_PID.ToString() )
                { FontWeight = ImportantFontWeight, Foreground = ImportantBrush }
            );
            inlines.Add(new LineBreak());
            inlines.Add(new Run($"PID_VID string: ") { FontWeight = ImportantFontWeight });
            inlines.Add(
                new Run( pnpEntityInfo.PID_VIDstring)
                { FontWeight = ImportantFontWeight, Foreground = ImportantBrush }
            );
            inlines.Add(new LineBreak());
            inlines.Add(new LineBreak());

            // Device ID formating
            var deviceID = pnpEntityInfo.DeviceID;

            /* Dbg
            for (int i = 0; i < deviceID.Length; i++)
            {
                Debug.WriteLine($"{i} {deviceID[i]}");
            }*/

            int pidIndex = deviceID.IndexOf(WMI.PIDprefix);
            int vidIndex = deviceID.IndexOf(WMI.VIDprefix);
            int igIndex = deviceID.IndexOf(WMI.XInputDeviceIDMarker);
            int igIDEndIndex = -1;

            //int[] startIndexes = new int[] { pidIndex, vidIndex, igIndex };
            //Array.Sort(startIndexes);
            var startIndexes = new List<int>(3);

            if (pidIndex != -1)
                startIndexes.Add(pidIndex);
            if (vidIndex != -1)
                startIndexes.Add(vidIndex);
            if (igIndex != -1)
            {
                startIndexes.Add(igIndex);
                igIDEndIndex = deviceID.IndexOf('&', igIndex) - 1;
            }

            startIndexes.Sort();

            int unimportantChunkStartIndex = 0; //startIndexes[0] - 1;
            inlines.Add(new Run($"DeviceID: ") { FontWeight = ImportantFontWeight });
            foreach (var startIndex in startIndexes)
            {
                inlines.Add(
                    new Run(
                        //deviceID.Substring(unimportantChunkStartIndex, startIndex - unimportantChunkStartIndex)
                        deviceID.SubstringR(unimportantChunkStartIndex, startIndex - 1)
                    )
                    //{ Foreground = _UnimportantBrush }
                );

                if (startIndex == pidIndex || startIndex == vidIndex)
                {
                    inlines.Add(
                        new Run(
                            deviceID.Substring(startIndex, 8)
                        )
                        { FontWeight = ImportantFontWeight, Foreground = ImportantBrush }
                    );
                    unimportantChunkStartIndex = startIndex + 8;
                }
                else // igIndex
                {
                    inlines.Add(
                        new Run(
                            deviceID.SubstringR(startIndex, igIDEndIndex)
                        )
                        { FontWeight = ImportantFontWeight, Foreground = ImportantBrush }
                    );
                    //unimportantChunkStartIndex = startIndex + 5;
                    unimportantChunkStartIndex = igIDEndIndex;
                }
            }
            inlines.Add(
                new Run( deviceID.Substring(unimportantChunkStartIndex + 1) )
                //{ Foreground = _UnimportantBrush }
            );
            inlines.Add(new LineBreak());

            inlines.Add(new Run($"PnPDeviceID: {pnpEntityInfo.PnPDeviceID}"));
            inlines.Add(new LineBreak());
            inlines.Add(new Run($"ClassGUID: {pnpEntityInfo.ClassGUID}"));
            inlines.Add(new LineBreak());
            inlines.Add(new LineBreak());
            inlines.Add(new Run($"Description: {pnpEntityInfo.Description}"));
            //inlines.Add(new LineBreak());
        }

        // from UI to Data
        public object[] ConvertBack(object values, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // do backward conversion

            throw new NotSupportedException();
        }
    }
}
