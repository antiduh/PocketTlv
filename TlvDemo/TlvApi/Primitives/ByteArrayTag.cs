using System;

namespace TlvDemo.TlvApi
{
    public class ByteArrayTag : ITag
    {
        public ByteArrayTag()
        {
        }

        public ByteArrayTag( byte[] array )
        {
            Array = array;
        }

        public byte[] Array { get; set; }

        public override bool Equals( object other )
        {
            return Equals( other as ByteArrayTag );
        }

        public bool Equals( ByteArrayTag other )
        {
            if( ReferenceEquals( other, null ) )
            {
                return false;
            }
            else if( ReferenceEquals( other, this ) )
            {
                return true;
            }
            else
            {
                if( this.Array.Length != other.Array.Length )
                {
                    return false;
                }

                for( int i = 0; i < this.Array.Length; i++ )
                {
                    if( this.Array[i] != other.Array[i] )
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
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