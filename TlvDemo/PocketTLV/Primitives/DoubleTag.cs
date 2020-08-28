using System;
using PocketTLV.ClassLib;

namespace PocketTLV.Primitives
{
    /// <summary>
    /// Stores a 64-bit double-precision floating point value as a TLV tag.
    /// </summary>
    public class DoubleTag : ITag
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleTag"/> class with default values.
        /// </summary>
        public DoubleTag()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleTag"/> class.
        /// </summary>
        /// <param name="value">The double value to store.</param>
        public DoubleTag( double value )
        {
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleTag"/> class.
        /// </summary>
        /// <param name="value">The double value to store.</param>
        public DoubleTag( int fieldId, double value )
        {
            this.FieldId = fieldId;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the double value stored by the tag.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the TLV field ID that the tag represents.
        /// </summary>
        public int FieldId { get; set; }

        /// <summary>
        /// Converts the <see cref="DoubleTag>"/>'s value to a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Value.ToString();
        }

        /// <summary>
        /// Returns whether this <see cref="DoubleTag"/> is equal to the provided object.
        /// </summary>
        /// <param name="other">The object to compare to.</param>
        /// <returns>True if the other object has the same value as this object, false otherwise.</returns>
        public override bool Equals( object other )
        {
            return Equals( other as DoubleTag );
        }

        /// <summary>
        /// Compares two <see cref="DoubleTag"/> objects.
        /// </summary>
        /// <param name="other">The tag to compare to.</param>
        /// <returns>True if the tag object has the same value as this tag, false otherwise.</returns>
        public bool Equals( DoubleTag other )
        {
            return this == other;
        }

        /// <summary>
        /// Returns a hash value for a <see cref="DoubleTag"/> instance.
        /// </summary>
        /// <returns>An integer hash code.</returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        /// <summary>
        /// Converts a <see cref="DoubleTag"/> to a <see cref="double"/>.
        /// </summary>
        /// <param name="tag">The tag to convert.</param>
        public static implicit operator double( DoubleTag tag )
        {
            return tag.Value;
        }

        /// <summary>
        /// Compares two <see cref="DoubleTag"/> objects to determine if they contain the same value.
        /// </summary>
        /// <param name="left">The first tag to compare.</param>
        /// <param name="right">The second tag to compare.</param>
        /// <returns>True if the two tags contain equal values, false otherwise.</returns>
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

        /// <summary>
        /// Compares two <see cref="DoubleTag"/> objects to determine if they contain different values.
        /// </summary>
        /// <param name="left">The first tag to compare.</param>
        /// <param name="right">The second tag to compare.</param>
        /// <returns>True if the two tags do not contain equal values, false otherwise.</returns>
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