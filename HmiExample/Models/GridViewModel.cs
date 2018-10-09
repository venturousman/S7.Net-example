using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HmiExample.Models
{
    public class GridViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<SelectableViewModel> _items3;
        private bool? _isAllItems3Selected;

        public GridViewModel()
        {
            _items3 = CreateData();
        }

        public bool? IsAllItems3Selected
        {
            get { return _isAllItems3Selected; }
            set
            {
                if (_isAllItems3Selected == value) return;

                _isAllItems3Selected = value;

                if (_isAllItems3Selected.HasValue)
                    SelectAll(_isAllItems3Selected.Value, Items3);

                OnPropertyChanged();
            }
        }

        private static void SelectAll(bool select, IEnumerable<SelectableViewModel> models)
        {
            foreach (var model in models)
            {
                model.IsSelected = select;
            }
        }

        private static ObservableCollection<SelectableViewModel> CreateData()
        {
            return new ObservableCollection<SelectableViewModel>
            {
                new SelectableViewModel
                {
                    Code = 'M',
                    Name = "Material Design",
                    Description = "Material Design in XAML Toolkit"
                },
                new SelectableViewModel
                {
                    Code = 'D',
                    Name = "Dragablz",
                    Description = "Dragablz Tab Control",
                    Food = "Fries"
                },
                new SelectableViewModel
                {
                    Code = 'P',
                    Name = "Predator",
                    Description = "If it bleeds, we can kill it"
                }
            };
        }

        public ObservableCollection<SelectableViewModel> Items3 => _items3;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable<string> Foods
        {
            get
            {
                yield return "Burger";
                yield return "Fries";
                yield return "Shake";
                yield return "Lettuce";
            }
        }
    }
}
