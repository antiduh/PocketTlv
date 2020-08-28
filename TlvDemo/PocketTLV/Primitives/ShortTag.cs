using System;
using PocketTLV.ClassLib;

namespace PocketTLV.Primitives
{
    /// <summary>
    /// Stores a 16-bit signed integer as a TLV tag.
    /// </summary>
    public class ShortTag : ITag
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShortTag"/> class with default values.
        /// </summary>
        public ShortTag()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShortTag"/> class.
        /// </summary>
        /// <param name="value">The short value to store.</param>
        public ShortTag( short value )
        {
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShortTag"/> class.
        /// </summary>
        /// <param name="fieldId">The TLV field ID that the tag represents.</param>
        /// <param name="value">The short value to store.</param>
        public ShortTag( int fieldId, short value )
        {
            this.FieldId = fieldId;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the short value stored by the tag.
        /// </summary>
        public short Value { get; set; }

        /// <summary>
        /// Gets or sets the TLV field ID that the tag represents.
        /// </summary>
        public int FieldId { get; set; }

        /// <summary>
        /// Converts the <see cref="ShortTag>"/>'s value to a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Value.ToString();
        }

        /// <summary>
        /// Returns whether this <see cref="ShortTag"/> is equal to the provided object.
        /// </summary>
        /// <param name="other">The object to compare to.</param>
        /// <returns>True if the other object has the same value as this object, false otherwise.</returns>
        public override bool Equals( object other )
        {
            return Equals( other as ShortTag );
        }

        /// <summary>
        /// Compares two <see cref="ShortTag"/> objects.
        /// </summary>
        /// <param name="other">The tag to compare to.</param>
        /// <returns>True if the tag object has the same value as this tag, false otherwise.</returns>
        public bool Equals( ShortTag other )
        {
            return this == other;
        }

        /// <summary>
        /// Returns a hash value for a <see cref="ShortTag"/> instance.
        /// </summary>
        /// <returns>An integer hash code.</returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        /// <summary>
        /// Converts a <see cref="ShortTag"/> to a <see cref="short"/>.
        /// </summary>
        /// <param name="tag">The tag to convert.</param>
        public static implicit operator short( ShortTag tag )
        {
            return tag.Value;
        }

        /// <summary>
        /// Compares two <see cref="ShortTag"/> objects to determine if they contain the same value.
        /// </summary>
        /// <param name="left">The first tag to compare.</param>
        /// <param name="right">The second tag to compare.</param>
        /// <returns>True if the two tags contain equal values, false otherwise.</returns>
        public static bool operator ==( ShortTag left, ShortTag right )
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
        /// Compares two <see cref="ShortTag"/> objects to determine if they contain different values.
        /// </summary>
        /// <param name="left">The first tag to compare.</param>
        /// <param name="right">The second tag to compare.</param>
        /// <returns>True if the two tags do not contain equal values, false otherwise.</returns>
        public static bool operator !=( ShortTag left, ShortTag right )
        {
            return !( left == right );
        }

        // --- ITag implementation ---

        WireType ITag.WireType => WireType.Short;

        int ITag.ComputeLength()
        {
            return 2;
        }

        void ITag.ReadValue( byte[] buffer, int position, int length )
        {
            if( length != 2 )
            {
                throw new InvalidOperationException( $"{nameof( length )} must be {sizeof( short )}." );
            }

            this.Value = DataConverter.ReadShortLE( buffer, position );
        }

        void ITag.WriteValue( byte[] buffer, int position )
        {
            DataConverter.WriteShortLE( this.Value, buffer, position );
        }
    }
}