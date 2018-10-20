using System.Windows.Media;

namespace HmiExample.Models
{
    public class PlanViewModel : SelectableViewModel
    {
        //public Guid Id { get; set; }
        //public string Machine { get; set; }
        //public string Supervisor { get; set; }
        //public string Status { get; set; }

        private string _machine;
        private string _employee;
        private string _product;
        private int _expectedQuantity;  // output, san luong san xuat theo plan
        private int _actualQuantity;  // output, san luong san xuat theo thuc te
        private int _dataBlockNo;

        private bool _isConnected;
        //private bool _isStarted;
        private SolidColorBrush _ledColor;

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

        public string Employee
        {
            get { return _employee; }
            set
            {
                if (_employee == value) return;
                _employee = value;
                OnPropertyChanged();
            }
        }

        public string Product
        {
            get { return _product; }
            set
            {
                if (_product == value) return;
                _product = value;
                OnPropertyChanged();
            }
        }

        public int ExpectedQuantity
        {
            get { return _expectedQuantity; }
            set
            {
                if (_expectedQuantity == value) return;
                _expectedQuantity = value;
                OnPropertyChanged();
            }
        }

        public int ActualQuantity
        {
            get { return _actualQuantity; }
            set
            {
                if (_actualQuantity == value) return;
                _actualQuantity = value;
                OnPropertyChanged();
            }
        }

        public int DataBlockNo
        {
            get { return _dataBlockNo; }
            set
            {
                if (_dataBlockNo == value) return;
                _dataBlockNo = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush LedColor
        {
            get { return _ledColor; }
            set
            {
                if (_ledColor == value) return;
                _ledColor = value;
                OnPropertyChanged();
            }
        }

        //public bool IsEnabledStart
        //{
        //    get { return _isConnected == true && _isStarted == false; }
        //}

        //public bool IsEnabledStop
        //{
        //    get { return _isConnected == true && _isStarted == true; }
        //}

        //public bool IsStarted
        //{
        //    get { return _isStarted; }
        //    set
        //    {
        //        if (_isStarted == value) return;
        //        _isStarted = value;
        //        OnPropertyChanged();
        //    }
        //}

        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                if (_isConnected == value) return;
                _isConnected = value;
                OnPropertyChanged();
            }
        }
    }
}
