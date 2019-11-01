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

        private static readonly Encoding FlyFFWriteStringEncoding = Encoding.GetEncoding(0);
        private static readonly Encoding FlyFFReadStringEncoding = Encoding.GetEncoding(1252);
        private short _mergedPacketCount;

        /// <inheritdoc />
        protected override Encoding StringReadEncoding => FlyFFReadStringEncoding;

        /// <inheritdoc />
        protected override Encoding StringWriteEncoding => FlyFFWriteStringEncoding;

        /// <summary>
        /// Gets the FFPacket buffer.
        /// </summary>
        public override byte[] Buffer => this.BuildPacketBuffer();

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
            this.WriteHeader(packetHeader);
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
        public void WriteHeader(object packetHeader) => this.Write((uint)packetHeader);

        /// <summary>
        /// Builds the packet buffer.
        /// </summary>
        /// <returns></returns>
        private byte[] BuildPacketBuffer()
        {
            long oldPointer = this.Position;

            this.Seek(1, SeekOrigin.Begin);
            base.Write((int)this.Length - 5);
            this.Seek(oldPointer, SeekOrigin.Begin);

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

            if (this._mergedPacketCount == 0)
            {
                this.Write((int)mainCommand);
                this.Write(0);
                this.Write(++this._mergedPacketCount);
            }
            else
            {
                this.Seek(13, SeekOrigin.Begin);
                this.Write(++this._mergedPacketCount);
                this.Seek(0, SeekOrigin.End);
            }

            this.Write(moverId);
            this.Write((short)packet);
        }

        /// <summary>
        /// Start a new merged packet.
        /// </summary>
        /// <param name="moverId"></param>
        /// <param name="command"></param>
        public void StartNewMergedPacket(uint moverId, object command) => this.StartNewMergedPacket(moverId, command, 0xFFFFFF00);
    }
}
