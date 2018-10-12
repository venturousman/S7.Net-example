﻿using HmiExample.Helpers;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;

namespace HmiExample.Models
{
    public class SettingsViewModel : ObservableBase
    {
        public ICommand AddMachineCommand => new CommandsImplementation(ExecuteAddMachineDialog);
        public ICommand AddEmployeeCommand => new CommandsImplementation(ExecuteAddEmployeeDialog);

        public GridViewModel<MachineViewModel> Machines { get; }
        public GridViewModel<EmployeeViewModel> Employees { get; }

        public SettingsViewModel()
        {
            Machines = LoadMachines();
            Employees = LoadEmployees();
        }

        private GridViewModel<EmployeeViewModel> LoadEmployees()
        {
            // TODO: load databases

            var employees = new ObservableCollection<EmployeeViewModel>
            {
                new EmployeeViewModel
                {
                    Code = "E001",
                    Name = "Employee A",
                },
                new EmployeeViewModel
                {
                    Code = "E002",
                    Name = "Employee B",
                }
            };
            employees.CollectionChanged += Employees_CollectionChanged;

            return new GridViewModel<EmployeeViewModel>(employees);
        }

        private GridViewModel<MachineViewModel> LoadMachines()
        {
            // TODO: load databases

            var machines = new ObservableCollection<MachineViewModel>
            {
                new MachineViewModel
                {
                    Code = "M001",
                    Name = "Machine A",
                },
                new MachineViewModel
                {
                    Code = "M002",
                    Name = "Machine B",
                }
            };
            machines.CollectionChanged += Machines_CollectionChanged;

            return new GridViewModel<MachineViewModel>(machines);
        }

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
    }
}
