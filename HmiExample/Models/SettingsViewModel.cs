using HmiExample.Data;
using HmiExample.Helpers;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;

namespace HmiExample.Models
{
    public class SettingsViewModel : ObservableBase
    {
        public ICommand AddMachineCommand => new CommandsImplementation(ExecuteAddMachineDialog);
        public ICommand AddEmployeeCommand => new CommandsImplementation(ExecuteAddEmployeeDialog);
        public ICommand AddProductCommand => new CommandsImplementation(ExecuteAddProductDialog);

        public GridViewModel<MachineViewModel> Machines { get; }    // should be GridMachinesVM
        public GridViewModel<EmployeeViewModel> Employees { get; }
        public GridViewModel<ProductViewModel> Products { get; }

        public SettingsViewModel()
        {
            Machines = LoadMachines();
            Employees = LoadEmployees();
            Products = LoadProducts();
        }

        private GridViewModel<EmployeeViewModel> LoadEmployees()
        {
            var employees = new ObservableCollection<EmployeeViewModel>();

            // load databases
            var mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
            if (mainWindow.applicationDbContext.Employees.Local != null)
            {
                var lstEmployees = mainWindow.applicationDbContext.Employees.Local.ToList();

                //using (var context = new ApplicationDbContext())
                //{
                //var dbEmployees = context.Employees; // define query
                //var lstEmployees = dbEmployees.ToList(); // query executed and data obtained from database
                foreach (var employee in lstEmployees)
                {
                    var employeeVM = new EmployeeViewModel
                    {
                        Name = employee.DisplayName,
                        Code = employee.Code
                    };
                    employees.Add(employeeVM);
                }
                //}
            }

            // register event
            employees.CollectionChanged += Employees_CollectionChanged;

            return new GridViewModel<EmployeeViewModel>(employees);
        }

        private GridViewModel<MachineViewModel> LoadMachines()
        {
            var machines = new ObservableCollection<MachineViewModel>();

            // load databases
            var mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
            if (mainWindow.applicationDbContext.Machines.Local != null)
            {
                var lstMachines = mainWindow.applicationDbContext.Machines.Local.ToList();

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
                    machines.Add(machineVM);
                }
                //}
            }

            // register event
            machines.CollectionChanged += Machines_CollectionChanged;

            return new GridViewModel<MachineViewModel>(machines);
        }

        private GridViewModel<ProductViewModel> LoadProducts()
        {
            var products = new ObservableCollection<ProductViewModel>();

            // load databases
            var mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
            if (mainWindow.applicationDbContext.Products.Local != null)
            {
                var lstProducts = mainWindow.applicationDbContext.Products.Local.ToList();

                //using (var context = new ApplicationDbContext())
                //{
                //var dbProducts = context.Products; // define query
                //var lstProducts = dbProducts.ToList(); // query executed and data obtained from database
                foreach (var product in lstProducts)
                {
                    var productVM = new ProductViewModel
                    {
                        Name = product.Name,
                        Code = product.Code
                    };
                    products.Add(productVM);
                }
                //}
            }

            // register event
            products.CollectionChanged += Products_CollectionChanged;

            return new GridViewModel<ProductViewModel>(products);
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

        private async void ExecuteAddMachineDialog(object o)
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

        private async void ExecuteAddEmployeeDialog(object o)
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

            if (!string.IsNullOrEmpty(context.Name) && !string.IsNullOrEmpty(context.Code))
            {
                var newEmployee = new EmployeeViewModel { Name = context.Name, Code = context.Code };
                Employees.Items.Add(newEmployee);
            }
        }

        #endregion

        #region Products

        private void Products_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
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

        private async void ExecuteAddProductDialog(object o)
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
                var newProduct = new ProductViewModel { Name = context.Name, Code = context.Code };
                Products.Items.Add(newProduct);
            }
        }

        #endregion
    }
}
