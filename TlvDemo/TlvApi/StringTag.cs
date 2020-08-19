using System;
using System.Text;

namespace TlvDemo.TlvApi
{
    public class StringTag : ITag
    {
        public StringTag()
        { }

        public StringTag( int fieldId )
        {
            this.FieldId = fieldId;
        }

        public StringTag( int fieldId, string value )
            : this( fieldId )
        {
            this.Value = value;
        }

        public int FieldId { get; private set; }

        public WireType WireType => WireType.String;

        public string Value { get; set; }

        public int ComputeLength()
        {
            return Encoding.UTF8.GetByteCount( this.Value );
        }

        public void ReadValue( byte[] buffer, int position, int length )
        {
            this.Value = Encoding.UTF8.GetString( buffer, position, length );
        }

        public void WriteValue( byte[] buffer, int position )
        {
            Encoding.UTF8.GetBytes( this.Value, 0, this.Value.Length, buffer, position );
        }

        public override string ToString()
        {
            return "StringTag - " + this.Value;
        }
    }
}