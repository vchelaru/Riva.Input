using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

//using Soopah.Xna.Input;
using Riva.Input;
using Microsoft.DirectX.DirectInput;

namespace GenericControllersTest
{
    /// <summary>
    /// Interaction logic for GamingDeviceUC.xaml
    /// </summary>
    public partial class GenericGamingDeviceUC : UserControl
    {
        public GenericGamingDeviceUC()
        {
            DataContextChanged += This_DataContextChanged;

            InitializeComponent();
        }

        private void This_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DirectInputGamepad gamingDevice = e.NewValue as DirectInputGamepad;
            if (gamingDevice != null)
            {
                TBlock_UniqueName.Text = gamingDevice.Name;
                //TBlock_InstanceName.Text = gamingDevice.DeviceInfo.InstanceName;

                TBlock_ThumbSticks_Left.Visibility = Common.BoolToVisibility(gamingDevice.ThumbSticks.HasLeft);
                TBlock_ThumbSticks_Right.Visibility = Common.BoolToVisibility(gamingDevice.ThumbSticks.HasRight);
                TBlock_ThumbSticks_Third.Visibility = Common.BoolToVisibility(gamingDevice.ThumbSticks.HasThird);

                TBlock_ButtonsCount.Text = gamingDevice.Buttons.NumberOfButtons.ToString() + " buttons";

                //RCommon.Cmn.Msg(gamingDevice.Device.DeviceInformation.ToString(), true);

                var sb = new StringBuilder();
                sb
                    .Append("ProductName: ").AppendLine(gamingDevice.DeviceInfo.ProductName)
                    .Append("InstanceName: ").AppendLine(gamingDevice.DeviceInfo.InstanceName)
                    .Append("DeviceType / SubType: ")
                        .Append(gamingDevice.RawDevice.DeviceInformation.DeviceType)
                        .Append(" / ")
                        .AppendLine(gamingDevice.DeviceInfo.DeviceSubType.ToString())
                    .Append("My DeviceType: ").Append(gamingDevice.Type.ToString());

                /*DeviceType[] subTypes = Common.GetUniqueFlags(gamingDevice.Types).Cast<DeviceType>().ToArray();

                sb.Append("Inst.DeviceType(").Append(subTypes.Length).Append("): ");

                if (subTypes.Length > 0)
                {
                    foreach (DeviceType typeFlag in subTypes)
                    {
                        sb.Append(typeFlag).Append(" - ");
                    }
                    sb.Remove(sb.Length - 3, 3);
                }
                sb.AppendLine();*/

                TBlock_Info.Text = sb.ToString();

                TBlock_ProductGUID.Text = "Product GUID: " + gamingDevice.DeviceInfo.ProductGuid.ToString();
                TBlock_DriverGUID.Text = "Driver GUID: " + gamingDevice.DeviceInfo.FFDriverGuid.ToString();
                TBlock_InstanceGUID.Text = "Instance GUID: " + gamingDevice.DeviceInfo.InstanceGuid.ToString();
            }
        }

        private void ButtonInfoToClipboard_Click(object sender, RoutedEventArgs e)
        {
            string infotext =
                TBlock_UniqueName.Text + Environment.NewLine +
                TBlock_Info.Text + Environment.NewLine +
                TBlock_ProductGUID.Text + Environment.NewLine +
                TBlock_InstanceGUID.Text + Environment.NewLine +
                TBlock_ButtonsCount.Text + Environment.NewLine +
                "Thumbsticks:";

            int thumbsticksCount = 0;

            if (TBlock_ThumbSticks_Left.Visibility == Visibility.Visible)
            {
                infotext += " Left";
                thumbsticksCount++;
            }
            if (TBlock_ThumbSticks_Right.Visibility == Visibility.Visible)
            {
                infotext += " Right";
                thumbsticksCount++;
            }
            if (TBlock_ThumbSticks_Third.Visibility == Visibility.Visible)
            {
                infotext += " Third";
                thumbsticksCount++;
            }
            if (thumbsticksCount == 0)
            {
                infotext += " none";
            }
            Clipboard.SetText(infotext);
        }

        private void ButtonGUIDToClipboard_Click(object sender, RoutedEventArgs e)
        {
            DirectInputGamepad gamingDevice = DataContext as DirectInputGamepad;
            if (gamingDevice != null)
                Clipboard.SetText(gamingDevice.DeviceInfo.InstanceGuid.ToString());
            else
                MessageBox.Show("Error getting DirectInputDevice from DataContext.");
        }
    }
}
