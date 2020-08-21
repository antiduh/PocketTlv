using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlvDemo.TlvApi
{
    public class DoubleTag : ITag
    {
        public DoubleTag()
        {
        }

        public DoubleTag( double value )
        {
            Value = value;
        }

        public double Value { get; set; }

        public override bool Equals( object other )
        {
            return Equals( other as DoubleTag );
        }

        public bool Equals( DoubleTag other )
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

        WireType ITag.WireType => WireType.Double;

        int ITag.ComputeLength()
        {
            return 8;
        }

        void ITag.ReadValue( byte[] buffer, int position, int length )
        {
            this.Value = DataConverter.ReadDouble( buffer, position );
        }

        void ITag.WriteValue( byte[] buffer, int position )
        {
            DataConverter.WriteDouble( this.Value, buffer, position );
        }
    }
}
