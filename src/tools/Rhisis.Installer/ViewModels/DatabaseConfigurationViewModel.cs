using Rhisis.Core.Helpers;
using Rhisis.Database;
using Rhisis.Tools.Core.MVVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace Rhisis.Installer.ViewModels
{
    public class DatabaseConfigurationViewModel : ViewModelBase
    {
        private const string DatabaseConfigFile = "config/database.json";
        private static readonly string DatabaseConfigFileFullPath = Path.Combine(Environment.CurrentDirectory, DatabaseConfigFile);

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
        {
            this.OkCommand = new Command(this.OnOk);
            this.CancelCommand = new Command(this.OnCancel);
            this.TestCommand = new Command(this.OnTest);
            this.Providers = Enum.GetValues(typeof(DatabaseProvider)).Cast<DatabaseProvider>().ToList();
            this.Configuration = File.Exists(DatabaseConfigFileFullPath)
                ? ConfigurationHelper.Load<DatabaseConfiguration>(DatabaseConfigFileFullPath)
                : new DatabaseConfiguration();
        }

        private void OnOk(object param)
        {

        }

        private void OnCancel(object param)
        {
            
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
