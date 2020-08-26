using System;
using System.Collections.Generic;
using System.IO;
using TlvDemo.TlvApi.Primitives;

namespace TlvDemo.TlvApi
{
    public class TlvReader
    {
        private readonly Stream stream;

        private readonly StreamConverter reader;

        private readonly ContractRegistry contractReg;

        private byte[] buffer;

        public TlvReader( Stream stream )
            : this( stream, 1024 )
        {
        }

        public TlvReader( Stream stream, int bufferSize )
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

            this.contractReg = new ContractRegistry();

            this.buffer = new byte[bufferSize];

            this.reader = new StreamConverter( this.stream );
        }

        public ITag ReadTag()
        {
            return ReadInternal( out _ );
        }

        public T ReadTag<T>() where T : ITag
        {
            return (T)ReadInternal( out _ );
        }

        public T ReadContract<T>() where T : ITlvContract, new()
        {
            T contract = new T();

            CompositeTag contractTag = ReadTag<CompositeTag>();

            if( contract.ContractId != ( (ITag)contractTag ).FieldId )
            {
                throw new InvalidOperationException( "Read unexpected tag." );
            }

            TlvParseContext parseContext = new TlvParseContext( contractTag, false );

            contract.Parse( parseContext );

            return contract;
        }

        public ITlvContract ReadContract()
        {
            var contractTag = ReadTag<CompositeTag>();

            int contractId = contractTag.FieldId;

            if( this.contractReg.Contains( contractId ) == false )
            {
                string msg = $"Cannot deserialize contract with ID {contractId} - no contract has been registered for that ID.";
                throw new KeyNotFoundException( msg );
            }

            ITlvContract contract = this.contractReg.Get( contractId );

            TlvParseContext parseContext = new TlvParseContext( contractTag, false );

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
            tag = ConstructTag( wireType, fieldId );

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

        private ITag ConstructTag( int wireTypeId, int fieldId )
        {
            WireType wireType = (WireType)wireTypeId;
            ITag result;

            switch( wireType )
            {
                case WireType.Composite:
                    result = new CompositeTag();
                    break;

                case WireType.Int:
                    result = new IntTag();
                    break;

                case WireType.Short:
                    result = new ShortTag();
                    break;

                case WireType.Long:
                    result = new LongTag();
                    break;

                case WireType.String:
                    result = new StringTag();
                    break;

                case WireType.ContractId:
                    result = new ContractIdTag();
                    break;

                case WireType.Double:
                    result = new DoubleTag();
                    break;

                case WireType.ByteArray:
                    result = new ByteArrayTag();
                    break;

                case WireType.VarInt:
                    result = new VarIntTag();
                    break;

                case WireType.Bool:
                    result = new BoolTag();
                    break;

                case WireType.Decimal:
                    result = new DecimalTag();
                    break;

                default:
                    throw new UnknownWireTypeException( $"Unknown wire type '{wireTypeId}'.", wireTypeId );
            }

            result.FieldId = fieldId;

            return result;
        }
    }
}