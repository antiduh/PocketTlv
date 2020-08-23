using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlvDemo.TlvApi.Primitives
{
    public class VarIntTag : ITag
    {
        public VarIntTag()
        {
        }

        public VarIntTag( long value )
        {
            Value = value;
        }

        public long Value { get; set; }

        public static implicit operator long( VarIntTag tag )
        {
            return tag.Value;
        }

        public override bool Equals( object other )
        {
            return Equals( other as VarIntTag );
        }

        public bool Equals( VarIntTag other )
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
                return this.Value == other.Value;
            }
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        // --- ITag implementation ---

        public int FieldId { get; set; }

        WireType ITag.WireType => WireType.VarInt;

        public int ComputeLength()
        {
            int numBytes = 0;
            
            for( long copy = this.Value; copy > 0; copy >>= 8 )
            {
                numBytes++;
            }

            return numBytes;
        }

        void ITag.ReadValue( byte[] buffer, int position, int length )
        {
            this.Value = 0;

            for( int i = 0; i < length; i++ )
            {
                this.Value <<= 8;
                this.Value |= buffer[position + i];
            }
        }

        void ITag.WriteValue( byte[] buffer, int position )
        {
            long copy = this.Value;

            for( int i = 0; i < ComputeLength(); i++ )
            {
                buffer[position + i] = (byte)copy;
                copy >>= 8;
            }
        }
    }
}
