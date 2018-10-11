using HmiExample.Helpers;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HmiExample.Models
{
    public class SettingsViewModel : ObservableBase
    {
        public ICommand AddMachineCommand => new CommandsImplementation(ExecuteAddMachineDialog);

        public GridViewModel<MachineViewModel> Machines => LoadMachines();

        public SettingsViewModel()
        {
        }

        private GridViewModel<MachineViewModel> LoadMachines()
        {
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

            return new GridViewModel<MachineViewModel>(machines);
        }

        private async void ExecuteAddMachineDialog(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new AddMachineDialog
            {
                DataContext = new MachineViewModel()
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog");//, OpenedEventHandler, ClosingEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }

        //private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        //{
        //    Console.WriteLine("You can intercept the closing event, and cancel here.");
        //}

        //private void OpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        //{
        //    Console.WriteLine("You could intercept the open and affect the dialog using eventArgs.Session.");
        //}
    }
}
