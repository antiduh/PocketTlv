using System;

namespace TlvDemo.TlvApi
{
    public class IntTag : ITag
    {
        public IntTag()
        { }

        public IntTag( int fieldId )
        {
            this.FieldId = fieldId;
        }

        public IntTag( int fieldId, int value )
            : this( fieldId )
        {
            this.Value = value;
        }

        public int FieldId { get; private set; }

        public WireType WireType => WireType.Int;

        public int Value { get; set; }

        public int ComputeLength()
        {
            return 4;
        }

        public void ReadValue( byte[] buffer, int position, int length )
        {
            if( length != 4 )
            {
                throw new InvalidOperationException( $"{nameof( length )} must be 4." );
            }

            this.Value = DataConverter.ReadIntLE( buffer, position );
        }

        public void WriteValue( byte[] buffer, int position )
        {
            DataConverter.WriteIntLE( this.Value, buffer, position );
        }

        public override string ToString()
        {
            return "IntTag - " + Value;
        }
    }
}