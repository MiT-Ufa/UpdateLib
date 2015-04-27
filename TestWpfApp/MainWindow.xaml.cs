using System.Windows;
using UpdateLib;

namespace TestWpfApp
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            AutoUpdater.AppCastUrl = "https://gist.githubusercontent.com/Snegovikufa/dbba6461db04bc7eb2c0/raw/605116ba229afd0600ad2e832c2bcecf4ef10d33/appcast.xml";
            AutoUpdater.Start();
        }
    }
}