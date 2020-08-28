using System;
using System.Linq;
using System.Text;
using PocketTLV.ClassLib;

namespace PocketTLV.Primitives
{
    /// <summary>
    /// Represents an array of bytes as a TLV tag.
    /// </summary>
    public class ByteArrayTag : ITag
    {
        private byte[] data;

        /// <summary>
        /// Initializes a new <see cref="ByteArrayTag"/> with an empty array.
        /// </summary>
        public ByteArrayTag()
        {
            this.data = System.Array.Empty<byte>();
        }

        /// <summary>
        /// Initializes a new <see cref="ByteArrayTag"/>.
        /// </summary>
        /// <param name="array">The byte array to store. Must not be null.</param>
        public ByteArrayTag( byte[] array )
            : this( 0, array )
        {
        }

        /// <summary>
        /// Initializes a new <see cref="ByteArrayTag"/>.
        /// </summary>
        /// <param name="fieldId">The TLV field ID to associate to the tag.</param>
        /// <param name="array">The byte array to store. Must not be null.</param>
        public ByteArrayTag( int fieldId, byte[] array )
        {
            if( array == null )
            {
                throw new ArgumentNullException( nameof( array ) );
            }

            this.FieldId = fieldId;
            this.data = array;
        }

        /// <summary>
        /// Gets or sets the byte array stored in the tag. Must not be null.
        /// </summary>
        public byte[] Data
        {
            get
            {
                return this.data;
            }
            set
            {
                if( value == null )
                {
                    throw new ArgumentNullException( nameof( value ) );
                }

                this.data = value;
            }
        }

        /// <summary>
        /// Gets or sets the TLV field ID that the tag represents.
        /// </summary>
        public int FieldId { get; set; }

        /// <summary>
        /// Converts the bytes contained in the tag to a string formatted as hex digits, printing a
        /// maximum of 10 bytes.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            int limit = Math.Min( 10, this.Data.Length );

            if( limit == 0 )
            {
                return "{ }";
            }
            else
            {
                var builder = new StringBuilder( limit * 5 );

                builder.Append( "{ " );
                builder.Append(
                    string.Join( ", ", this.Data.Take( limit ).Select( x => "0x" + x.ToString( "X2" ) ) )
                );

                if( this.Data.Length > limit )
                {
                    builder.Append( ", ..." );
                }

                builder.Append( " }" );

                return builder.ToString();
            }
        }

        /// <summary>
        /// Returns whether this <see cref="ByteArrayTag"/> is equal to the provided object.
        /// </summary>
        /// <param name="other">The object to compare to.</param>
        /// <returns>True if the other object has the same value as this object.</returns>
        public override bool Equals( object other )
        {
            return Equals( other as ByteArrayTag );
        }

        /// <summary>
        /// Compares two <see cref="ByteArrayTag"/> objects.
        /// </summary>
        /// <param name="other">The tag to compare to.</param>
        /// <returns>True if the other tag has the same bytes as this tag.</returns>
        public bool Equals( ByteArrayTag other )
        {
            return this == other;
        }

        /// <summary>
        /// Returns a hash value for a <see cref="ByteArrayTag"/> instance.
        /// </summary>
        /// <returns>An integer hash code.</returns>
        public override int GetHashCode()
        {
            return HashHelper.GetHashCode( this.data );
        }

        /// <summary>
        /// Converts a <see cref="ByteArrayTag"/> the array contained by it.
        /// </summary>
        /// <param name="tag"></param>
        public static implicit operator byte[]( ByteArrayTag tag )
        {
            return tag.Data;
        }

        /// <summary>
        /// Compares two <see cref="ByteArrayTag"/> objects to determine if they contain the same value.
        /// </summary>
        /// <param name="left">The first tag to compare.</param>
        /// <param name="right">The second tag to compare.</param>
        /// <returns>True if the two tags contain equal values, false otherwise.</returns>
        public static bool operator ==( ByteArrayTag left, ByteArrayTag right )
        {
            if( ReferenceEquals( left, null ) )
            {
                return ReferenceEquals( right, null );
            }
            else if( ReferenceEquals( right, null ) )
            {
                return false;
            }
            else
            {
                // We can guarantee that the Array properties can't be null here, so we don't need
                // to check them.

                if( left.Data.Length != right.Data.Length )
                {
                    return false;
                }

                for( int i = 0; i < left.Data.Length; i++ )
                {
                    if( left.Data[i] != right.Data[i] )
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Compares two <see cref="ByteArrayTag"/> objects to determine if they contain different values.
        /// </summary>
        /// <param name="left">The first tag to compare.</param>
        /// <param name="right">The second tag to compare.</param>
        /// <returns>True if the two tags do not contain equal values, false otherwise.</returns>
        public static bool operator !=( ByteArrayTag left, ByteArrayTag right )
        {
            return !( left == right );
        }

        // --- ITag implementation ---

        WireType ITag.WireType => WireType.ByteArray;

        int ITag.ComputeLength()
        {
            return this.Data.Length;
        }

        void ITag.ReadValue( byte[] buffer, int position, int length )
        {
            this.Data = new byte[length];

            Buffer.BlockCopy( buffer, position, this.Data, 0, length );
        }

        void ITag.WriteValue( byte[] buffer, int position )
        {
            Buffer.BlockCopy( this.Data, 0, buffer, position, this.Data.Length );
        }
    }
}