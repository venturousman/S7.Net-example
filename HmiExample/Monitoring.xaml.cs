#region Using
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HmiExample.PlcConnectivity;
using S7NetWrapper;
using System.Globalization;
using System.Windows.Threading;
using HmiExample.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using HmiExample.Helpers;
using MaterialDesignThemes.Wpf;
using System.Collections.Specialized;
using Microsoft.Win32;
#endregion

namespace HmiExample
{
    /// <summary>
    /// Interaction logic for Monitoring.xaml
    /// </summary>
    public partial class Monitoring : Page
    {
        // http://www.abhisheksur.com/2011/03/all-about-net-timers-comparison.html
        // https://www.codeproject.com/Articles/167365/All-about-NET-Timers-A-Comparison
        DispatcherTimer timer = new DispatcherTimer();

        public GridViewModel<PlanViewModel> Plans { get; }

        public ICommand AddPlanCommand => new CommandsImplementation(ExecuteAddPlanCommand);

        public Monitoring()
        {
            InitializeComponent();

            // timer
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += timer_Tick;
            timer.IsEnabled = true;

            // default values
            txtIpAddress.Text = Properties.Settings.Default.IpAddress;

            // testing
            Plans = LoadPlans();
            DataContext = this;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            btnConnect.IsEnabled = Plc.Instance.ConnectionState == ConnectionStates.Offline;
            btnDisconnect.IsEnabled = Plc.Instance.ConnectionState != ConnectionStates.Offline;
            lblConnectionState.Text = Plc.Instance.ConnectionState.ToString();
            ledMachineInRun.Fill = Plc.Instance.Db1.BitVariable0 ? Brushes.Green : Brushes.Gray;
            //lblSpeed.Content = Plc.Instance.Db1.IntVariable;
            //lblTemperature.Content = Plc.Instance.Db1.RealVariable;
            //lblAutomaticSpeed.Content = Plc.Instance.Db1.DIntVariable;
            //lblSetDwordVariable.Content = Plc.Instance.Db1.DWordVariable;
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
                MessageBox.Show("Successfully connected to " + txtIpAddress.Text, Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
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
                // TODO: will start many machines
                // dgPlans.SelectedItems
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

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx";
            if (openFileDialog.ShowDialog() == true)
            {
                txtFilePath.Text = openFileDialog.FileName;
            }
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            var filePath = txtFilePath.Text.Trim();
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("No file selected. Please select a valid import file!", "Import Plan List", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MessageBox.Show("Plans were imported successfully.", "Import Plan List", MessageBoxButton.OK, MessageBoxImage.Information);
        }




        private GridViewModel<PlanViewModel> LoadPlans()
        {
            // TODO: listen Plans

            var plans = new ObservableCollection<PlanViewModel>
            {
                new PlanViewModel
                {
                    Machine = "Machine 002",
                    Employee = "Employee A",
                    Product = "Product A",
                    ExpectedQuantity = 23,
                    ActualQuantity = 34
                },
                new PlanViewModel
                {
                    Machine = "Machine 001",
                    Employee = "Employee B",
                    Product = "Product A",
                    ExpectedQuantity = 45,
                    ActualQuantity = 49
                }
            };
            plans.CollectionChanged += Plans_CollectionChanged;

            return new GridViewModel<PlanViewModel>(plans);
        }

        #region Plans

        private void Plans_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var tmp = e.OldItems;
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var tmp = e.NewItems;
            }
            // TODO: save databases
        }

        private async void ExecuteAddPlanCommand(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new AddPlanDialog
            {
                DataContext = new PlanViewModel()
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", OpenedPlanDialogEventHandler, ClosingPlanDialogEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }

        private void ClosingPlanDialogEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, true)) return;

            var context = (PlanViewModel)((AddPlanDialog)eventArgs.Session.Content).DataContext;

            //if (!string.IsNullOrEmpty(context.Name) && !string.IsNullOrEmpty(context.Code))
            //{
            //    var newPlan = new PlanViewModel { Name = context.Name, Code = context.Code };
            //    Plans.Items.Add(newPlan);
            //}
        }

        private void OpenedPlanDialogEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            Console.WriteLine("You could intercept the open and affect the dialog using eventArgs.Session.");
        }

        #endregion


    }
}
