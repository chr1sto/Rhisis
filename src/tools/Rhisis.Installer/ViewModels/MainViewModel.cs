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
            this.ConfigureCommand = new Command(this.OnConfigure);
            this.StartInstallCommand = new Command(this.OnStartInstall);
            this.CancelInstallCommand = new Command(this.OnCancelInstall);
            this.ChangeLanguageCommand = new Command(this.OnChangeLanguage);
            this.SetCurrentLanguage();
        }

        protected void OnConfigure(object parameter)
        {
            if (!(parameter is ConfigurationType configurationType))
                return;

            ViewModelBase selectedOptionViewModel = null;

            switch (configurationType)
            {
                case ConfigurationType.Database:
                    selectedOptionViewModel = new DatabaseConfigurationViewModel(this._databaseConfiguration);
                    break;
                case ConfigurationType.Login:
                    selectedOptionViewModel = new LoginConfigurationViewModel();
                    break;
                case ConfigurationType.Cluster:
                    selectedOptionViewModel = new ClusterConfigurationViewModel();
                    break;
                case ConfigurationType.World:
                    selectedOptionViewModel = new WorldConfigurationViewModel();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(parameter), parameter, null);
            }

            selectedOptionViewModel?.ShowDialog();
        }

        protected void OnStartInstall()
        {
            // TODO: start writing configuration
        }

        protected void OnCancelInstall()
        {
            // TODO: exit program
        }

        protected void OnChangeLanguage(object parameter)
        {
            App.Instance.ChangeLanguage(parameter.ToString());
            this.SetCurrentLanguage();
        }

        protected T LoadConfiguration<T>(string path) where T : class, new()
        {
            string fullPath = Path.Combine(Environment.CurrentDirectory, path);

            return File.Exists(fullPath) ? ConfigurationHelper.Load<T>(fullPath) : new T();
        }

        protected void SetCurrentLanguage()
        {
            if (Thread.CurrentThread.CurrentUICulture.ToString() == "fr")
                this.CurrentLanguage = "/Rhisis.Installer;component/Resources/Images/french.png";
            else
                this.CurrentLanguage = "/Rhisis.Installer;component/Resources/Images/english.png";
        }
    }
}
