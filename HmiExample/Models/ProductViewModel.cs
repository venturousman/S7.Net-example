namespace HmiExample.Models
{
    public class ProductViewModel : SelectableViewModel
    {
        private string _name;
        private string _code;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Code
        {
            get { return _code; }
            set
            {
                if (_code == value) return;
                _code = value;
                OnPropertyChanged();
            }
        }
    }
}
