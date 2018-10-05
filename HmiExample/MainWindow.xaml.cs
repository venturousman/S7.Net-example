using System.ComponentModel;
using System.Windows;

namespace HmiExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // set default
            //mainFrame.Navigate(new Monitoring());
        }

        private void miAbout_Click(object sender, RoutedEventArgs e)
        {
            // create and show the About box when the user click Help > About menu item.
            About aboutBox = new About(this);
            aboutBox.ShowDialog();
        }

        private void miMonitoring_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Monitoring());
        }

        private void miReport_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Report());
        }

        //private void miExit_Click(object sender, RoutedEventArgs e)
        //{
        //    Application.Current.Shutdown();
        //}

        protected override void OnClosing(CancelEventArgs e)
        {
            // save user settings
            // Properties.Settings.Default.Save();

            // base.OnClosing(e);
            // show the message box here and collect the result
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to exit?", "Exit Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                // if you want to stop it, set e.Cancel = true
                e.Cancel = true;
            }
        }

        //private void miSettings_Click(object sender, RoutedEventArgs e)
        //{
        //    mainFrame.Navigate(new Settings());
        //}
    }
}
