﻿using System;

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

        public static implicit operator bool( BoolTag tag )
        {
            return tag.Value;
        }

        // --- ITag implementation ---

        WireType ITag.WireType => WireType.Bool;

        int ITag.ComputeLength()
        {
            return 1;
        }

        void ITag.ReadValue( byte[] buffer, int position, int length )
        {
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