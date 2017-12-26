using System;
using System.Windows;

namespace Rhisis.Configuration.Services
{
    public interface IDialogService
    {
        Window CurrentWindow { get; }
 
        string OpenFolderDialog();

        void ShowError(string title, string message);

        void ShowInformation(string title, string message);

        void ShowWindow(Type viewModelType);
    }
}
