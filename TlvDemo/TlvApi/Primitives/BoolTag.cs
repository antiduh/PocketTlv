using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlvDemo.TlvApi.Primitives
{
    public class BoolTag : ITag
    {
        public BoolTag()
        {
        }

        public BoolTag( bool value )
        {
            this.Value = value;
        }

        public BoolTag( int fieldId, bool value )
        {
            this.FieldId = fieldId;
            this.Value = value;
        }

        public bool Value { get; private set; }

        public int FieldId { get; set; }

        WireType ITag.WireType => WireType.Bool;

        int ITag.ComputeLength()
        {
            return 1;
        }

        void ITag.ReadValue( byte[] buffer, int position, int length )
        {
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
