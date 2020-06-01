using Rhisis.Core.Common;
using Rhisis.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rhisis.Cluster.Structures
{
    public class ClusterCharacter
    {
        public int Id { get; }

        public string Name { get; }

        public GenderType Gender { get; }

        public int Level { get; }

        public int Slot { get; }

        public int MapId { get; }

        public float PositionX { get; }

        public float PositionY { get; }

        public float PositionZ { get; }

        public int SkinSetId { get; }

        public int HairId { get; }

        public uint HairColor { get; }

        public int FaceId { get; }

        public int JobId { get; }

        public int Strength { get; }

        public int Stamina { get; }

        public int Intelligence { get; }

        public int Dexterity { get; }

        /// <summary>
        /// Gets the character equiped items ids.
        /// </summary>
        public IEnumerable<int> EquipedItems { get; }

        public ClusterCharacter(DbCharacter dbCharacter)
        {
            Id = dbCharacter.Id;
            Name = dbCharacter.Name;
            Gender = (GenderType)dbCharacter.Gender;
            Level = dbCharacter.Level;
            Slot = dbCharacter.Slot;
            MapId = dbCharacter.MapId;
            PositionX = dbCharacter.PosX;
            PositionY = dbCharacter.PosY;
            PositionZ = dbCharacter.PosZ;
            SkinSetId = dbCharacter.SkinSetId;
            HairId = dbCharacter.HairId;
            HairColor = (uint)dbCharacter.HairColor;
            FaceId = dbCharacter.FaceId;
            JobId = dbCharacter.JobId;
            Strength = dbCharacter.Strength;
            Stamina = dbCharacter.Stamina;
            Intelligence = dbCharacter.Intelligence;
            Dexterity = dbCharacter.Dexterity;
            EquipedItems = dbCharacter.Items.AsQueryable()
                .Where(x => x.ItemSlot > 42 && !x.IsDeleted)
                .OrderBy(x => x.ItemSlot)
                .Select(x => x.ItemId);
        }
    }
}
