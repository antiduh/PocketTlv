﻿using System;

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

        public IntTag( int fieldId, int value )
        {
            this.FieldId = fieldId;
            this.Value = value;
        }

        public int Value { get; set; }

        public override string ToString()
        {
            return "IntTag - " + Value;
        }

        public override bool Equals( object other )
        {
            return Equals( other as IntTag );
        }

        public bool Equals( IntTag other )
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

        public static implicit operator int( IntTag tag )
        {
            return tag.Value;
        }

        // --- ITag implementation ---

        public int FieldId { get; set; }

        WireType ITag.WireType => WireType.Int;

        int ITag.ComputeLength()
        {
            return sizeof( long );
        }

        void ITag.ReadValue( byte[] buffer, int position, int length )
        {
            if( length != sizeof( long ) )
            {
                throw new InvalidOperationException( $"{nameof( length )} must be {sizeof( int )}." );
            }

            this.Value = DataConverter.ReadIntLE( buffer, position );
        }

        void ITag.WriteValue( byte[] buffer, int position )
        {
            DataConverter.WriteIntLE( this.Value, buffer, position );
        }
    }
}