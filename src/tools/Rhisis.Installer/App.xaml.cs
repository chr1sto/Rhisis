using System.Windows;
using Rhisis.Installer.ViewModels;
using Rhisis.Installer.Views;
using Rhisis.Tools.Core;

namespace Rhisis.Installer
{
    public partial class App : Application
    {
        public App()
        {
            ViewFactory.Register<LoadingView, LoadingViewModel>();
        }
    }
}
