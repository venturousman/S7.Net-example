#region Using
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HmiExample.PlcConnectivity;
using S7NetWrapper;
using System.Globalization;
using System.Windows.Threading;
using System.Text.RegularExpressions;
using HmiExample.Helpers;
using System.Windows.Input;
#endregion

namespace HmiExample
{
    /// <summary>
    /// Interaction logic for Monitoring.xaml
    /// </summary>
    public partial class Monitoring : Page
    {
        DispatcherTimer timer = new DispatcherTimer();

        public Monitoring()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += timer_Tick;
            timer.IsEnabled = true;
            txtIpAddress.Text = Properties.Settings.Default.IpAddress;

            // init values
            if (SettingHelpers.hasSetting(Constants.MoldLife))
            {
                txtMoldLife.Text = Properties.Settings.Default[Constants.MoldLife].ToString();
            }

            if (SettingHelpers.hasSetting(Constants.MaxCycleTime))
            {
                txtMaxCycleTime.Text = Properties.Settings.Default[Constants.MaxCycleTime].ToString();
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            btnConnect.IsEnabled = Plc.Instance.ConnectionState == ConnectionStates.Offline;
            btnDisconnect.IsEnabled = Plc.Instance.ConnectionState != ConnectionStates.Offline;
            lblConnectionState.Text = Plc.Instance.ConnectionState.ToString();
            ledMachineInRun.Fill = Plc.Instance.Db1.BitVariable0 ? Brushes.Green : Brushes.Gray;
            lblSpeed.Content = Plc.Instance.Db1.IntVariable;
            lblTemperature.Content = Plc.Instance.Db1.RealVariable;
            lblAutomaticSpeed.Content = Plc.Instance.Db1.DIntVariable;
            lblSetDwordVariable.Content = Plc.Instance.Db1.DWordVariable;
            // statusbar
            lblReadTime.Text = Plc.Instance.CycleReadTime.TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Plc.Instance.Connect(txtIpAddress.Text);
                Properties.Settings.Default.IpAddress = txtIpAddress.Text;
                Properties.Settings.Default.Save();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void btnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Plc.Instance.Disconnect();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        /// <summary>
        /// Writes a bit to 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Plc.Instance.Write(PlcTags.BitVariable, 1);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        /// <summary>
        /// Writes a bit to 0
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Plc.Instance.Write(PlcTags.BitVariable, 0);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void txtSetRealVariable_TextChanged(object sender, TextChangedEventArgs e)
        {
            double realVar;
            bool canConvert = Double.TryParse(txtSetTemperature.Text, out realVar);
            if (canConvert)
            {
                Plc.Instance.Write(PlcTags.DoubleVariable, realVar);
            }
        }

        private void txtSetWordVariable_TextChanged(object sender, TextChangedEventArgs e)
        {
            short wordVar;
            bool canConvert = short.TryParse(txtSetSpeed.Text, out wordVar);
            if (canConvert)
            {
                Plc.Instance.Write(PlcTags.IntVariable, wordVar);
            }
        }

        private void txtSetDIntVariable_TextChanged(object sender, TextChangedEventArgs e)
        {
            int dintVar;
            bool canConvert = int.TryParse(txtSetAutomaticSpeed.Text, out dintVar);
            if (canConvert)
            {
                Plc.Instance.Write(PlcTags.DIntVariable, dintVar);
            }
        }

        private void txtSetSetDwordVariable_TextChanged(object sender, TextChangedEventArgs e)
        {
            ushort dwordVar;
            bool canConvert = ushort.TryParse(txtSetDwordVariable.Text, out dwordVar);
            if (canConvert)
            {
                Plc.Instance.Write(PlcTags.DwordVariable, dwordVar);
            }
        }






        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SettingHelpers.hasSetting(Constants.MoldLife))
                {
                    int intMoldLife = (int)Properties.Settings.Default[Constants.MoldLife];
                    if (int.TryParse(txtMoldLife.Text, out intMoldLife))
                    {
                        Properties.Settings.Default[Constants.MoldLife] = intMoldLife;
                    }
                }

                if (SettingHelpers.hasSetting(Constants.MaxCycleTime))
                {
                    int intMaxCycleTime = (int)Properties.Settings.Default[Constants.MaxCycleTime];
                    if (int.TryParse(txtMaxCycleTime.Text, out intMaxCycleTime))
                    {
                        Properties.Settings.Default[Constants.MaxCycleTime] = intMaxCycleTime;
                    }
                }

                Properties.Settings.Default.Save();
                MessageBox.Show("The changes have been saved.", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
    }
}
