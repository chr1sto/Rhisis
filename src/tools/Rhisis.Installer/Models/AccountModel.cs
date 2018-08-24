using Rhisis.Core.Common;

namespace Rhisis.Installer.Models
{
    public class AccountModel
    {
        public string Username { get; set; }
        
        public string Password { get; set; }

        public string PasswordConfirmation { get; set; }

        public AuthorityType Type { get; set; }

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
            this.Type = AuthorityType.Player;
            this.IsValid = false;
        }
    }
}
