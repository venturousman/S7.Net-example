using LiveCharts;
using System;

namespace HmiExample.Models
{
    public class Machine
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Code { get; set; }    // unique

        public string Supervisor { get; set; }  // TODO: User Supervisor

        // testing
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
    }
}
