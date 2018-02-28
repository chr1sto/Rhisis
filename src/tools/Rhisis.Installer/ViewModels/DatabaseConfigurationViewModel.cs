using Rhisis.Core.Helpers;
using Rhisis.Database;
using Rhisis.Tools.Core.MVVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rhisis.Installer.ViewModels
{
    public class DatabaseConfigurationViewModel : ViewModelBase
    {
        public DatabaseConfiguration Configuration { get; }

        public IList<DatabaseProvider> Providers { get; }

        public DatabaseConfigurationViewModel()
        {
        }
    }
}
