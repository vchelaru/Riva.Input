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

namespace GenericControllersTest
{
    public partial class ThumbStickGaugeUC : UserControl
    {
        public const float RatioX = 100f;    // between FRB I2DInput [-1..0..1] and gauge range [0..100..200]
        public const float RatioY = -100f;   // between FRB I2DInput [1..0..-1] and gauge range [0..100..200]

        public ThumbStickGaugeUC()
        {
            InitializeComponent();
        }


    }
}
