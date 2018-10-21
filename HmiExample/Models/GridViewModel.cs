using System.Collections.ObjectModel;

namespace HmiExample.Models
{
    public class GridViewModel<T> : ObservableBase where T : SelectableViewModel
    {
        private readonly ObservableCollection<T> _items;
        private bool? _isAllItemsSelected = false;

        //public ObservableCollection<T> Items => _items;
        //public ICommand AddItemCommand => new CommandsImplementation(ExecuteAddItem);

        public GridViewModel()
        {
            this._items = new ObservableCollection<T>();
        }

        public GridViewModel(ObservableCollection<T> _items)
        {
            this._items = _items;
        }

        public ObservableCollection<T> Items
        {
            get { return _items; }
        }

        public bool? IsAllItemsSelected
        {
            get { return _isAllItemsSelected; }
            set
            {
                if (_isAllItemsSelected == value) return;

                _isAllItemsSelected = value;

                if (_isAllItemsSelected.HasValue)
                    SelectAll(_isAllItemsSelected.Value, Items);

                OnPropertyChanged();
            }
        }

        private void SelectAll(bool select, ObservableCollection<T> models)
        {
            foreach (var model in models)
            {
                model.IsSelected = select;
            }
        }

        //private void ExecuteAddItem(object o)
        //{
        //    var newItem = o as T;
        //    if (newItem != null)
        //    {
        //        Items.Add(newItem);
        //    }
        //}
    }
}
