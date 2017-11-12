using Rhisis.Core.IO;
using Rhisis.Core.Resources;
using Rhisis.Core.Structures;
using Rhisis.World.Core.Systems;
using Rhisis.World.Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Rhisis.World
{
    public partial class WorldServer
    {
        private static readonly string DataPath = Path.Combine(Directory.GetCurrentDirectory(), "data");
        private static readonly string ResourcePath = Path.Combine(DataPath, "res");
        private static readonly IDictionary<string, int> _defines = new Dictionary<string, int>();
        private static readonly IDictionary<string, string> _texts = new Dictionary<string, string>();
        private static readonly IDictionary<int, MoverData> _movers = new Dictionary<int, MoverData>();

        /// <summary>
        /// Gets the Movers data.
        /// </summary>
        public static IReadOnlyDictionary<int, MoverData> Movers => _movers as IReadOnlyDictionary<int, MoverData>;

        /// <summary>
        /// Loads the server's resources.
        /// </summary>
        private void LoadResources()
        {
            Logger.Info("Loading resources...");
            Profiler.Start("LoadResources");

            this.LoadDefinesAndTexts();
            this.LoadMovers();
            this.LoadMaps();
            this.CleanUp();

            var time = Profiler.Stop("LoadResources");
            Logger.Info("Resources loaded in {0}ms", time.ElapsedMilliseconds);
        }

        private void LoadDefinesAndTexts()
        {
            var headerFiles = from x in Directory.GetFiles(ResourcePath, "*.*", SearchOption.AllDirectories)
                              where DefineFile.Extensions.Contains(Path.GetExtension(x))
                              select x;

            var textFiles = from x in Directory.GetFiles(ResourcePath, "*.*", SearchOption.AllDirectories)
                            where TextFile.Extensions.Contains(Path.GetExtension(x)) && x.EndsWith(".txt.txt")
                            select x;

            foreach (var headerFile in headerFiles)
            {
                using (var defineFile = new DefineFile(headerFile))
                {
                    foreach (var define in defineFile.Defines)
                    {
                        if (!_defines.ContainsKey(define.Key) && define.Value is int)
                            _defines.Add(define.Key, int.Parse(define.Value.ToString()));
                    }
                }
            }

            foreach (var textFilePath in textFiles)
            {
                using (var textFile = new TextFile(textFilePath))
                {
                    textFile.Parse();

                    foreach (var text in textFile.Texts)
                    {
                        if (!_texts.ContainsKey(text.Key) && !string.IsNullOrEmpty(text.Value))
                            _texts.Add(text);
                    }
                }
            }
        }

        private void LoadMovers()
        {
            string propMoverPath = Path.Combine(ResourcePath, "data", "propMover.txt");

            Logger.Loading("Loading movers...");
            using (var propMoverFile = new ResourceTable(propMoverPath))
            {
                propMoverFile.AddDefines(_defines);
                propMoverFile.AddTexts(_texts);
                propMoverFile.SetTableHeaders("dwID", "szName", "dwAI", "dwStr", "dwSta", "dwDex", "dwInt", "dwHR", "dwER", "dwRace", "dwBelligerence", "dwGender", "dwLevel", "dwFlightLevel", "dwSize", "dwClass", "bIfPart", "dwKarma", "dwUseable", "dwActionRadius", "dwAtkMin", "dwAtkMax", "dwAtk1", "dwAtk2", "dwAtk3", "dwHorizontalRate", "dwVerticalRate", "dwDiagonalRate", "dwThrustRate", "dwChestRate", "dwHeadRate", "dwArmRate", "dwLegRate", "dwAttackSpeed", "dwReAttackDelay", "dwAddHp", "dwAddMp", "dwNaturealArmor", "nAbrasion", "nHardness", "dwAdjAtkDelay", "eElementType", "wElementAtk", "dwHideLevel", "fSpeed", "dwShelter", "bFlying", "dwJumping", "dwAirJump", "bTaming", "dwResistMagic", "fResistElectricity", "fResistFire", "fResistWind", "fResistWater", "fResistEarth", "dwCash", "dwSourceMaterial", "dwMaterialAmount", "dwCohesion", "dwHoldingTime", "dwCorrectionValue", "dwExpValue", "nFxpValue", "nBodyState", "dwAddAbility", "bKillable", "dwVirtItem1", "dwVirtType1", "dwVirtItem2", "dwVirtType2", "dwVirtItem3", "dwVirtType3", "dwSndAtk1", "dwSndAtk2", "dwSndDie1", "dwSndDie2", "dwSndDmg1", "dwSndDmg2", "dwSndDmg3", "dwSndIdle1", "dwSndIdle2", "szComment");
                propMoverFile.Parse();

                while (propMoverFile.Read())
                {
                    var mover = new MoverData(propMoverFile);

                    if (_movers.ContainsKey(mover.Id))
                        _movers[mover.Id] = mover;
                    else
                        _movers.Add(mover.Id, mover);

                    Logger.Loading("Loading {0}/{1} movers...", propMoverFile.ReadingIndex, propMoverFile.Count());
                }
            }
            Logger.Info("{0} movers loaded!\t\t", _movers.Count);
        }

        private void LoadMaps()
        {
            Logger.Loading("Loading maps...\t\t");
            IEnumerable<Type> systemTypes = this.LoadSystems();
            IDictionary<string, string> worldsPaths = this.LoadWorldScript();

            foreach (string mapId in this.WorldConfiguration.Maps)
            {
                if (!worldsPaths.TryGetValue(mapId, out string mapName))
                {
                    Logger.Warning("Cannot load map with Id: {0}. Please check your world script file.", mapId);
                    continue;
                }

                if (!_defines.TryGetValue(mapId, out int id))
                {
                    Logger.Warning("Cannot find map Id in define files: {0}. Please check you defineWorld.h file.", mapId);
                    continue;
                }
                
                var map = Map.Load(Path.Combine(DataPath, "maps", mapName), mapName, id);

                foreach (var type in systemTypes)
                    map.Context.AddSystem(Activator.CreateInstance(type, map.Context) as ISystem);

                map.Start();
                _maps.Add(id, map);
            }

            Logger.Info("{0} maps loaded! \t\t", _maps.Count);
        }

        private IEnumerable<Type> LoadSystems()
        {
            return from x in Assembly.GetExecutingAssembly().GetTypes()
                   where x.GetTypeInfo().GetCustomAttribute<SystemAttribute>() != null
                   select x;
        }

        private IDictionary<string, string> LoadWorldScript()
        {
            var worldsPaths = new Dictionary<string, string>();

            using (var textFile = new TextFile(Path.Combine(ResourcePath, "data", "World.inc")))
            {
                textFile.Parse();
                foreach (var text in textFile.Texts)
                    worldsPaths.Add(text.Key, text.Value.Replace('"', ' ').Trim());
            }

            return worldsPaths;
        }

        private void CleanUp()
        {
            _defines.Clear();
            _texts.Clear();
            GC.Collect();
        }
    }
}
