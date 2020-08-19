using System;
using System.Collections.Generic;

namespace TlvDemo.TlvApi
{
    public class CompositeTag : ITag
    {
        public CompositeTag()
        {
            this.Children = new List<ITag>();
        }

        public CompositeTag( int fieldId )
            : this()
        {
            this.FieldId = fieldId;
        }

        public CompositeTag( int fieldId, params ITag[] children )
            : this( fieldId )
        {
            this.Children.AddRange( children );
        }

        public int FieldId { get; private set; }

        public WireType WireType => WireType.Composite;

        public List<ITag> Children { get; private set; }

        public int ComputeLength()
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

        public void ReadValue( byte[] buffer, int position, int length )
        {
            // A composite tag's value is handled directly by TlvReader.
        }

        public void WriteValue( byte[] buffer, int position )
        {
            // A composite tag's value is handled directly by TlvWriter.

            foreach( ITag child in this.Children )
            {
                position += TlvWriter.WriteInternal( child, ref buffer, position );
            }
        }

        public override string ToString()
        {
            return $"CompositeTag - {this.Children.Count} children";
        }
    }
}