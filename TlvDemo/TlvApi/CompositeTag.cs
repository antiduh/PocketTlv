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

        public CompositeTag( params ITag[] children )
        {
            this.Children.AddRange( children );
        }

        public List<ITag> Children { get; private set; }

        public override string ToString()
        {
            return $"CompositeTag - {this.Children.Count} children";
        }

        // --- ITag implementation ---

        int ITag.FieldId { get; set; }

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

            foreach( ITag child in this.Children )
            {
                position += TlvWriter.WriteInternal( child, ref buffer, position );
            }
        }
    }
}