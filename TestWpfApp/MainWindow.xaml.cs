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
            AutoUpdater.Start();
        }
    }
}