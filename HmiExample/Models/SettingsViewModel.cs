using HmiExample.Data;
using HmiExample.Helpers;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HmiExample.Models
{
    public class SettingsViewModel : ObservableBase
    {
        public ICommand AddMachineCommand => new CommandsImplementation(ExecuteAddMachine);
        public ICommand AddEmployeeCommand => new CommandsImplementation(ExecuteAddEmployee);

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
            // load databases
            var mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
            if (mainWindow.applicationDbContext.Employees.Local != null)
            {
                var lstEmployees = mainWindow.applicationDbContext.Employees.Local.Where(x => !x.IsDeleted).OrderBy(x => x.DisplayName).ToList();

                //using (var context = new ApplicationDbContext())
                //{
                //var dbEmployees = context.Employees; // define query
                //var lstEmployees = dbEmployees.ToList(); // query executed and data obtained from database
                foreach (var employee in lstEmployees)
                {
                    var employeeVM = new EmployeeViewModel
                    {
                        DisplayName = employee.DisplayName,
                        Code = employee.Code
                    };
                    _employees.Items.Add(employeeVM);
                }
                //}
            }

            // register event
            _employees.Items.CollectionChanged += Employees_CollectionChanged;
        }

        public void LoadMachines()
        {
            // load databases
            var mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
            if (mainWindow.applicationDbContext.Machines.Local != null)
            {
                var lstMachines = mainWindow.applicationDbContext.Machines.Local.Where(x => !x.IsDeleted).OrderBy(x => x.Name).ToList();

                //using (var context = new ApplicationDbContext())
                //{
                //var dbMachines = context.Machines; // define query
                //var lstMachines = dbMachines.ToList(); // query executed and data obtained from database
                foreach (var machine in lstMachines)
                {
                    var machineVM = new MachineViewModel
                    {
                        Name = machine.Name,
                        Code = machine.Code
                    };
                    _machines.Items.Add(machineVM);
                }
                //}
            }

            // register event
            _machines.Items.CollectionChanged += Machines_CollectionChanged;
        }

        public void LoadProducts()
        {
            // load databases
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            if (mainWindow.applicationDbContext.Products.Local != null)
            {
                var lstProducts = mainWindow.applicationDbContext.Products.Local.Where(x => !x.IsDeleted).OrderBy(x => x.Name).ToList();

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
            }

            // register event
            _products.Items.CollectionChanged += Products_CollectionChanged;
        }

        #region Machines

        private void Machines_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
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
                var newMachine = new MachineViewModel { Name = context.Name, Code = context.Code };

                //var command = Machines.AddItemCommand;
                //if (command != null && command.CanExecute(newMachine))
                //    command.Execute(newMachine);
                Machines.Items.Add(newMachine);
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

            //if (!string.IsNullOrEmpty(context.Name) && !string.IsNullOrEmpty(context.Code))
            //{
            //    var newEmployee = new EmployeeViewModel { Name = context.Name, Code = context.Code };
            //    Employees.Items.Add(newEmployee);
            //}
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
                        var deletingProduct = mainWindow.applicationDbContext.Products.Local.Where(x => x.Id == item.Id).FirstOrDefault();
                        if (deletingProduct != null)
                        {
                            deletingProduct.IsDeleted = true; // soft delete
                            deletingProduct.ModifiedOn = DateTime.UtcNow;

                            int index = mainWindow.applicationDbContext.Products.Local.IndexOf(deletingProduct);
                            if (index >= 0)
                            {
                                mainWindow.applicationDbContext.Products.Local.Insert(index, deletingProduct);
                            }
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

                        var editingProduct = mainWindow.applicationDbContext.Products.Local.Where(x => x.Id == oldItem.Id).FirstOrDefault();
                        if (editingProduct != null)
                        {
                            editingProduct.Name = newItem.Name;
                            editingProduct.Code = newItem.Code;
                            editingProduct.ModifiedOn = DateTime.UtcNow;

                            int index = mainWindow.applicationDbContext.Products.Local.IndexOf(editingProduct);
                            if (index >= 0)
                            {
                                mainWindow.applicationDbContext.Products.Local.Insert(index, editingProduct);
                            }
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
                var msg = ex.GetAllExceptionInfo(); // TODO: log file or db
                var msg1 = ex.GetRootMessage();
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
