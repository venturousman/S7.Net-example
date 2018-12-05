using log4net;
using MaterialDesignThemes.Wpf;
using ProductionEquipmentControlSoftware.Data;
using ProductionEquipmentControlSoftware.Helpers;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ProductionEquipmentControlSoftware.Models
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
            using (var applicationDbContext = new ApplicationDbContext())
            {
                // reset
                _employees.Items.Clear();

                // load databases
                var lstEmployees = applicationDbContext.Employees.Where(x => !x.IsDeleted).OrderBy(x => x.DisplayName).ToList();

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
            }
        }

        public void LoadMachines()
        {
            using (var applicationDbContext = new ApplicationDbContext())
            {
                // reset
                _machines.Items.Clear();

                // load databases
                var lstMachines = applicationDbContext.Machines.Where(x => !x.IsDeleted).OrderBy(x => x.Name).ToList();

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
                        Count = machine.Count,
                        CumulativeCount = machine.CumulativeCount
                    };
                    _machines.Items.Add(machineVM);
                }
            }
        }

        public void LoadProducts()
        {
            using (var applicationDbContext = new ApplicationDbContext())
            {
                // reset
                _products.Items.Clear();

                // load databases
                var lstProducts = applicationDbContext.Products.Where(x => !x.IsDeleted).OrderBy(x => x.Name).ToList();

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
            }
        }

        #region Machines

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

            try
            {
                using (var applicationDbContext = new ApplicationDbContext())
                {
                    if (!string.IsNullOrEmpty(context.Name) && !string.IsNullOrEmpty(context.Code))
                    {
                        if (context.Id != Guid.Empty)
                        {
                            // update existing machine
                            var editingMachine = applicationDbContext.Machines.Where(x => x.Id == context.Id).FirstOrDefault();
                            if (editingMachine != null)
                            {
                                editingMachine.Name = context.Name;
                                editingMachine.Code = context.Code;
                                editingMachine.TagIndex = context.TagIndex;
                                //editingMachine.Counts = context.Counts;
                                editingMachine.ModifiedOn = DateTime.UtcNow;
                            }

                            // save databases
                            applicationDbContext.SaveChanges();

                            // notify
                            MessageBox.Show("Successfully updated machine", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);

                            // update UI
                            LoadMachines();
                        }
                        else
                        {
                            // create new machine
                            var newMachine = new Machine
                            {
                                Id = Guid.NewGuid(),
                                Name = context.Name,
                                Code = context.Code,
                                TagIndex = context.TagIndex,
                                Count = 0,
                                CumulativeCount = 0,
                                CreatedOn = DateTime.UtcNow,
                            };
                            applicationDbContext.Machines.Add(newMachine);

                            // save databases
                            applicationDbContext.SaveChanges();

                            // notify
                            MessageBox.Show("Successfully created machine", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);

                            // update UI
                            LoadMachines();
                        }
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
                    MessageBox.Show("Cannot edit machine", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    // create
                    MessageBox.Show("Cannot create machine", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
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
                    Count = editingMachine.Count,
                    CumulativeCount = editingMachine.CumulativeCount
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
                try
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to delete this machine?",
                    Constants.ApplicationName, MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (messageBoxResult == MessageBoxResult.No)
                    {
                        return;
                    }

                    using (var applicationDbContext = new ApplicationDbContext())
                    {
                        var machine = obj as MachineViewModel;

                        var deletingMachine = applicationDbContext.Machines.Where(x => x.Id == machine.Id).FirstOrDefault();
                        if (deletingMachine != null)
                        {
                            deletingMachine.IsDeleted = true; // soft delete
                            deletingMachine.ModifiedOn = DateTime.UtcNow;
                        }

                        // save databases
                        applicationDbContext.SaveChanges();

                        // notify
                        MessageBox.Show("Successfully deleted machine", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);

                        // update UI
                        LoadMachines();
                    }
                }
                catch (Exception ex)
                {
                    var msg = ex.GetAllExceptionInfo();
                    log.Error(msg, ex);
                    MessageBox.Show("Cannot remove the selected machine", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        #endregion

        #region Employees

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

            try
            {
                using (var applicationDbContext = new ApplicationDbContext())
                {
                    if (!string.IsNullOrEmpty(context.Code) && !string.IsNullOrEmpty(context.FirstName)
                    && !string.IsNullOrEmpty(context.LastName) && !string.IsNullOrEmpty(context.Email))
                    {
                        if (context.Id != Guid.Empty)
                        {
                            // update existing employee
                            var editingEmployee = applicationDbContext.Employees.Where(x => x.Id == context.Id).FirstOrDefault();
                            if (editingEmployee != null)
                            {
                                editingEmployee.Code = context.Code;
                                editingEmployee.FirstName = context.FirstName;
                                editingEmployee.MiddleName = context.MiddleName;
                                editingEmployee.LastName = context.LastName;
                                editingEmployee.DisplayName = context.DisplayName;
                                editingEmployee.Email = context.Email;
                                editingEmployee.PhoneNumber = context.PhoneNumber;
                                editingEmployee.Photo = context.Photo;
                                editingEmployee.PhotoContent = context.PhotoContent;
                                editingEmployee.ModifiedOn = DateTime.UtcNow;
                            }

                            // save databases
                            applicationDbContext.SaveChanges();

                            // notify
                            MessageBox.Show("Successfully updated employee", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);

                            // update UI
                            LoadEmployees();
                        }
                        else
                        {
                            // create new employee
                            var newEmployee = new Employee
                            {
                                Id = Guid.NewGuid(),
                                Code = context.Code,
                                FirstName = context.FirstName,
                                MiddleName = context.MiddleName,
                                LastName = context.LastName,
                                DisplayName = context.DisplayName,
                                Email = context.Email,
                                PhoneNumber = context.PhoneNumber,
                                Photo = context.Photo,
                                PhotoContent = context.PhotoContent,
                                CreatedOn = DateTime.UtcNow,
                            };
                            applicationDbContext.Employees.Add(newEmployee);

                            // save databases
                            applicationDbContext.SaveChanges();

                            // notify
                            MessageBox.Show("Successfully created employee", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);

                            // update UI
                            LoadEmployees();
                        }
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
                    MessageBox.Show("Cannot edit employee", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    // create
                    MessageBox.Show("Cannot create employee", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
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
                try
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to delete this employee?",
                    Constants.ApplicationName, MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (messageBoxResult == MessageBoxResult.No)
                    {
                        return;
                    }

                    using (var applicationDbContext = new ApplicationDbContext())
                    {
                        var employee = obj as EmployeeViewModel;

                        var deletingEmployee = applicationDbContext.Employees.Where(x => x.Id == employee.Id).FirstOrDefault();
                        if (deletingEmployee != null)
                        {
                            deletingEmployee.IsDeleted = true; // soft delete
                            deletingEmployee.ModifiedOn = DateTime.UtcNow;
                        }

                        // save databases
                        applicationDbContext.SaveChanges();

                        // notify
                        MessageBox.Show("Successfully deleted employee", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);

                        // update UI
                        LoadEmployees();
                    }
                }
                catch (Exception ex)
                {
                    var msg = ex.GetAllExceptionInfo();
                    log.Error(msg, ex);
                    MessageBox.Show("Cannot remove the selected employee", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        #endregion

        #region Products

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

            try
            {
                using (var applicationDbContext = new ApplicationDbContext())
                {
                    if (!string.IsNullOrEmpty(context.Name) && !string.IsNullOrEmpty(context.Code))
                    {
                        if (context.Id != Guid.Empty)
                        {
                            // update existing product
                            var editingProduct = applicationDbContext.Products.Where(x => x.Id == context.Id).FirstOrDefault();
                            if (editingProduct != null)
                            {
                                editingProduct.Name = context.Name;
                                editingProduct.Code = context.Code;
                                editingProduct.ModifiedOn = DateTime.UtcNow;
                            }

                            // save databases
                            applicationDbContext.SaveChanges();

                            // notify
                            MessageBox.Show("Successfully updated product", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);

                            // update UI                        
                            LoadProducts();
                        }
                        else
                        {
                            // create new product
                            var newProduct = new Product
                            {
                                Id = Guid.NewGuid(),
                                Name = context.Name,
                                Code = context.Code,
                                CreatedOn = DateTime.UtcNow
                            };
                            applicationDbContext.Products.Add(newProduct);

                            // save databases
                            applicationDbContext.SaveChanges();

                            // notify
                            MessageBox.Show("Successfully created product", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);

                            // update UI
                            LoadProducts();
                        }
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
                    MessageBox.Show("Cannot edit product", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    // create
                    MessageBox.Show("Cannot create product", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
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
                try
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to delete this product?",
                    Constants.ApplicationName, MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (messageBoxResult == MessageBoxResult.No)
                    {
                        return;
                    }

                    using (var applicationDbContext = new ApplicationDbContext())
                    {
                        var product = obj as ProductViewModel;

                        var deletingProduct = applicationDbContext.Products.Where(x => x.Id == product.Id).FirstOrDefault();
                        if (deletingProduct != null)
                        {
                            deletingProduct.IsDeleted = true; // soft delete
                            deletingProduct.ModifiedOn = DateTime.UtcNow;
                        }

                        // save databases
                        applicationDbContext.SaveChanges();

                        // notify
                        MessageBox.Show("Successfully deleted product", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);

                        // update UI
                        LoadProducts();
                    }
                }
                catch (Exception ex)
                {
                    var msg = ex.GetAllExceptionInfo();
                    log.Error(msg, ex);
                    MessageBox.Show("Cannot remove the selected product", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        #endregion
    }
}
