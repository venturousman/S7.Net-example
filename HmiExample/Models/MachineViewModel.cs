namespace HmiExample.Models
{
    /// <summary>
    /// MachineViewModel is a selectable view model
    /// </summary>
    public class MachineViewModel : SelectableViewModel
    {
        private string _name;
        private string _code;        

        public string Name
        {
            get { return _name; }
            set
            {
                // this.MutateVerbose(ref _name, value, RaisePropertyChanged());
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
        
        //private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        //{
        //    return args => PropertyChanged?.Invoke(this, args);
        //}        
    }
}
