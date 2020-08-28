using System;
using PocketTLV.ClassLib;

namespace PocketTLV.Primitives
{
    /// <summary>
    /// Stores a 32-bit signed integer as a TLV tag.
    /// </summary>
    public class IntTag : ITag
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntTag"/> class with default values.
        /// </summary>
        public IntTag()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntTag"/> class.
        /// </summary>
        /// <param name="value">The integer value to store.</param>
        public IntTag( int value )
        {
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntTag"/> class.
        /// </summary>
        /// <param name="value">The integer value to store.</param>
        public IntTag( int fieldId, int value )
        {
            this.FieldId = fieldId;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the integer value stored by the tag.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Gets or sets the TLV field ID that the tag represents.
        /// </summary>
        public int FieldId { get; set; }

        /// <summary>
        /// Converts the <see cref="IntTag>"/>'s value to a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Value.ToString();
        }

        /// <summary>
        /// Returns whether this <see cref="IntTag"/> is equal to the provided object.
        /// </summary>
        /// <param name="other">The object to compare to.</param>
        /// <returns>True if the other object has the same value as this object, false otherwise.</returns>
        public override bool Equals( object other )
        {
            return Equals( other as IntTag );
        }

        /// <summary>
        /// Compares two <see cref="IntTag"/> objects.
        /// </summary>
        /// <param name="other">The tag to compare to.</param>
        /// <returns>True if the tag object has the same value as this tag, false otherwise.</returns>
        public bool Equals( IntTag other )
        {
            return this == other;
        }

        /// <summary>
        /// Returns a hash value for a <see cref="IntTag"/> instance.
        /// </summary>
        /// <returns>An integer hash code.</returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        /// <summary>
        /// Converts a <see cref="IntTag"/> to a <see cref="int"/>.
        /// </summary>
        /// <param name="tag">The tag to convert.</param>
        public static implicit operator int( IntTag tag )
        {
            return tag.Value;
        }

        /// <summary>
        /// Compares two <see cref="IntTag"/> objects to determine if they contain the same value.
        /// </summary>
        /// <param name="left">The first tag to compare.</param>
        /// <param name="right">The second tag to compare.</param>
        /// <returns>True if the two tags contain equal values, false otherwise.</returns>
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

        /// <summary>
        /// Compares two <see cref="IntTag"/> objects to determine if they contain different values.
        /// </summary>
        /// <param name="left">The first tag to compare.</param>
        /// <param name="right">The second tag to compare.</param>
        /// <returns>True if the two tags do not contain equal values, false otherwise.</returns>
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