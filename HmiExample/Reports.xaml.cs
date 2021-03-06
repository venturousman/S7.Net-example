﻿using LiveCharts;
using LiveCharts.Wpf;
using log4net;
using OfficeOpenXml;
using ProductionEquipmentControlSoftware.Data;
using ProductionEquipmentControlSoftware.Helpers;
using ProductionEquipmentControlSoftware.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace ProductionEquipmentControlSoftware
{
    /// <summary>
    /// Interaction logic for Reports.xaml
    /// </summary>
    public partial class Reports : Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
                var mainWindow = (MainWindow)Application.Current.MainWindow;

                if (!Directory.Exists(baseDirectory))
                {
                    Directory.CreateDirectory(baseDirectory);
                }

                // Create the file using the FileInfo object
                var file = new FileInfo(filePath);

                // find all charts and grids
                var grids = CommonHelpers.FindVisualChildren<DataGrid>(mainWindow).ToList();
                var charts = CommonHelpers.FindVisualChildren<CartesianChart>(mainWindow).ToList();

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage(file))
                {
                    // add a new worksheet to the empty workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Productivity - " + currentDate.ToShortDateString());

                    // --------- Data and styling goes here -------------- //
                    var rowIndex = 1;
                    var pixelTop = 0d;
                    var pixelLeft = 0;
                    var chartHeight = 300;
                    var rowHeight = 39;
                    var calculatedChartHeight = CommonHelpers.GetHeight(chartHeight);
                    var calculatedRowHeight = CommonHelpers.GetHeight(rowHeight);
                    foreach (var chartVM in ChartViewModels)
                    {
                        var chart = charts.Where(x => x.Tag != null && x.Tag.ToString() == chartVM.ChartName).FirstOrDefault();
                        if (chart != null)
                        {
                            worksheet.Row(rowIndex).Height = calculatedChartHeight;

                            var imageChart = ChartHelpers.ChartToImage(chart, chart.ActualWidth, chart.ActualHeight);
                            var picture = worksheet.Drawings.AddPicture(chartVM.ChartName, imageChart);

                            int intPixelTop = (int)pixelTop;
                            picture.SetPosition(intPixelTop, pixelLeft);
                            //picture.SetSize(Height, Width);

                            pixelTop += calculatedChartHeight;
                            rowIndex++;
                        }

                        var grid = grids.Where(x => x.Tag != null && x.Tag.ToString() == chartVM.GridName).FirstOrDefault();
                        if (grid != null)
                        {
                            for (int i = 0; i < grid.Columns.Count; i++)
                            {
                                var header = grid.Columns[i].Header != null ? grid.Columns[i].Header.ToString() : string.Empty;
                                worksheet.Cells[rowIndex, i + 1].Value = header;
                            }

                            //worksheet.Row(rowIndex).Height = calculatedRowHeight;
                            pixelTop += rowHeight;
                            rowIndex++;

                            for (int i = 0; i < grid.Items.Count; i++)
                            {
                                for (int j = 0; j < grid.Columns.Count; j++)
                                {
                                    //loop throught cell
                                    DataGridCell cell = CommonHelpers.GetCell(grid, i, j);
                                    if (cell != null)
                                    {
                                        if (cell.Content is TextBlock)
                                        {
                                            TextBlock tb = cell.Content as TextBlock;
                                            worksheet.Cells[rowIndex, j + 1].Value = tb.Text;
                                        }
                                    }
                                }
                                //worksheet.Row(rowIndex).Height = calculatedRowHeight;
                                pixelTop += rowHeight;
                                rowIndex++;
                            }
                        }
                    }

                    // var img = System.Drawing.Image.FromFile(@"D:\pics\success.jpg"); // testing ok
                    // var pic = worksheet.Drawings.AddPicture("Sample", img);

                    // end
                    package.Save();
                }

                MessageBox.Show("Successfully exported report " + fileName, Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                var msg = ex.GetAllExceptionInfo();
                log.Error(msg, ex);
                MessageBox.Show("Cannot export report", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRunReport_Click(object sender, RoutedEventArgs e)
        {
            using (var applicationDbContext = new ApplicationDbContext())
            {
                if (dtFrom.SelectedDate.HasValue && dtTo.SelectedDate.HasValue)
                {
                    // get date range
                    DateTime fromDate = dtFrom.SelectedDate.Value;
                    DateTime toDate = dtTo.SelectedDate.Value;

                    fromDate = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, 0, 0, 0);
                    toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);

                    // build labels
                    var dictActualQuantity = new Dictionary<string, double>();
                    var dictExpectedQuantity = new Dictionary<string, double>();

                    for (var dt = fromDate; dt <= toDate; dt = dt.AddDays(1))
                    {
                        var key = dt.ToShortDateString();
                        dictActualQuantity[key] = 0;
                        dictExpectedQuantity[key] = 0;
                    }

                    // calculate width of chart
                    var calculatedWidth = dictExpectedQuantity.Count * 35 + 400; // px

                    // get data
                    var groups = applicationDbContext.Plans
                        .Where(x => !x.IsDeleted && x.CreatedOn.HasValue
                                && x.CreatedOn.Value >= fromDate && x.CreatedOn.Value <= toDate)// && x.IsProcessed == true)
                        .GroupBy(x => new { x.MachineId }) //.GroupBy(x => new { x.EmployeeId, x.MachineId })
                        .ToList();

                    _chartViewModels.Clear();
                    foreach (var group in groups)
                    {
                        var chartVM = new ChartViewModel
                        {
                            Labels = dictExpectedQuantity.Keys.ToArray(),
                            Formatter = value => value.ToString("N"),
                            Width = calculatedWidth
                        };

                        var chartName = string.Empty;
                        var gridName = string.Empty;
                        var machineName = string.Empty;

                        // grid info
                        var orderedGroup = group.OrderBy(x => x.CreatedOn);
                        foreach (var plan in orderedGroup)
                        {
                            var planVM = new PlanViewModel(plan);
                            chartVM.GridPlanVMs.Items.Add(planVM);

                            if (string.IsNullOrEmpty(machineName) && plan.Machine != null)
                            {
                                machineName = plan.Machine.Name;
                            }
                            if (string.IsNullOrEmpty(chartName))
                            {
                                chartName = "chart-" + plan.Id;
                            }
                            if (string.IsNullOrEmpty(gridName))
                            {
                                gridName = "grid-" + plan.Id;
                            }

                            // build series
                            if (plan.CreatedOn.HasValue)
                            {
                                var key = plan.CreatedOn.Value.ToShortDateString();
                                if (dictExpectedQuantity.ContainsKey(key))
                                {
                                    dictExpectedQuantity[key] += plan.ExpectedQuantity.HasValue ? plan.ExpectedQuantity.Value : 0;
                                }
                                if (dictActualQuantity.ContainsKey(key))
                                {
                                    dictActualQuantity[key] += plan.ActualQuantity.HasValue ? plan.ActualQuantity.Value : 0;
                                }
                            }
                        }

                        // update chartVM
                        chartVM.GridName = gridName;
                        chartVM.ChartName = chartName;
                        chartVM.Machine = machineName;
                        var seriesActualQuantity = new ChartValues<double>(dictActualQuantity.Values.ToArray());
                        var seriesExpectedQuantity = new ChartValues<double>(dictExpectedQuantity.Values.ToArray());
                        chartVM.SeriesCollection = new SeriesCollection
                            {
                                new ColumnSeries
                                {
                                    Title = "Actual Quantity",
                                    Values = seriesActualQuantity,
                                    //MaxColumnWidth = double.PositiveInfinity
                                },
                                new LineSeries
                                {
                                    Title = "Expected Quantity",
                                    Values = seriesExpectedQuantity
                                }
                            };

                        // add chartVM
                        _chartViewModels.Add(chartVM);
                    }
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
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = Constants.DefaultDateFormat;
            Thread.CurrentThread.CurrentCulture = ci;

            dtFrom.DisplayDateStart = Constants.DefaultStartDate;
            dtFrom.SelectedDate = DateTime.Now;

            dtTo.DisplayDateStart = Constants.DefaultStartDate;
            dtTo.SelectedDate = DateTime.Now;
        }
    }
}
