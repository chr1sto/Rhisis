using System;

namespace Rhisis.Core.Cryptography
{
    public sealed class Crc32
    {
        private static readonly uint[] _crc32Table = new uint[256];

        /// <summary>
        /// Initialize the <see cref="Crc32"/> static instance.
        /// </summary>
        static Crc32()
        {
            InitializeCrc32Table();
        }

        /// <summary>
        /// Computes Crc32 checksum.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static uint Crc32ComputeChecksum(byte[] input)
        {
            uint crc = 0xffffffff;

            for (int i = 0; i < input.Length; ++i)
            {
                var index = (byte)(((crc) & 0xff) ^ input[i]);
                crc = ((crc >> 8) ^ _crc32Table[index]);
            }

            return ~crc;
        }

        /// <summary>
        /// Comptures Crc32 checksum as a byte array.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] Crc32ComputeChecksumBytes(byte[] input) => BitConverter.GetBytes(Crc32ComputeChecksum(input));

        /// <summary>
        /// Initialize the Crc32 table.
        /// </summary>
        private static void InitializeCrc32Table()
        {
            uint poly = 0xedb88320;
            uint temp = 0;

            for (uint i = 0; i < _crc32Table.Length; ++i)
            {
                temp = i;

                for (int j = 8; j > 0; --j)
                {
                    if ((temp & 1) == 1)
                        temp = ((temp >> 1) ^ poly);
                    else
                        temp >>= 1;
                }

                _crc32Table[i] = temp;
            }
        }
    }
}
