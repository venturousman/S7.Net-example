#region Using
using HmiExample.Helpers;
using HmiExample.Models;
using HmiExample.PlcConnectivity;
using log4net;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using S7NetWrapper;
using System;
using System.Collections.Generic;
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

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly GridViewModel<PlanViewModel> _gridPlanVMs = new GridViewModel<PlanViewModel>();
        public GridViewModel<PlanViewModel> GridPlanVMs
        {
            get
            {
                return _gridPlanVMs;
            }
        }

        public ICommand AddPlanCommand => new CommandsImplementation(ExecuteAddPlan);
        public ICommand DeletePlanCommand => new CommandsImplementation(ExecuteDeletePlan);
        public ICommand EditPlanCommand => new CommandsImplementation(ExecuteEditPlan);

        public Monitoring()
        {
            InitializeComponent();

            // timer
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += timer_Tick;
            timer.IsEnabled = true;

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
                var msg = exc.GetAllExceptionInfo();
                log.Error(msg, exc);
                MessageBox.Show("Couldn't connect to PLC", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
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
                var msg = exc.GetAllExceptionInfo();
                log.Error(msg, exc);
                MessageBox.Show("Couldn't disconnect PLC", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
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
                var msg = exc.GetAllExceptionInfo();
                log.Error(msg, exc);
                MessageBox.Show("Couldn't start all machines", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
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
                var msg = exc.GetAllExceptionInfo();
                log.Error(msg, exc);
                MessageBox.Show("Couldn't stop all machines", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
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
                if (obj != null && obj.Machine != null)
                {
                    string name = string.Format(PlcTags.BitVariable1, obj.Machine.TagIndex);
                    Plc.Instance.Write(name, true);
                }
            }
            catch (Exception exc)
            {
                var msg = exc.GetAllExceptionInfo();
                log.Error(msg, exc);
                MessageBox.Show("Couldn't start machine", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnStopMachine_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PlanViewModel obj = ((FrameworkElement)sender).DataContext as PlanViewModel;
                if (obj != null && obj.Machine != null)
                {
                    string name = string.Format(PlcTags.BitVariable1, obj.Machine.TagIndex);
                    Plc.Instance.Write(name, false);
                }
            }
            catch (Exception exc)
            {
                var msg = exc.GetAllExceptionInfo();
                log.Error(msg, exc);
                MessageBox.Show("Couldn't stop machine", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
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
                var isConnected = Plc.Instance.ConnectionState == ConnectionStates.Online;
                foreach (PlanViewModel item in GridPlanVMs.Items)
                {
                    // enable buttons
                    item.IsConnected = isConnected;

                    if (item.Machine != null)
                    {
                        // read start/stop state of each machine
                        string name = string.Format(PlcTags.BitVariable1, item.Machine.TagIndex);
                        var newTag = new Tag { ItemName = name };
                        tags.Add(newTag);

                        // read expected quantity of each machine
                        name = string.Format(PlcTags.IntVariable, (item.Machine.TagIndex - 1) * 6 + 2);
                        newTag = new Tag { ItemName = name };
                        tags.Add(newTag);

                        // read actual quantity of each machine
                        name = string.Format(PlcTags.IntVariable, (item.Machine.TagIndex - 1) * 6 + 4);
                        newTag = new Tag { ItemName = name };
                        tags.Add(newTag);
                    }
                }

                // TODO: should be unique by tag name
                tags = Plc.Instance.Read(tags);

                // update leds
                foreach (PlanViewModel item in GridPlanVMs.Items)
                {
                    if (item.Machine != null)
                    {
                        // read start/stop state of each machine
                        string name = string.Format(PlcTags.BitVariable1, item.Machine.TagIndex);
                        var foundTag = tags.Where(x => x.ItemName == name).FirstOrDefault();
                        if (foundTag != null)
                        {
                            object value = foundTag.ItemValue;

                            if (foundTag.ItemValue is byte)
                            {
                                //var flag = CommonHelpers.IsBitSet((byte)value, 2); // bit 1 at pos 2
                                var flag = S7.Net.Types.Boolean.GetValue((byte)value, 1); // bit 1 at pos 1
                                item.LedColor = flag ? Brushes.Green : Brushes.Gray;
                            }
                        }

                        // read expected quantity of each machine
                        name = string.Format(PlcTags.IntVariable, (item.Machine.TagIndex - 1) * 6 + 2);
                        foundTag = tags.Where(x => x.ItemName == name).FirstOrDefault();
                        if (foundTag != null)
                        {
                            object value = foundTag.ItemValue;

                            if (foundTag.ItemValue is ushort)
                            {
                                item.ExpectedQuantity = (ushort)foundTag.ItemValue;
                            }
                        }

                        // read actual quantity of each machine
                        name = string.Format(PlcTags.IntVariable, (item.Machine.TagIndex - 1) * 6 + 4);
                        foundTag = tags.Where(x => x.ItemName == name).FirstOrDefault();
                        if (foundTag != null)
                        {
                            object value = foundTag.ItemValue;

                            if (foundTag.ItemValue is ushort)
                            {
                                item.ActualQuantity = (ushort)foundTag.ItemValue;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = ex.GetAllExceptionInfo();
                log.Error(msg, ex);
                MessageBox.Show(Constants.ApplicationCommonErrorMessage, Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadPlanData()
        {
            // reset
            _gridPlanVMs.Items.Clear();

            // load databases
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            var lstPlans = mainWindow.applicationDbContext.Plans.Where(x => !x.IsDeleted && !x.IsProcessed).ToList();

            foreach (var plan in lstPlans)
            {
                var planVM = new PlanViewModel
                {
                    Id = plan.Id,
                    Machine = plan.Machine != null ? new MachineViewModel(plan.Machine) : null,
                    MachineId = plan.MachineId,
                    //MachineName = plan.Machine != null ? plan.Machine.Name : string.Empty,
                    Employee = plan.Employee != null ? new EmployeeViewModel(plan.Employee) : null,
                    EmployeeId = plan.EmployeeId,
                    //EmployeeName = plan.Employee != null ? plan.Employee.DisplayName : string.Empty,
                    Product = plan.Product != null ? new ProductViewModel(plan.Product) : null,
                    ProductId = plan.ProductId,
                    //ProductName = plan.Product != null ? plan.Product.Name : string.Empty,
                    ExpectedQuantity = plan.ExpectedQuantity,
                    ActualQuantity = plan.ActualQuantity,
                    StartTime = plan.StartTime,
                    EndTime = plan.EndTime,
                    IsProcessed = plan.IsProcessed,
                };
                // planVM.PropertyChanged += PlanViewModel_PropertyChanged;
                _gridPlanVMs.Items.Add(planVM);
            }

            // register event
            _gridPlanVMs.Items.CollectionChanged += Plans_CollectionChanged;
        }

        private void PlanViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == "ActualQuantity")
            //{
            //}
            throw new NotImplementedException();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // default values
            txtIpAddress.Text = Properties.Settings.Default.IpAddress;

            // load plan
            LoadPlanData();
        }

        #region Plans

        private void Plans_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {
                var mainWindow = (MainWindow)Application.Current.MainWindow;

                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (PlanViewModel item in e.OldItems)
                    {
                        var deletingPlan = mainWindow.applicationDbContext.Plans.Where(x => x.Id == item.Id).FirstOrDefault();
                        if (deletingPlan != null)
                        {
                            deletingPlan.IsDeleted = true; // soft delete
                            deletingPlan.ModifiedOn = DateTime.UtcNow;
                        }
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    var newPlans = new List<Plan>();
                    foreach (PlanViewModel item in e.NewItems)
                    {
                        var newPlan = new Plan
                        {
                            Id = Guid.NewGuid(),
                            MachineId = item.MachineId,
                            EmployeeId = item.EmployeeId,
                            ProductId = item.ProductId,
                            ExpectedQuantity = item.ExpectedQuantity,
                            CreatedOn = DateTime.UtcNow,
                            IsProcessed = false
                        };
                        newPlans.Add(newPlan);
                    }
                    mainWindow.applicationDbContext.Plans.AddRange(newPlans);
                }
                else if (e.Action == NotifyCollectionChangedAction.Replace)
                {
                    for (int i = 0; i < e.OldItems.Count; i++)
                    {
                        var oldItem = (PlanViewModel)e.OldItems[i];
                        var newItem = (PlanViewModel)e.NewItems[i];

                        var editingPlan = mainWindow.applicationDbContext.Plans.Where(x => x.Id == oldItem.Id).FirstOrDefault();
                        if (editingPlan != null)
                        {
                            editingPlan.MachineId = newItem.MachineId;
                            editingPlan.EmployeeId = newItem.EmployeeId;
                            editingPlan.ProductId = newItem.ProductId;
                            editingPlan.ExpectedQuantity = newItem.ExpectedQuantity;
                            //editingPlan.ActualQuantity = newItem.ActualQuantity;
                            //editingPlan.StartTime = newItem.StartTime;
                            //editingPlan.EndTime = newItem.EndTime;
                            //editingPlan.IsProcessed = newItem.IsProcessed;
                            editingPlan.ModifiedOn = DateTime.UtcNow;
                        }
                    }
                }

                // save databases
                mainWindow.applicationDbContext.SaveChanges();

                // notify
                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    MessageBox.Show("Successfully deleted plans", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    MessageBox.Show("Successfully created plans", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (e.Action == NotifyCollectionChangedAction.Replace)
                {
                    MessageBox.Show("Successfully updated plans", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                var msg = ex.GetAllExceptionInfo();
                log.Error(msg, ex);
                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    MessageBox.Show("Cannot remove the selected plans", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    MessageBox.Show("Cannot create these plans", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (e.Action == NotifyCollectionChangedAction.Replace)
                {
                    MessageBox.Show("Cannot edit these plans", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void ExecuteAddPlan(object o)
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

        private void ExecuteDeletePlan(object obj)
        {
            if (obj is PlanViewModel)
            {
                var plan = obj as PlanViewModel;
                GridPlanVMs.Items.Remove(plan);
            }
        }

        private async void ExecuteEditPlan(object o)
        {
            if (!(o is PlanViewModel)) return;

            var editingPlan = o as PlanViewModel;

            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new AddPlanDialog
            {
                DataContext = new PlanViewModel()
                {
                    Id = editingPlan.Id,
                    MachineId = editingPlan.MachineId,
                    EmployeeId = editingPlan.EmployeeId,
                    ProductId = editingPlan.ProductId,
                    ExpectedQuantity = editingPlan.ExpectedQuantity,
                    ActualQuantity = editingPlan.ActualQuantity,
                    StartTime = editingPlan.StartTime,
                    EndTime = editingPlan.EndTime,
                    IsProcessed = editingPlan.IsProcessed,
                    //Employee = editingPlan.Employee,
                    //Machine = editingPlan.Machine,
                    //Product = editingPlan.Product
                }
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

            if (context.MachineId != Guid.Empty && context.ProductId != Guid.Empty)
            {
                if (context.Id != Guid.Empty)
                {
                    var index = -1;
                    for (int i = 0; i < GridPlanVMs.Items.Count; i++)
                    {
                        if (GridPlanVMs.Items[i].Id == context.Id)
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index != -1)
                    {
                        //context.LedColor = GridPlanVMs.Items[index].LedColor;
                        //context.ActualQuantity = GridPlanVMs.Items[index].ActualQuantity;

                        GridPlanVMs.Items[index] = context;
                    }
                }
                else
                {
                    var newPlan = new PlanViewModel
                    {
                        EmployeeId = context.EmployeeId,
                        MachineId = context.MachineId,
                        ProductId = context.ProductId,
                        ExpectedQuantity = context.ExpectedQuantity,
                        IsProcessed = false,
                        Employee = context.Employee, // support elements
                        Machine = context.Machine,
                        Product = context.Product
                    };
                    GridPlanVMs.Items.Add(newPlan);
                }
            }
        }

        private void OpenedPlanDialogEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            Console.WriteLine("You could intercept the open and affect the dialog using eventArgs.Session.");
        }

        #endregion

    }
}
