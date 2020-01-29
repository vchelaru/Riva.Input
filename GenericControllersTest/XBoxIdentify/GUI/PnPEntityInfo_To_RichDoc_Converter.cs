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

// Debug
//using System.Diagnostics;


namespace GenericControllersTest.XBoxIdentify
{
    class PnPEntityInfo_To_RichDoc_Converter : IMultiValueConverter
    {
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

            var textBox = values[1] as RichTextBox;
            if (textBox == null)
                return null;

            // - Do conversion

            var para = (Paragraph)textBox.Document.Blocks.FirstBlock; //new Paragraph();

            PnPEntityInfo_To_RichText_Converter.FormatDataIntoRichText(para.Inlines, pnpEntityInfo);

            /*var doc = new FlowDocument();
            doc.Blocks.Add(para);
            textBox.Document = doc;*/
            //textBox.Document.Blocks.Add(para);

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
