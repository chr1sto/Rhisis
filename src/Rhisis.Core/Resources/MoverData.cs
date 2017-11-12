using Rhisis.Core.Resources;

namespace Rhisis.Core.Structures
{
    /// <summary>
    /// Represents a Mover data structure from the mover.txt resource file from the data.res.
    /// </summary>
    public class MoverData
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public float SizeFactor { get; private set; }
        public int AI { get; private set; }
        public int Str { get; private set; }
        public int Sta { get; private set; }
        public int Dex { get; private set; }
        public int Int { get; private set; }
        public int HR { get; private set; }
        public int ER { get; private set; }
        public int Race { get; private set; }
        public int Belligerence { get; private set; }
        public int Gender { get; private set; }
        public int Level { get; private set; }
        public int FlightLevel { get; private set; }
        public int Size { get; private set; }
        public int Class { get; private set; }
        public bool IfPart { get; private set; }
        public int Karma { get; private set; }
        public int Useable { get; private set; }
        public int ActionRadius { get; private set; }
        public int AtkMin { get; private set; }
        public int AtkMax { get; private set; }
        public int Atk1 { get; private set; }
        public int Atk2 { get; private set; }
        public int Atk3 { get; private set; }
        public int Atk4 { get; private set; }
        public int HorizontalRate { get; private set; }
        public int VerticalRate { get; private set; }
        public int DiagonalRate { get; private set; }
        public int ThrustRate { get; private set; }
        public int ChestRate { get; private set; }
        public int HeadRate { get; private set; }
        public int ArmRate { get; private set; }
        public int LegRate { get; private set; }
        public int AttackSpeed { get; private set; }
        public int ReAttackDelay { get; private set; }
        public int AddHp { get; private set; }
        public int MaxHP { get; private set; }
        public int AddMp { get; private set; }
        public int NaturalArmor { get; private set; }
        public int nAbrasion { get; private set; }
        public int nHardness { get; private set; }
        public int AdjAtkDelay { get; private set; }
        public int eElementType { get; private set; }
        public int wElementAtk { get; private set; }
        public int HideLevel { get; private set; }
        public float Speed { get; private set; }
        public int Shelter { get; private set; }
        public bool Flying { get; private set; }
        public int Jumping { get; private set; }
        public int AirJump { get; private set; }
        public bool Taming { get; private set; }
        public int ResistMagic { get; private set; }
        public float ResistElectricity { get; private set; }
        public float ResistFire { get; private set; }
        public float ResistWind { get; private set; }
        public float ResistWater { get; private set; }
        public float ResistEarth { get; private set; }
        public int Cash { get; private set; }
        public int SourceMaterial { get; private set; }
        public int MaterialAmount { get; private set; }
        public int Cohesion { get; private set; }
        public int HoldingTime { get; private set; }
        public int CorrectionValue { get; private set; }
        public int ExpValue { get; private set; }
        public int nFxpValue { get; private set; }
        public int nBodyState { get; private set; }
        public int AddAbility { get; private set; }
        public bool Killable { get; private set; }
        public int VirtItem1 { get; private set; }
        public int VirtType1 { get; private set; }
        public int VirtItem2 { get; private set; }
        public int VirtType2 { get; private set; }
        public int VirtItem3 { get; private set; }
        public int VirtType3 { get; private set; }
        public int SndAtk1 { get; private set; }
        public int SndAtk2 { get; private set; }
        public int SndDie1 { get; private set; }
        public int SndDie2 { get; private set; }
        public int SndDmg1 { get; private set; }
        public int SndDmg2 { get; private set; }
        public int SndDmg3 { get; private set; }
        public int SndIdle1 { get; private set; }
        public int SndIdle2 { get; private set; }
        public string Comment { get; private set; }

        public int MinPenya { get; set; }
        public int MaxPenya { get; set; }
        public int MaxItems { get; set; }

        /// <summary>
        /// Creates an empty <see cref="MoverData"/> instance.
        /// </summary>
        public MoverData()
        {
        }

        /// <summary>
        /// Creates a <see cref="MoverData"/> instance from a <see cref="ResourceTable"/>.
        /// </summary>
        /// <param name="table"></param>
        public MoverData(ResourceTable table)
        {
            if (table == null)
                return;

            this.Id = table.Get<int>("dwID");
            this.Name = table.Get<string>("szName");
            this.AI = table.Get<int>("dwAI");
            this.Str = table.Get<int>("dwStr");
            this.Sta = table.Get<int>("dwSta");
            this.Dex = table.Get<int>("dwDex");
            this.Int = table.Get<int>("dwInt");
            this.HR = table.Get<int>("dwHR");
            this.ER = table.Get<int>("dwER");
            this.Race = table.Get<int>("dwRace");
            this.Belligerence = table.Get<int>("dwBelligerence");
            this.Gender = table.Get<int>("dwGender");
            this.Level = table.Get<int>("dwLevel");
            this.FlightLevel = table.Get<int>("dwFlightLevel");
            this.Size = table.Get<int>("dwSize");
            this.Class = table.Get<int>("dwClass");
            this.IfPart = table.Get<int>("bIfPart") != 0;
            this.Karma = table.Get<int>("dwKarma");
            this.Useable = table.Get<int>("dwUseable");
            this.ActionRadius = table.Get<int>("dwActionRadius");
            this.AtkMin = table.Get<int>("dwAtkMin");
            this.AtkMax = table.Get<int>("dwAtkMax");
            this.Atk1 = table.Get<int>("dwAtk1");
            this.Atk2 = table.Get<int>("dwAtk2");
            this.Atk3 = table.Get<int>("dwAtk3");
            this.Atk4 = table.Get<int>("dwHorizontalRate");
            this.ArmRate = table.Get<int>("dwArmRate");
            this.LegRate = table.Get<int>("dwLegRate");
            this.AttackSpeed = table.Get<int>("dwAttackSpeed");
            this.ReAttackDelay = table.Get<int>("dwReAttackDelay");
            this.AddHp = (int)((double)table.Get<int>("dwAddHp") * 1.0); // MOVER HP RATE
            this.MaxHP = this.AddHp;
            this.AddMp = table.Get<int>("dwAddMp");
            this.NaturalArmor = table.Get<int>("dwNaturealArmor");
            this.nAbrasion = table.Get<int>("nAbrasion");
            this.nHardness = table.Get<int>("nHardness");
            this.AdjAtkDelay = table.Get<int>("dwAdjAtkDelay");
            this.eElementType = table.Get<int>("eElementType");
            this.wElementAtk = table.Get<int>("wElementAtk");
            this.HideLevel = table.Get<int>("dwHideLevel");
            this.Speed = table.Get<float>("fSpeed");
            this.Shelter = table.Get<int>("dwShelter");
            this.Flying = table.Get<int>("bFlying") != 0;
            this.Jumping = table.Get<int>("dwJumping");
            this.AirJump = table.Get<int>("dwAirJump");
            this.Taming = table.Get<int>("bTaming") != 0;
            this.ResistMagic = table.Get<int>("dwResistMagic");
            this.ResistElectricity = table.Get<float>("fResistElectricity");
            this.ResistFire = table.Get<float>("fResistFire");
            this.ResistWind = table.Get<float>("fResistWind");
            this.ResistWater = table.Get<float>("fResistWater");
            this.ResistEarth = table.Get<float>("fResistEarth");
            this.Cash = table.Get<int>("dwCash");
            this.SourceMaterial = table.Get<int>("dwSourceMaterial");
            this.MaterialAmount = table.Get<int>("dwMaterialAmount");
            this.Cohesion = table.Get<int>("dwCohesion");
            this.HoldingTime = table.Get<int>("dwHoldingTime");
            this.CorrectionValue = table.Get<int>("dwCorrectionValue");
            this.ExpValue = table.Get<int>("dwExpValue");
            this.nFxpValue = table.Get<int>("nFxpValue");
            this.nBodyState = table.Get<int>("nBodyState");
            this.AddAbility = table.Get<int>("dwAddAbility");
            this.Killable = table.Get<int>("bKillable") != 0;
            this.VirtItem1 = table.Get<int>("dwVirtItem1");
            this.VirtType1 = table.Get<int>("dwVirtType1");
            this.VirtItem2 = table.Get<int>("dwVirtItem2");
            this.VirtType2 = table.Get<int>("dwVirtType2");
            this.VirtItem3 = table.Get<int>("dwVirtItem3");
            this.VirtType3 = table.Get<int>("dwVirtType3");
            this.SndAtk1 = table.Get<int>("dwSndAtk1");
            this.SndAtk2 = table.Get<int>("dwSndAtk2");
            this.SndDie1 = table.Get<int>("dwSndDie1");
            this.SndDie2 = table.Get<int>("dwSndDie2");
            this.SndDmg1 = table.Get<int>("dwSndDmg1");
            this.SndDmg2 = table.Get<int>("dwSndDmg2");
            this.SndDmg3 = table.Get<int>("dwSndDmg3");
            this.SndIdle1 = table.Get<int>("dwSndIdle1");
            this.SndIdle2 = table.Get<int>("dwSndIdle2");
            this.Comment = table.Get<string>("szComment");
        }
    }
}
