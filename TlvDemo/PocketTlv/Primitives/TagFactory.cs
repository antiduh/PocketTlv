using System;

namespace PocketTlv
{
    public static class TagFactory
    {
        public static ITag Construct( int wireTypeId, int fieldId )
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
