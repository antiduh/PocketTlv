using System;

namespace TlvDemo.TlvApi
{
    public enum WireType : int
    {
        Composite = 1,
        ContractId = 2,
        Bool = 3,
        String = 4,
        Short = 5, 
        Int = 6,
        Long = 7,
        VarInt = 8,
        Double = 9,
        ByteArray = 10,
        Decimal = 11,
    }
}