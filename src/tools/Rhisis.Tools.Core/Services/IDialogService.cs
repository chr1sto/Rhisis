using System;

namespace Rhisis.Tools.Services
{
    public interface IDialogService
    {
        /// <summary>
        /// Open a folder dialog form.
        /// </summary>
        /// <returns></returns>
        string OpenFolderDialog();

        /// <summary>
        /// Displays an error message box.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        void ShowError(string title, string message);

        /// <summary>
        /// Displays an information message box.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        void ShowInformation(string title, string message);

        /// <summary>
        /// Shows a window based on the type passed as parameter.
        /// </summary>
        /// <param name="viewModelType"></param>
        void ShowWindow(Type viewModelType);

        /// <summary>
        /// Asks a YesNo question to the user.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <returns>Yes: true, No: false</returns>
        bool ShowQuestion(string title, string message);
    }
}
