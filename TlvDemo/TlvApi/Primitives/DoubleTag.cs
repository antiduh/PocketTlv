using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlvDemo.TlvApi.Primitives
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
