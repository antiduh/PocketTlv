using System;
using PocketTLV.ClassLib;

namespace PocketTLV.Primitives
{
    /// <summary>
    /// Stores a 64-bit signed integer as a TLV tag.
    /// </summary>
    public class LongTag : ITag
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LongTag"/> class with default values.
        /// </summary>
        public LongTag()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LongTag"/> class.
        /// </summary>
        /// <param name="value">The long value to store.</param>
        public LongTag( long value )
        {
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LongTag"/> class.
        /// </summary>
        /// <param name="value">The long value to store.</param>
        public LongTag( int fieldId, long value )
        {
            this.FieldId = fieldId;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the long value stored by the tag.
        /// </summary>
        public long Value { get; set; }

        /// <summary>
        /// Gets or sets the TLV field ID that the tag represents.
        /// </summary>
        public int FieldId { get; set; }

        /// <summary>
        /// Converts the <see cref="LongTag>"/>'s value to a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Value.ToString();
        }

        /// <summary>
        /// Returns whether this <see cref="LongTag"/> is equal to the provided object.
        /// </summary>
        /// <param name="other">The object to compare to.</param>
        /// <returns>True if the other object has the same value as this object, false otherwise.</returns>
        public override bool Equals( object other )
        {
            return Equals( other as LongTag );
        }

        /// <summary>
        /// Compares two <see cref="LongTag"/> objects.
        /// </summary>
        /// <param name="other">The tag to compare to.</param>
        /// <returns>True if the tag object has the same value as this tag, false otherwise.</returns>
        public bool Equals( LongTag other )
        {
            return this == other;
        }

        /// <summary>
        /// Returns a hash value for a <see cref="LongTag"/> instance.
        /// </summary>
        /// <returns>An integer hash code.</returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        /// <summary>
        /// Converts a <see cref="LongTag"/> to a <see cref="long"/>.
        /// </summary>
        /// <param name="tag">The tag to convert.</param>
        public static implicit operator long( LongTag tag )
        {
            return tag.Value;
        }

        /// <summary>
        /// Compares two <see cref="LongTag"/> objects to determine if they contain the same value.
        /// </summary>
        /// <param name="left">The first tag to compare.</param>
        /// <param name="right">The second tag to compare.</param>
        /// <returns>True if the two tags contain equal values, false otherwise.</returns>
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

        /// <summary>
        /// Compares two <see cref="LongTag"/> objects to determine if they contain different values.
        /// </summary>
        /// <param name="left">The first tag to compare.</param>
        /// <param name="right">The second tag to compare.</param>
        /// <returns>True if the two tags do not contain equal values, false otherwise.</returns>
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