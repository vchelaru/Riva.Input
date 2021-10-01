using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace GenericControllersTest
{
    /// <summary>Interaction logic for App.xaml</summary>
    public partial class App : Application
    {
        private XInputIdentifyWindow XBoxIdentifyWindow;

        [STAThread]
        private void AppStartup(object sender, StartupEventArgs e)
        {
            ShutdownMode = ShutdownMode.OnLastWindowClose;

            MainWindow =
                new MainWindow();
                //new XInputIdentifyWindow();
            MainWindow.Show();

            //XInputIdentifyWindow = new XInputIdentifyWindow();
            //XInputIdentifyWindow.Show();
        }
    }
}
