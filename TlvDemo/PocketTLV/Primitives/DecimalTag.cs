using System;
using PocketTLV.ClassLib;

namespace PocketTLV.Primitives
{
    /// <summary>
    /// Stores a decimal value as a TLV tag.
    /// </summary>
    public class DecimalTag : ITag
    {
        public DecimalTag()
        {
        }

        public DecimalTag( decimal value )
        {
            this.Value = value;
        }

        public DecimalTag( int fieldId, decimal value )
        {
            this.FieldId = fieldId;
            this.Value = value;
        }

        public decimal Value { get; set; }

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
            return Equals( other as DecimalTag );
        }

        public bool Equals( DecimalTag other )
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public static implicit operator decimal( DecimalTag tag )
        {
            return tag.Value;
        }

        public static bool operator ==( DecimalTag left, DecimalTag right )
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

        public static bool operator !=( DecimalTag left, DecimalTag right )
        {
            return !( left == right );
        }

        // --- ITag implementation ---

        WireType ITag.WireType => WireType.Decimal;

        int ITag.ComputeLength()
        {
            return 16;
        }

        void ITag.ReadValue( byte[] buffer, int position, int length )
        {
            if( length != 16 )
            {
                throw new ArgumentOutOfRangeException( nameof( length ), $"Length must always be 16 bytes." );
            }

            this.Value = DataConverter.ReadDecimalLE( buffer, position );
        }

        void ITag.WriteValue( byte[] buffer, int position )
        {
            DataConverter.WriteDecimalLE( this.Value, buffer, position );
        }
    }
}