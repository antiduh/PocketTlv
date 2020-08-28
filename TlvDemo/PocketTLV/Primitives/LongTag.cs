using System;
using PocketTLV.ClassLib;

namespace PocketTLV.Primitives
{
    public class LongTag : ITag
    {
        public LongTag()
        {
        }

        public LongTag( long value )
        {
            this.Value = value;
        }

        public LongTag( int fieldId, long value )
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
            return Equals( other as LongTag );
        }

        public bool Equals( LongTag other )
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public static implicit operator long( LongTag tag )
        {
            return tag.Value;
        }

        public static bool operator ==( LongTag left, LongTag right )
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

        public static bool operator !=( LongTag left, LongTag right )
        {
            return !( left == right );
        }

        // --- ITag implementation ---

        WireType ITag.WireType => WireType.Long;

        int ITag.ComputeLength()
        {
            return sizeof( long );
        }

        void ITag.ReadValue( byte[] buffer, int position, int length )
        {
            if( length != sizeof( long ) )
            {
                throw new InvalidOperationException( $"{nameof( length )} must be {sizeof( long )}." );
            }

            this.Value = DataConverter.ReadLongLE( buffer, position );
        }

        void ITag.WriteValue( byte[] buffer, int position )
        {
            DataConverter.WriteLongLE( this.Value, buffer, position );
        }
    }
}