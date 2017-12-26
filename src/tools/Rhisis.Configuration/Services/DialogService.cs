using Rhisis.Configuration.Core;
using System;
using System.Windows;
using WinForm = System.Windows.Forms;

namespace Rhisis.Configuration.Services
{
    public class DialogService : IDialogService
    {
        public Window CurrentWindow { get; private set; }

        public string OpenFolderDialog()
        {
            string selectedFolder = null;

            using (var dialog = new WinForm.FolderBrowserDialog())
            {
                WinForm.DialogResult result = dialog.ShowDialog();

                if (result == WinForm.DialogResult.OK)
                    selectedFolder = dialog.SelectedPath;
            }

            return selectedFolder;
        }

        public void ShowError(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowInformation(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowWindow(Type viewModelType)
        {
            var window = ViewFactory.CreateInstance(viewModelType) as Window;
            window.DataContext = Activator.CreateInstance(viewModelType);

            this.CurrentWindow = window;

            window.ShowDialog();
        }
    }
}
