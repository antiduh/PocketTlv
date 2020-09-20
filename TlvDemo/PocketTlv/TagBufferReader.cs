using PocketTlv.ClassLib;

namespace PocketTlv
{
    public static class TagBufferReader
    {
        public static ITag Read( byte[] buffer, int position, out int amountRead )
        {
            amountRead = 0;

            // Read the header 
            ushort packedType = DataConverter.ReadUShortLE( buffer, position );
            position += 2;
            amountRead += 2;

            int tagValueLength = DataConverter.ReadIntLE( buffer, position );
            position += 4;
            amountRead += 4;

            // Construct the new tag
            int wireType;
            int fieldId;

            TypePacking.Unpack( packedType, out wireType, out fieldId );

            ITag tag;
            tag = TagFactory.Construct( wireType, fieldId );

            tag.ReadValue( buffer, position, tagValueLength );

            amountRead += tagValueLength;

            return tag;
        }
    }
}