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
            return ReadInternal( out _ );
        }

        public T ReadTag<T>() where T : ITag
        {
            return (T)ReadInternal( out _ );
        }

        public ITlvContract ReadContract()
        {
            var contractTag = ReadTag<CompositeTag>();

            int contractId = contractTag.FieldId;

            ITlvContract contract;

            if( this.contractReg.Contains( contractId ) )
            {
                contract = this.contractReg.Get( contractId );
            }
            else
            {
                contract = new UnresolvedContract( contractTag, contractId );
            }

            TlvParseContext parseContext = new TlvParseContext( contractTag.Children );

            contract.Parse( parseContext );

            return contract;
        }

        public T ReadContract<T>() where T : ITlvContract, new()
        {
            T contract = new T();

            var contractTag = ReadTag<CompositeTag>();

            if( contract.ContractId != ( (ITag)contractTag ).FieldId )
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

        private ITag ReadInternal( out int amountRead )
        {
            amountRead = 0;

            // -- Type --
            ushort packedType;
            if( this.reader.ReadUShortLE( out packedType ) == false )
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

            TypePacking.Unpack( packedType, out wireType, out fieldId );

            ITag tag;
            tag = TagFactory.Construct( wireType, fieldId );

            if( tag is CompositeTag compositeTag )
            {
                while( amountRead < length )
                {
                    ITag childTag;
                    int childReadAmount;

                    childTag = ReadInternal( out childReadAmount );

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
            else if( tag is ContractTag contractTag )
            {
                if( this.reader.ReadIntLE( out int contractId ) == false )
                {
                    return null;
                }

                contractTag.ContractId = contractId;

                amountRead += 4;

                while( amountRead < length )
                {
                    ITag childTag;
                    int childReadAmount;

                    childTag = ReadInternal( out childReadAmount );

                    if( childTag == null )
                    {
                        // End of stream before finishing the tag.
                        amountRead = -1;
                        return null;
                    }
                    else
                    {
                        amountRead += childReadAmount;
                        contractTag.Children.Add( childTag );
                    }
                }
            }
            else
            {
                EnsureSize( ref this.buffer, length );
                if( this.reader.ReadHarder( this.buffer, 0, length ) == false )
                {
                    return null;
                }

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
    }
}