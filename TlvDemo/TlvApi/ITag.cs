using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlvDemo.TlvApi
{
    public interface ITag
    {
        int FieldId { get; set; }

        WireType WireType { get; }

        int ComputeLength();

        void ReadValue( byte[] buffer, int position, int length );

        void WriteValue( byte[] buffer, int position );
    }
}
