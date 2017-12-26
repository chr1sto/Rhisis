using Rhisis.Manager.Core.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Rhisis.Manager.ViewModels
{
    public sealed class MainViewModel : ViewModelBase
    {
        public ICommand ChangeLanguageCommand { get; }

        public MainViewModel()
        {
            this.ChangeLanguageCommand = new Command(this.OnChangeLanguage);
        }

        private void OnChangeLanguage(object param)
        {
            App.Instance.SwitchLanguage(param.ToString());
        }
    }
}
