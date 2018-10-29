using System;

namespace HmiExample.Models
{
    public class ProductViewModel : SelectableViewModel
    {
        private Guid _id;
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

        public Guid Id
        {
            get { return _id; }
            set
            {
                if (_id == value) return;
                _id = value;
                OnPropertyChanged();
            }
        }

        #region Constructors
        public ProductViewModel()
        {

        }

        public ProductViewModel(Product product)
        {
            _id = product.Id;
            _name = product.Name;
            _code = product.Code;
        }
        #endregion
    }
}
