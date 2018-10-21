#region Using
using HmiExample.Helpers;
using HmiExample.Models;
using HmiExample.PlcConnectivity;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using S7NetWrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
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

        private readonly GridViewModel<PlanViewModel> _gridPlanVMs = new GridViewModel<PlanViewModel>();
        public GridViewModel<PlanViewModel> GridPlanVMs
        {
            get
            {
                return _gridPlanVMs;
            }
        }

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

            //LoadData(); // testing
            DataContext = this;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            btnConnect.IsEnabled = Plc.Instance.ConnectionState == ConnectionStates.Offline;
            btnDisconnect.IsEnabled = Plc.Instance.ConnectionState != ConnectionStates.Offline;
            lblConnectionState.Text = Plc.Instance.ConnectionState.ToString();

            //ledMachineInRun.Fill = Plc.Instance.Db1.BitVariable0 ? Brushes.Green : Brushes.Gray;

            //lblSpeed.Content = Plc.Instance.Db1.IntVariable;
            //lblTemperature.Content = Plc.Instance.Db1.RealVariable;
            //lblAutomaticSpeed.Content = Plc.Instance.Db1.DIntVariable;
            //lblSetDwordVariable.Content = Plc.Instance.Db1.DWordVariable;

            // statusbar
            lblReadTime.Text = Plc.Instance.CycleReadTime.TotalMilliseconds.ToString(CultureInfo.InvariantCulture);


            // update grid
            UpdateGridPlan();
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
                // TODO: will start many machines
                // dgPlans.SelectedItems
                MessageBox.Show("start all");
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
                // Plc.Instance.Write(PlcTags.BitVariable, 0);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void btnStartMachine_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (Plc.Instance.ConnectionState == ConnectionStates.Offline)
                //{
                //    return; // or messagebox
                //}                

                PlanViewModel obj = ((FrameworkElement)sender).DataContext as PlanViewModel;
                string name = string.Format(PlcTags.BitVariable0, obj.DataBlockNo);
                Plc.Instance.Write(name, true);
                //obj.IsStarted = true;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void btnStopMachine_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PlanViewModel obj = ((FrameworkElement)sender).DataContext as PlanViewModel;
                string name = string.Format(PlcTags.BitVariable0, obj.DataBlockNo);
                Plc.Instance.Write(name, false);
                //obj.IsStarted = false;
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


        private void UpdateGridPlan()
        {
            try
            {
                var tags = new List<Tag>();
                foreach (var item in GridPlanVMs.Items)
                {
                    // update buttons
                    item.IsConnected = Plc.Instance.ConnectionState == ConnectionStates.Online;

                    string name = string.Format(PlcTags.BitVariable1, item.DataBlockNo);
                    var newTag = new Tag { ItemName = name };
                    tags.Add(newTag);
                }

                tags = Plc.Instance.Read(tags);

                // update leds
                foreach (var item in GridPlanVMs.Items)
                {
                    string name = string.Format(PlcTags.BitVariable1, item.DataBlockNo);
                    var foundTag = tags.Where(x => x.ItemName == name).FirstOrDefault();
                    if (foundTag != null)
                    {
                        object value = foundTag.ItemValue;

                        if (foundTag.ItemValue is byte)
                        {
                            var flag = CommonHelpers.IsBitSet((byte)value, 1);
                            item.LedColor = flag ? Brushes.Green : Brushes.Gray;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadData()
        {
            // load databases
            var mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
            if (mainWindow.applicationDbContext.Plans.Local != null)
            {
                var lstPlans = mainWindow.applicationDbContext.Plans.Local.ToList();

                foreach (var plan in lstPlans)
                {
                    var planVM = new PlanViewModel
                    {
                        Machine = plan.Machine.Name,
                        Employee = plan.Employee.DisplayName,
                        Product = plan.Product.Name,
                        ExpectedQuantity = plan.ExpectedQuantity,
                        ActualQuantity = 0,
                        DataBlockNo = plan.Machine.TagIndex
                    };
                    _gridPlanVMs.Items.Add(planVM);
                }
            }

            // register event
            _gridPlanVMs.Items.CollectionChanged += Plans_CollectionChanged;
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
