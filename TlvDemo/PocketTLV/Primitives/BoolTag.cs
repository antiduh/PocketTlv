using System;

namespace PocketTLV.Primitives
{
    public class BoolTag : ITag
    {
        public BoolTag()
        {
        }

        public BoolTag( bool value )
        {
            this.Value = value;
        }

        public BoolTag( int fieldId, bool value )
        {
            this.FieldId = fieldId;
            this.Value = value;
        }

        public bool Value { get; private set; }

        public int FieldId { get; set; }

        public override bool Equals( object other )
        {
            return Equals( other as BoolTag );
        }

        public bool Equals( BoolTag other )
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public static implicit operator bool( BoolTag tag )
        {
            return tag.Value;
        }

        public static bool operator==( BoolTag left, BoolTag right )
        {
            if( left is null )
            {
                return right is null;
            }
            else if( right is null )
            {
                return false;
            }

            return left.Value == right.Value;
        }

        public static bool operator !=( BoolTag left, BoolTag right )
        {
            return !( left == right );
        }

        // --- ITag implementation ---

        WireType ITag.WireType => WireType.Bool;

        int ITag.ComputeLength()
        {
            return 1;
        }

        void ITag.ReadValue( byte[] buffer, int position, int length )
        {
            if( length != 1 )
            {
                throw new ArgumentOutOfRangeException( nameof( length ), "Length must always be 1 byte." );
            }

            this.Value = buffer[position] > 0;
        }

        void ITag.WriteValue( byte[] buffer, int position )
        {
            if( this.Value )
            {
                buffer[position] = 1;
            }
            else
            {
                buffer[position] = 0;
            }
        }
    }
}