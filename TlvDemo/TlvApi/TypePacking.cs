using System;

namespace TlvDemo.TlvApi
{
    /// <summary>
    /// Performs operations on packed and unpacked TLV type fields.
    /// </summary>
    /// <remarks>
    /// The type field is 16 bits, with the lower 4 bits storing the wire type and the upper 12 bits storing the fieldId.
    /// </remarks>
    public class TypePacking
    {
        public static short Pack( WireType wireType, int fieldId )
        {
            // Type fields are 16 bits. The lower 4 bits store the wire type. The rest store the
            // field id.

            return (short)( (int)wireType | fieldId << 4 );
        }

        public static void Unpack( short packedType, out int wireType, out int fieldId )
        {
            // Type fields are 16 bits. The lower 4 bits store the wire type. The rest store the
            // field id.

            wireType = packedType & 0b1111;

            fieldId = packedType >> 4;
        }
    }
}