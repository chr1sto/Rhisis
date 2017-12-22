using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Rhisis.Manager.Core.MVVM
{
    /// <summary>
    /// Provides a basic ViewModel abstraction.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// On property changed event.
        /// </summary>
        /// <param name="propName"></param>
        protected void OnPropertyChanged([CallerMemberName] string propName = null) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        /// <summary>
        /// Notify and sets the property value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storage">Property reference</param>
        /// <param name="value">New value</param>
        /// <param name="propertyName">Property name</param>
        /// <returns></returns>
        protected bool NotifyPropertyChanged<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value) == true)
                return false;

            storage = value;
            this.OnPropertyChanged(propertyName);

            return true;
        }
    }
}
