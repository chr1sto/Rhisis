using Rhisis.Tools.Core;
using System;
using System.Windows;
using WinForm = System.Windows.Forms;

namespace Rhisis.Tools.Services
{
    public class DialogService : IDialogService
    {
        /// <summary>
        /// Open a folder dialog form.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Displays an error message box.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public void ShowError(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Displays an information message box.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public void ShowInformation(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Shows a window based on the type passed as parameter.
        /// </summary>
        /// <param name="viewModelType"></param>
        public bool ShowQuestion(string title, string message)
        {
            MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);

            return result == MessageBoxResult.Yes;
        }

        /// <summary>
        /// Asks a YesNo question to the user.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <returns>Yes: true, No: false</returns>
        public void ShowWindow(Type viewModelType)
        {
            var window = ViewFactory.CreateInstance(viewModelType) as Window;
            window.DataContext = Activator.CreateInstance(viewModelType);
            
            window.ShowDialog();
        }
    }
}
