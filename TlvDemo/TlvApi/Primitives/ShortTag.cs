using System;

namespace TlvDemo.TlvApi
{
    public class ShortTag : ITag
    {
        public ShortTag()
        {
        }

        public ShortTag( short value )
        {
            this.Value = value;
        }

        public ShortTag( int fieldId, short value )
        {
            this.FieldId = fieldId;
            this.Value = value;
        }

        public short Value { get; set; }

        public override string ToString()
        {
            return "ShortTag - " + Value;
        }

        public override bool Equals( object other )
        {
            return Equals( other as ShortTag );
        }

        public bool Equals( ShortTag other )
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

        public static implicit operator short( ShortTag tag )
        {
            return tag.Value;
        }

        // --- ITag implementation ---

        public int FieldId { get; set; }

        WireType ITag.WireType => WireType.Short;

        int ITag.ComputeLength()
        {
            return 2;
        }

        void ITag.ReadValue( byte[] buffer, int position, int length )
        {
            if( length != 2 )
            {
                throw new InvalidOperationException( $"{nameof( length )} must be {sizeof( short )}." );
            }

            this.Value = DataConverter.ReadShortLE( buffer, position );
        }

        void ITag.WriteValue( byte[] buffer, int position )
        {
            DataConverter.WriteShortLE( this.Value, buffer, position );
        }
    }
}