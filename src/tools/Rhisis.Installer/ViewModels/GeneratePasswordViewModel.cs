using Rhisis.Tools.Core.MVVM;
using System.Windows.Input;

namespace Rhisis.Installer.ViewModels
{
    public class GeneratePasswordViewModel : ViewModelBase
    {
        private string _salt;
        private string _password;
        private string _generatedPassword;

        public string Salt
        {
            get => this._salt;
            set => this.NotifyPropertyChanged(ref this._salt, value);
        }

        public string Password
        {
            get => this._password;
            set => this.NotifyPropertyChanged(ref this._password, value);
        }

        public string GeneratedPassword
        {
            get => this._generatedPassword;
            set => this.NotifyPropertyChanged(ref this._generatedPassword, value);
        }

        public ICommand OkCommand { get; }

        public ICommand CancelCommand { get; }

        public ICommand GeneratePasswordCommand { get; }

        public GeneratePasswordViewModel()
        {
            this.OkCommand = new Command(this.OnOk);
            this.CancelCommand = new Command(this.OnCancel);
            this.GeneratePasswordCommand = new Command(this.OnGeneratePassword);
        }

        private void OnOk()
        {
            this.Close();
        }

        private void OnCancel()
        {
            this.GeneratedPassword = string.Empty;
            this.Close();
        }

        private void OnGeneratePassword()
        {
            var password = string.Concat(this.Salt, this.Password);

            this.GeneratedPassword = "dfgsfd"; // TODO: generate MD5
        }
    }
}
