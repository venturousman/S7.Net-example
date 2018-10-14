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
        private int _expectedquantity;  // output, san luong san xuat theo plan
        private int _actualquantity;  // output, san luong san xuat theo thuc te

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
            get { return _expectedquantity; }
            set
            {
                if (_expectedquantity == value) return;
                _expectedquantity = value;
                OnPropertyChanged();
            }
        }

        public int ActualQuantity
        {
            get { return _actualquantity; }
            set
            {
                if (_actualquantity == value) return;
                _actualquantity = value;
                OnPropertyChanged();
            }
        }
    }
}
