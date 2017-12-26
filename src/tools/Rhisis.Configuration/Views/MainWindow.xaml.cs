using Rhisis.Configuration.Services;
using Rhisis.Configuration.ViewModels;
using System.Windows;

namespace Rhisis.Configuration.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = new MainViewModel(new DialogService());
        }
    }
}
