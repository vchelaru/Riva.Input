using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

using System.Windows.Data; // mine - IValueConverter
using System.Windows.Documents;
//using System.Globalization; // mine - IValueConverter > CultureInfo

namespace GenericControllersTest.XBoxIdentify
{
    [ValueConversion(typeof(PnPEntityInfo), typeof(Inline))]
    class PnPEntityInfo_To_RichText_Converter : IValueConverter
    {
        // from Data to UI (UI sosa data z Dat)
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // do conversion

            Run 
        }

        // from UI to Data (Data sosaji data z UI)
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // do backward conversion

            throw new NotSupportedException();
        }
    }
}
