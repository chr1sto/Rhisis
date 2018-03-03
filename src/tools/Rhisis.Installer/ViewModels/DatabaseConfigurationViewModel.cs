using Rhisis.Database;
using Rhisis.Tools.Core.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Rhisis.Installer.ViewModels
{
    public class DatabaseConfigurationViewModel : ViewModelBase
    {
        private readonly DatabaseConfiguration _databaseConfigurationCopy;

        /// <summary>
        /// Gets the Ok command.
        /// </summary>
        public ICommand OkCommand { get; }

        /// <summary>
        /// Gets the cancel command.
        /// </summary>
        public ICommand CancelCommand { get; }

        /// <summary>
        /// Gets the test command.
        /// </summary>
        public ICommand TestCommand { get; }

        /// <summary>
        /// Gets the database configuration.
        /// </summary>
        public DatabaseConfiguration Configuration { get; }

        /// <summary>
        /// Gets list of providers.
        /// </summary>
        public IEnumerable<DatabaseProvider> Providers { get; }

        /// <summary>
        /// Creates a new <see cref="DatabaseConfigurationViewModel"/> instance.
        /// </summary>
        public DatabaseConfigurationViewModel()
            : this(new DatabaseConfiguration())
        {
        }

        /// <summary>
        /// Creates a new <see cref="DatabaseConfigurationViewModel"/> instance.
        /// </summary>
        public DatabaseConfigurationViewModel(DatabaseConfiguration configuration)
        {
            this.OkCommand = new Command(this.OnOk);
            this.CancelCommand = new Command(this.OnCancel);
            this.TestCommand = new Command(this.OnTest);
            this.Providers = Enum.GetValues(typeof(DatabaseProvider)).Cast<DatabaseProvider>();
            this.Configuration = configuration;
            this._databaseConfigurationCopy = new DatabaseConfiguration
            {
                Database = this.Configuration.Database,
                Host = this.Configuration.Host,
                Password = this.Configuration.Password,
                Port = this.Configuration.Port,
                Provider = this.Configuration.Provider,
                Username = this.Configuration.Username
            };
        }

        private void OnOk(object param) => this.Close();

        private void OnCancel(object param)
        {
            this.Configuration.Database = this._databaseConfigurationCopy.Database;
            this.Configuration.Host = this._databaseConfigurationCopy.Host;
            this.Configuration.Password = this._databaseConfigurationCopy.Password;
            this.Configuration.Port = this._databaseConfigurationCopy.Port;
            this.Configuration.Provider = this._databaseConfigurationCopy.Provider;
            this.Configuration.Username = this._databaseConfigurationCopy.Username;
            this.Close();
        }

        private void OnTest(object param)
        {
            DatabaseContext context = null;

            try
            {
                DatabaseService.Configure(this.Configuration);

                context = DatabaseService.GetContext();
                context.OpenConnection();

                this.DialogService.ShowInformation("Connection success", "The connection has succeeded");
            }
            catch (Exception e)
            {
                this.DialogService.ShowError("Connection error", $"Cannot connect to database. {e.Message}");
                //this.CreateDatabaseIfDontExists();
            }
            finally
            {
                context?.Dispose();
                DatabaseService.UnloadConfiguration();
            }
        }
    }
}
