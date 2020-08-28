using System;

namespace PocketTLV.Primitives
{
    /// <summary>
    /// Stores an integer as a TLV tag using a variable-length encoding.
    /// </summary>
    /// <remarks>
    /// The <see cref="VarIntTag"/> can store a maximum integer of 64 bits. When serializing the
    /// integer's data, only the non-zero bytes are stored.
    /// </remarks>
    public class VarIntTag : ITag
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VarIntTag"/> class.
        /// </summary>
        public VarIntTag()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VarIntTag"/> class.
        /// </summary>
        /// <param name="value">The integer to store.</param>
        public VarIntTag( long value )
        {
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VarIntTag"/> class.
        /// </summary>
        /// <param name="fieldId">The TLV field ID to associate to the tag.</param>
        /// <param name="value">The integer to store.</param>
        public VarIntTag( int fieldId, long value )
        {
            this.FieldId = fieldId;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the integer stored by the tag.
        /// </summary>
        public long Value { get; set; }

        /// <summary>
        /// Gets or sets the TLV field ID that the tag represents.
        /// </summary>
        public int FieldId { get; set; }

        /// <summary>
        /// Converts the tag to a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Value.ToString();
        }

        /// <summary>
        /// Returns whether this <see cref="VarIntTag"/> is equal to the provided object.
        /// </summary>
        /// <param name="other">The object to compare to.</param>
        /// <returns>True if the other object has the same value as this object.</returns>
        public override bool Equals( object other )
        {
            return Equals( other as VarIntTag );
        }

        /// <summary>
        /// Compares two <see cref="VarIntTag"/> objects.
        /// </summary>
        /// <param name="other">The tag to compare to.</param>
        /// <returns>True if the tag object has the same value as this tag.</returns>
        public bool Equals( VarIntTag other )
        {
            return this == other;
        }

        /// <summary>
        /// Returns a hash value for a <see cref="VarIntTag"/> instance.
        /// </summary>
        /// <returns>An integer hash code.</returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        /// <summary>
        /// Converts a <see cref="VarIntTag"/> to a <see cref="bool"/>.
        /// </summary>
        /// <param name="tag">The tag to convert.</param>
        public static implicit operator long( VarIntTag tag )
        {
            return tag.Value;
        }

        /// <summary>
        /// Compares two <see cref="VarIntTag"/> objects to determine if they contain the same value.
        /// </summary>
        /// <param name="left">The first tag to compare.</param>
        /// <param name="right">The second tag to compare.</param>
        /// <returns>True if the two tags contain equal values, false otherwise.</returns>
        public static bool operator ==( VarIntTag left, VarIntTag right )
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
        /// Compares two <see cref="VarIntTag"/> objects to determine if they contain different values.
        /// </summary>
        /// <param name="left">The first tag to compare.</param>
        /// <param name="right">The second tag to compare.</param>
        /// <returns>True if the two tags do not contain equal values, false otherwise.</returns>
        public static bool operator !=( VarIntTag left, VarIntTag right )
        {
            return !( left == right );
        }

        // --- ITag implementation ---

        WireType ITag.WireType => WireType.VarInt;

        public int ComputeLength()
        {
            int numBytes = 0;

            for( ulong copy = (ulong)this.Value; copy > 0; copy >>= 8 )
            {
                numBytes++;
            }

            return numBytes;
        }

        void ITag.ReadValue( byte[] buffer, int position, int length )
        {
            ulong result = 0;

            for( int i = 0; i < length; i++ )
            {
                result |= ( (ulong)buffer[position + i] ) << i * 8;
            }

            this.Value = (long)result;
        }

        void ITag.WriteValue( byte[] buffer, int position )
        {
            ulong copy = (ulong)this.Value;

            for( int i = 0; i < ComputeLength(); i++ )
            {
                buffer[position + i] = (byte)copy;
                copy >>= 8;
            }
        }
    }
}