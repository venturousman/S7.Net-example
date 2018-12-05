using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProductionEquipmentControlSoftware.Models
{
    public abstract class ObservableBase : INotifyPropertyChanged
    {
        //public void Set<TValue>(ref TValue field, TValue newValue, [CallerMemberName] string propertyName = "")
        //{
        //    if (EqualityComparer<TValue>.Default.Equals(field, default(TValue)) || !field.Equals(newValue))
        //    {
        //        field = newValue;
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}

        //public event PropertyChangedEventHandler PropertyChanged;


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            //var handler = PropertyChanged;
            //if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
