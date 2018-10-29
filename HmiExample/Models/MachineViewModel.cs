using System;

namespace HmiExample.Models
{
    /// <summary>
    /// MachineViewModel is a selectable view model
    /// </summary>
    public class MachineViewModel : SelectableViewModel
    {
        private Guid _id;
        private string _name;
        private string _code;
        private int _tagIndex;
        private int _counts;

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

        public int TagIndex
        {
            get { return _tagIndex; }
            set
            {
                if (_tagIndex == value) return;
                _tagIndex = value;
                OnPropertyChanged();
            }
        }

        public int Counts
        {
            get { return _counts; }
            set
            {
                if (_counts == value) return;
                _counts = value;
                OnPropertyChanged();
            }
        }

        #region Constructors
        public MachineViewModel()
        {

        }

        public MachineViewModel(Machine machine)
        {
            _id = machine.Id;
            _name = machine.Name;
            _code = machine.Code;
            _tagIndex = machine.TagIndex;
            _counts = machine.Counts;
        }
        #endregion
    }
}
