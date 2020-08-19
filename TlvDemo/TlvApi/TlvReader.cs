using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace TlvDemo.TlvApi
{
    public class TlvReader
    {
        private readonly Stream stream;

        private readonly StreamConverter reader;

        private byte[] buffer;

        public TlvReader( Stream stream )
        {
            this.stream = stream;

            this.buffer = new byte[1024];

            this.reader = new StreamConverter( this.stream );
        }

        public ITag Read()
        {
            return Read( out _ );
        }

        private ITag Read( out int amountRead )
        {
            amountRead = 0;

            // -- Type --
            short packedType;
            if( this.reader.ReadShortLE( out packedType ) == false )
            {
                return null;
            }

            amountRead += TlvConsts.TypeSize;

            // -- Length --
            int length;

            if( this.reader.ReadIntLE( out length ) == false )
            {
                return null;
            }

            amountRead += TlvConsts.LengthSize;

            int wireType;
            int fieldId;

            UnpackType( packedType, out wireType, out fieldId );

            ITag tag;
            tag = ConstructTag( wireType, fieldId );

            if( tag is CompositeTag compositeTag )
            {
                while( amountRead < length )
                {
                    ITag childTag;
                    int childReadAmount;

                    childTag = Read( out childReadAmount );

                    if( childTag == null )
                    {
                        // End of stream before finishing the tag.
                        amountRead = -1;
                        return null;
                    }
                    else
                    {
                        amountRead += childReadAmount;
                        compositeTag.Children.Add( childTag );
                    }
                }
            }
            else
            {
                EnsureSize( ref this.buffer, length );
                this.reader.ReadHarder( this.buffer, 0, length );
                tag.ReadValue( this.buffer, 0, length );

                amountRead += length;
            }

            return tag;
        }

        private static void EnsureSize( ref byte[] buffer, int size )
        {
            if( buffer.Length < size )
            {
                Array.Resize( ref buffer, size );
            }
        }

        private static void UnpackType( short packedType, out int wireType, out int fieldId )
        {
            // Type fields are 16 bits. The lower 4 bits store the wire type. The rest store the
            // field id.

            wireType = packedType & 0b1111;

            fieldId = packedType >> 4;
        }

        private ITag ConstructTag( int wireTypeId, int fieldId )
        {
            WireType wireType = (WireType)wireTypeId;

            if( wireType == WireType.Composite )
            {
                return new CompositeTag( fieldId );
            }
            else if( wireType == WireType.Int )
            {
                return new IntTag( fieldId );
            }
            else if( wireType == WireType.String )
            {
                return new StringTag( fieldId );
            }

            throw new InvalidOperationException( "Unknown wire type or field ID." );
        }
    }
}
