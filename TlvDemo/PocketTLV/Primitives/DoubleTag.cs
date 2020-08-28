using System;
using PocketTLV.ClassLib;

namespace PocketTLV.Primitives
{
    /// <summary>
    /// Stores a 64-bit double-precision floating point value as a TLV tag.
    /// </summary>
    public class DoubleTag : ITag
    {
        public DoubleTag()
        {
        }

        public DoubleTag( double value )
        {
            this.Value = value;
        }

        public DoubleTag( int fieldId, double value )
        {
            this.FieldId = fieldId;
            this.Value = value;
        }

        public double Value { get; set; }

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
            return Equals( other as DoubleTag );
        }

        public bool Equals( DoubleTag other )
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public static implicit operator double( DoubleTag tag )
        {
            return tag.Value;
        }

        public static bool operator ==( DoubleTag left, DoubleTag right )
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

        public static bool operator !=( DoubleTag left, DoubleTag right )
        {
            return !( left == right );
        }

        // --- ITag implementation ---

        WireType ITag.WireType => WireType.Double;

        int ITag.ComputeLength()
        {
            return 8;
        }

        void ITag.ReadValue( byte[] buffer, int position, int length )
        {
            if( length != 8 )
            {
                throw new ArgumentOutOfRangeException( nameof( length ), "must always be 8 bytes." );
            }

            this.Value = DataConverter.ReadDoubleLE( buffer, position );
        }

        void ITag.WriteValue( byte[] buffer, int position )
        {
            DataConverter.WriteDoubleLE( this.Value, buffer, position );
        }
    }
}