using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using Rhisis.World.Packets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Systems.PlayerData
{
    [Injectable]
    public sealed class PlayerDataSystem : IPlayerDataSystem
    {
        private readonly IMoverPacketFactory _moverPacketFactory;
        private readonly ITextPacketFactory _textPacketFactory;

        /// <summary>
        /// Creates a new <see cref="PlayerDataSystem"/> instance.
        /// </summary>
        /// <param name="moverPacketFactory">Mover packet factory.</param>
        /// <param name="textPacketFactory">Text packet factory.</param>
        public PlayerDataSystem(IMoverPacketFactory moverPacketFactory, ITextPacketFactory textPacketFactory)
        {
            _moverPacketFactory = moverPacketFactory;
            _textPacketFactory = textPacketFactory;
        }

        /// <inheritdoc />
        public bool IncreaseGold(IPlayerEntity player, int goldAmount)
        {
            // We cast player gold to long because otherwise it would use Int32 arithmetic and would overflow
            long gold = (long)player.PlayerData.Gold + goldAmount;

            if (gold > int.MaxValue || gold < 0) // Check gold overflow
            {
                _textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_TOOMANYMONEY_USE_PERIN);
                return false;
            }
            else
            {
                player.PlayerData.Gold = (int)gold;
                _moverPacketFactory.SendUpdateAttributes(player, DefineAttributes.GOLD, player.PlayerData.Gold);
            }

            return true;
        }

        /// <inheritdoc />
        public bool DecreaseGold(IPlayerEntity player, int goldAmount)
        {
            player.PlayerData.Gold = Math.Max(player.PlayerData.Gold - goldAmount, 0);

            _moverPacketFactory.SendUpdateAttributes(player, DefineAttributes.GOLD, player.PlayerData.Gold);

            return true;
        }

        /// <inheritdoc />
        public void CalculateDefense(IPlayerEntity player)
        {
            var defenseMin = 0;
            var defenseMax = 0;
            IEnumerable<Item> equipedItems = player.Inventory.GetEquipedItems();

            if (equipedItems.Any())
            {
                foreach (Item equipedItem in equipedItems)
                {
                    if (equipedItem == null || (equipedItem != null && equipedItem.Id == -1))
                    {
                        continue;
                    }

                    if (equipedItem.Data.ItemKind2 == ItemKind2.ARMOR || equipedItem.Data.ItemKind2 == ItemKind2.ARMORETC)
                    {
                        int refineValue = equipedItem.Refine > 0 ? (int)Math.Pow(equipedItem.Refine, 1.5f) : 0;
                        const float itemMultiplier = 1; // TODO: implement GetItemMultiplier() on the Item class of the Rhisis Domain.

                        defenseMin += (int)(equipedItem.Data.AbilityMin * itemMultiplier) + refineValue;
                        defenseMax += (int)(equipedItem.Data.AbilityMax * itemMultiplier) + refineValue;
                    }
                }
            }

            defenseMin += player.Attributes[DefineAttributes.ABILITY_MIN];
            defenseMax += player.Attributes[DefineAttributes.ABILITY_MAX];

            player.PlayerData.DefenseMin = defenseMin;
            player.PlayerData.DefenseMax = defenseMax;
        }
    }
}
