using HmiExample.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HmiExample
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
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

        private void SettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (SettingHelpers.hasSetting(Constants.MoldLife))
            {
                txtMoldLife.Text = Properties.Settings.Default[Constants.MoldLife].ToString();
            }

            if (SettingHelpers.hasSetting(Constants.MaxCycleTime))
            {
                txtMaxCycleTime.Text = Properties.Settings.Default[Constants.MaxCycleTime].ToString();
            }
        }
    }
}
