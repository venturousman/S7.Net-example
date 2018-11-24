#region Using
using HmiExample.Helpers;
using HmiExample.Models;
using HmiExample.PlcConnectivity;
using log4net;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using S7NetWrapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

            try
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("Cannot find your file: ", filePath);
                }

                var mainWindow = (MainWindow)Application.Current.MainWindow;

                // Create the file using the FileInfo object
                var file = new FileInfo(filePath);

                //var sheetIndex = 1; // only read sheet 1 in this application
                var sheetName = "Plans";

                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[sheetName];
                    //if (worksheet == null) worksheet = package.Workbook.Worksheets[sheetIndex];

                    if (worksheet == null)
                    {
                        throw new Exception("Cannot find sheet: " + sheetName);
                    }

                    if (worksheet.Dimension == null)
                    {
                        throw new Exception("There is no data in this file");
                    }

                    #region load data from db
                    var lstMachines = mainWindow.applicationDbContext.Machines
                                        .Where(x => !x.IsDeleted)
                                        .Select(x => new MachineViewModel
                                        {
                                            Id = x.Id,
                                            Name = x.Name,
                                            Code = x.Code,
                                        })
                                        .OrderBy(x => x.Name)
                                        .ToList();

                    var lstEmployees = mainWindow.applicationDbContext.Employees
                                        .Where(x => !x.IsDeleted)
                                        .Select(x => new EmployeeViewModel
                                        {
                                            Id = x.Id,
                                            Code = x.Code,
                                            DisplayName = x.DisplayName,
                                            FirstName = x.FirstName,
                                            MiddleName = x.MiddleName,
                                            LastName = x.LastName,
                                        })
                                        .OrderBy(x => x.DisplayName)
                                        .ToList();

                    var lstProducts = mainWindow.applicationDbContext.Products
                                        .Where(x => !x.IsDeleted)
                                        .Select(x => new ProductViewModel
                                        {
                                            Id = x.Id,
                                            Name = x.Name,
                                            Code = x.Code,
                                        })
                                        .OrderBy(x => x.Name)
                                        .ToList();
                    #endregion

                    var succeededPlans = new List<Plan>();
                    var failedRows = new List<int>();
                    var failedRow = false;

                    var start = worksheet.Dimension.Start;
                    var end = worksheet.Dimension.End;

                    for (int r = start.Row + 1; r <= end.Row; r++) // ignore header at row 1
                    {
                        var newPlan = new Plan
                        {
                            Id = Guid.NewGuid(),
                            IsProcessed = false,
                            CreatedOn = DateTime.UtcNow
                        };
                        failedRow = false; // reset

                        for (int c = start.Column; c <= end.Column; c++)
                        {
                            if (failedRow == true) break; // for loop

                            var columnName = worksheet.Cells[1, c].Text.Trim();    // header at row 1
                            var value = worksheet.Cells[r, c].Text.Trim();

                            switch (columnName)
                            {
                                case "Machine":
                                    var foundMachine = lstMachines.Where(x => (x.Name + " - " + x.Code) == value).FirstOrDefault();
                                    if (foundMachine == null)
                                    {
                                        failedRow = true;
                                        break;  // break switch
                                    }
                                    newPlan.MachineId = foundMachine.Id;
                                    break;
                                case "Employee":
                                    if (!string.IsNullOrEmpty(value))
                                    {
                                        var foundEmployee = lstEmployees.Where(x => (x.DisplayName + " - " + x.Code) == value).FirstOrDefault();
                                        if (foundEmployee == null)
                                        {
                                            failedRow = true;
                                            break;  // break switch
                                        }
                                        newPlan.EmployeeId = foundEmployee.Id;
                                    }
                                    break;
                                case "Product":
                                    var foundProduct = lstProducts.Where(x => (x.Name + " - " + x.Code) == value).FirstOrDefault();
                                    if (foundProduct == null)
                                    {
                                        failedRow = true;
                                        break;  // break switch
                                    }
                                    newPlan.ProductId = foundProduct.Id;
                                    break;
                                case "Expected Quantity":
                                    int expectedQuantity = 0;
                                    if (int.TryParse(value, out expectedQuantity))
                                    {
                                        newPlan.ExpectedQuantity = expectedQuantity;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }

                        if (failedRow == true)
                        {
                            failedRows.Add(r);
                        }
                        else
                        {
                            succeededPlans.Add(newPlan);
                        }
                    }

                    if (failedRows.Count > 0)
                    {
                        var strRows = string.Join(",", failedRows);
                        MessageBoxResult messageBoxResult = MessageBox.Show("There is(are) error(s) at row(s) " + strRows + ". " +
                            "Do you really want to continue?", Constants.ApplicationName, MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (messageBoxResult == MessageBoxResult.No)
                        {
                            return;
                        }
                    }

                    // save databases
                    mainWindow.applicationDbContext.Plans.AddRange(succeededPlans);
                    mainWindow.applicationDbContext.SaveChanges();

                    // notify
                    MessageBox.Show("Plans were imported successfully.", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);

                    // update UI
                    foreach (var plan in succeededPlans)
                    {
                        var planVM = new PlanViewModel(plan);
                        GridPlanVMs.Items.Add(planVM);
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = ex.GetAllExceptionInfo();
                log.Error(msg, ex);
                MessageBox.Show("Cannot import some plans", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            // Set the file name and get the output directory
            var fileName = "ProductPlanImportTemplate" + ".xlsx";
            var baseDirectory = @"C:\" + Constants.ApplicationName + @"\templates\";
            var filePath = Path.Combine(baseDirectory, fileName);

            try
            {
                var mainWindow = (MainWindow)Application.Current.MainWindow;

                if (!Directory.Exists(baseDirectory))
                {
                    Directory.CreateDirectory(baseDirectory);
                }

                // Create the file using the FileInfo object
                var file = new FileInfo(filePath);

                using (var package = new ExcelPackage(file))
                {
                    // add a new worksheet to the empty workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Plans");

                    var properties = new string[] { "Machine", "Employee", "Product", "Expected Quantity" };
                    for (var i = 0; i < properties.Length; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = properties[i];
                        worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                        worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                    }
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    // --------- DataValidations goes here -------------- //
                    //https://social.msdn.microsoft.com/Forums/vstudio/en-US/cdaf187c-df21-4cd9-9c05-7e94abb03f04/create-excel-sheet-with-drop-down-list-using-epplus?forum=exceldev

                    #region Machine Validation
                    // int FromRow, int FromCol, int ToRow, int ToCol
                    var ddMachines = worksheet.DataValidations.AddListValidation(worksheet.Cells[2, 1, 10000, 1].Address);
                    ddMachines.AllowBlank = false; //Set to true if blank value is accepted

                    var lstMachines = mainWindow.applicationDbContext.Machines
                                        .Where(x => !x.IsDeleted)
                                        .Select(x => new MachineViewModel
                                        {
                                            Id = x.Id,
                                            Name = x.Name,
                                            Code = x.Code,
                                        })
                                        .OrderBy(x => x.Name)
                                        .ToList();

                    foreach (var machine in lstMachines)
                    {
                        ddMachines.Formula.Values.Add(machine.Name + " - " + machine.Code);
                    }

                    // Or load from another sheet
                    //package.Workbook.Worksheets.Add("OtherSheet");
                    //list1.Formula.ExcelFormula = "OtherSheet!A1:A4";
                    #endregion

                    #region Employee Validation
                    var ddEmployees = worksheet.DataValidations.AddListValidation(worksheet.Cells[2, 2, 10000, 2].Address);
                    ddEmployees.AllowBlank = true; //Set to true if blank value is accepted

                    var lstEmployees = mainWindow.applicationDbContext.Employees
                                        .Where(x => !x.IsDeleted)
                                        .Select(x => new EmployeeViewModel
                                        {
                                            Id = x.Id,
                                            Code = x.Code,
                                            DisplayName = x.DisplayName,
                                            FirstName = x.FirstName,
                                            MiddleName = x.MiddleName,
                                            LastName = x.LastName,
                                        })
                                        .OrderBy(x => x.DisplayName)
                                        .ToList();

                    foreach (var employee in lstEmployees)
                    {
                        ddEmployees.Formula.Values.Add(employee.DisplayName + " - " + employee.Code);
                    }

                    #endregion

                    #region Product Validation
                    var ddProducts = worksheet.DataValidations.AddListValidation(worksheet.Cells[2, 3, 10000, 3].Address);
                    ddProducts.AllowBlank = false; //Set to true if blank value is accepted

                    var lstProducts = mainWindow.applicationDbContext.Products
                                        .Where(x => !x.IsDeleted)
                                        .Select(x => new ProductViewModel
                                        {
                                            Id = x.Id,
                                            Name = x.Name,
                                            Code = x.Code,
                                        })
                                        .OrderBy(x => x.Name)
                                        .ToList();

                    foreach (var product in lstProducts)
                    {
                        ddProducts.Formula.Values.Add(product.Name + " - " + product.Code);
                    }

                    #endregion

                    #region Expected Quantity Validation
                    //var ivExpectedQuantity = worksheet.DataValidations.AddIntegerValidation(worksheet.Cells[2, 4, 10000, 4].Address);
                    //ivExpectedQuantity.AllowBlank = true;
                    #endregion

                    // end
                    package.Save();
                }

                MessageBox.Show("Successfully download template at " + filePath, Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                var msg = ex.GetAllExceptionInfo();
                log.Error(msg, ex);
                MessageBox.Show("Cannot download template file", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                try
                {
                    var plan = obj as PlanViewModel;
                    var mainWindow = (MainWindow)Application.Current.MainWindow;

                    var deletingPlan = mainWindow.applicationDbContext.Plans.Where(x => x.Id == plan.Id).FirstOrDefault();
                    if (deletingPlan != null)
                    {
                        deletingPlan.IsDeleted = true; // soft delete
                        deletingPlan.ModifiedOn = DateTime.UtcNow;
                    }

                    // save databases
                    mainWindow.applicationDbContext.SaveChanges();

                    // notify
                    MessageBox.Show("Successfully deleted plan", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);

                    // update UI
                    GridPlanVMs.Items.Remove(plan);
                }
                catch (Exception ex)
                {
                    var msg = ex.GetAllExceptionInfo();
                    log.Error(msg, ex);
                    MessageBox.Show("Cannot remove the selected plan", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                }
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

            try
            {
                var mainWindow = (MainWindow)Application.Current.MainWindow;

                if (context.MachineId != Guid.Empty && context.ProductId != Guid.Empty)
                {
                    if (context.Id != Guid.Empty)
                    {
                        // update existing plan
                        var editingPlan = mainWindow.applicationDbContext.Plans.Where(x => x.Id == context.Id).FirstOrDefault();
                        if (editingPlan != null)
                        {
                            editingPlan.MachineId = context.MachineId;
                            editingPlan.EmployeeId = context.EmployeeId;
                            editingPlan.ProductId = context.ProductId;
                            editingPlan.ExpectedQuantity = context.ExpectedQuantity;
                            //editingPlan.ActualQuantity = newItem.ActualQuantity;
                            //editingPlan.StartTime = newItem.StartTime;
                            //editingPlan.EndTime = newItem.EndTime;
                            //editingPlan.IsProcessed = newItem.IsProcessed;
                            editingPlan.ModifiedOn = DateTime.UtcNow;
                        }

                        // save databases
                        mainWindow.applicationDbContext.SaveChanges();

                        // notify
                        MessageBox.Show("Successfully updated plan", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);

                        // update UI
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
                            GridPlanVMs.Items[index] = context;
                        }
                    }
                    else
                    {
                        // create new plan
                        var newPlan = new Plan
                        {
                            Id = Guid.NewGuid(),
                            MachineId = context.MachineId,
                            EmployeeId = context.EmployeeId,
                            ProductId = context.ProductId,
                            ExpectedQuantity = context.ExpectedQuantity,
                            CreatedOn = DateTime.UtcNow,
                            IsProcessed = false
                        };
                        mainWindow.applicationDbContext.Plans.Add(newPlan);

                        // save databases
                        mainWindow.applicationDbContext.SaveChanges();

                        // notify
                        MessageBox.Show("Successfully created plan", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);

                        // update UI
                        var newPlanVM = new PlanViewModel(newPlan);
                        GridPlanVMs.Items.Add(newPlanVM);
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = ex.GetAllExceptionInfo();
                log.Error(msg, ex);

                if (context.Id != Guid.Empty)
                {
                    // update
                    MessageBox.Show("Cannot edit plan", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    // create
                    MessageBox.Show("Cannot create plan", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
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
