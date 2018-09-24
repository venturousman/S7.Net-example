using LiveCharts;
using System;

namespace HmiExample.Models
{
    public class Machine
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        // testing
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
    }
}
