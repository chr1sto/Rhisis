using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Rhisis.Manager.ViewModels
{
    public sealed class MenuItemViewModel
    {
        /// <summary>
        /// Gets or sets the menu header.
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Gets or sets the menu icon.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Gets the menu icon source.
        /// </summary>
        public string IconSource => $"pack://application:,,,/Rhisis.Manager;component/{this.Icon}";

        /// <summary>
        /// Gets or sets the menu command.
        /// </summary>
        public ICommand Command { get; set; }

        /// <summary>
        /// Gets the Sub menu items.
        /// </summary>
        public ObservableCollection<MenuItemViewModel> MenuItems { get; }

        /// <summary>
        /// Creates a new empty <see cref="MenuItemViewModel"/> instance.
        /// </summary>
        public MenuItemViewModel()
            : this(string.Empty, string.Empty, null)
        {
        }

        /// <summary>
        /// Creates a new <see cref="MenuItemViewModel"/> instance.
        /// </summary>
        /// <param name="header">Menu Header</param>
        /// <param name="icon">Menu icon</param>
        /// <param name="command">Menu command</param>
        public MenuItemViewModel(string header, string icon, ICommand command)
        {
            this.Header = header;
            this.Icon = icon;
            this.Command = command;
            this.MenuItems = new ObservableCollection<MenuItemViewModel>();
        }
    }
}
