using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GenericControllersTest.XBoxIdentify
{
    public class DeviceInfosDTSelector : DataTemplateSelector
    {
        private DataTemplate _DIDeviceInfoDT;
        public DataTemplate DIDeviceInfoDT 
        {
            get { return _DIDeviceInfoDT; }
            set { _DIDeviceInfoDT = value; }
        }

        private DataTemplate _PnPEntityInfoDT;
        public DataTemplate PnPEntityInfoDT
        {
            get { return _PnPEntityInfoDT; }
            set { _PnPEntityInfoDT = value; }
        }



        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            //FrameworkElement element = container as FrameworkElement;
            //if (element != null && item != null && item is Task)

            DIDeviceInfo diDeviceInfo = item as DIDeviceInfo;
            if (diDeviceInfo != null)
            {
                return _DIDeviceInfoDT;
            }
            else
            {
                PnPEntityInfo pnpEntityInfo = item as PnPEntityInfo;
                if (pnpEntityInfo != null)
                {
                    return _PnPEntityInfoDT;
                }
            }

            return null;
        }
    }
}
