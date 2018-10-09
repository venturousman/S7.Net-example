using HmiExample.Helpers;
using HmiExample.Models;
using LiveCharts;
using LiveCharts.Wpf;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
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
        public Reports()
        {
            InitializeComponent();

            // can be ColumnSeries or LineSeries
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Machine 1",
                    Values = new ChartValues<double> { 4, 6, 5, 2, 7, 6, 7, 3, 4, 6 },
                    Fill = Brushes.Transparent
                },
                new LineSeries
                {
                    Title = "Machine 2",
                    Values = new ChartValues<double> { 6, 7, 3, 4, 6, 2, 8, 4, 1, 6 },
                    Fill = Brushes.Transparent
                },
                new LineSeries
                {
                    Title = "Machine 3",
                    Values = new ChartValues<double> { 2, 8, 4, 1, 6, 4, 6, 5, 2, 7 },
                    Fill = Brushes.Transparent
                },
                new LineSeries
                {
                    Title = "Machine 4",
                    Values = new ChartValues<double> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                    Fill = Brushes.Transparent
                },
                new LineSeries
                {
                    Title = "Machine 5",
                    Values = new ChartValues<double> { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 },
                    Fill = Brushes.Transparent
                }
            };

            Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct" };
            //Formatter = value => value.ToString("C");
            Formatter = value => value.ToString();


            /*
            // sample 1
            Machines = new List<Machine>();
            for (int i = 0; i < 30; i++)
            {
                var machine = new Machine
                {
                    SeriesCollection = new SeriesCollection
                    {
                        new ColumnSeries
                        {
                            Title = "2015",
                            Values = new ChartValues<double> { 10, 50, 39, 50 }
                        },
                        new ColumnSeries
                        {
                            Title = "2016",
                            Values = new ChartValues<double> { 11, 56, 42, 48 }
                        }
                    },
                    Labels = new[] { "Maria", "Susan", "Charles", "Frida" },
                    Formatter = value => value.ToString("N"),
                    Id = Guid.NewGuid(),
                    Name = "machine_" + i
                };
                Machines.Add(machine);
            }
            */

            this.DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

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
                    var imageChart = ChartHelpers.ChartToImage(chartProductivity);
                    var picture = worksheet.Drawings.AddPicture("chart", imageChart);

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

        //public List<Machine> Machines { get; set; }
    }
}
