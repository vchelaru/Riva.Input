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

        private DirectInputDevice _SelectedDevice;
        public DirectInputDevice SelectedDevice
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
        private Thickness _GUI_BottonIndicatorMargin = new Thickness(4);
        private Thickness _GUI_BottonIndicatorBorderThickness = new Thickness(1);

        private List<Border> _GUI_BottonIndicators = new List<Border>(20);
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
        }

        void _UpdateDeviceButtons(DirectInputButtons buttons)
        {
            buttons.Refresh();

            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i] == ButtonState.Pressed)
                    _GUI_BottonIndicators[i].Background = Brushes.YellowGreen;
                else
                    _GUI_BottonIndicators[i].Background = Brushes.Transparent;
            }
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
            gauge.Vector.Text = thumbStickVector.ToString();
#if DEBUG
            gauge.VectorRaw.Text = thumbStickVectorRaw.ToString();
#endif
            /*ThumbStickGauge_Left.Line.X2 = // 0 Left to 200 Right
                (thumbSticks.Left.X // normalized vector = -1 Left to 1 Right
                + 1) // 0 to 2
                * 100;*/
            //thumbSticks.Left.Normalize();

            _X = (thumbStickVector.X + 1f) * 100d;
            _Y = (thumbStickVector.Y + 1f) * 100d;
            gauge.Line.X2 = _X;
            gauge.Line.Y2 = _Y;

            gauge.Thumb.Margin = new Thickness(_X - 15d, _Y - 15d, 0d, 0d);
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
                newBorder = _CreateBottonIndicator(index);
                _GUI_BottonIndicators.Add(newBorder);
                Stack_Buttons.Children.Add(newBorder);
                index++;
            }
        }

        Border _CreateBottonIndicator(int index)
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
                    Text = index.ToString()
                }
            };
        }

        void _SetThumbStickGaugeControls(DirectInputThumbSticks thumbSticks)
        {
            ThumbStickGauge_Left.Visibility = Common.BoolToVisibility(thumbSticks.HasLeft);
            ThumbStickGauge_Right.Visibility = Common.BoolToVisibility(thumbSticks.HasRight);
            ThumbStickGauge_Third.Visibility = Common.BoolToVisibility(thumbSticks.HasThird);
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
