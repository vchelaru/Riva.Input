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
        private XBoxIdentifyWindow XBoxIdentifyWindow;

        private void AppStartup(object sender, StartupEventArgs e)
        {
            ShutdownMode = ShutdownMode.OnLastWindowClose;

            MainWindow =
                new MainWindow();
                //new XBoxIdentifyWindow();
            MainWindow.Show();

            //XBoxIdentifyWindow = new XBoxIdentifyWindow();
            //XBoxIdentifyWindow.Show();
        }
    }
}
