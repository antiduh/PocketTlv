using System;
using PocketTLV.ClassLib;

namespace PocketTLV.Primitives
{
    /// <summary>
    /// Stores a 32-bit signed integer as a TLV tag.
    /// </summary>
    public class IntTag : ITag
    {
        public IntTag()
        {
        }

        public IntTag( int value )
        {
            this.Value = value;
        }

        public IntTag( int fieldId, int value )
        {
            this.FieldId = fieldId;
            this.Value = value;
        }

        public int Value { get; set; }

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
            return Equals( other as IntTag );
        }

        public bool Equals( IntTag other )
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public static implicit operator int( IntTag tag )
        {
            return tag.Value;
        }

        public static bool operator ==( IntTag left, IntTag right )
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

        public static bool operator !=( IntTag left, IntTag right )
        {
            return !( left == right );
        }

        // --- ITag implementation ---

        WireType ITag.WireType => WireType.Int;

        int ITag.ComputeLength()
        {
            return 4;
        }

        void ITag.ReadValue( byte[] buffer, int position, int length )
        {
            if( length != 4 )
            {
                throw new ArgumentOutOfRangeException( nameof( length ), "length must always be 4 bytes." );
            }

            this.Value = DataConverter.ReadIntLE( buffer, position );
        }

        void ITag.WriteValue( byte[] buffer, int position )
        {
            DataConverter.WriteIntLE( this.Value, buffer, position );
        }
    }
}