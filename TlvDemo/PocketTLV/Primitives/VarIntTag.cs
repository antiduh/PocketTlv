using System;

namespace PocketTLV.Primitives
{
    public class VarIntTag : ITag
    {
        public VarIntTag()
        {
        }

        public VarIntTag( long value )
        {
            this.Value = value;
        }

        public VarIntTag( int fieldId, long value )
        {
            this.FieldId = fieldId;
            this.Value = value;
        }

        public long Value { get; set; }

        /// <summary>
        /// Gets or sets the TLV field ID that the tag represents.
        /// </summary>
        public int FieldId { get; set; }

        public override string ToString()
        {
            return this.Value.ToString();
        }

        public override bool Equals( object other )
        {
            return Equals( other as VarIntTag );
        }

        public bool Equals( VarIntTag other )
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public static implicit operator long( VarIntTag tag )
        {
            return tag.Value;
        }

        public static bool operator ==( VarIntTag left, VarIntTag right )
        {
            if( left is null )
            {
                return right is null;
            }
            else if( right is null )
            {
                return false;
            }
            else
            {
                return left.Value == right.Value;
            }
        }

        public static bool operator !=( VarIntTag left, VarIntTag right )
        {
            return !( left == right );
        }

        // --- ITag implementation ---

        WireType ITag.WireType => WireType.VarInt;

        public int ComputeLength()
        {
            int numBytes = 0;

            for( long copy = this.Value; copy > 0; copy >>= 8 )
            {
                numBytes++;
            }

            return numBytes;
        }

        void ITag.ReadValue( byte[] buffer, int position, int length )
        {
            this.Value = 0;

            for( int i = 0; i < length; i++ )
            {
                this.Value <<= 8;
                this.Value |= buffer[position + i];
            }
        }

        void ITag.WriteValue( byte[] buffer, int position )
        {
            long copy = this.Value;

            for( int i = 0; i < ComputeLength(); i++ )
            {
                buffer[position + i] = (byte)copy;
                copy >>= 8;
            }
        }
    }
}