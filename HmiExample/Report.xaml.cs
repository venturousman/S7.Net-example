using HmiExample.Models;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace HmiExample
{
    /// <summary>
    /// Interaction logic for Report.xaml
    /// </summary>
    public partial class Report : Page
    {
        public Report()
        {
            InitializeComponent();

            //SeriesCollection = new SeriesCollection
            //{
            //    new ColumnSeries
            //    {
            //        Title = "2015",
            //        Values = new ChartValues<double> { 10, 50, 39, 50 }
            //    }
            //};

            ////adding series will update and animate the chart automatically
            //SeriesCollection.Add(new ColumnSeries
            //{
            //    Title = "2016",
            //    Values = new ChartValues<double> { 11, 56, 42 }
            //});

            ////also adding values updates and animates the chart automatically
            //SeriesCollection[1].Values.Add(48d);

            //Labels = new[] { "Maria", "Susan", "Charles", "Frida" };
            //Formatter = value => value.ToString("N");


            // add user controls 
            //for (int i = 0; i < 30; i++)
            //{
            //    UCBarChart chart = new UCBarChart();
            //    wrapPanelReport.Children.Add(chart);
            //}

            //this.chart1.SeriesCollection = this.SeriesCollection;
            //this.chart1.Labels = this.Labels;
            //this.chart1.Formatter = this.Formatter;

            //this.chart2.SeriesCollection = this.SeriesCollection;
            //this.chart2.Labels = this.Labels;
            //this.chart2.Formatter = this.Formatter;

            //this.chart3.SeriesCollection = this.SeriesCollection;
            //this.chart3.Labels = this.Labels;
            //this.chart3.Formatter = this.Formatter; 


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

            this.DataContext = this;
        }

        //public SeriesCollection SeriesCollection { get; set; }
        //public string[] Labels { get; set; }
        //public Func<double, string> Formatter { get; set; }
        public List<Machine> Machines { get; set; }
    }
}
