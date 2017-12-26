using Rhisis.Configuration.Core;
using Rhisis.Configuration.Core.MVVM;
using Rhisis.Configuration.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Rhisis.Configuration.ViewModels
{
    public sealed class MainViewModel : ViewModelBase
    {
        private string _folder;
        private bool _isFolderValid;

        public string Folder
        {
            get { return this._folder; }
            set { this.NotifyPropertyChanged(ref this._folder, value); }
        }

        public bool IsFolderValid
        {
            get { return this._isFolderValid; }
            set { this.NotifyPropertyChanged(ref this._isFolderValid, value); }
        }

        public ICommand SelectFolderCommand { get; }

        public ICommand ConfigureCommand { get; }

        public MainViewModel(IDialogService dialogService)
        {
            this.DialogService = dialogService;
            this.IsFolderValid = true;
            this.SelectFolderCommand = new Command(this.OnSelectFolder);
            this.ConfigureCommand = new Command(this.OnConfigure);
        }

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

        private void OnConfigure(object param)
        {
            var viewModel = new DatabaseConfigurationViewModel();

            viewModel.ShowDialog();
        }
    }
}
