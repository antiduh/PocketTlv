using System;

namespace TlvDemo.TlvApi
{
    public class DoubleTag : ITag
    {
        public DoubleTag()
        {
        }

        public DoubleTag( double value )
        {
            this.Value = value;
        }

        public DoubleTag( int fieldId, double value )
        {
            this.FieldId = fieldId;
            this.Value = value;
        }

        public double Value { get; set; }

        public override bool Equals( object other )
        {
            return Equals( other as DoubleTag );
        }

        public bool Equals( DoubleTag other )
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

        public static implicit operator double( DoubleTag tag )
        {
            return tag.Value;
        }

        // --- ITag implementation ---

        public int FieldId { get; set; }

        WireType ITag.WireType => WireType.Double;

        int ITag.ComputeLength()
        {
            return 8;
        }

        void ITag.ReadValue( byte[] buffer, int position, int length )
        {
            if( length != 8 )
            {
                throw new ArgumentOutOfRangeException( nameof( length ), "must always be 8 bytes." );
            }

            this.Value = DataConverter.ReadDoubleLE( buffer, position );
        }

        void ITag.WriteValue( byte[] buffer, int position )
        {
            DataConverter.WriteDoubleLE( this.Value, buffer, position );
        }
    }
}