using Sylver.Network.Data;
using System.IO;
using System.Text;

namespace Rhisis.Network
{
    /// <summary>
    /// Represents a FlyFF packet.
    /// </summary>
    public class FFPacket : NetPacketStream
    {
        public const byte Header = 0x5E;
        public const uint NullId = 0xFFFFFFFF;

        private static readonly Encoding FlyFFWriteStringEncoding = Encoding.GetEncoding(1252);
        private static readonly Encoding FlyFFReadStringEncoding = Encoding.GetEncoding(1252);
        private short _mergedPacketCount;

        /// <inheritdoc />
        protected override Encoding ReadEncoding => FlyFFReadStringEncoding;

        /// <inheritdoc />
        protected override Encoding WriteEncoding => FlyFFWriteStringEncoding;

        /// <summary>
        /// Gets the FFPacket buffer.
        /// </summary>
        public override byte[] Buffer => BuildPacketBuffer();

        /// <summary>
        /// Creates a new FFPacket in write-only mode.
        /// </summary>
        public FFPacket()
        {
            base.Write(Header);
            base.Write(0);
        }

        /// <summary>
        /// Creates a new FFPacket with a header.
        /// </summary>
        /// <param name="packetHeader"></param>
        public FFPacket(object packetHeader)
            : this()
        {
            WriteHeader(packetHeader);
        }

        /// <summary>
        /// Creates a new FFPacket in read-only mode.
        /// </summary>
        /// <param name="buffer"></param>
        public FFPacket(byte[] buffer)
            : base(buffer)
        {
        }

        /// <summary>
        /// Write packet header.
        /// </summary>
        /// <param name="packetHeader">FFPacket header</param>
        public void WriteHeader(object packetHeader) => Write((uint)packetHeader);

        /// <summary>
        /// Builds the packet buffer.
        /// </summary>
        /// <returns></returns>
        private byte[] BuildPacketBuffer()
        {
            long oldPointer = Position;

            Seek(1, SeekOrigin.Begin);
            base.Write((int)Length - 5);
            Seek(oldPointer, SeekOrigin.Begin);

            return base.Buffer;
        }

        /// <summary>
        /// Start a new merged packet.
        /// </summary>
        /// <param name="moverId"></param>
        /// <param name="command"></param>
        /// <param name="mainCommand"></param>
        public void StartNewMergedPacket(uint moverId, object command, uint mainCommand)
        {
            var packet = (uint)command;

            if (_mergedPacketCount == 0)
            {
                Write((int)mainCommand);
                Write(0);
                Write(++_mergedPacketCount);
            }
            else
            {
                Seek(13, SeekOrigin.Begin);
                Write(++_mergedPacketCount);
                Seek(0, SeekOrigin.End);
            }

            Write(moverId);
            Write((short)packet);
        }

        /// <summary>
        /// Start a new merged packet.
        /// </summary>
        /// <param name="moverId"></param>
        /// <param name="command"></param>
        public void StartNewMergedPacket(uint moverId, object command) => StartNewMergedPacket(moverId, command, 0xFFFFFF00);
    }
}
