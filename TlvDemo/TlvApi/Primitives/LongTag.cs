using System;

namespace TlvDemo.TlvApi
{
    public class LongTag : ITag
    {
        public LongTag()
        {
        }

        public LongTag( long value )
        {
            this.Value = value;
        }

        public long Value { get; set; }

        public override string ToString()
        {
            return "LongTag - " + Value;
        }

        public override bool Equals( object other )
        {
            return Equals( other as LongTag );
        }

        public bool Equals( LongTag other )
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

        public static implicit operator long( LongTag tag )
        {
            return tag.Value;
        }

        // --- ITag implementation ---

        public int FieldId { get; set; }

        WireType ITag.WireType => WireType.Long;

        int ITag.ComputeLength()
        {
            return sizeof(long);
        }

        void ITag.ReadValue( byte[] buffer, int position, int length )
        {
            if( length != sizeof(long) )
            {
                throw new InvalidOperationException( $"{nameof( length )} must be {sizeof(long)}." );
            }

            this.Value = DataConverter.ReadLongLE( buffer, position );
        }

        void ITag.WriteValue( byte[] buffer, int position )
        {
            DataConverter.WriteLongLE( this.Value, buffer, position );
        }
    }
}