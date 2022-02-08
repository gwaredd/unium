// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT

using System;

namespace gw.proto.utils
{
    /// <summary>
    /// A utility library for endian operations
    /// </summary>

    public static class Endian
    {
        public static ushort Swap( ushort val )
        {
            unchecked
            {
                return (ushort) ( ( ( val & 0xFF00U ) >> 8 ) | ( ( val & 0x00FFU ) << 8 ) );
            }
        }

        public static uint Swap( uint val )
        {
            val = ( val >> 16 ) | ( val << 16 );                                                        // swap adjacent 16-bit blocks
            val = ( ( val & 0xFF00FF00U ) >> 8 ) | ( ( val & 0x00FF00FFU ) << 8 );                      // swap adjacent 8-bit blocks
            return val;
        }

        public static int Swap( int val )
        {
            unchecked
            {
                return (int) Swap( (uint) val );
            }
        }

        public static ulong Swap( ulong val )
        {
            val = ( val >> 32 ) | ( val << 32 );                                                        // swap adjacent 32-bit blocks
            val = ( ( val & 0xFFFF0000FFFF0000U ) >> 16 ) | ( ( val & 0x0000FFFF0000FFFFU ) << 16 );    // swap adjacent 16-bit blocks
            val = ( ( val & 0xFF00FF00FF00FF00U ) >> 8 ) | ( ( val & 0x00FF00FF00FF00FFU ) << 8 );      // swap adjacent 8-bit blocks
            return val;
        }


        public static ushort NetworkToHost( ushort val )
        {
            return BitConverter.IsLittleEndian ? Swap( val ) : val;
        }

        public static ushort HostToNetwork( ushort val )
        {
            return BitConverter.IsLittleEndian ? Swap( val ) : val;
        }

        public static ulong NetworkToHost( ulong val )
        {
            return BitConverter.IsLittleEndian ? Swap( val ) : val;
        }

        public static ulong HostToNetwork( ulong val )
        {
            return BitConverter.IsLittleEndian ? Swap( val ) : val;
        }
    }
}
