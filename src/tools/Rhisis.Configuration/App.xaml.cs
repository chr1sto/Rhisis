using Rhisis.Configuration.Core;
using Rhisis.Configuration.ViewModels;
using Rhisis.Configuration.Views;
using System.Windows;

namespace Rhisis.Configuration
{
    public partial class App : Application
    {
        private static App _instance;

        /// <summary>
        /// Gets the current <see cref="App"/> instance.
        /// </summary>
        public static App Instance => _instance;

        /// <summary>
        /// Gets or sets the Rhisis working folder.
        /// </summary>
        public string RhisisFolder { get; set; }

        /// <summary>
        /// Creates a new <see cref="App"/> instance.
        /// </summary>
        public App()
        {
            _instance = this;
            ViewFactory.Register<MainWindow, MainViewModel>();
            ViewFactory.Register<DatabaseConfigurationWindow, DatabaseConfigurationViewModel>();
        }
    }
}
