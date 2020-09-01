using System;

namespace PocketTlv
{
    public static class TlvConsts
    {
        /// <summary>
        /// The length of the 'type' field in a TLV in bytes - 2 bytes.
        /// </summary>
        public const int TypeSize = 2;

        /// <summary>
        /// The length of the 'length' field in a TLV in bytes - 4 bytes.
        /// </summary>
        public const int LengthSize = 4;

        /// <summary>
        /// The length of the 'type' and 'length' field in a TLV in bytes - 6 bytes.
        /// </summary>
        public const int HeaderSize = TypeSize + LengthSize;
    }
}