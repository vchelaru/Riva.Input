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

using System.Windows.Threading;

//using Soopah.Xna.Input;
using Riva.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using RCommon;

using System.Diagnostics;

namespace GenericControllersTest
{
    public partial class MainWindow : RObservableWindow
    {
        /*private List<DirectInputDevice> _GamingDevices;
        public List<DirectInputDevice> GamingDevices
        {
            get { return _GamingDevices; }
            set { SetField(ref _GamingDevices, value, "GamingDevices"); }
        }*/

        private DirectInputGamepad _SelectedDevice;
        public DirectInputGamepad SelectedDevice
        {
            get { return _SelectedDevice; }
            set { SetField(ref _SelectedDevice, value, "SelectedDevice"); }
        }

        private DispatcherTimer _PollTimer;
        private const double _PollFrequency =
            1d / 60d
            //1d
            ;

        #region    -- GUI data
        //private DataTemplate _BottonIndicatorDT;

        private List<Border> _GUI_BottonIndicators = new List<Border>(20);

        private List<DPadGaugeUC> _GUI_DPadsGauges = new List<DPadGaugeUC>(3);
        #endregion -- GUI data END


        public MainWindow()
        {
            DataContext = this;

            //GetGamingDevices();

            _PollTimer = new DispatcherTimer();
            _PollTimer.Interval = TimeSpan.FromSeconds(_PollFrequency);
            _PollTimer.Tick += _PollTimer_Tick;

            InitializeComponent();

            Stack_Buttons.Visibility = Visibility.Hidden;
            Stack_ThumbSticks.Visibility = Visibility.Hidden;
            Stack_DPads.Visibility = Visibility.Hidden;

            ThumbStickGauge_Left.Label.Text = "Left";
            ThumbStickGauge_Right.Label.Text = "Right";
            ThumbStickGauge_Third.Label.Text = "Third";

            _DbgListDevices();
        }





        /*void GetGamingDevices()
        {
            DirectInputManager.ReloadDevices();

            GamingDevices = DirectInputManager.Devices;
        }*/

        private void ListBoxDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxDevices.SelectedIndex == -1)
            {
                SelectedDevice = null;

                Stack_Buttons.Visibility = Visibility.Hidden;
                Stack_ThumbSticks.Visibility = Visibility.Hidden;

                _PollTimer.Stop();
            }
            else
            {
                SelectedDevice = DirectInputManager.Devices[ListBoxDevices.SelectedIndex];

                /*var infoArray = SelectedDevice.Device.ImageInformation.ImageInformationArray;
                foreach (var item in infoArray)
                {
                    if (item.ImagePath != null)
                    {
                        Cmn.Msg(item.ImagePath);
                    }
                }*/


                _CreateBottonIndicatorControls(SelectedDevice.Buttons.NumberOfButtons);

                Stack_Buttons.Visibility = Visibility.Visible;

                _SetThumbStickGaugeControls(SelectedDevice.ThumbSticks);

                Stack_ThumbSticks.Visibility = Visibility.Visible;

                _SetDPadsGaugeControls(SelectedDevice.DPads);

                Stack_DPads.Visibility = Visibility.Visible;

                _PollTimer.Start();

                //Cmn.Msg(
                ///* Microsoft.DirectX.DirectInput.Device */ SelectedDevice.Device
                //    // .GetBufferedData.
                //    //.Properties
                //        //.GetKeyName(Microsoft.DirectX.DirectInput.ParameterHow.ById, 0)

                //);
            }
        }

        private void _PollTimer_Tick(object sender, EventArgs e)
        {
            _SelectedDevice.Poll();

            _UpdateDeviceButtons(_SelectedDevice.Buttons);
            
            _UpdateThumbSticks(_SelectedDevice.ThumbSticks);

            _UpdateDPads(_SelectedDevice.DPads);
        }

        void _UpdateDeviceButtons(DirectInputButtons buttons)
        {
            buttons.Refresh();

            for (int i = 0; i < buttons.Count; i++)
                _UpdateButtonIndicator(_GUI_BottonIndicators[i], buttons[i]);
        }

        void _UpdateThumbSticks(DirectInputThumbSticks thumbSticks)
        {
            thumbSticks.Refresh();

#if DEBUG
            if (thumbSticks.HasLeft)
                _UpdateThumbStick(thumbSticks.Left, ThumbStickGauge_Left, thumbSticks.RawPositions[0]);

            if (thumbSticks.HasRight)
                _UpdateThumbStick(thumbSticks.Right, ThumbStickGauge_Right, thumbSticks.RawPositions[1]);

            if (thumbSticks.HasThird)
                _UpdateThumbStick(thumbSticks.Third, ThumbStickGauge_Third, thumbSticks.RawPositions[2]);
#else
            if (thumbSticks.HasLeft)
                _UpdateThumbStick(thumbSticks.Left, ThumbStickGauge_Left);

            if (thumbSticks.HasRight)
                _UpdateThumbStick(thumbSticks.Right, ThumbStickGauge_Right);

            if (thumbSticks.HasThird)
                _UpdateThumbStick(thumbSticks.Third, ThumbStickGauge_Third);
#endif
        }

        void _UpdateDPads(DirectInputDPad[] dPads)
        {
            DirectInputDPad dPad;
            for (int i = 0; i < dPads.Length; i++)
            {
                dPad = dPads[i];
                dPad.Refresh();
                _UpdateDPad( dPad, _GUI_DPadsGauges[i], i );
            }
        }

        //Size _WPFCoords;
        double _X;
        double _Y;
        void _UpdateThumbStick(
            Vector2 thumbStickVector, ThumbStickGaugeUC gauge
#if DEBUG
            , Vector2 thumbStickVectorRaw
#endif
        )
        {
            gauge.Vector.Text = thumbStickVector.ToStringF4(); // F4
            gauge.Lenght.Text = thumbStickVector.Length().ToString("F4");
#if DEBUG
            gauge.VectorRaw.Text = thumbStickVectorRaw.ToString();
#endif
            // Vector: 0,0 = center
            // Line: 100,100 = center
            // Vector: -1,-1 = bottom left
            // Line: 0,200 = bottom left
            // Vector: 1,1 = top right
            // Line: 200,0 = top right

            _X = Cmn.RangeConversion(-1, 0, ThumbStickGaugeUC.RatioX, thumbStickVector.X);
            _Y = Cmn.RangeConversion(1, 0, ThumbStickGaugeUC.RatioY, thumbStickVector.Y);
            gauge.Line.X2 = _X;
            gauge.Line.Y2 = _Y;

            gauge.Thumb.Margin = new Thickness(_X - 15d, _Y - 15d, 0d, 0d);
        }

        void _UpdateDPad(DirectInputDPad dPad, DPadGaugeUC gauge, int dPadIndex)
        {
            gauge.Vector.Text = dPad.Vector.ToStringF4();
            gauge.Lenght.Text = dPad.Vector.Length().ToString("F4");
#if DEBUG
            gauge.VectorRaw.Text =
                //dPad.ParentDevice.RawDPadsStates[dPadIndex].ToString();
                dPad.AngleRad.ToString();
#endif
            _X = Cmn.RangeConversion(-1, 0, DPadGaugeUC.RatioX, dPad.Vector.X);
            _Y = Cmn.RangeConversion(1, 0, DPadGaugeUC.RatioY, dPad.Vector.Y);
            gauge.Line.X2 = _X;
            gauge.Line.Y2 = _Y;

            gauge.Thumb.Margin = new Thickness(_X - 10d, _Y - 10d, 0d, 0d);

            if (dPad.Up == ButtonState.Pressed)
                gauge.ButtonUp.Background = Brushes.YellowGreen;
            else
                gauge.ButtonUp.Background = null;

            _UpdateButtonIndicator(gauge.ButtonUp, dPad.Up);
            _UpdateButtonIndicator(gauge.ButtonDown, dPad.Down);
            _UpdateButtonIndicator(gauge.ButtonLeft, dPad.Left);
            _UpdateButtonIndicator(gauge.ButtonRight, dPad.Right);
        }

        void _UpdateButtonIndicator(Border indicatorControl, ButtonState buttonState)
        {
            if (buttonState == ButtonState.Pressed)
                indicatorControl.Background = Brushes.YellowGreen;
            else
                indicatorControl.Background = null;
        }


        #region    -- GUI
        void _CreateBottonIndicatorControls(int buttonsCount)
        {
            int index = Stack_Buttons.Children.Count - 1;
            while (Stack_Buttons.Children.Count > buttonsCount)
            {
                _GUI_BottonIndicators.RemoveAt(index);
                Stack_Buttons.Children.RemoveAt(index);
                index--;
            }

            index = Stack_Buttons.Children.Count;
            Border newBorder;
            while (Stack_Buttons.Children.Count < buttonsCount)
            {
                newBorder = Cmn.CreateBottonIndicator(index.ToString());
                _GUI_BottonIndicators.Add(newBorder);
                Stack_Buttons.Children.Add(newBorder);
                index++;
            }
        }

        void _SetThumbStickGaugeControls(DirectInputThumbSticks thumbSticks)
        {
            ThumbStickGauge_Left.Visibility = Common.BoolToVisibility(thumbSticks.HasLeft);
            ThumbStickGauge_Right.Visibility = Common.BoolToVisibility(thumbSticks.HasRight);
            ThumbStickGauge_Third.Visibility = Common.BoolToVisibility(thumbSticks.HasThird);
        }

        void _SetDPadsGaugeControls(DirectInputDPad[] dPads)
        {
            int index = Stack_DPads.Children.Count - 1;
            while (Stack_DPads.Children.Count > dPads.Length)
            {
                _GUI_DPadsGauges.RemoveAt(index);
                Stack_DPads.Children.RemoveAt(index);
                index--;
            }

            index = Stack_DPads.Children.Count;
            DPadGaugeUC gaugeControl;
            while (Stack_DPads.Children.Count < dPads.Length)
            {
                gaugeControl = new DPadGaugeUC();
                gaugeControl.Label.Text = index.ToString();
                _GUI_DPadsGauges.Add(gaugeControl);
                Stack_DPads.Children.Add(gaugeControl);
                index++;
            }
        }
        #endregion -- GUI END



        // -- Debug
        void Test()
        {
            
        }

        void _DbgListDevices()
        {
            var gamingDevices = DirectInputManager.Devices;

            var sb = new StringBuilder();
            foreach (var device in gamingDevices)
            {
                sb.AppendLine(device.Name);
            }

            var str = sb.ToString();

            Debug.WriteLine(str);
        }
    }
}
