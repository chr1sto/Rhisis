using System;
using System.Collections.Generic;
using System.Windows.Input;
using Rhisis.Database;
using Rhisis.Installer.Enums;
using Rhisis.Tools.Core;
using Rhisis.Tools.Core.MVVM;

namespace Rhisis.Installer.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IDictionary<ConfigurationType, Type> _configurationViewModelsTypes;

        public ICommand ConfigureCommand { get; }

        public ICommand StartInstallCommand { get; }

        public ICommand CancelInstallCommand { get; }

        public MainViewModel()
        {
            this._configurationViewModelsTypes = new Dictionary<ConfigurationType, Type>
            {
                { ConfigurationType.Database, typeof(DatabaseConfigurationViewModel) },
            };
            this.ConfigureCommand = new Command(o => this.OnConfigure((ConfigurationType)o));
            this.StartInstallCommand = new Command(this.OnStartInstall);
            this.CancelInstallCommand = new Command(this.OnCancelInstall);
        }

        private void OnConfigure(ConfigurationType parameter)
        {
            if (this._configurationViewModelsTypes.TryGetValue(parameter, out Type viewModelType))
            {
                if (Activator.CreateInstance(viewModelType) is ViewModelBase viewModel)
                {
                    viewModel.ShowDialog();
                }
            }
        }

        private void OnStartInstall(object parameter)
        {

        }

        private void OnCancelInstall(object parameter)
        {
            
        }
    }
}
