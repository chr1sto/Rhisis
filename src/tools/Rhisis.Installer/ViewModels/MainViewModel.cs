using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using Rhisis.Core.Helpers;
using Rhisis.Database;
using Rhisis.Installer.Enums;
using Rhisis.Tools.Core;
using Rhisis.Tools.Core.MVVM;

namespace Rhisis.Installer.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly DatabaseConfiguration _databaseConfiguration;

        public ICommand ConfigureCommand { get; }

        public ICommand StartInstallCommand { get; }

        public ICommand CancelInstallCommand { get; }

        public MainViewModel()
        {
            this._databaseConfiguration = this.LoadConfiguration<DatabaseConfiguration>("config/database.json");
            this.ConfigureCommand = new Command(o => this.OnConfigure((ConfigurationType)o));
            this.StartInstallCommand = new Command(this.OnStartInstall);
            this.CancelInstallCommand = new Command(this.OnCancelInstall);
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

        private T LoadConfiguration<T>(string path) where T : class, new()
        {
            string fullPath = Path.Combine(Environment.CurrentDirectory, path);

            return File.Exists(fullPath) ? ConfigurationHelper.Load<T>(fullPath) : new T();
        }
    }
}
