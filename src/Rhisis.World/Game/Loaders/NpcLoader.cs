using Microsoft.Extensions.Logging;
using Rhisis.Core.Resources;
using Rhisis.Core.Resources.Include;
using Rhisis.Core.Resources.Loaders;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Core.Structures.Game;
using Rhisis.Core.Structures.Game.Dialogs;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rhisis.World.Game.Loaders
{
    public sealed class NpcLoader : IGameResourceLoader
    {
        private readonly ILogger<NpcLoader> _logger;
        private readonly IDictionary<string, NpcData> _npcData;
        private readonly WorldConfiguration _configuration;
        private readonly DefineLoader _defines;
        private readonly TextLoader _texts;
        private readonly DialogLoader _dialogs;

        /// <summary>
        /// Gets the <see cref="NpcData"/> associated to the npc id.
        /// </summary>
        /// <param name="npcId">Npc ID</param>
        /// <returns><see cref="NpcData"/> if npc id exists; null otherwise</returns>
        public NpcData this[string npcId] => this.GetNpcData(npcId);

        /// <summary>
        /// Creates a new <see cref="NpcLoader"/> instance.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="configuration">World server configuration</param>
        /// <param name="defines">Define loader</param>
        /// <param name="texts">Text loader</param>
        /// <param name="dialogs">Dialogs loader</param>
        public NpcLoader(ILogger<NpcLoader> logger, WorldConfiguration configuration, DefineLoader defines, TextLoader texts, DialogLoader dialogs)
        {
            this._logger = logger;
            this._npcData = new Dictionary<string, NpcData>();
            this._configuration = configuration;
            this._defines = defines;
            this._texts = texts;
            this._dialogs = dialogs;
        }
        
        /// <inheritdoc />
        public void Load()
        {
            IEnumerable<string> files = Directory.GetFiles(GameResources.ResourcePath, "character*.inc", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                using (var npcFile = new IncludeFile(file))
                {
                    foreach (IStatement npcStatement in npcFile.Statements)
                    {
                        if (!(npcStatement is Block npcBlock))
                            continue;

                        string npcId = npcStatement.Name;
                        string npcName = npcId;

                        // We gets the npc name.
                        foreach (IStatement npcInfoStatement in npcBlock.Statements)
                        {
                            if (npcInfoStatement is Instruction instruction && npcInfoStatement.Name == "SetName")
                            {
                                if (instruction.Parameters.Count > 0)
                                {
                                    npcName = this._texts[instruction.Parameters.First().ToString()];
                                }
                            }
                        }

                        //TODO: implement other npc settings (image, music, actions...)
                        //      + constants for statement (like SetName)

                        // We gets shop and dialog of this npc.
                        //ShopData.TryGetValue(npcId, out ShopData npcShop);
                        DialogData dialog = this._dialogs.GetDialogData(npcId, this._configuration.Language);

                        var npc = new NpcData(npcId, npcName, null, dialog);

                        if (this._npcData.ContainsKey(npc.Id))
                        {
                            this._npcData[npc.Id] = npc;
                            this._logger.LogWarning(GameResources.ObjectOverridedMessage, "NPC", npc.Id, "already declared");
                        }
                        else
                            this._npcData.Add(npc.Id, npc);
                    }
                }
            }

            this._logger.LogInformation($"-> {this._npcData.Count} NPCs loaded.");
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this._npcData.Clear();
        }

        /// <summary>
        /// Gets a <see cref="NpcData"/> by the Npc ID.
        /// </summary>
        /// <param name="npcId">Npc Id</param>
        /// <returns></returns>
        public NpcData GetNpcData(string npcId) => this._npcData.TryGetValue(npcId, out NpcData value) ? value : null;
    }
}
