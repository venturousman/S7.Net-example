using LiveCharts;
using System;

namespace ProductionEquipmentControlSoftware.Models
{
    public class ChartViewModel : ObservableBase
    {
        private string _machine;

        private SeriesCollection _seriesCollection;
        private string[] _labels;
        private Func<double, string> _formatter;
        private int _width;
        private string _gridName;
        private string _chartName;

        private GridViewModel<PlanViewModel> _gridPlanVMs = new GridViewModel<PlanViewModel>();
        public GridViewModel<PlanViewModel> GridPlanVMs
        {
            get { return _gridPlanVMs; }
            //set
            //{
            //    if (_gridPlanVMs == value) return;
            //    _gridPlanVMs = value;
            //    OnPropertyChanged();
            //}
        }

        public string Machine
        {
            get { return _machine; }
            set
            {
                if (_machine == value) return;
                _machine = value;
                OnPropertyChanged();
            }
        }

        public SeriesCollection SeriesCollection
        {
            get { return _seriesCollection; }
            set
            {
                if (_seriesCollection == value) return;
                _seriesCollection = value;
                OnPropertyChanged();
            }
        }

        public string[] Labels
        {
            get { return _labels; }
            set
            {
                if (_labels == value) return;
                _labels = value;
                OnPropertyChanged();
            }
        }

        public Func<double, string> Formatter
        {
            get { return _formatter; }
            set
            {
                if (_formatter == value) return;
                _formatter = value;
                OnPropertyChanged();
            }
        }

        public int Width
        {
            get { return _width; }
            set
            {
                if (_width == value) return;
                _width = value;
                OnPropertyChanged();
            }
        }

        public string GridName
        {
            get { return _gridName; }
            set
            {
                if (_gridName == value) return;
                _gridName = value;
                OnPropertyChanged();
            }
        }

        public string ChartName
        {
            get { return _chartName; }
            set
            {
                if (_chartName == value) return;
                _chartName = value;
                OnPropertyChanged();
            }
        }
    }
}
