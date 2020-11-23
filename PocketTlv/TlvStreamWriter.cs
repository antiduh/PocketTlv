using System;
using System.IO;
using PocketTlv.ClassLib;

namespace PocketTlv
{
    public class TlvStreamWriter : ITlvWriter
    {
        public const int DefaultBufferSize = 1024;

        private Stream stream;
        private byte[] buffer;

        public TlvStreamWriter( Stream stream )
            : this( stream, DefaultBufferSize )
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
            RequireConnected();

            if( tag is null )
            {
                throw new ArgumentNullException( nameof( tag ) );
            }

            int position = WriteInternal( tag, ref this.buffer, 0 );

            this.stream.Write( this.buffer, 0, position );
        }

        public void Write( ITlvContract contract )
        {
            RequireConnected();
            
            if( contract is null )
            {
                throw new ArgumentNullException( nameof( contract ) );
            }

            var contractTag = new ContractTag()
            {
                FieldId = contract.ContractId,
                ContractId = contract.ContractId
            };

            var saveContext = new TlvSaveContext( contractTag.Children );

            contract.Save( saveContext );

            Write( contractTag );
        }

        internal static int WriteInternal( ITag tag, ref byte[] buffer, int position )
        {
            int tagValueLength = tag.ComputeLength();

            //  requiredSize = type size, length field size, value field size
            int requiredSpace = TlvConsts.HeaderSize + tagValueLength;

            EnsureSize( ref buffer, requiredSpace );

            return TagBufferWriter.Write( tag, tagValueLength, buffer, position );
        }

        private static void EnsureSize( ref byte[] buffer, int size )
        {
            if( buffer.Length < size )
            {
                Array.Resize( ref buffer, size );
            }
        }

        private void RequireConnected()
        {
            if( this.stream == null )
            {
                throw new InvalidOperationException( "Cannot write while not connected to a stream." );
            }
        }

    }

    public static class TagBufferWriter
    {
        public static int Write( ITag tag, byte[] buffer, int position )
        {
            return Write( tag, tag.ComputeLength(), buffer, position );
        }

        public static int Write( ITag tag, int tagValueLength, byte[] buffer, int position )
        {
            int written = 0;

            // -- Type --
            ushort type = TypePacking.Pack( tag.WireType, tag.FieldId );
            DataConverter.WriteUShortLE( type, buffer, position + written );
            written += TlvConsts.TypeSize;

            // -- Length --
            DataConverter.WriteIntLE( tagValueLength, buffer, position + written );
            written += TlvConsts.LengthSize;

            // -- Value --
            tag.WriteValue( buffer, position + written );
            written += tagValueLength;

            return written;
        }
    }
}