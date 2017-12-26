using Rhisis.Configuration.Core;
using Rhisis.Configuration.ViewModels;
using Rhisis.Configuration.Views;
using System.Windows;

namespace Rhisis.Configuration
{
    public partial class App : Application
    {
        private static App _instance;

        public static App Instance => _instance;

        public string RhisisFolder { get; set; }

        public App()
        {
            _instance = this;
            ViewFactory.Register<MainWindow, MainViewModel>();
            ViewFactory.Register<DatabaseConfigurationWindow, DatabaseConfigurationViewModel>();
        }
    }
}
