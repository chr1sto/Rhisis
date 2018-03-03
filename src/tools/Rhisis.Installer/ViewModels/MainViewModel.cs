using System;
using System.IO;
using System.Threading;
using System.Windows.Input;
using Rhisis.Core.Helpers;
using Rhisis.Database;
using Rhisis.Installer.Enums;
using Rhisis.Tools.Core.MVVM;

namespace Rhisis.Installer.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly DatabaseConfiguration _databaseConfiguration;

        private string _currentLanguage;

        public string CurrentLanguage
        {
            get => this._currentLanguage;
            set => this.NotifyPropertyChanged(ref this._currentLanguage, value);
        }

        public ICommand ConfigureCommand { get; }

        public ICommand StartInstallCommand { get; }

        public ICommand CancelInstallCommand { get; }

        public ICommand ChangeLanguageCommand { get; }

        public MainViewModel()
        {
            this._databaseConfiguration = this.LoadConfiguration<DatabaseConfiguration>("config/database.json");
            this.ConfigureCommand = new Command(o => this.OnConfigure((ConfigurationType)o));
            this.StartInstallCommand = new Command(this.OnStartInstall);
            this.CancelInstallCommand = new Command(this.OnCancelInstall);
            this.ChangeLanguageCommand = new Command(this.OnChangeLanguage);
            this.SetCurrentLanguage();
        }

        private void OnConfigure(ConfigurationType parameter)
        {
            ViewModelBase selectedOptionViewModel = null;

            switch (parameter)
            {
                case ConfigurationType.Database:
                    selectedOptionViewModel = new DatabaseConfigurationViewModel(this._databaseConfiguration);
                    break;
            }

            selectedOptionViewModel?.ShowDialog();
        }

        private void OnStartInstall(object parameter)
        {
            // TODO: start writing configuration
        }

        private void OnCancelInstall(object parameter)
        {
            // TODO: exit program
        }

        private void OnChangeLanguage(object parameter)
        {
            App.Instance.ChangeLanguage(parameter.ToString());
            this.SetCurrentLanguage();
        }

        private T LoadConfiguration<T>(string path) where T : class, new()
        {
            string fullPath = Path.Combine(Environment.CurrentDirectory, path);

            return File.Exists(fullPath) ? ConfigurationHelper.Load<T>(fullPath) : new T();
        }

        private void SetCurrentLanguage()
        {
            switch (Thread.CurrentThread.CurrentUICulture.ToString())
            {
                case "en":
                    this.CurrentLanguage = "/Rhisis.Installer;component/Resources/Images/english.png";
                    break;
                case "fr":
                    this.CurrentLanguage = "/Rhisis.Installer;component/Resources/Images/french.png";
                    break;
                default:
                    this.CurrentLanguage = "/Rhisis.Installer;component/Resources/Images/english.png";
                    break;
            }
        }
    }
}
