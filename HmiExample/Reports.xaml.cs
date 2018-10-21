using HmiExample.Helpers;
using HmiExample.Models;
using LiveCharts;
using LiveCharts.Wpf;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HmiExample
{
    /// <summary>
    /// Interaction logic for Reports.xaml
    /// </summary>
    public partial class Reports : Page
    {
        private readonly ObservableCollection<ChartViewModel> _chartViewModels = new ObservableCollection<ChartViewModel>();
        public ObservableCollection<ChartViewModel> ChartViewModels
        {
            get
            {
                return _chartViewModels;
            }
        }

        public Reports()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        private void btnExport_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // sample 1
            // ChartHelpers.SaveToPng(chartProductivity, "chart.png");

            // https://tedgustaf.com/blog/2012/create-excel-20072010-spreadsheets-with-c-and-epplus/
            // sample 2
            var currentDate = DateTime.Now;

            // Set the file name and get the output directory
            var fileName = "Report-" + currentDate.ToString("yyyy-MM-dd--hh-mm-ss") + ".xlsx";
            var baseDirectory = @"C:\" + Constants.ApplicationName + @"\reports\";
            var filePath = Path.Combine(baseDirectory, fileName);

            try
            {
                if (!Directory.Exists(baseDirectory))
                {
                    Directory.CreateDirectory(baseDirectory);
                }

                // Create the file using the FileInfo object
                var file = new FileInfo(filePath);

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage(file))
                {
                    // add a new worksheet to the empty workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Productivity list - " + currentDate.ToShortDateString());

                    // --------- Data and styling goes here -------------- //
                    //var imageChart = ChartHelpers.ChartToImage(chartProductivity);
                    //var picture = worksheet.Drawings.AddPicture("chart", imageChart);



                    //picture.SetPosition(rowIndex, 0, colIndex, 0);
                    //picture.SetSize(Height, Width);

                    // var img = System.Drawing.Image.FromFile(@"D:\pics\success.jpg"); // testing ok
                    // var pic = worksheet.Drawings.AddPicture("Sample", img);

                    // end
                    package.Save();
                }

                MessageBox.Show("Successfully exported report " + fileName, Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRunReport_Click(object sender, RoutedEventArgs e)
        {
            if (dtFrom.SelectedDate.HasValue && dtTo.SelectedDate.HasValue)
            {
                // get date range
                DateTime fromDate = dtFrom.SelectedDate.Value;
                DateTime toDate = dtTo.SelectedDate.Value;

                var mainWindow = (MainWindow)Application.Current.MainWindow;
                if (mainWindow.applicationDbContext.Plans.Local != null)
                {
                    // build labels
                    var labels = new List<string>();
                    var dates = new List<DateTime>();

                    for (var dt = fromDate; dt <= toDate; dt = dt.AddDays(1))
                    {
                        dates.Add(dt);
                        labels.Add(dt.ToShortDateString());
                    }

                    // get data
                    var lstPlans = mainWindow.applicationDbContext.Plans.Local
                        .Where(x => x.CreatedOn.HasValue && x.CreatedOn.Value >= fromDate && x.CreatedOn.Value <= toDate).ToList();

                    var groupMachines = lstPlans.GroupBy(x => x.MachineId).ToList();

                    // build series
                    _chartViewModels.Clear();
                    foreach (var plan in lstPlans)
                    {
                        var chartVM = new ChartViewModel
                        {
                            Machine = plan.Machine.Name,
                            SeriesCollection = new SeriesCollection
                            {
                                new ColumnSeries
                                {
                                    Title = plan.Machine.Name,
                                    Values = new ChartValues<double> { 10, 50, 39, 50 }
                                }
                            },
                            Labels = new[] { "Maria", "Susan", "Charles", "Frida" },
                            Formatter = value => value.ToString("N")
                        };
                        _chartViewModels.Add(chartVM);
                    }

                    // group by machine
                }
            }

        }

        private void datePickerSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            btnRunReport.IsEnabled = false;
            btnExport.IsEnabled = false;

            if (dtFrom.SelectedDate.HasValue && dtTo.SelectedDate.HasValue)
            {
                // get date range
                DateTime fromDate = dtFrom.SelectedDate.Value;
                DateTime toDate = dtTo.SelectedDate.Value;

                if (fromDate > toDate)
                {
                    MessageBox.Show("From Date must be less than To Date", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Warning);
                    btnRunReport.IsEnabled = false;
                    btnExport.IsEnabled = false;
                }
                else
                {
                    btnRunReport.IsEnabled = true;
                    btnExport.IsEnabled = true;
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dtFrom.DisplayDateStart = Constants.DefaultStartDate;
            dtFrom.SelectedDate = DateTime.Now;
            dtTo.DisplayDateStart = Constants.DefaultStartDate;
            dtTo.SelectedDate = DateTime.Now;
        }

    }
}
