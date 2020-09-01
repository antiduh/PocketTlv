using System;
using System.IO;
using PocketTLV.ClassLib;
using PocketTLV.Primitives;

namespace PocketTLV
{
    public class TlvStreamWriter : ITlvWriter
    {
        private readonly Stream stream;
        private byte[] buffer;

        public TlvStreamWriter( Stream stream )
            : this( stream, 1024 )
        {
        }

        public TlvStreamWriter( Stream stream, int bufferSize )
        {
            if( stream == null )
            {
                throw new ArgumentNullException( nameof( stream ) );
            }

            if( bufferSize <= 0 )
            {
                throw new ArgumentOutOfRangeException( nameof( bufferSize ), "must be a positive integer." );
            }

            this.stream = stream;
            this.buffer = new byte[bufferSize];
        }

        public void Write( ITag tag )
        {
            if( tag is null )
            {
                throw new ArgumentNullException( nameof( tag ) );
            }

            int position = WriteInternal( tag, ref this.buffer, 0 );

            this.stream.Write( this.buffer, 0, position );
        }

        public void Write( ITlvContract contract )
        {
            if( contract is null )
            {
                throw new ArgumentNullException( nameof( contract ) );
            }

            var contractTag = new CompositeTag()
            {
                FieldId = contract.ContractId
            };

            var saveContext = new TlvSaveContext( contractTag );

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
            ushort type = TypePacking.Pack( tag.WireType, tag.FieldId );
            DataConverter.WriteUShortLE( type, buffer, position + written );
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

        private static void EnsureSize( ref byte[] buffer, int size )
        {
            if( buffer.Length < size )
            {
                Array.Resize( ref buffer, size );
            }
        }
    }
}