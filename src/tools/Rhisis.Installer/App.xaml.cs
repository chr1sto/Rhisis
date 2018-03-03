using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using Rhisis.Installer.ViewModels;
using Rhisis.Installer.Views;
using Rhisis.Tools.Core;

namespace Rhisis.Installer
{
    public partial class App : Application
    {
        public static App Instance { get; private set; }

        public App()
        {
            Instance = this;
            ViewFactory.Register<DatabaseConfigurationWindow, DatabaseConfigurationViewModel>();
        }

        public void ChangeLanguage(string culture)
        {
            var dict = new ResourceDictionary();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

            switch (culture)
            {
                case "en":
                    dict.Source = new Uri("Resources/Translations/App.xaml", UriKind.Relative);
                    break;
                case "fr":
                    dict.Source = new Uri("Resources/Translations/App.fr.xaml", UriKind.Relative);
                    break;
                default:
                    dict.Source = new Uri("Resources/Translations/App.xaml", UriKind.Relative);
                    break;
            }

            this.Resources.MergedDictionaries.Add(dict);
            this.Resources.MergedDictionaries.RemoveAt(this.Resources.MergedDictionaries.Count - 2);
        }
    }
}
