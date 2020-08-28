using System;
using System.Text;

namespace PocketTLV.Primitives
{
    /// <summary>
    /// Stores a string as a TLV tag using UTF-8 encoding.
    /// </summary>
    public class StringTag : ITag
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringTag"/> class with default values.
        /// </summary>
        public StringTag()
            : this( "" )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringTag"/> class.
        /// </summary>
        /// <param name="value">The string value to store.</param>
        public StringTag( string value )
        {
            if( value == null )
            {
                throw new ArgumentNullException( nameof( value ) );
            }

            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringTag"/> class.
        /// </summary>
        /// <param name="fieldId">The TLV field ID that the tag represents.</param>
        /// <param name="value">The string value to store.</param>
        public StringTag( int fieldId, string value )
        {
            this.FieldId = fieldId;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the double value stored by the tag.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the TLV field ID that the tag represents.
        /// </summary>
        public int FieldId { get; set; }

        /// <summary>
        /// Converts the <see cref="StringTag>"/>'s value to a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Value;
        }

        /// <summary>
        /// Returns whether this <see cref="StringTag"/> is equal to the provided object.
        /// </summary>
        /// <param name="other">The object to compare to.</param>
        /// <returns>True if the other object has the same value as this object, false otherwise.</returns>
        public override bool Equals( object other )
        {
            return Equals( other as StringTag );
        }

        /// <summary>
        /// Compares two <see cref="StringTag"/> objects.
        /// </summary>
        /// <param name="other">The tag to compare to.</param>
        /// <returns>True if the tag object has the same value as this tag, false otherwise.</returns>
        public bool Equals( StringTag other )
        {
            return this == other;
        }

        /// <summary>
        /// Returns a hash value for a <see cref="StringTag"/> instance.
        /// </summary>
        /// <returns>An integer hash code.</returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        /// <summary>
        /// Converts a <see cref="StringTag"/> to a <see cref="double"/>.
        /// </summary>
        /// <param name="tag">The tag to convert.</param>
        public static implicit operator string( StringTag tag )
        {
            return tag.Value;
        }

        /// <summary>
        /// Compares two <see cref="StringTag"/> objects to determine if they contain the same value.
        /// </summary>
        /// <param name="left">The first tag to compare.</param>
        /// <param name="right">The second tag to compare.</param>
        /// <returns>True if the two tags contain equal values, false otherwise.</returns>
        public static bool operator ==( StringTag left, StringTag right )
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
        /// Compares two <see cref="StringTag"/> objects to determine if they contain different values.
        /// </summary>
        /// <param name="left">The first tag to compare.</param>
        /// <param name="right">The second tag to compare.</param>
        /// <returns>True if the two tags do not contain equal values, false otherwise.</returns>
        public static bool operator !=( StringTag left, StringTag right )
        {
            return !( left == right );
        }

        // --- ITag implementation ---

        WireType ITag.WireType => WireType.String;

        int ITag.ComputeLength()
        {
            return Encoding.UTF8.GetByteCount( this.Value );
        }

        void ITag.ReadValue( byte[] buffer, int position, int length )
        {
            this.Value = Encoding.UTF8.GetString( buffer, position, length );
        }

        void ITag.WriteValue( byte[] buffer, int position )
        {
            Encoding.UTF8.GetBytes( this.Value, 0, this.Value.Length, buffer, position );
        }
    }
}