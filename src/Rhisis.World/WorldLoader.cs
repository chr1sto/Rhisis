using Rhisis.Core.IO;
using Rhisis.Core.Resources;
using Rhisis.World.Game;
using Rhisis.World.Systems;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rhisis.World
{
    public partial class WorldServer
    {
        private static readonly string DataPath = Path.Combine(Directory.GetCurrentDirectory(), "data");
        private static readonly string ResourcePath = Path.Combine(DataPath, "res");
        private static readonly IDictionary<string, int> _defines = new Dictionary<string, int>();
        private static readonly IDictionary<string, string> _texts = new Dictionary<string, string>();

        private void LoadResources()
        {
            Logger.Info("Loading resources...");
            Profiler.Start("LoadResources");

            this.LoadDefines();
            this.LoadTexts();
            this.LoadMovers();
            this.LoadSystems();
            this.LoadMaps();
            this.CleanUp();

            var time = Profiler.Stop("LoadResources");
            Logger.Info("Resources loaded in {0}ms", time.ElapsedMilliseconds);
        }

        private void LoadDefines()
        {
            var headerFiles = from x in Directory.GetFiles(ResourcePath, "*.*", SearchOption.AllDirectories)
                              where DefineFile.Extensions.Contains(Path.GetExtension(x))
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
        }

        private void LoadTexts()
        {
            var textFiles = from x in Directory.GetFiles(ResourcePath, "*.*", SearchOption.AllDirectories)
                            where TextFile.Extensions.Contains(Path.GetExtension(x)) && x.EndsWith(".txt.txt")
                            select x;

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
                    // TODO: Add movers
                    Logger.Loading("Loading {0}/{1} movers...", propMoverFile.ReadingIndex, propMoverFile.Count());
                }
            }
            Logger.Info("{0} movers loaded!\t\t", 0);
        }

        private void LoadSystems()
        {
            Logger.Loading("Loading systems...");

            // TODO: Load systems using reflection

            Logger.Info("Systems loaded! \t\t");
        }

        private void LoadMaps()
        {
            Logger.Loading("Loading maps...\t\t");

            var map = Map.Load("data/maps/WdMadrigal"); // Load map
            map.Context.AddSystem(new VisibilitySystem(map.Context));
            map.Context.AddSystem(new MobilitySystem(map.Context));
            map.Context.AddSystem(new ChatSystem(map.Context));
            map.Start(); // Start map update thread

            _maps.Add(1, map); // Add the map to the 

            Logger.Info("All maps loaded! \t\t");
        }

        private void CleanUp()
        {
            _defines.Clear();
            _texts.Clear();
        }
    }
}
