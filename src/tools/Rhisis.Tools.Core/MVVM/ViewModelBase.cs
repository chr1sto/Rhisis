using Rhisis.Tools.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Rhisis.Tools.Core.MVVM
{
    /// <summary>
    /// Provides a basic ViewModel abstraction.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private Window _currentWindow;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the dialog service.
        /// </summary>
        protected IDialogService DialogService { get; set; }

        /// <summary>
        /// Creates a new <see cref="ViewModelBase"/> instance.
        /// </summary>
        protected ViewModelBase()
        {
            this.DialogService = new DialogService();
        }

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
            if (Equals(storage, value))
                return false;

            storage = value;
            this.OnPropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Show this View model's interface.
        /// </summary>
        public void ShowDialog()
        {
            if (!(ViewFactory.CreateInstance(this.GetType()) is Window window))
            {
                this.DialogService.ShowError("Fatal error", "An error occured while creating the window.");
                return;
            }

            this._currentWindow = window;
            window.DataContext = this;
            window.ShowDialog();
        }

        /// <summary>
        /// Close the current View model's window.
        /// </summary>
        public void Close()
        {
            this._currentWindow.Close();
        }
    }
}
