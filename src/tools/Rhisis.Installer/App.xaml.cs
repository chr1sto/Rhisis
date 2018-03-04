using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using Rhisis.Core.Helpers;
using Rhisis.Installer.Models;
using Rhisis.Installer.ViewModels;
using Rhisis.Installer.Views;
using Rhisis.Tools.Core;

namespace Rhisis.Installer
{
    public partial class App : Application
    {
        private readonly string _configurationFile = Path.Combine(Environment.CurrentDirectory, "config.json");

        public AppConfiguration Configuration { get; private set; }

        public static App Instance { get; private set; }

        public App()
        {
            Instance = this;
            ViewFactory.Register<DatabaseConfigurationWindow, DatabaseConfigurationViewModel>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            this.LoadLanguage();
        }

        protected void LoadLanguage()
        {
            if (!File.Exists(this._configurationFile))
            {
                var configuration = new AppConfiguration
                {
                    Culture = "en"
                };

                ConfigurationHelper.Save(this._configurationFile, configuration);
            }

            this.Configuration = ConfigurationHelper.Load<AppConfiguration>(this._configurationFile);
            this.ChangeLanguage(this.Configuration.Culture);
        }

        public void ChangeLanguage(string culture)
        {
            var dict = new ResourceDictionary();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

            if (culture == "fr")
                dict.Source = new Uri("Resources/Translations/App.fr.xaml", UriKind.Relative);
            else
                dict.Source = new Uri("Resources/Translations/App.xaml", UriKind.Relative);

            this.Resources.MergedDictionaries.Add(dict);
            this.Resources.MergedDictionaries.RemoveAt(this.Resources.MergedDictionaries.Count - 2);

            this.Configuration.Culture = culture;
            ConfigurationHelper.Save(this._configurationFile, this.Configuration);
        }
    }
}
