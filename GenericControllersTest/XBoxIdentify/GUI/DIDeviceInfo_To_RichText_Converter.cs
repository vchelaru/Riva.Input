using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

using System.Windows.Data; // mine - IValueConverter
using System.Globalization; // mine - IValueConverter > CultureInfo
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;
using Riva.Input;
using System.Collections.Generic;

// Debug
using System.Diagnostics;


namespace GenericControllersTest.XBoxIdentify
{
    class DIDeviceInfo_To_RichText_Converter : IMultiValueConverter
    {
        // from Data to UI
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // - Check input data

            if (values.Length < 2)
                return null;

            // Check right Type for reference type (class, string, array)
            var diDeviceInfo = values[0] as DIDeviceInfo;
            if (diDeviceInfo == null)
                return null;

            var textBlock = values[1] as TextBlock;
            if (textBlock == null)
                return null;

            // - Do conversion

            var inlines = textBlock.Inlines;
            inlines.Clear();

            FormatDataIntoRichText(inlines, diDeviceInfo);

            return null;
        }

        public static void FormatDataIntoRichText(InlineCollection inlines, DIDeviceInfo diDeviceInfo)
        {
            inlines.Add(
                new Run($"ProductName: {diDeviceInfo.ProductName}")
                {
                    FontWeight = PnPEntityInfo_To_RichText_Converter.ImportantFontWeight,
                    Foreground = PnPEntityInfo_To_RichText_Converter.ImportantBrush
                }
            );
            inlines.Add(new LineBreak());
            inlines.Add(new Run($"InstanceName: {diDeviceInfo.ProductName}"));
            inlines.Add(new LineBreak());
            inlines.Add(new LineBreak());


            inlines.Add(new Run($"VID_PID: ") { FontWeight = PnPEntityInfo_To_RichText_Converter.ImportantFontWeight });
            inlines.Add(
                new Run(diDeviceInfo.VID_PID.ToString())
                {
                    FontWeight = PnPEntityInfo_To_RichText_Converter.ImportantFontWeight,
                    Foreground = PnPEntityInfo_To_RichText_Converter.ImportantBrush
                }
            );
            inlines.Add(new LineBreak());
            inlines.Add(new Run($"PID_VID string: ") { FontWeight = PnPEntityInfo_To_RichText_Converter.ImportantFontWeight });
            inlines.Add(
                new Run(diDeviceInfo.PID_VIDstring.ToString())
                {
                    FontWeight = PnPEntityInfo_To_RichText_Converter.ImportantFontWeight,
                    Foreground = PnPEntityInfo_To_RichText_Converter.ImportantBrush
                }
            );
            inlines.Add(new LineBreak());
            inlines.Add(new LineBreak());


            // ProductGuid formating
            string productGuidString = diDeviceInfo.ProductGuid.ToString();
            int productGuidPartAEndIndex = productGuidString.IndexOf('-');
            inlines.Add(new Run("ProductGuid: ") { FontWeight = PnPEntityInfo_To_RichText_Converter.ImportantFontWeight });
            inlines.Add(
                new Run(productGuidString.SubstringR(0, productGuidPartAEndIndex - 1))
                {
                    FontWeight = PnPEntityInfo_To_RichText_Converter.ImportantFontWeight,
                    Foreground = PnPEntityInfo_To_RichText_Converter.ImportantBrush
                }
            );
            inlines.Add(new Run(productGuidString.Substring(productGuidPartAEndIndex)));
            inlines.Add(new LineBreak());

            inlines.Add(new Run("             " + diDeviceInfo.ProductGuid.ToString("D")));
            inlines.Add(new LineBreak());
            inlines.Add(new Run("             " + diDeviceInfo.ProductGuid.ToStringDecimals()));
            inlines.Add(new LineBreak());

            inlines.Add(new Run($"InstanceGuid: {diDeviceInfo.InstanceGuid}"));
            inlines.Add(new LineBreak());
            inlines.Add(new Run($"FFDriverGuid: {diDeviceInfo.FFDriverGuid}"));
            inlines.Add(new LineBreak());
            inlines.Add(new LineBreak());


            inlines.Add(new Run($"DeviceType: {diDeviceInfo.DeviceType}"));
            inlines.Add(new LineBreak());
            inlines.Add(new Run($"DeviceSubType: {diDeviceInfo.DeviceSubType}"));
            inlines.Add(new LineBreak());
            inlines.Add(new Run($"Riva.Input GamingDeviceType: {diDeviceInfo.RGamingDeviceType}"));
            inlines.Add(new LineBreak());
            inlines.Add(new LineBreak());


            inlines.Add(new Run($"Usage: {diDeviceInfo.Usage}"));
            inlines.Add(new LineBreak());
            inlines.Add(new Run($"UsagePage: {diDeviceInfo.UsagePage}"));
            inlines.Add(new LineBreak());
        }

        // from UI to Data
        public object[] ConvertBack(object values, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // do backward conversion

            throw new NotSupportedException();
        }
    }
}
