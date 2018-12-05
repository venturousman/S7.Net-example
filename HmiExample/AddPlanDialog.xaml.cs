using ProductionEquipmentControlSoftware.Data;
using ProductionEquipmentControlSoftware.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProductionEquipmentControlSoftware
{
    /// <summary>
    /// Interaction logic for AddPlanDialog.xaml
    /// </summary>
    public partial class AddPlanDialog : UserControl
    {
        public AddPlanDialog()
        {
            InitializeComponent();
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void cbMachine_Loaded(object sender, RoutedEventArgs e)
        {
            using (var applicationDbContext = new ApplicationDbContext())
            {
                // load databases
                var lstMachines = applicationDbContext.Machines
                        .Where(x => !x.IsDeleted)
                        .Select(x => new MachineViewModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Code = x.Code,
                            Count = x.Count,
                            CumulativeCount = x.CumulativeCount,
                            TagIndex = x.TagIndex
                        })
                        .OrderBy(x => x.Name)
                        .ToList();

                // ... Get the ComboBox reference.
                var comboBox = sender as ComboBox;

                comboBox.Items.Clear();
                comboBox.ItemsSource = lstMachines;
                comboBox.DisplayMemberPath = "Name";
                if (comboBox.SelectedIndex < 0)
                {
                    if (DataContext is PlanViewModel)
                    {
                        var context = DataContext as PlanViewModel;
                        if (context.MachineId != Guid.Empty)
                        {
                            var foundMachine = lstMachines.Where(x => x.Id == context.MachineId).FirstOrDefault();
                            if (foundMachine != null)
                            {
                                var index = comboBox.Items.IndexOf(foundMachine);
                                comboBox.SelectedIndex = index;
                            }
                        }
                        else
                        {
                            comboBox.SelectedIndex = 0;
                        }
                    }
                }
            }
        }

        private void cbMachine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ... Get the ComboBox.
            var comboBox = sender as ComboBox;

            // ... Set SelectedItem as Window Title.
            if (comboBox.SelectedItem is MachineViewModel)
            {
                var selectedMachine = comboBox.SelectedItem as MachineViewModel;

                if (DataContext is PlanViewModel)
                {
                    var context = DataContext as PlanViewModel;
                    context.Machine = selectedMachine;
                    context.MachineId = selectedMachine.Id;
                }
            }
        }

        private void cbEmployee_Loaded(object sender, RoutedEventArgs e)
        {
            using (var applicationDbContext = new ApplicationDbContext())
            {
                // load databases
                var lstEmployees = applicationDbContext.Employees
                        .Where(x => !x.IsDeleted)
                        .Select(x => new EmployeeViewModel
                        {
                            Id = x.Id,
                            Code = x.Code,
                            DisplayName = x.DisplayName,
                            FirstName = x.FirstName,
                            MiddleName = x.MiddleName,
                            LastName = x.LastName,
                            PhoneNumber = x.PhoneNumber,
                            Photo = x.Photo,
                            PhotoContent = x.PhotoContent,
                            Email = x.Email
                        })
                        .OrderBy(x => x.DisplayName)
                        .ToList();

                var finalEmployees = new List<EmployeeViewModel> {
                    new EmployeeViewModel
                    {
                        Id = Guid.Empty,
                        DisplayName = "-- Please Select --"
                    }
                };
                finalEmployees.AddRange(lstEmployees);

                // ... Get the ComboBox reference.
                var comboBox = sender as ComboBox;

                comboBox.Items.Clear();
                comboBox.ItemsSource = finalEmployees;
                comboBox.DisplayMemberPath = "DisplayName";
                if (comboBox.SelectedIndex < 0)
                {
                    if (DataContext is PlanViewModel)
                    {
                        var context = DataContext as PlanViewModel;
                        if (context.EmployeeId != Guid.Empty)
                        {
                            var foundEmployee = finalEmployees.Where(x => x.Id == context.EmployeeId).FirstOrDefault();
                            if (foundEmployee != null)
                            {
                                var index = comboBox.Items.IndexOf(foundEmployee);
                                comboBox.SelectedIndex = index;
                            }
                        }
                        else
                        {
                            comboBox.SelectedIndex = 0;
                        }
                    }
                }
            }
        }

        private void cbEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ... Get the ComboBox.
            var comboBox = sender as ComboBox;

            // ... Set SelectedItem as Window Title.
            if (comboBox.SelectedItem is EmployeeViewModel)
            {
                var selectedEmployee = comboBox.SelectedItem as EmployeeViewModel;

                if (DataContext is PlanViewModel)
                {
                    var context = DataContext as PlanViewModel;
                    if (selectedEmployee.Id != Guid.Empty)
                    {
                        context.Employee = selectedEmployee;
                        context.EmployeeId = selectedEmployee.Id;
                    }
                    else
                    {
                        context.Employee = null;
                        context.EmployeeId = null;
                    }
                }
            }
        }

        private void cbProduct_Loaded(object sender, RoutedEventArgs e)
        {
            using (var applicationDbContext = new ApplicationDbContext())
            {
                // load databases
                var lstProducts = applicationDbContext.Products
                        .Where(x => !x.IsDeleted)
                        .Select(x => new ProductViewModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Code = x.Code,
                        })
                        .OrderBy(x => x.Name)
                        .ToList();

                // ... Get the ComboBox reference.
                var comboBox = sender as ComboBox;

                comboBox.Items.Clear();
                comboBox.ItemsSource = lstProducts;
                comboBox.DisplayMemberPath = "Name";
                if (comboBox.SelectedIndex < 0)
                {
                    if (DataContext is PlanViewModel)
                    {
                        var context = DataContext as PlanViewModel;
                        if (context.ProductId != Guid.Empty)
                        {
                            var foundProduct = lstProducts.Where(x => x.Id == context.ProductId).FirstOrDefault();
                            if (foundProduct != null)
                            {
                                var index = comboBox.Items.IndexOf(foundProduct);
                                comboBox.SelectedIndex = index;
                            }
                        }
                        else
                        {
                            comboBox.SelectedIndex = 0;
                        }
                    }
                }
            }
        }

        private void cbProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ... Get the ComboBox.
            var comboBox = sender as ComboBox;

            // ... Set SelectedItem as Window Title.
            if (comboBox.SelectedItem is ProductViewModel)
            {
                var selectedProduct = comboBox.SelectedItem as ProductViewModel;

                if (DataContext is PlanViewModel)
                {
                    var context = DataContext as PlanViewModel;
                    context.Product = selectedProduct;
                    context.ProductId = selectedProduct.Id;
                }
            }
        }
    }
}
