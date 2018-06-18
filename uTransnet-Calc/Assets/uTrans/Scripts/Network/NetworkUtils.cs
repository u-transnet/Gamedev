using System;
using System.IO;

namespace uTrans.Network
{
    public class NetworkUtils
    {
        /// <summary>
        /// Reads a varint encoded integer. An exception is thrown if the data is not all available.
        /// </summary>
        public static int DirectReadVarintInt32(Stream source)
        {
            uint val;
            int bytes = TryReadUInt32Variant(source, out val);
            if (bytes <= 0) throw new EndOfStreamException(null);
            return (int) val;
        }

        /// <returns>The number of bytes consumed; 0 if no data available</returns>
        private static int TryReadUInt32Variant(Stream source, out uint value)
        {
            value = 0;
            int b = source.ReadByte();
            if (b < 0) { return 0; }
            value = (uint)b;
            if ((value & 0x80) == 0) { return 1; }
            value &= 0x7F;

            b = source.ReadByte();
            if (b < 0) throw new EndOfStreamException(null);
            value |= ((uint)b & 0x7F) << 7;
            if ((b & 0x80) == 0) return 2;

            b = source.ReadByte();
            if (b < 0) throw new EndOfStreamException(null);
            value |= ((uint)b & 0x7F) << 14;
            if ((b & 0x80) == 0) return 3;

            b = source.ReadByte();
            if (b < 0) throw new EndOfStreamException(null);
            value |= ((uint)b & 0x7F) << 21;
            if ((b & 0x80) == 0) return 4;

            b = source.ReadByte();
            if (b < 0) throw new EndOfStreamException(null);
            value |= (uint)b << 28; // can only use 4 bits from this chunk
            if ((b & 0xF0) == 0) return 5;

            throw new OverflowException();
        }


    }
}