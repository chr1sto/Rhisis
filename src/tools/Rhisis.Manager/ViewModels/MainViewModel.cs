using Rhisis.Manager.Core.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhisis.Manager.ViewModels
{
    public sealed class MainViewModel : ViewModelBase
    {
        public ObservableCollection<MenuItemViewModel> MenuItems { get; }

        public MainViewModel()
        {
            this.MenuItems = new ObservableCollection<MenuItemViewModel>
            {
                new MenuItemViewModel("File", "/Resources/Images/Icons/document.png", null),
                new MenuItemViewModel("Tools", "/Resources/Images/Icons/settings.png", null),
                new MenuItemViewModel("About", "/Resources/Images/Icons/about.png", null)
            };
        }
    }
}
