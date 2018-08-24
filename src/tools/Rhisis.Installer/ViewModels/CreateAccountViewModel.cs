using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Rhisis.Core.Common;
using Rhisis.Installer.Models;
using Rhisis.Tools.Core.MVVM;

namespace Rhisis.Installer.ViewModels
{
    public class CreateAccountViewModel : ViewModelBase
    {
        public ICommand OkCommand { get; }

        public ICommand CancelCommand { get; }

        public ICommand CreatePasswordCommand { get; }

        public IEnumerable<AuthorityType> AccountTypes { get; }

        public AccountModel Account { get; }

        public CreateAccountViewModel()
        {
            this.OkCommand = new Command(this.OnOk);
            this.CancelCommand = new Command(this.OnCancel);
            this.CreatePasswordCommand = new Command(this.OnCreatePassword);
            this.AccountTypes = Enum.GetValues(typeof(AuthorityType)).Cast<AuthorityType>();
            this.Account = new AccountModel();
        }

        protected void OnOk()
        {
            if (this.Account.Password != this.Account.PasswordConfirmation)
            {
                this.DialogService.ShowError(
                    App.Instance.GetTranslation("CreateAccountErrorTitle"), 
                    App.Instance.GetTranslation("CreateAccountErrorPassNotMatching"));
                return;
            }

            this.Account.IsValid = true;
            this.Close();
        }

        protected void OnCancel()
        {
            this.Account.Reset();
            this.Close();
        }

        private void OnCreatePassword()
        {
            var viewModel = new GeneratePasswordViewModel();

            viewModel.ShowDialog();

            if (!string.IsNullOrEmpty(viewModel.GeneratedPassword))
            {
                this.Account.Password = viewModel.GeneratedPassword;
                this.Account.PasswordConfirmation = viewModel.GeneratedPassword;
                this.OnPropertyChanged(nameof(this.Account));
            }
        }
    }
}
