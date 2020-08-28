using System;
using System.Linq;
using System.Text;

namespace PocketTLV.Primitives
{
    public class ByteArrayTag : ITag
    {
        public ByteArrayTag()
        {
            this.Array = System.Array.Empty<byte>();
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
            this.Array = array;
        }

        public byte[] Array { get; set; }

        public override string ToString()
        {
            int limit = Math.Min( 10, this.Array.Length );
            var builder = new StringBuilder( limit * 5 );

            builder.Append( "{ " );

            builder.Append( 
                string.Join( ", ", this.Array.Take( limit ).Select( x => x.ToString( "X2" ) ) )
            );

            if( this.Array.Length > limit )
            {
                builder.Append( ", ..." );
            }

            builder.Append( " } " );

            return builder.ToString();
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
            return tag.Array;
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

                if( left.Array.Length != right.Array.Length )
                {
                    return false;
                }

                for( int i = 0; i < left.Array.Length; i++ )
                {
                    if( left.Array[i] != right.Array[i] )
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
            return this.Array.Length;
        }

        void ITag.ReadValue( byte[] buffer, int position, int length )
        {
            this.Array = new byte[length];

            Buffer.BlockCopy( buffer, position, this.Array, 0, length );
        }

        void ITag.WriteValue( byte[] buffer, int position )
        {
            Buffer.BlockCopy( this.Array, 0, buffer, position, this.Array.Length );
        }
    }
}