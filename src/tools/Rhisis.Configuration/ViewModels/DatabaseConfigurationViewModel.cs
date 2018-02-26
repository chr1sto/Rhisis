using Rhisis.Tools.Core.MVVM;
using Rhisis.Core.Helpers;
using Rhisis.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace Rhisis.Tools.ViewModels
{
    public sealed class DatabaseConfigurationViewModel : ViewModelBase
    {
        private static readonly string DatabaseConfigFile = "config/database.json";
        private static readonly string DatabaseConfigFileFullPath = Path.Combine(App.Instance.RhisisFolder, DatabaseConfigFile);

        /// <summary>
        /// Gets or sets the database configuration.
        /// </summary>
        public DatabaseConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets the list of available database providers.
        /// </summary>
        public List<DatabaseProvider> Providers { get; }

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
        /// Creates a new <see cref="DatabaseConfigurationViewModel"/> instance.
        /// </summary>
        public DatabaseConfigurationViewModel()
            : base()
        {
            this.Providers = Enum.GetValues(typeof(DatabaseProvider)).Cast<DatabaseProvider>().ToList();
            this.Configuration = ConfigurationHelper.Load<DatabaseConfiguration>(DatabaseConfigFileFullPath, true);
            this.OkCommand = new Command(this.OnOk);
            this.CancelCommand = new Command(this.OnCancel);
            this.TestCommand = new Command(this.OnTest);
        }

        /// <summary>
        /// Save the database configuration to Rhisis working folder.
        /// </summary>
        /// <param name="param"></param>
        private void OnOk(object param)
        {
            try
            {
                ConfigurationHelper.Save(DatabaseConfigFileFullPath, this.Configuration);
                DatabaseService.Configure(this.Configuration);
                this.CreateDatabaseIfDontExists();
            }
            catch (Exception e)
            {
                this.DialogService.ShowError("Unhandled error", $"An error occured while save the database configuration. {e.Message}");
            }
            finally
            {
                DatabaseService.UnloadConfiguration();
                this.Close();
            }
        }

        /// <summary>
        /// Aborts the changes and closes the current window.
        /// </summary>
        /// <param name="param"></param>
        private void OnCancel(object param)
        {
            this.Close();
        }

        /// <summary>
        /// Tests the database connection.
        /// </summary>
        /// <param name="param"></param>
        private void OnTest(object param)
        {
            try
            {
                DatabaseService.Configure(this.Configuration);
                var context = DatabaseService.GetContext();
                context.OpenConnection();
                context.Dispose();
                this.DialogService.ShowInformation("Connection success", "The connection has succeeded");
            }
            catch (Exception e)
            {
                this.DialogService.ShowError("Connection error", $"Cannot connect to database. {e.Message}");
                this.CreateDatabaseIfDontExists();
            }
            finally
            {
                DatabaseService.UnloadConfiguration();
            }
        }

        /// <summary>
        /// Ask the user if he wants to create the database if it doesn't exists.
        /// </summary>
        private void CreateDatabaseIfDontExists()
        {
            using (var context = DatabaseService.GetContext())
            {
                if (!context.DatabaseExists())
                {
                    bool result = this.DialogService.ShowQuestion("Database not found.", $"Cannot find database '{this.Configuration.Database}'. Do you want to create it.");

                    if (result)
                    {
                        context.CreateDatabase();
                        context.Migrate();
                    }
                }
            }
        }
    }
}
