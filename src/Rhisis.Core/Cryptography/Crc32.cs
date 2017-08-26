using System;

namespace Rhisis.Core.Cryptography
{
    public static class Crc32
    {
        private static readonly uint[] _table;

        static Crc32()
        {
            _table = new uint[256];
            uint poly = 0xedb88320;
            uint temp = 0;

            for (uint i = 0; i < _table.Length; ++i)
            {
                temp = i;

                for (int j = 8; j > 0; --j)
                {
                    if ((temp & 1) == 1)
                        temp = ((temp >> 1) ^ poly);
                    else
                        temp >>= 1;
                }

                _table[i] = temp;
            }
        }

        public static uint ComputeChecksum(byte[] bytes)
        {
            uint crc = 0xffffffff;

            for (int i = 0; i < bytes.Length; ++i)
            {
                var index = (byte)(((crc) & 0xff) ^ bytes[i]);
                crc = ((crc >> 8) ^ _table[index]);
            }

            return ~crc;
        }

        public static byte[] ComputeChecksumBytes(byte[] bytes)
        {
            return BitConverter.GetBytes(ComputeChecksum(bytes));
        }
    }
}