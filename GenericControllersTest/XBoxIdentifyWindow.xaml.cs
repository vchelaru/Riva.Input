using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using GenericControllersTest.XBoxIdentify;

// Debug
using System.Diagnostics;


namespace GenericControllersTest
{
    public partial class XBoxIdentifyWindow : Window
    {
        public XBoxIdentifyWindow()
        {
            ContentRendered += _ContentRendered;

            InitializeComponent();
        }

        private void _ContentRendered(object sender, EventArgs e)
        {
            ContentRendered -= _ContentRendered;

            var xboxRecognition = new XBoxRecognition();

            #region    --- Load data from files
            List<GenericControllersTest.XBoxIdentify.PnPEntityInfo> PnPList_XBox;
            List<GenericControllersTest.XBoxIdentify.DIDeviceInfo> DInputList_XBox;

            //xboxRecognition.LoadAndCrunchFromGrabberFiles(
            xboxRecognition.LoadFromGrabberFiles(
                /* From Vic - XBox One and XBox 360 generic ("knockoff") gamepads */
                @"Data\Device ID's from Vic\XboxPnpDevices 02 cleaned.xml",
                @"Data\Device ID's from Vic\XboxDiDevices.xml",
                out PnPList_XBox, out DInputList_XBox
            );

            foreach (var item in PnPList_XBox)
                item.RTag_FW = Brushes.Green;
            foreach (var item in DInputList_XBox)
                item.RTag_FW = Brushes.Green;

            List<GenericControllersTest.XBoxIdentify.PnPEntityInfo> PnPList_Switch;
            List<GenericControllersTest.XBoxIdentify.DIDeviceInfo> DInputList_Switch;

            xboxRecognition.LoadFromGrabberFiles(
                /* From Vic - Switch controller */
                @"Data\Device ID's from Vic\SwitchPnPDevices.xml",
                @"Data\Device ID's from Vic\SwitchDIDevices.xml",
                out PnPList_Switch, out DInputList_Switch
            );

            foreach (var item in PnPList_Switch)
                item.RTag_FW = Brushes.Brown;
            foreach (var item in DInputList_Switch)
                item.RTag_FW = Brushes.Brown;
            #endregion --- Load data from files END


            #region    --- Crunch data
            xboxRecognition.CrunchData(
                PnPList_XBox.Concat(PnPList_Switch).ToList(),
                DInputList_XBox.Concat(DInputList_Switch).ToList()
            );
            #endregion --- Crunch data END


            #region    --- Assign result data to GUI
            if (xboxRecognition.DeviceInfosPaired.Count > 0)
                ItemsControlPairedDevices.ItemsSource = xboxRecognition.DeviceInfosPaired;
            else
            {
                TBlockMatches.Visibility = Visibility.Collapsed;
                ItemsControlPairedDevices.Visibility = Visibility.Collapsed;
            }

            if (xboxRecognition.DInputGroupsListUnpaired.Count > 0)
                ItemsControlUnpairedDIDevices.ItemsSource = xboxRecognition.DInputGroupsListUnpaired;
            else
            {
                TBlockUnpairedDIDevices.Visibility = Visibility.Collapsed;
                ItemsControlUnpairedDIDevices.Visibility = Visibility.Collapsed;
            }

            if (xboxRecognition.PnPGroupsUnpaired.Count > 0)
                ItemsControlUnpairedPnPDevices.ItemsSource = xboxRecognition.PnPGroupsUnpaired;
            else
            {
                TBlockUnpairedPnPDevices.Visibility = Visibility.Collapsed;
                ItemsControlUnpairedPnPDevices.Visibility = Visibility.Collapsed;
            }

            //_Test(XBoxRecognition.DeviceInfosPaired[0].Item1_FW.First()); 
            #endregion --- Assign result data to GUI END


            //_Test();
        }

        // Debug
        /*private void _Test()
        {
            //new RichTextBox().
            var para = (Paragraph)RichTextBoxTest.Document.Blocks.FirstBlock;

            para.Inlines.Add(new Run("XXXXX"));
            para.Inlines.Add(new Run("XXXXX") { Foreground = Brushes.DarkRed });
            
        }*/

        /*private void _Test(PnPEntityInfo pnpEntityInfo)
        {
            var converter = new PnPEntityInfo_To_RichText_Converter();
            var tBlock = new TextBlock();

            converter.Convert(new object[] { pnpEntityInfo, tBlock }, null, null, null);

            Debug.WriteLine(
                RCommon.Diag.PrintCollection(
                    tBlock.Inlines, 
                    (item) =>
                    {
                        var run = item as Run;
                        if (run != null)
                        {
                            return $"[{run.Text}]";
                        }
                        else
                            return item.ToString();
                    }
                )
            );
        }*/
    }
}
