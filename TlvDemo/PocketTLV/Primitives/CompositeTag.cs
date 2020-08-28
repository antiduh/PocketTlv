using System;
using System.Collections.Generic;
using PocketTLV.ClassLib;

namespace PocketTLV.Primitives
{
    public class CompositeTag : ITag
    {
        public CompositeTag()
        {
            this.Children = new List<ITag>();
        }

        public CompositeTag( params ITag[] children )
            : this()
        {
            this.Children.AddRange( children );
        }

        public CompositeTag( int fieldId, params ITag[] children )
            : this()
        {
            this.FieldId = fieldId;
            this.Children.AddRange( children );
        }

        public List<ITag> Children { get; private set; }

        /// <summary>
        /// Gets or sets the TLV field ID that the tag represents.
        /// </summary>
        public int FieldId { get; set; }

        public void AddChild( ITag child )
        {
            this.Children.Add( child );
        }

        public void AddChild( int fieldId, ITag child )
        {
            child.FieldId = fieldId;
            this.Children.Add( child );
        }

        public override string ToString()
        {
            return $"CompositeTag - {this.Children.Count} children";
        }

        public override bool Equals( object other )
        {
            return Equals( other as CompositeTag );
        }

        public bool Equals( CompositeTag other )
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return HashHelper.GetHashCode( this.Children );
        }

        public static bool operator ==( CompositeTag left, CompositeTag right )
        {
            if( left is null )
            {
                return right is null;
            }
            else if( right is null )
            {
                return false;
            }

            if( left.Children.Count != right.Children.Count )
            {
                return false;
            }
            else
            {
                for( int i = 0; i < left.Children.Count; i++ )
                {
                    if( left.Children[i].Equals( right.Children[i] ) == false )
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public static bool operator !=( CompositeTag left, CompositeTag right )
        {
            return !( left == right );
        }

        // --- ITag implementation ---

        WireType ITag.WireType => WireType.Composite;

        int ITag.ComputeLength()
        {
            int length = 0;

            foreach( ITag child in this.Children )
            {
                int childLen = child.ComputeLength();

                // Tlv's Type and Length fields
                length += TlvConsts.HeaderSize;

                // Tlv's Value field.
                length += childLen;
            }

            return length;
        }

        void ITag.ReadValue( byte[] buffer, int position, int length )
        {
            // A composite tag's value is handled directly by TlvReader.
        }

        void ITag.WriteValue( byte[] buffer, int position )
        {
            // A composite tag's value is handled directly by TlvWriter.
        }
    }
}