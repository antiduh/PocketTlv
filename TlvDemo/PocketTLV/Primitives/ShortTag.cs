using System;
using PocketTLV.ClassLib;

namespace PocketTLV.Primitives
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
            return this == other;
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public static implicit operator short( ShortTag tag )
        {
            return tag.Value;
        }

        public static bool operator ==( ShortTag left, ShortTag right )
        {
            if( left is null )
            {
                return right is null;
            }
            else if( right is null )
            {
                return false;
            }
            else
            {
                return left.Value == right.Value;
            }
        }

        public static bool operator !=( ShortTag left, ShortTag right )
        {
            return !( left == right );
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