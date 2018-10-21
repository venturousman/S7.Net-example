﻿using HmiExample.Data;
using System.ComponentModel;
using System.Data.Entity;
using System.Windows;
using System.Windows.Data;

namespace HmiExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ApplicationDbContext applicationDbContext = new ApplicationDbContext();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void miAbout_Click(object sender, RoutedEventArgs e)
        {
            // create and show the About box when the user click Help > About menu item.
            About aboutBox = new About(this);
            aboutBox.ShowDialog();
        }

        private void miMonitoring_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Monitoring());
        }

        private void miReports_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Reports());
        }

        private void miSettings_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Settings());
        }

        //private void miExit_Click(object sender, RoutedEventArgs e)
        //{
        //    Application.Current.Shutdown();
        //}

        protected override void OnClosing(CancelEventArgs e)
        {
            // save user settings
            // Properties.Settings.Default.Save();

            // base.OnClosing(e);
            // show the message box here and collect the result
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to exit?", "Exit Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                // if you want to stop it, set e.Cancel = true
                e.Cancel = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //CollectionViewSource machineViewSource = ((CollectionViewSource)(this.FindResource("machineViewSource")));
            //CollectionViewSource productViewSource = ((CollectionViewSource)(this.FindResource("productViewSource")));
            //CollectionViewSource employeeViewSource = ((CollectionViewSource)(this.FindResource("employeeViewSource")));
            //CollectionViewSource planViewSource = ((CollectionViewSource)(this.FindResource("planViewSource")));

            // Load is an extension method on IQueryable, defined in the System.Data.Entity namespace.
            // This method enumerates the results of the query, similar to ToList but without creating a list.
            // When used with Linq to Entities this method creates entity objects and adds them to the context.
            applicationDbContext.Machines.Load();
            applicationDbContext.Products.Load();
            applicationDbContext.Employees.Load();
            applicationDbContext.Plans.Load();

            // After the data is loaded call the DbSet<T>.Local property to use the DbSet<T> as a binding source.
            //machineViewSource.Source = applicationDbContext.Machines.Local;
            //productViewSource.Source = applicationDbContext.Products.Local;
            //employeeViewSource.Source = applicationDbContext.Employees.Local;
            //planViewSource.Source = applicationDbContext.Plans.Local;

            // set default
            mainFrame.Navigate(new Monitoring());
        }
    }
}
