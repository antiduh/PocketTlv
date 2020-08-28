using System;
using PocketTLV.ClassLib;

namespace PocketTLV.Primitives
{
    /// <summary>
    /// Stores a decimal value as a TLV tag.
    /// </summary>
    public class DecimalTag : ITag
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DecimalTag"/> class with default values.
        /// </summary>
        public DecimalTag()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DecimalTag"/> class.
        /// </summary>
        /// <param name="value">The decimal value to store.</param>
        public DecimalTag( decimal value )
        {
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DecimalTag"/> class.
        /// </summary>
        /// <param name="fieldId">The TLV field ID to associate to the tag.</param>
        /// <param name="value">The decimal value to store.</param>
        public DecimalTag( int fieldId, decimal value )
        {
            this.FieldId = fieldId;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the decimal value stored by the tag.
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Gets or sets the TLV field ID that the tag represents.
        /// </summary>
        public int FieldId { get; set; }

        /// <summary>
        /// Converts the <see cref="DecimalTag>"/>'s value to a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Value.ToString();
        }

        /// <summary>
        /// Returns whether this <see cref="DecimalTag"/> is equal to the provided object.
        /// </summary>
        /// <param name="other">The object to compare to.</param>
        /// <returns>True if the other object has the same value as this object.</returns>
        public override bool Equals( object other )
        {
            return Equals( other as DecimalTag );
        }

        /// <summary>
        /// Compares two <see cref="DecimalTag"/> objects.
        /// </summary>
        /// <param name="other">The tag to compare to.</param>
        /// <returns>True if the tag object has the same value as this tag, false otherwise.</returns>
        public bool Equals( DecimalTag other )
        {
            return this == other;
        }

        /// <summary>
        /// Returns a hash value for a <see cref="DecimalTag"/> instance.
        /// </summary>
        /// <returns>An integer hash code.</returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        /// <summary>
        /// Converts a <see cref="DecimalTag"/> to a <see cref="decimal"/>.
        /// </summary>
        /// <param name="tag">The tag to convert.</param>
        public static implicit operator decimal( DecimalTag tag )
        {
            return tag.Value;
        }

        /// <summary>
        /// Compares two <see cref="DecimalTag"/> objects to determine if they contain the same value.
        /// </summary>
        /// <param name="left">The first tag to compare.</param>
        /// <param name="right">The second tag to compare.</param>
        /// <returns>True if the two tags contain equal values, false otherwise.</returns>
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

        /// <summary>
        /// Compares two <see cref="DecimalTag"/> objects to determine if they contain different values.
        /// </summary>
        /// <param name="left">The first tag to compare.</param>
        /// <param name="right">The second tag to compare.</param>
        /// <returns>True if the two tags do not contain equal values, false otherwise.</returns>
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