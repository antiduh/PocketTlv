using System;

namespace PocketTLV.Primitives
{
    /// <summary>
    /// Stores a boolean value as a TLV tag.
    /// </summary>
    public class BoolTag : ITag
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoolTag"/> class with default values.
        /// </summary>
        public BoolTag()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoolTag"/> class.
        /// </summary>
        /// <param name="value"></param>
        public BoolTag( bool value )
            : this( 0, value )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoolTag"/> class.
        /// </summary>
        /// <param name="fieldId">The field ID that the tag represents.</param>
        /// <param name="value">The boolean value to store.</param>
        public BoolTag( int fieldId, bool value )
        {
            this.FieldId = fieldId;
            this.Value = value;
        }

        /// <summary>
        /// Gets the boolean value.
        /// </summary>
        public bool Value { get; private set; }

        /// <summary>
        /// Gets or sets the field ID the tag represents.
        /// </summary>
        public int FieldId { get; set; }

        /// <summary>
        /// Returns whether this <see cref="BoolTag"/> is equal to the provided object.
        /// </summary>
        /// <param name="other">The object to compare to.</param>
        /// <returns>True if the other object has the same value as this object.</returns>
        public override bool Equals( object other )
        {
            return Equals( other as BoolTag );
        }

        /// <summary>
        /// Compares two <see cref="BoolTag"/> objects.
        /// </summary>
        /// <param name="other">The object to compare to.</param>
        /// <returns>True if the other object has the same value as this object.</returns>
        public bool Equals( BoolTag other )
        {
            return this == other;
        }

        /// <summary>
        /// Returns a hash value for a <see cref="BoolTag"/>.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        /// <summary>
        /// Converts a <see cref="BoolTag"/> to a <see cref="bool"/>.
        /// </summary>
        /// <param name="tag"></param>
        public static implicit operator bool( BoolTag tag )
        {
            return tag.Value;
        }

        /// <summary>
        /// Compares two <see cref="BoolTag"/> objects to determine if they contain the same value.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator==( BoolTag left, BoolTag right )
        {
            if( left is null )
            {
                return right is null;
            }
            else if( right is null )
            {
                return false;
            }

            return left.Value == right.Value;
        }

        /// <summary>
        /// Compares two <see cref="BoolTag"/> objects to determine if they contain different values.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=( BoolTag left, BoolTag right )
        {
            return !( left == right );
        }

        // --- ITag implementation ---

        WireType ITag.WireType => WireType.Bool;

        int ITag.ComputeLength()
        {
            return 1;
        }

        void ITag.ReadValue( byte[] buffer, int position, int length )
        {
            if( length != 1 )
            {
                throw new ArgumentOutOfRangeException( nameof( length ), "Length must always be 1 byte." );
            }

            this.Value = buffer[position] > 0;
        }

        void ITag.WriteValue( byte[] buffer, int position )
        {
            if( this.Value )
            {
                buffer[position] = 1;
            }
            else
            {
                buffer[position] = 0;
            }
        }
    }
}