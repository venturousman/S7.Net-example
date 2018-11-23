using HmiExample.Helpers;
using log4net;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HmiExample.Models
{
    public class SettingsViewModel : ObservableBase
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ICommand AddMachineCommand => new CommandsImplementation(ExecuteAddMachine);
        public ICommand DeleteMachineCommand => new CommandsImplementation(ExecuteDeleteMachine);
        public ICommand EditMachineCommand => new CommandsImplementation(ExecuteEditMachine);

        public ICommand AddEmployeeCommand => new CommandsImplementation(ExecuteAddEmployee);
        public ICommand DeleteEmployeeCommand => new CommandsImplementation(ExecuteDeleteEmployee);
        public ICommand EditEmployeeCommand => new CommandsImplementation(ExecuteEditEmployee);

        public ICommand AddProductCommand => new CommandsImplementation(ExecuteAddProduct);
        public ICommand DeleteProductCommand => new CommandsImplementation(ExecuteDeleteProduct);
        public ICommand EditProductCommand => new CommandsImplementation(ExecuteEditProduct);

        private GridViewModel<ProductViewModel> _products = new GridViewModel<ProductViewModel>();
        private GridViewModel<EmployeeViewModel> _employees = new GridViewModel<EmployeeViewModel>();
        private GridViewModel<MachineViewModel> _machines = new GridViewModel<MachineViewModel>();

        public GridViewModel<MachineViewModel> Machines { get { return _machines; } }    // should be GridMachinesVM
        public GridViewModel<EmployeeViewModel> Employees { get { return _employees; } }
        public GridViewModel<ProductViewModel> Products { get { return _products; } }

        public SettingsViewModel()
        {
        }

        public void LoadEmployees()
        {
            // reset
            _employees.Items.Clear();

            // load databases
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            var lstEmployees = mainWindow.applicationDbContext.Employees.Where(x => !x.IsDeleted).OrderBy(x => x.DisplayName).ToList();

            //using (var context = new ApplicationDbContext())
            //{
            //var dbEmployees = context.Employees; // define query
            //var lstEmployees = dbEmployees.ToList(); // query executed and data obtained from database
            foreach (var employee in lstEmployees)
            {
                var employeeVM = new EmployeeViewModel
                {
                    Id = employee.Id,
                    DisplayName = employee.DisplayName,
                    Code = employee.Code,
                    Email = employee.Email,
                    FirstName = employee.FirstName,
                    MiddleName = employee.MiddleName,
                    LastName = employee.LastName,
                    PhoneNumber = employee.PhoneNumber,
                    Photo = employee.Photo,
                    PhotoContent = employee.PhotoContent
                };
                _employees.Items.Add(employeeVM);
            }
            //}

            // register event
            _employees.Items.CollectionChanged += Employees_CollectionChanged;
        }

        public void LoadMachines()
        {
            // reset
            _machines.Items.Clear();

            // load databases
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            var lstMachines = mainWindow.applicationDbContext.Machines.Where(x => !x.IsDeleted).OrderBy(x => x.Name).ToList();

            //using (var context = new ApplicationDbContext())
            //{
            //var dbMachines = context.Machines; // define query
            //var lstMachines = dbMachines.ToList(); // query executed and data obtained from database
            foreach (var machine in lstMachines)
            {
                var machineVM = new MachineViewModel
                {
                    Id = machine.Id,
                    Name = machine.Name,
                    Code = machine.Code,
                    TagIndex = machine.TagIndex,
                    Counts = machine.Counts
                };
                _machines.Items.Add(machineVM);
            }
            //}

            // register event
            _machines.Items.CollectionChanged += Machines_CollectionChanged;
        }

        public void LoadProducts()
        {
            // reset
            _products.Items.Clear();

            // load databases
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            var lstProducts = mainWindow.applicationDbContext.Products.Where(x => !x.IsDeleted).OrderBy(x => x.Name).ToList();

            //using (var context = new ApplicationDbContext())
            //{
            //var dbProducts = context.Products; // define query
            //var lstProducts = dbProducts.ToList(); // query executed and data obtained from database
            foreach (var product in lstProducts)
            {
                var productVM = new ProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Code = product.Code
                };
                _products.Items.Add(productVM);
            }
            //}

            // register event
            _products.Items.CollectionChanged += Products_CollectionChanged;
        }

        #region Machines

        private void Machines_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;

            try
            {
                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (MachineViewModel item in e.OldItems)
                    {
                        var deletingMachine = mainWindow.applicationDbContext.Machines.Where(x => x.Id == item.Id).FirstOrDefault();
                        if (deletingMachine != null)
                        {
                            deletingMachine.IsDeleted = true; // soft delete
                            deletingMachine.ModifiedOn = DateTime.UtcNow;
                        }
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    var newMachines = new List<Machine>();
                    foreach (MachineViewModel item in e.NewItems)
                    {
                        var newMachine = new Machine
                        {
                            Id = Guid.NewGuid(),
                            Name = item.Name,
                            Code = item.Code,
                            TagIndex = item.TagIndex,
                            Counts = 0,
                            CreatedOn = DateTime.UtcNow,
                        };
                        newMachines.Add(newMachine);
                    }
                    mainWindow.applicationDbContext.Machines.AddRange(newMachines);
                }
                else if (e.Action == NotifyCollectionChangedAction.Replace)
                {
                    for (int i = 0; i < e.OldItems.Count; i++)
                    {
                        var oldItem = (MachineViewModel)e.OldItems[i];
                        var newItem = (MachineViewModel)e.NewItems[i];

                        var editingMachine = mainWindow.applicationDbContext.Machines.Where(x => x.Id == oldItem.Id).FirstOrDefault();
                        if (editingMachine != null)
                        {
                            editingMachine.Name = newItem.Name;
                            editingMachine.Code = newItem.Code;
                            editingMachine.TagIndex = newItem.TagIndex;
                            // editingMachine.Counts = newItem.Counts;
                            editingMachine.ModifiedOn = DateTime.UtcNow;
                        }
                    }
                }

                // save databases
                mainWindow.applicationDbContext.SaveChanges();

                // notify
                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    MessageBox.Show("Successfully deleted machines", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    MessageBox.Show("Successfully created machines", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (e.Action == NotifyCollectionChangedAction.Replace)
                {
                    MessageBox.Show("Successfully updated machines", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                var msg = ex.GetAllExceptionInfo();
                log.Error(msg, ex);
                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    MessageBox.Show("Cannot remove the selected machines", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    MessageBox.Show("Cannot create these machines", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);

                    //Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
                    //{
                    //    _machines.Items.CollectionChanged -= Machines_CollectionChanged;

                    //    foreach (MachineViewModel item in e.NewItems)
                    //    {
                    //        Machines.Items.Remove(item);
                    //    }

                    //    _machines.Items.CollectionChanged += Machines_CollectionChanged;
                    //}));
                }
                else if (e.Action == NotifyCollectionChangedAction.Replace)
                {
                    MessageBox.Show("Cannot edit these machines", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            // mainWindow.mainFrame.Navigate(new Settings());
            // mainWindow.mainFrame.Refresh();


            //rollback
            //await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
            //    CoreDispatcherPriority.Normal, () => {

            //    //disable/enable event handler
            //    Items.CollectionChanged -= ItemsChanged;

            //        Items.Remove(e.NewItems[0]);

            //        Items.CollectionChanged += ItemsChanged;
            //    })).AsTask();
        }

        private async void ExecuteAddMachine(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new AddMachineDialog
            {
                DataContext = new MachineViewModel()
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", OpenedMachineDialogEventHandler, ClosingMachineDialogEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }

        private void ClosingMachineDialogEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, true)) return;

            var context = (MachineViewModel)((AddMachineDialog)eventArgs.Session.Content).DataContext;

            if (!string.IsNullOrEmpty(context.Name) && !string.IsNullOrEmpty(context.Code))
            {
                if (context.Id != Guid.Empty)
                {
                    var index = -1;
                    for (int i = 0; i < Machines.Items.Count; i++)
                    {
                        if (Machines.Items[i].Id == context.Id)
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index != -1)
                    {
                        Machines.Items[index] = context;
                    }
                }
                else
                {
                    var newMachine = new MachineViewModel
                    {
                        Name = context.Name,
                        Code = context.Code,
                        TagIndex = context.TagIndex
                    };
                    Machines.Items.Add(newMachine);
                }
            }
        }

        private void OpenedMachineDialogEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            Console.WriteLine("You could intercept the open and affect the dialog using eventArgs.Session.");
        }

        private async void ExecuteEditMachine(object o)
        {
            if (!(o is MachineViewModel)) return;

            var editingMachine = o as MachineViewModel;

            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new AddMachineDialog
            {
                DataContext = new MachineViewModel()
                {
                    Id = editingMachine.Id,
                    Name = editingMachine.Name,
                    Code = editingMachine.Code,
                    TagIndex = editingMachine.TagIndex,
                    Counts = editingMachine.Counts
                }
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", OpenedMachineDialogEventHandler, ClosingMachineDialogEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }

        private void ExecuteDeleteMachine(object obj)
        {
            if (obj is MachineViewModel)
            {
                var machine = obj as MachineViewModel;
                Machines.Items.Remove(machine);
            }
        }

        #endregion

        #region Employees

        private void Employees_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {
                var mainWindow = (MainWindow)Application.Current.MainWindow;

                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (EmployeeViewModel item in e.OldItems)
                    {
                        var deletingEmployee = mainWindow.applicationDbContext.Employees.Where(x => x.Id == item.Id).FirstOrDefault();
                        if (deletingEmployee != null)
                        {
                            deletingEmployee.IsDeleted = true; // soft delete
                            deletingEmployee.ModifiedOn = DateTime.UtcNow;
                        }
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    var newEmployees = new List<Employee>();
                    foreach (EmployeeViewModel item in e.NewItems)
                    {
                        var newEmployee = new Employee
                        {
                            Id = Guid.NewGuid(),
                            Code = item.Code,
                            FirstName = item.FirstName,
                            MiddleName = item.MiddleName,
                            LastName = item.LastName,
                            DisplayName = item.DisplayName,
                            Email = item.Email,
                            PhoneNumber = item.PhoneNumber,
                            Photo = item.Photo,
                            PhotoContent = item.PhotoContent,
                            CreatedOn = DateTime.UtcNow,
                        };
                        newEmployees.Add(newEmployee);
                    }
                    mainWindow.applicationDbContext.Employees.AddRange(newEmployees);
                }
                else if (e.Action == NotifyCollectionChangedAction.Replace)
                {
                    for (int i = 0; i < e.OldItems.Count; i++)
                    {
                        var oldItem = (EmployeeViewModel)e.OldItems[i];
                        var newItem = (EmployeeViewModel)e.NewItems[i];

                        var editingEmployee = mainWindow.applicationDbContext.Employees.Where(x => x.Id == oldItem.Id).FirstOrDefault();
                        if (editingEmployee != null)
                        {
                            editingEmployee.Code = newItem.Code;
                            editingEmployee.FirstName = newItem.FirstName;
                            editingEmployee.MiddleName = newItem.MiddleName;
                            editingEmployee.LastName = newItem.LastName;
                            editingEmployee.DisplayName = newItem.DisplayName;
                            editingEmployee.Email = newItem.Email;
                            editingEmployee.PhoneNumber = newItem.PhoneNumber;
                            editingEmployee.Photo = newItem.Photo;
                            editingEmployee.PhotoContent = newItem.PhotoContent;
                            editingEmployee.ModifiedOn = DateTime.UtcNow;
                        }
                    }
                }

                // save databases
                mainWindow.applicationDbContext.SaveChanges();

                // notify
                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    MessageBox.Show("Successfully deleted employees", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    MessageBox.Show("Successfully created employees", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (e.Action == NotifyCollectionChangedAction.Replace)
                {
                    MessageBox.Show("Successfully updated employees", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                var msg = ex.GetAllExceptionInfo();
                log.Error(msg, ex);
                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    MessageBox.Show("Cannot remove the selected employees", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    MessageBox.Show("Cannot create these employees", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (e.Action == NotifyCollectionChangedAction.Replace)
                {
                    MessageBox.Show("Cannot edit these employees", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void ExecuteAddEmployee(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new AddEmployeeDialog
            {
                DataContext = new EmployeeViewModel()
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", OpenedEmployeeDialogEventHandler, ClosingEmployeeDialogEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }

        private void OpenedEmployeeDialogEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            Console.WriteLine("You could intercept the open and affect the dialog using eventArgs.Session.");
        }

        private void ClosingEmployeeDialogEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, true)) return;

            var context = (EmployeeViewModel)((AddEmployeeDialog)eventArgs.Session.Content).DataContext;

            if (!string.IsNullOrEmpty(context.Code) && !string.IsNullOrEmpty(context.FirstName)
                && !string.IsNullOrEmpty(context.LastName) && !string.IsNullOrEmpty(context.Email))
            {
                if (context.Id != Guid.Empty)
                {
                    var index = -1;
                    for (int i = 0; i < Employees.Items.Count; i++)
                    {
                        if (Employees.Items[i].Id == context.Id)
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index != -1)
                    {
                        Employees.Items[index] = context;
                    }
                }
                else
                {
                    var newEmployee = new EmployeeViewModel
                    {
                        DisplayName = context.DisplayName,
                        Code = context.Code,
                        FirstName = context.FirstName,
                        MiddleName = context.MiddleName,
                        LastName = context.LastName,
                        Photo = context.Photo,
                        PhotoContent = context.PhotoContent,
                        PhoneNumber = context.PhoneNumber,
                        Email = context.Email
                    };
                    Employees.Items.Add(newEmployee);
                }
            }
        }

        private async void ExecuteEditEmployee(object o)
        {
            if (!(o is EmployeeViewModel)) return;

            var editingEmployee = o as EmployeeViewModel;

            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new AddEmployeeDialog
            {
                DataContext = new EmployeeViewModel()
                {
                    Id = editingEmployee.Id,
                    Code = editingEmployee.Code,
                    DisplayName = editingEmployee.DisplayName,
                    FirstName = editingEmployee.FirstName,
                    MiddleName = editingEmployee.MiddleName,
                    LastName = editingEmployee.LastName,
                    Email = editingEmployee.Email,
                    PhoneNumber = editingEmployee.PhoneNumber,
                    Photo = editingEmployee.Photo,
                    PhotoContent = editingEmployee.PhotoContent
                }
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", OpenedEmployeeDialogEventHandler, ClosingEmployeeDialogEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }

        private void ExecuteDeleteEmployee(object obj)
        {
            if (obj is EmployeeViewModel)
            {
                var employee = obj as EmployeeViewModel;
                Employees.Items.Remove(employee);
            }
        }

        #endregion

        #region Products

        private void Products_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {
                var mainWindow = (MainWindow)Application.Current.MainWindow;

                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (ProductViewModel item in e.OldItems)
                    {
                        var deletingProduct = mainWindow.applicationDbContext.Products.Where(x => x.Id == item.Id).FirstOrDefault();
                        if (deletingProduct != null)
                        {
                            deletingProduct.IsDeleted = true; // soft delete
                            deletingProduct.ModifiedOn = DateTime.UtcNow;
                        }
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    var newProducts = new List<Product>();
                    foreach (ProductViewModel item in e.NewItems)
                    {
                        var newProduct = new Product
                        {
                            Id = Guid.NewGuid(),
                            Name = item.Name,
                            Code = item.Code,
                            CreatedOn = DateTime.UtcNow
                        };
                        newProducts.Add(newProduct);
                    }
                    mainWindow.applicationDbContext.Products.AddRange(newProducts);
                }
                else if (e.Action == NotifyCollectionChangedAction.Replace)
                {
                    for (int i = 0; i < e.OldItems.Count; i++)
                    {
                        var oldItem = (ProductViewModel)e.OldItems[i];
                        var newItem = (ProductViewModel)e.NewItems[i];

                        var editingProduct = mainWindow.applicationDbContext.Products.Where(x => x.Id == oldItem.Id).FirstOrDefault();
                        if (editingProduct != null)
                        {
                            editingProduct.Name = newItem.Name;
                            editingProduct.Code = newItem.Code;
                            editingProduct.ModifiedOn = DateTime.UtcNow;
                        }
                    }
                }

                // save databases
                mainWindow.applicationDbContext.SaveChanges();

                // notify
                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    MessageBox.Show("Successfully deleted products", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    MessageBox.Show("Successfully created products", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (e.Action == NotifyCollectionChangedAction.Replace)
                {
                    MessageBox.Show("Successfully updated products", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                var msg = ex.GetAllExceptionInfo();
                log.Error(msg, ex);
                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    MessageBox.Show("Cannot remove the selected products", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    MessageBox.Show("Cannot create these products", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (e.Action == NotifyCollectionChangedAction.Replace)
                {
                    MessageBox.Show("Cannot edit these products", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void ExecuteAddProduct(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new AddProductDialog
            {
                DataContext = new ProductViewModel()
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", OpenedProductDialogEventHandler, ClosingProductDialogEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }

        private void OpenedProductDialogEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            Console.WriteLine("You could intercept the open and affect the dialog using eventArgs.Session.");
        }

        private void ClosingProductDialogEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, true)) return;

            var context = (ProductViewModel)((AddProductDialog)eventArgs.Session.Content).DataContext;

            if (!string.IsNullOrEmpty(context.Name) && !string.IsNullOrEmpty(context.Code))
            {
                if (context.Id != Guid.Empty)
                {
                    var index = -1;
                    for (int i = 0; i < Products.Items.Count; i++)
                    {
                        if (Products.Items[i].Id == context.Id)
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index != -1)
                    {
                        // Products.Items.Insert(index, context);
                        Products.Items[index] = context;
                    }
                }
                else
                {
                    var newProduct = new ProductViewModel { Name = context.Name, Code = context.Code };
                    Products.Items.Add(newProduct);
                }
            }
        }

        private async void ExecuteEditProduct(object o)
        {
            if (!(o is ProductViewModel)) return;

            var editingProduct = o as ProductViewModel;

            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new AddProductDialog
            {
                DataContext = new ProductViewModel()
                {
                    Id = editingProduct.Id,
                    Name = editingProduct.Name,
                    Code = editingProduct.Code
                }
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", OpenedProductDialogEventHandler, ClosingProductDialogEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }

        private void ExecuteDeleteProduct(object obj)
        {
            if (obj is ProductViewModel)
            {
                var product = obj as ProductViewModel;
                Products.Items.Remove(product);
            }
        }

        #endregion
    }
}
