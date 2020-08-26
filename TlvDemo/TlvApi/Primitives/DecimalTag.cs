using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace TlvDemo.TlvApi.Primitives
{
    public class DecimalTag : ITag
    {
        public DecimalTag()
        {
        }

        public DecimalTag( decimal value )
        {
            this.Value = value;
        }

        public DecimalTag( int fieldId, decimal value )
        {
            this.FieldId = fieldId;
            this.Value = value;
        }
        
        public decimal Value { get; set; }

        public override bool Equals( object other )
        {
            return Equals( other as DecimalTag );
        }

        public bool Equals( DecimalTag other )
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

        public static implicit operator decimal( DecimalTag tag )
        {
            return tag.Value;
        }

        // --- ITag implementation ---

        public int FieldId { get; set; }
        
        WireType ITag.WireType => WireType.Decimal;

        int ITag.ComputeLength()
        {
            return 16;
        }

        void ITag.ReadValue( byte[] buffer, int position, int length )
        {
            if( length != 16 )
            {
                throw new ArgumentOutOfRangeException( nameof( length ), $"Length must always be 16 bytes." );
            }

            this.Value = DataConverter.ReadDecimalLE( buffer, position );
        }

        void ITag.WriteValue( byte[] buffer, int position )
        {
            DataConverter.WriteDecimalLE( this.Value, buffer, position );
        }
    }
}
