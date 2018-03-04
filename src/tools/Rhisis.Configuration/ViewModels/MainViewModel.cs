using Rhisis.Tools.Core.MVVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;

namespace Rhisis.Tools.ViewModels
{
    public sealed class MainViewModel : ViewModelBase
    {
        private readonly IDictionary<string, Type> _configurationViewModelsTypes;
        private string _folder;
        private bool _isFolderValid;

        /// <summary>
        /// Gets or sets the Rhisis working folder.
        /// </summary>
        public string Folder
        {
            get { return this._folder; }
            set { this.NotifyPropertyChanged(ref this._folder, value); }
        }

        /// <summary>
        /// Gets or sets the state that indicates if the working folder is correct or not.
        /// </summary>
        public bool IsFolderValid
        {
            get { return this._isFolderValid; }
            set { this.NotifyPropertyChanged(ref this._isFolderValid, value); }
        }

        /// <summary>
        /// Gets the command to pick a folder.
        /// </summary>
        public ICommand SelectFolderCommand { get; }

        /// <summary>
        /// Gets the command to configure a configuration section.
        /// </summary>
        public ICommand ConfigureCommand { get; }

        /// <summary>
        /// Creates a new <see cref="MainViewModel"/> instance.
        /// </summary>
        public MainViewModel()
            : base()
        {
            this.IsFolderValid = true;
            this.SelectFolderCommand = new Command(this.OnSelectFolder);
            this.ConfigureCommand = new Command(this.OnConfigure);
            this._configurationViewModelsTypes = new Dictionary<string, Type>
            {
                { "database", typeof(DatabaseConfigurationViewModel) },
                { "login", typeof(LoginConfigurationViewModel) },
                { "cluster", typeof(ClusterConfigurationViewModel) },
                { "world", typeof(WorldConfigurationViewModel) },
            };
        }

        /// <summary>
        /// Select the Rhisis working folder.
        /// </summary>
        /// <param name="param"></param>
        private void OnSelectFolder(object param)
        {
            this.Folder = this.DialogService.OpenFolderDialog();

            if (string.IsNullOrEmpty(this.Folder))
            {
                this.IsFolderValid = false;
                this.DialogService.ShowError("Error", "The selected folder is not a Rhisis folder.");
                return;
            }

            string loginFile = Path.Combine(this.Folder, "login.bat");
            string clusterFile = Path.Combine(this.Folder, "cluster.bat");
            string worldFile = Path.Combine(this.Folder, "world.bat");

            if (File.Exists(loginFile) && File.Exists(clusterFile) && File.Exists(worldFile))
            {
                this.IsFolderValid = true;
                App.Instance.RhisisFolder = this.Folder;
            }
            else
            {
                this.IsFolderValid = false;
                this.DialogService.ShowError("Error", "The selected folder is not a Rhisis folder.");
            }
        }

        /// <summary>
        /// Configure the select section.
        /// </summary>
        /// <param name="param"></param>
        private void OnConfigure(object param)
        {
            string type = param.ToString();

            if (this._configurationViewModelsTypes.ContainsKey(type))
            {
                var viewModel = Activator.CreateInstance(this._configurationViewModelsTypes[type]) as ViewModelBase;

                viewModel.ShowDialog();
            }
        }
    }
}
