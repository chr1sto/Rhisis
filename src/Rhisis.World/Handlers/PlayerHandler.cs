using Microsoft.Extensions.Logging;
using Rhisis.Core.Data;
using Rhisis.Network.Packets;
using Rhisis.Network.Packets.World;
using Rhisis.World.Client;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Death;
using Rhisis.World.Systems.Follow;
using Rhisis.World.Systems.Interaction;
using Rhisis.World.Systems.SpecialEffect;
using Sylver.HandlerInvoker.Attributes;
using Sylver.Network.Data;
using System;

namespace Rhisis.World.Handlers
{
    [Handler]
    public sealed class PlayerHandler
    {
        private readonly ILogger<PlayerHandler> _logger;
        private readonly ISpecialEffectSystem _specialEffectSystem;
        private readonly IInteractionSystem _interationSystem;
        private readonly IFollowSystem _followSystem;
        private readonly IMoverPacketFactory _moverPacketFactory;
        private readonly IDeathSystem _deathSystem;

        public PlayerHandler(ILogger<PlayerHandler> logger, ISpecialEffectSystem specialEffectSystem, IInteractionSystem interationSystem, IFollowSystem followSystem, IMoverPacketFactory moverPacketFactory, IDeathSystem deathSystem)
        {
            _logger = logger;
            _specialEffectSystem = specialEffectSystem;
            _interationSystem = interationSystem;
            _followSystem = followSystem;
            _moverPacketFactory = moverPacketFactory;
            _deathSystem = deathSystem;
        }

        [HandlerAction(PacketType.STATEMODE)]
        public void OnStateMode(IWorldClient client, StateModePacket packet)
        {
            if (client.Player.Object.StateMode == packet.StateMode)
            {
                if (packet.Flag == StateModeBaseMotion.BASEMOTION_CANCEL)
                {
                    _specialEffectSystem.SetStateModeBaseMotion(client.Player, packet.Flag);
                    client.Player.Delayer.CancelAction(client.Player.Inventory.ItemInUseActionId);
                    client.Player.Inventory.ItemInUseActionId = Guid.Empty;
                }
            }
        }

        [HandlerAction(PacketType.SETTARGET)]
        public void OnSetTarget(IWorldClient client, SetTargetPacket packet)
        {
            _interationSystem.SetTarget(client.Player, packet.TargetId, packet.TargetMode);
        }

        [HandlerAction(PacketType.PLAYERSETDESTOBJ)]
        public void OnPlayerSetDestObject(IWorldClient client, PlayerDestObjectPacket packet)
        {
            _followSystem.Follow(client.Player, packet.TargetObjectId, packet.Distance);
        }

        [HandlerAction(PacketType.QUERY_PLAYER_DATA)]
        public void OnQueryPlayerData(IWorldClient client, INetPacketStream packet)
        {
        }

        [HandlerAction(PacketType.QUERY_PLAYER_DATA2)]
        public void OnQueryPlayerData2(IWorldClient client, INetPacketStream packet)
        {
        }

        [HandlerAction(PacketType.PLAYERMOVED)]
        public void OnPlayerMoved(IWorldClient client, PlayerMovedPacket packet)
        {
            if (client.Player.IsDead)
            {
                _logger.LogError($"Player {client.Player.Object.Name} is dead, he cannot move with keyboard.");
                return;
            }

            // TODO: Check if player is flying

            client.Player.Follow.Reset();
            client.Player.Battle.Reset();
            client.Player.Object.Position = packet.BeginPosition + packet.DestinationPosition;
            client.Player.Object.Angle = packet.Angle;
            client.Player.Object.MovingFlags = (ObjectState)packet.State;
            client.Player.Object.MotionFlags = (StateFlags)packet.StateFlag;
            client.Player.Moves.IsMovingWithKeyboard = client.Player.Object.MovingFlags.HasFlag(ObjectState.OBJSTA_FMOVE) || 
                client.Player.Object.MovingFlags.HasFlag(ObjectState.OBJSTA_BMOVE);
            client.Player.Moves.DestinationPosition = packet.BeginPosition + packet.DestinationPosition;

            _moverPacketFactory.SendMoverMoved(client.Player,
                packet.BeginPosition,
                packet.DestinationPosition,
                client.Player.Object.Angle, 
                (uint)client.Player.Object.MovingFlags, 
                (uint)client.Player.Object.MotionFlags,
                packet.Motion,
                packet.MotionEx,
                packet.Loop,
                packet.MotionOption,
                packet.TickCount);
        }

        [HandlerAction(PacketType.PLAYERBEHAVIOR)]
        public void OnPlayerBehavior(IWorldClient client, PlayerBehaviorPacket packet)
        {
            if (client.Player.IsDead)
            {
                _logger.LogError($"Player {client.Player.Object.Name} is dead, he cannot move with keyboard.");
                return;
            }

            // TODO: check if player is flying

            client.Player.Object.Position = packet.BeginPosition + packet.DestinationPosition;
            client.Player.Object.Angle = packet.Angle;
            client.Player.Object.MovingFlags = (ObjectState)packet.State;
            client.Player.Object.MotionFlags = (StateFlags)packet.StateFlag;
            client.Player.Moves.IsMovingWithKeyboard = client.Player.Object.MovingFlags.HasFlag(ObjectState.OBJSTA_FMOVE) ||
                client.Player.Object.MovingFlags.HasFlag(ObjectState.OBJSTA_BMOVE);
            client.Player.Moves.DestinationPosition = packet.BeginPosition + packet.DestinationPosition;

            _moverPacketFactory.SendMoverBehavior(client.Player,
                packet.BeginPosition,
                packet.DestinationPosition,
                client.Player.Object.Angle,
                (uint)client.Player.Object.MovingFlags,
                (uint)client.Player.Object.MotionFlags,
                packet.Motion,
                packet.MotionEx,
                packet.Loop,
                packet.MotionOption,
                packet.TickCount);
        }

        [HandlerAction(PacketType.REVIVAL_TO_LODESTAR)]
        public void OnRevivalToLodestar(IWorldClient client, INetPacketStream _)
        {
            if (!client.Player.IsDead)
            {
                _logger.LogWarning($"Player '{client.Player.Object.Name}' tried to revival to lodestar without being dead.");
                return;
            }

            _deathSystem.ResurectLodelight(client.Player);
        }
    }
}
