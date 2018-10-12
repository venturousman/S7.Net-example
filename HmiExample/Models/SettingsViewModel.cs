using HmiExample.Helpers;
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

        public GridViewModel<MachineViewModel> Machines { get; }

        public SettingsViewModel()
        {
            Machines = LoadMachines();
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

        private void Machines_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
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
            var result = await DialogHost.Show(view, "RootDialog", OpenedEventHandler, ClosingEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
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

        private void OpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            Console.WriteLine("You could intercept the open and affect the dialog using eventArgs.Session.");
        }
    }
}
