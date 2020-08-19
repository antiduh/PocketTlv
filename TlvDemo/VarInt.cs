using System;
using System.Runtime.InteropServices;

namespace TlvDemo
{
    public struct VarInt
    {
        private ulong rawValue;

        /// <summary>
        /// The maximum number of bytes that a VarInt could be encoded as.
        /// </summary>
        public const int MaxWireLength = 10;

        public VarInt( long value )
        {
            this.rawValue = (ulong)value;
        }

        public int ComputeLength()
        {
            int length = 0;

            // Each output byte stores 7 bits of value. So count how many times we have to remove 7
            // bits before we have a zero value.

            for( ulong current = this.rawValue; current != 0; current >>= 7 )
            {
                length++;
            }

            return length;
        }

        public int Write( byte[] buffer, int position )
        {
            int numWritten = 0;
            ulong current = this.rawValue;
            byte place;

            buffer[position] = 0;

            while( current != 0 )
            {
                // Read off the lowest 7 bits from current into place.
                place = (byte)(current & 0b0111_1111);
                current >>= 7;

                // If current no longer has value, then this is the last byte.
                if( current != 0 )
                {
                    place |= 0b1000_0000;
                }

                // Store the place, and advance our position index.
                buffer[position] = place;
                position++;
                numWritten++;
            }

            return numWritten;
        }

        public int Read( byte[] buffer, int position )
        {
            return Read( buffer, position, buffer.Length );
        }

        public int Read( byte[] buffer, int position, int maxReadLen )
        {
            int numRead = 0;

            int wireLength = -1;

            // Search the buffer from the starting position, with a maximum search of the length of
            // the buffer. If we find a stop-byte in that time, save how many total bytes we found.
            // If we don't, we'll stop without ever initializing the 

            int maxSearchLength = Math.Min( Math.Min( buffer.Length, MaxWireLength ), maxReadLen );

            for( int i = position; i < maxSearchLength; i++ )
            {
                if( ( buffer[i] & 0b1000_0000 ) == 0 )
                {
                    // Off-by-one check: If we start at position 42, find a terminator at position
                    // 42, then we found 1 byte of match.
                    wireLength = i - position + 1;
                    break;
                }
            }

            if( wireLength < 1 )
            {
                throw new ArgumentOutOfRangeException(
                    "No valid VarInt was found before the end of the data - terminator byte not found."
                );
            }

            int terminator = position + wireLength - 1;
            ulong result = 0;
            this.rawValue = 0L;

            while( terminator >= position )
            {
                byte place = buffer[terminator];
                terminator--;

                result <<= 7;
                result |= (byte)( place & 0b0111_1111 );

                numRead++;
            }

            this.rawValue = result;
            
            return numRead;
        }

        public static implicit operator VarInt( short value )
        {
            return new VarInt( value );
        }

        public static implicit operator VarInt( int value )
        {
            return new VarInt( value );
        }

        public static implicit operator int( VarInt value )
        {
            return (int)value.rawValue;
        }

        public static implicit operator VarInt( long value )
        {
            return new VarInt( value );
        }
        
        public static implicit operator long( VarInt value )
        {
            return (long)value.rawValue;
        }

        public static implicit operator VarInt( ulong value )
        {
            return new VarInt() { rawValue = value };
        }

        public static bool operator ==( VarInt left, VarInt right )
        {
            return left.rawValue == right.rawValue;
        }

        public static bool operator !=( VarInt left, VarInt right )
        {
            return left.rawValue != right.rawValue;
        }

        public static bool operator ==( VarInt left, long right )
        {
            return left.rawValue == (ulong)right;
        }

        public static bool operator !=( VarInt left, long right )
        {
            return left.rawValue != (ulong)right;
        }

        public override bool Equals( object other )
        {
            if( other is VarInt otherVarInt )
            {
                return this.Equals( otherVarInt );
            }
            else
            {
                return false;
            }
        }

        public bool Equals( VarInt other )
        {
            return other.rawValue == this.rawValue;
        }

        public override int GetHashCode()
        {
            return this.rawValue.GetHashCode();
        }

        public override string ToString()
        {
            return this.rawValue.ToString();
        }
    }
}