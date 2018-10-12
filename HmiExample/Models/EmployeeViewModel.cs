namespace HmiExample.Models
{
    /// <summary>
    /// EmployeeViewModel is a selectable view model
    /// </summary>
    public class EmployeeViewModel : SelectableViewModel
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
