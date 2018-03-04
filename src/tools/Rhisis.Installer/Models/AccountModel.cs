using Rhisis.Core.Common;

namespace Rhisis.Installer.Models
{
    public class AccountModel
    {
        public string Username { get; set; }
        
        public string Password { get; set; }

        public string PasswordConfirmation { get; set; }

        public AccountType Type { get; set; }

        public bool IsValid { get; set; }

        public AccountModel()
        {
            this.Reset();
        }

        public void Reset()
        {
            this.Username = string.Empty;
            this.Password = string.Empty;
            this.PasswordConfirmation = string.Empty;
            this.Type = AccountType.Player;
            this.IsValid = false;
        }
    }
}
