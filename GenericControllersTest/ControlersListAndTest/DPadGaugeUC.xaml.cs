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
    

    public partial class DPadGaugeUC : UserControl
    {
        public const float RatioX = 50f;    // between FRB I2DInput [-1..0..1] and gauge range [0..50..100]
        public const float RatioY = -50f;   // between FRB I2DInput [1..0..-1] and gauge range [0..50..100]

        public DPadGaugeUC()
        {
            InitializeComponent();
        }


    }
}
