using Rhisis.Network.Packets;
using Rhisis.Network.Packets.World;
using Rhisis.World.Client;
using Rhisis.World.Systems.Customization;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.World.Handlers
{
    /// <summary>
    /// Handles requests related to the player customization system.
    /// </summary>
    [Handler]
    public class CustomizationHandler
    {
        private readonly ICustomizationSystem _customizationSystem;

        /// <summary>
        /// Creates a new <see cref="CustomizationHandler"/> instance.
        /// </summary>
        /// <param name="customizationSystem">Customization system.</param>
        public CustomizationHandler(ICustomizationSystem customizationSystem)
        {
            _customizationSystem = customizationSystem;
        }

        /// <summary>
        /// Changes the face of a player.
        /// </summary>
        /// <param name="serverClient"></param>
        /// <param name="packet"></param>
        [HandlerAction(PacketType.CHANGEFACE)]
        public void OnChangeFace(IWorldServerClient serverClient, ChangeFacePacket packet)
        {
            _customizationSystem.ChangeFace(serverClient.Player, packet.ObjectId, packet.FaceNumber, packet.UseCoupon);
        }

        /// <summary>
        /// Changes the hair and color of a player.
        /// </summary>
        /// <param name="serverClient"></param>
        /// <param name="packet"></param>
        [HandlerAction(PacketType.SET_HAIR)]
        public void OnSetHair(IWorldServerClient serverClient, SetHairPacket packet)
        {
            _customizationSystem.ChangeHair(serverClient.Player, packet.HairId, packet.R, packet.G, packet.B, packet.UseCoupon);
        }
    }
}
