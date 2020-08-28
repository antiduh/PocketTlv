using System;
using System.Linq;
using System.Text;

namespace PocketTLV.Primitives
{
    /// <summary>
    /// Represents an array of bytes as a TLV tag.
    /// </summary>
    public class ByteArrayTag : ITag
    {
        private byte[] data;

        public ByteArrayTag()
        {
            this.data = System.Array.Empty<byte>();
        }

        public ByteArrayTag( byte[] array )
            : this( 0, array )
        { 
        }

        public ByteArrayTag( int fieldId, byte[] array )
        {
            if( array == null )
            {
                throw new ArgumentNullException( nameof( array ) );
            }

            this.FieldId = fieldId;
            this.data = array;
        }

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

        public override bool Equals( object other )
        {
            return Equals( other as ByteArrayTag );
        }

        public bool Equals( ByteArrayTag other )
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static implicit operator byte[]( ByteArrayTag tag )
        {
            return tag.Data;
        }

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

        public static bool operator !=( ByteArrayTag left, ByteArrayTag right )
        {
            return !( left == right );
        }

        // --- ITag implementation ---

        public int FieldId { get; set; }

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