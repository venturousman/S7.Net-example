using HmiExample.Helpers;
using HmiExample.Models;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

            // default values
            if (SettingHelpers.hasSetting(Constants.MoldLife))
            {
                txtMoldLife.Text = Properties.Settings.Default[Constants.MoldLife].ToString();
            }

            if (SettingHelpers.hasSetting(Constants.MaxCycleTime))
            {
                txtMaxCycleTime.Text = Properties.Settings.Default[Constants.MaxCycleTime].ToString();
            }

            DataContext = new SettingsViewModel();            
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
