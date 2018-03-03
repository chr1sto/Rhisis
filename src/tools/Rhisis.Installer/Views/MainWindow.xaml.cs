using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using Rhisis.Installer.ViewModels;

namespace Rhisis.Installer.Views
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void OnChangeLanguage(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is MainViewModel vm && sender is MenuItem menu)
            {
                vm.ChangeLanguageCommand?.Execute(menu.Tag);
            }
        }
    }
}
