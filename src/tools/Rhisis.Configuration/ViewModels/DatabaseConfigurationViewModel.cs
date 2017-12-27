using Rhisis.Configuration.Core.MVVM;
using Rhisis.Core.Helpers;
using Rhisis.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace Rhisis.Configuration.ViewModels
{
    public sealed class DatabaseConfigurationViewModel : ViewModelBase
    {
        private static readonly string DatabaseConfigFile = "config/database.json";
        private static readonly string DatabaseConfigFileFullPath = Path.Combine(App.Instance.RhisisFolder, DatabaseConfigFile);

        public DatabaseConfiguration Configuration { get; set; }

        public List<DatabaseProvider> Providers { get; }

        public ICommand OkCommand { get; }

        public ICommand CancelCommand { get; }

        public ICommand TestCommand { get; }

        public DatabaseConfigurationViewModel()
        {
            this.Providers = Enum.GetValues(typeof(DatabaseProvider)).Cast<DatabaseProvider>().ToList();
            this.Configuration = ConfigurationHelper.Load<DatabaseConfiguration>(DatabaseConfigFileFullPath, true);
            this.OkCommand = new Command(this.OnOk);
            this.CancelCommand = new Command(this.OnCancel);
            this.TestCommand = new Command(this.OnTest);
        }

        private void OnOk(object param)
        {
        }

        private void OnCancel(object param)
        {
            this.Close();
        }

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
            }
            finally
            {
                DatabaseService.UnloadConfiguration();
            }
        }
    }
}
