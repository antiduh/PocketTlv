﻿using System;
using System.Collections.Generic;
using PocketTlv.ClassLib;

namespace PocketTlv
{
    /// <summary>
    /// Represents a TLV tag that can store other tags as children tags.
    /// </summary>
    public class CompositeTag : ITag
    {
        /// <summary>
        /// Initializes an initially-empty instance of <see cref="CompositeTag"/>.
        /// </summary>
        public CompositeTag()
        {
            this.Children = new List<ITag>();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="CompositeTag"/> with the given children nodes.
        /// </summary>
        /// <param name="children">An array of tags to store.</param>
        public CompositeTag( params ITag[] children )
            : this()
        {
            this.Children.AddRange( children );
        }

        /// <summary>
        /// Initializes a new instance of <see cref="CompositeTag"/> with the given field ID and
        /// children nodes.
        /// </summary>
        /// <param name="fieldId">The TLV field ID that identifies the tag.</param>
        /// <param name="children">An array of tags to store.</param>
        public CompositeTag( int fieldId, params ITag[] children )
            : this()
        {
            this.FieldId = fieldId;
            this.Children.AddRange( children );
        }

        /// <summary>
        /// Gets the list of children tags.
        /// </summary>
        public List<ITag> Children { get; private set; }

        /// <summary>
        /// Gets or sets the TLV field ID that the tag represents.
        /// </summary>
        public int FieldId { get; set; }

        /// <summary>
        /// Returns a string describing the number of children in the tag.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"CompositeTag - {this.Children.Count} children";
        }

        /// <summary>
        /// Compares this <see cref="CompositeTag"/> to the given object.
        /// </summary>
        /// <param name="other">The object to compare to.</param>
        /// <returns>
        /// True if the object is a <see cref="CompositeTag"/> and contains the same children values
        /// as this object.
        /// </returns>
        public override bool Equals( object other )
        {
            return Equals( other as CompositeTag );
        }

        /// <summary>
        /// Compares two <see cref="CompositeTag"/> objects.
        /// </summary>
        /// <param name="other">The tag to compare to.</param>
        /// <returns></returns>
        public bool Equals( CompositeTag other )
        {
            return this == other;
        }

        /// <summary>
        /// Returns a hash code for a <see cref="CompositeTag"/> instance.
        /// </summary>
        /// <returns>An integer hash code.</returns>
        public override int GetHashCode()
        {
            return HashHelper.GetHashCode( this.Children );
        }

        /// <summary>
        /// Compares two <see cref="CompositeTag"/> objects to determine if they are same.
        /// </summary>
        /// <param name="left">The first tag to compare.</param>
        /// <param name="right">The second tag to compare.</param>
        /// <returns>True if the two tags are equal.</returns>
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
                    ITag leftChild = left.Children[i];
                    ITag rightChild = right.Children[i];

                    if( leftChild.FieldId != rightChild.FieldId || leftChild.Equals( rightChild ) == false )
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Compares two <see cref="CompositeTag"/> objects to determine if they are different.
        /// </summary>
        /// <param name="left">The first tag to compare.</param>
        /// <param name="right">The second tag to compare.</param>
        /// <returns>True if the two tags are not equal.</returns>
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
            int amountRead = 0;

            while( amountRead < length )
            {
                int subAmountRead;

                ITag child = TagBufferReader.Read( buffer, position, out subAmountRead );
                position += subAmountRead;
                amountRead += subAmountRead;

                this.Children.Add( child );
            }
        }

        void ITag.WriteValue( byte[] buffer, int position )
        {
            foreach( ITag child in this.Children )
            {
                position += TagBufferWriter.Write( child, buffer, position );
            }
        }
    }
}