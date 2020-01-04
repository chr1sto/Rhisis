using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using System;

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
            this._moverPacketFactory = moverPacketFactory;
            this._textPacketFactory = textPacketFactory;
        }

        /// <inheritdoc />
        public bool IncreaseGold(IPlayerEntity player, int goldAmount)
        {
            long gold = player.PlayerData.Gold + goldAmount;

            if (gold > int.MaxValue || gold < 0) // Check gold overflow
            {
                this._textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_TOOMANYMONEY_USE_PERIN);
                return false;
            }
            else
            {
                player.PlayerData.Gold = (int)gold;
                this._moverPacketFactory.SendUpdateAttributes(player, DefineAttributes.GOLD, player.PlayerData.Gold);
            }

            return true;
        }

        /// <inheritdoc />
        public bool DecreaseGold(IPlayerEntity player, int goldAmount)
        {
            player.PlayerData.Gold = Math.Max(player.PlayerData.Gold - goldAmount, 0);

            this._moverPacketFactory.SendUpdateAttributes(player, DefineAttributes.GOLD, player.PlayerData.Gold);

            return true;
        }
    }
}
