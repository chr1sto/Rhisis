using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Packets.Internal
{
    [Injectable(ServiceLifetime.Singleton)]
    public sealed class SpecialEffectPacketFactory : ISpecialEffectPacketFactory
    {
        private readonly IPacketFactoryUtilities _packetFactoryUtilities;

        /// <summary>
        /// Creates a new <see cref="SpecialEffectPacketFactory"/> instance.
        /// </summary>
        /// <param name="packetFactoryUtilities">Packet factory utilities.</param>
        public SpecialEffectPacketFactory(IPacketFactoryUtilities packetFactoryUtilities)
        {
            this._packetFactoryUtilities = packetFactoryUtilities;
        }

        /// <inheritdoc />
        public void SendSpecialEffect(IWorldEntity entity, int specialEffectId, bool sfxNoFollow)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(entity.Id, SnapshotType.CREATESFXOBJ);
                packet.Write(specialEffectId);
                if (sfxNoFollow == true)
                {
                    packet.Write(entity.Object.Position.X); // X
                    packet.Write(entity.Object.Position.Y); // Y
                    packet.Write(entity.Object.Position.Z); // Z
                }
                else 
                {
                    packet.Write(0f); // X
                    packet.Write(0f); // Y
                    packet.Write(0f); // Z
                }

                packet.Write(false); // Flag

                this._packetFactoryUtilities.SendToVisible(packet, entity, sendToPlayer: true);
            }
        }

        /// <inheritdoc />
        public void SendSpecialEffect(IWorldEntity entity, DefineSpecialEffects specialEffect, bool sfxNoFollow)
            => this.SendSpecialEffect(entity, (int)specialEffect, sfxNoFollow);
    }
}
