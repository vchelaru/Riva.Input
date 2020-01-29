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
    class DIDeviceInfo_To_RichDoc_Converter : IMultiValueConverter
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

            var textBox = values[1] as RichTextBox;
            if (textBox == null)
                return null;

            // - Do conversion

            var para = (Paragraph)textBox.Document.Blocks.FirstBlock; // new Paragraph();

            DIDeviceInfo_To_RichText_Converter.FormatDataIntoRichText(para.Inlines, diDeviceInfo);

            /*var doc = new FlowDocument();
            doc.Blocks.Add(para);
            textBox.Document = doc;*/

            return null;
        }

        // from UI to Data
        public object[] ConvertBack(object values, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // do backward conversion

            throw new NotSupportedException();
        }
    }
}
