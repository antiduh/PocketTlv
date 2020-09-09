using System;
using System.IO;
using PocketTlv.ClassLib;

namespace PocketTlv
{
    public class TlvStreamReader : ITlvReader
    {
        private readonly StreamConverter reader;

        private readonly ContractRegistry contractReg;

        private byte[] buffer;

        public TlvStreamReader( Stream stream )
            : this( stream, 1024 )
        {
        }

        public TlvStreamReader( Stream stream, int bufferSize )
        {
            if( stream == null )
            {
                throw new ArgumentNullException( nameof( stream ) );
            }

            if( bufferSize <= 0 )
            {
                throw new ArgumentOutOfRangeException( nameof( bufferSize ), "must be a positive integer." );
            }

            this.contractReg = new ContractRegistry();

            this.buffer = new byte[bufferSize];

            this.reader = new StreamConverter( stream );
        }

        public ITag ReadTag()
        {
            return ReadInternal();
        }

        public T ReadTag<T>() where T : ITag
        {
            return (T)ReadInternal();
        }

        public ITlvContract ReadContract()
        {
            var contractTag = ReadTag<ContractTag>();

            int contractId = contractTag.FieldId;

            ITlvContract contract;

            if( this.contractReg.Contains( contractId ) )
            {
                contract = this.contractReg.Get( contractId );
            }
            else
            {
                contract = new UnresolvedContract( contractTag );
            }

            TlvParseContext parseContext = new TlvParseContext( contractTag.Children );

            contract.Parse( parseContext );

            return contract;
        }

        public T ReadContract<T>() where T : ITlvContract, new()
        {
            T contract = new T();

            var contractTag = ReadTag<ContractTag>();

            if( contract.ContractId != contractTag.ContractId )
            {
                throw new InvalidOperationException( "Read unexpected tag." );
            }

            TlvParseContext parseContext = new TlvParseContext( contractTag.Children );

            contract.Parse( parseContext );

            return contract;
        }

        public void RegisterContract<T>() where T : ITlvContract, new()
        {
            this.contractReg.Register<T>();
        }

        private ITag ReadInternal()
        {
            int tagValueLength;

            // This reads the type and length fields.
            if( this.reader.ReadHarder( this.buffer, 0, 6 ) == false )
            {
                return null;
            }

            // Figure out how long the value field is.
            tagValueLength = DataConverter.ReadIntLE( this.buffer, 2 );

            // Make sure our buffer can fit the whole thing
            int requiredSize = TlvConsts.HeaderSize + tagValueLength;
            EnsureSize( ref this.buffer, requiredSize );

            // Read the value portion into the buffer.
            if( this.reader.ReadHarder( this.buffer, 6, tagValueLength ) == false )
            {
                return null;
            }

            // Turn the buffer into a full tag chain.
            return TagBufferReader.Read( this.buffer, 0, out _ );
        }

        private static void EnsureSize( ref byte[] buffer, int size )
        {
            if( buffer.Length < size )
            {
                Array.Resize( ref buffer, size );
            }
        }
    }

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