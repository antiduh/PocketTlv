using System;

namespace TlvDemo.TlvApi
{
    public enum WireType : int
    {
        Composite = 1,
        ContractId = 2,
        Int = 3,
        String = 4,
        Double = 5,
        ByteArray = 6,
        Long = 7,
        Short = 8,
    }
}