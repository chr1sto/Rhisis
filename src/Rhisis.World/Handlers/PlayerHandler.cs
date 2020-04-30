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
        public void OnStateMode(IWorldServerClient serverClient, StateModePacket packet)
        {
            if (serverClient.Player.Object.StateMode == packet.StateMode)
            {
                if (packet.Flag == StateModeBaseMotion.BASEMOTION_CANCEL)
                {
                    _specialEffectSystem.SetStateModeBaseMotion(serverClient.Player, packet.Flag);
                    serverClient.Player.Delayer.CancelAction(serverClient.Player.Inventory.ItemInUseActionId);
                    serverClient.Player.Inventory.ItemInUseActionId = Guid.Empty;
                }
            }
        }

        [HandlerAction(PacketType.SETTARGET)]
        public void OnSetTarget(IWorldServerClient serverClient, SetTargetPacket packet)
        {
            _interationSystem.SetTarget(serverClient.Player, packet.TargetId, packet.TargetMode);
        }

        [HandlerAction(PacketType.PLAYERSETDESTOBJ)]
        public void OnPlayerSetDestObject(IWorldServerClient serverClient, PlayerDestObjectPacket packet)
        {
            _followSystem.Follow(serverClient.Player, packet.TargetObjectId, packet.Distance);
        }

        [HandlerAction(PacketType.QUERY_PLAYER_DATA)]
        public void OnQueryPlayerData(IWorldServerClient serverClient, INetPacketStream packet)
        {
        }

        [HandlerAction(PacketType.QUERY_PLAYER_DATA2)]
        public void OnQueryPlayerData2(IWorldServerClient serverClient, INetPacketStream packet)
        {
        }

        [HandlerAction(PacketType.PLAYERMOVED)]
        public void OnPlayerMoved(IWorldServerClient serverClient, PlayerMovedPacket packet)
        {
            if (serverClient.Player.IsDead)
            {
                _logger.LogError($"Player {serverClient.Player.Object.Name} is dead, he cannot move with keyboard.");
                return;
            }

            // TODO: Check if player is flying

            serverClient.Player.Follow.Reset();
            serverClient.Player.Battle.Reset();
            serverClient.Player.Object.Position = packet.BeginPosition + packet.DestinationPosition;
            serverClient.Player.Object.Angle = packet.Angle;
            serverClient.Player.Object.MovingFlags = (ObjectState)packet.State;
            serverClient.Player.Object.MotionFlags = (StateFlags)packet.StateFlag;
            serverClient.Player.Moves.IsMovingWithKeyboard = serverClient.Player.Object.MovingFlags.HasFlag(ObjectState.OBJSTA_FMOVE) || 
                serverClient.Player.Object.MovingFlags.HasFlag(ObjectState.OBJSTA_BMOVE);
            serverClient.Player.Moves.DestinationPosition = packet.BeginPosition + packet.DestinationPosition;

            _moverPacketFactory.SendMoverMoved(serverClient.Player,
                packet.BeginPosition,
                packet.DestinationPosition,
                serverClient.Player.Object.Angle, 
                (uint)serverClient.Player.Object.MovingFlags, 
                (uint)serverClient.Player.Object.MotionFlags,
                packet.Motion,
                packet.MotionEx,
                packet.Loop,
                packet.MotionOption,
                packet.TickCount);
        }

        [HandlerAction(PacketType.PLAYERBEHAVIOR)]
        public void OnPlayerBehavior(IWorldServerClient serverClient, PlayerBehaviorPacket packet)
        {
            if (serverClient.Player.IsDead)
            {
                _logger.LogError($"Player {serverClient.Player.Object.Name} is dead, he cannot move with keyboard.");
                return;
            }

            // TODO: check if player is flying

            serverClient.Player.Object.Position = packet.BeginPosition + packet.DestinationPosition;
            serverClient.Player.Object.Angle = packet.Angle;
            serverClient.Player.Object.MovingFlags = (ObjectState)packet.State;
            serverClient.Player.Object.MotionFlags = (StateFlags)packet.StateFlag;
            serverClient.Player.Moves.IsMovingWithKeyboard = serverClient.Player.Object.MovingFlags.HasFlag(ObjectState.OBJSTA_FMOVE) ||
                serverClient.Player.Object.MovingFlags.HasFlag(ObjectState.OBJSTA_BMOVE);
            serverClient.Player.Moves.DestinationPosition = packet.BeginPosition + packet.DestinationPosition;

            _moverPacketFactory.SendMoverBehavior(serverClient.Player,
                packet.BeginPosition,
                packet.DestinationPosition,
                serverClient.Player.Object.Angle,
                (uint)serverClient.Player.Object.MovingFlags,
                (uint)serverClient.Player.Object.MotionFlags,
                packet.Motion,
                packet.MotionEx,
                packet.Loop,
                packet.MotionOption,
                packet.TickCount);
        }

        [HandlerAction(PacketType.REVIVAL_TO_LODESTAR)]
        public void OnRevivalToLodestar(IWorldServerClient serverClient, INetPacketStream _)
        {
            if (!serverClient.Player.IsDead)
            {
                _logger.LogWarning($"Player '{serverClient.Player.Object.Name}' tried to revival to lodestar without being dead.");
                return;
            }

            _deathSystem.ResurectLodelight(serverClient.Player);
        }
    }
}
