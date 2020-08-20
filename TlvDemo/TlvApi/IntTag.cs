using System;

namespace TlvDemo.TlvApi
{
    public class IntTag : ITag
    {
        public IntTag()
        {
        }

        public IntTag( int value )
        {
            this.Value = value;
        }

        public int Value { get; set; }

        public override string ToString()
        {
            return "IntTag - " + Value;
        }

        public static implicit operator int( IntTag tag )
        {
            return tag.Value;
        }

        // --- ITag implementation ---

        int ITag.FieldId { get; set; }

        WireType ITag.WireType => WireType.Int;

        int ITag.ComputeLength()
        {
            return 4;
        }

        void ITag.ReadValue( byte[] buffer, int position, int length )
        {
            if( length != 4 )
            {
                throw new InvalidOperationException( $"{nameof( length )} must be 4." );
            }

            this.Value = DataConverter.ReadIntLE( buffer, position );
        }

        void ITag.WriteValue( byte[] buffer, int position )
        {
            DataConverter.WriteIntLE( this.Value, buffer, position );
        }
    }
}