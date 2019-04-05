using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Rhisis.Core.Structures.Game.Dialogs;
using System.Collections.Generic;
using System.IO;

namespace Rhisis.Core.Resources.Loaders
{
    public sealed class DialogLoader : IGameResourceLoader
    {
        private readonly ILogger<DialogLoader> _logger;
        private readonly IDictionary<string, DialogSet> _dialogs;
        private readonly JsonLoadSettings _jsonSettings = new JsonLoadSettings { CommentHandling = CommentHandling.Ignore };

        /// <summary>
        /// Gets a dialog data.
        /// </summary>
        /// <param name="dialog">Dialog to retrieve</param>
        /// <param name="lang">Language</param>
        /// <returns></returns>
        public DialogData this[string dialog, string lang] => this.GetDialogData(dialog, lang);

        /// <summary>
        /// Creates a new <see cref="DialogLoader"/> instance.
        /// </summary>
        /// <param name="logger">Logger</param>
        public DialogLoader(ILogger<DialogLoader> logger)
        {
            this._logger = logger;
            this._dialogs = new Dictionary<string, DialogSet>();
        }

        /// <inheritdoc />
        public void Load()
        {
            string[] dialogsDirectories = Directory.GetDirectories(GameResources.DialogsPath);

            foreach (string dialogPath in dialogsDirectories)
            {
                string lang = new DirectoryInfo(dialogPath).Name;
                string[] dialogFiles = Directory.GetFiles(dialogPath);

                var dialogSet = new DialogSet(lang);

                foreach (string dialogFile in dialogFiles)
                {
                    string dialogFileContent = File.ReadAllText(dialogFile);
                    var dialogsParsed = JToken.Parse(dialogFileContent, this._jsonSettings);

                    if (dialogsParsed.Type == JTokenType.Array)
                    {
                        var dialogs = dialogsParsed.ToObject<DialogData[]>();

                        foreach (DialogData dialog in dialogs)
                            this.AddDialog(dialogSet, dialog);
                    }
                    else
                    {
                        this.AddDialog(dialogSet, dialogsParsed.ToObject<DialogData>());   
                    }
                }

                this._dialogs.Add(lang, dialogSet);
                this._logger.LogInformation($"-> {dialogSet.Count} dialogs loaded for language {lang}.");
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            foreach (var dialogSet in this._dialogs.Values)
                dialogSet.Clear();

            this._dialogs.Clear();
        }

        /// <summary>
        /// Gets a dialog data.
        /// </summary>
        /// <param name="dialog">Dialog to retrieve</param>
        /// <param name="lang">Language</param>
        /// <returns></returns>
        public DialogData GetDialogData(string dialog, string lang) 
            => this._dialogs.TryGetValue(lang, out DialogSet dialogSet) && dialogSet.TryGetValue(dialog, out DialogData dialogData)
                ? dialogData
                : null;

        /// <summary>
        /// Adds a dialog to a given <see cref="DialogSet"/>.
        /// </summary>
        /// <param name="dialogSet">Dialog set</param>
        /// <param name="dialog">Dialog to add</param>
        private void AddDialog(DialogSet dialogSet, DialogData dialog)
        {
            if (dialogSet.ContainsKey(dialog.Name))
                this._logger.LogDebug(GameResources.ObjectIgnoredMessage, "Dialog", dialog.Name, "already declared");
            else
                dialogSet.Add(dialog.Name, dialog);
        }
    }
}
