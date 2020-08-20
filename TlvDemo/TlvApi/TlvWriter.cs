using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace TlvDemo.TlvApi
{
    public class TlvWriter
    {
        private const int typeFieldSize = 2;

        private const int lenFieldSize = 4;

        private readonly Stream stream;
        private byte[] buffer;


        public TlvWriter( Stream stream )
        {
            this.stream = stream;

            this.buffer = new byte[64];
        }

        public void Write( ITag tag )
        {
            int position = WriteInternal( tag, ref this.buffer, 0 );
            
            this.stream.Write( this.buffer, 0, position );
        }

        public void Write( ITlvContract contract )
        {
            CompositeTag contractTag = new CompositeTag();

            ( (ITag)contractTag ).FieldId = contract.ContractId;

            TlvSaveContext saveContext = new TlvSaveContext( contractTag );

            contract.Save( saveContext );

            Write( contractTag );
        }

        internal static int WriteInternal( ITag tag, ref byte[] buffer, int position )
        {
            int written = 0;

            int valueLength = tag.ComputeLength();
            
            //  requiredSize = type size, length field size, value field size
            int requiredSpace = TlvConsts.HeaderSize + valueLength;

            EnsureSize( ref buffer, requiredSpace + position );

            // -- Type --
            short type = PackTypeField( tag );
            DataConverter.WriteShortLE( type, buffer, position + written );
            written += TlvConsts.TypeSize;

            // -- Length --
            DataConverter.WriteIntLE( valueLength, buffer, position + written );
            written += TlvConsts.LengthSize;

            // -- Value --

            if( tag is CompositeTag compositeTag )
            {
                int subPosition = position + written;

                foreach( ITag child in compositeTag.Children )
                {
                    subPosition += WriteInternal( child, ref buffer, subPosition );
                }
            }
            else
            {
                tag.WriteValue( buffer, position + written );
            }
            
            written += valueLength;

            return written;
        }

        private static short PackTypeField( ITag tag )
        {
            // Type fields are 16 bits. The lower 4 bits store the wire type. The rest store the
            // field id.

            return (short)((int)tag.WireType | tag.FieldId << 4);
        }

        private static void EnsureSize( ref byte[] buffer, int size )
        {
            if( buffer.Length < size )
            {
                Array.Resize( ref buffer, size );
            }
        }
    }
}
