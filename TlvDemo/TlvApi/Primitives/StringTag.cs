using System;
using System.Text;

namespace TlvDemo.TlvApi
{
    public class StringTag : ITag
    {
        public StringTag()
        {
        }

        public StringTag( string value )
        {
            this.Value = value;
        }

        public StringTag( int fieldId, string value )
        {
            this.FieldId = fieldId;
            this.Value = value;
        }

        public string Value { get; set; }

        public override string ToString()
        {
            return "StringTag - " + this.Value;
        }

        public override bool Equals( object other )
        {
            return Equals( other as StringTag );
        }

        public bool Equals( StringTag other )
        {
            if( ReferenceEquals( other, null ) )
            {
                return false;
            }
            else if( ReferenceEquals( other, this ) )
            {
                return true;
            }
            else
            {
                return this.Value == other.Value;
            }
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public static implicit operator string( StringTag tag )
        {
            return tag.Value;
        }

        // --- ITag implementation ---

        public int FieldId { get; set; }

        WireType ITag.WireType => WireType.String;

        int ITag.ComputeLength()
        {
            return Encoding.UTF8.GetByteCount( this.Value );
        }

        void ITag.ReadValue( byte[] buffer, int position, int length )
        {
            this.Value = Encoding.UTF8.GetString( buffer, position, length );
        }

        void ITag.WriteValue( byte[] buffer, int position )
        {
            Encoding.UTF8.GetBytes( this.Value, 0, this.Value.Length, buffer, position );
        }
    }
}