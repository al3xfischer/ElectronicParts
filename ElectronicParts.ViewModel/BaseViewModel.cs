using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ElectronicParts.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void FirePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void Set<TValue>(ref TValue item, TValue value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<TValue>.Default.Equals(item, value))
            {
                item = value;
                this.FirePropertyChanged(propertyName);
            }
        }
    }
}
