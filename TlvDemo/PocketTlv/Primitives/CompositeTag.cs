using System;
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
        /// Stores the given tag as a child tag.
        /// </summary>
        /// <param name="child">A tag to store.</param>
        public void AddChild( ITag child )
        {
            if( child is null )
            {
                throw new ArgumentNullException( nameof( child ) );
            }

            this.Children.Add( child );
        }

        /// <summary>
        /// Stores the given tag as a child tag, overwriting the tag's <see cref="ITag.FieldId"/>
        /// property with the given field ID.
        /// </summary>
        /// <param name="fieldId">The child tag's field ID.</param>
        /// <param name="child">A tag to store.</param>
        public void AddChild( int fieldId, ITag child )
        {
            if( child is null )
            {
                throw new ArgumentNullException( nameof( child ) );
            }

            if( fieldId < 0 )
            {
                throw new ArgumentOutOfRangeException( nameof( fieldId ), "must at least zero." );
            }

            child.FieldId = fieldId;
            this.Children.Add( child );
        }

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
                ITag leftChild;
                ITag rightChild;

                for( int i = 0; i < left.Children.Count; i++ )
                {
                    leftChild = left.Children[i];
                    rightChild = right.Children[i];

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
            // A composite tag's value is handled directly by TlvReader.
        }

        void ITag.WriteValue( byte[] buffer, int position )
        {
            // A composite tag's value is handled directly by TlvWriter.
        }
    }
}