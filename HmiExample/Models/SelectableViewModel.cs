namespace ProductionEquipmentControlSoftware.Models
{
    public class SelectableViewModel : ObservableBase
    {
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                OnPropertyChanged();
            }
        }
    }
}
