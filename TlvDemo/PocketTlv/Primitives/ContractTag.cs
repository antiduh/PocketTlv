using System;
using System.Collections.Generic;
using PocketTlv.ClassLib;

namespace PocketTlv
{
    public class ContractTag : ITag
    {
        public ContractTag()
        {
            this.Children = new List<ITag>();
        }

        public ContractTag( params ITag[] children )
            : this()
        {
            this.Children.AddRange( children );
        }

        public ContractTag( int fieldId, params ITag[] children )
            : this()
        {
            this.FieldId = fieldId;
            this.Children.AddRange( children );
        }
        
        public int FieldId { get; set; }

        public int ContractId { get; set; }

        public List<ITag> Children { get; private set; }


        /// <summary>
        /// Returns a string describing the number of children in the tag.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"ContractTag - {this.Children.Count} children";
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
            return Equals( other as ContractTag );
        }

        /// <summary>
        /// Compares two <see cref="ContractTag"/> objects.
        /// </summary>
        /// <param name="other">The tag to compare to.</param>
        /// <returns></returns>
        public bool Equals( ContractTag other )
        {
            return this == other;
        }

        /// <summary>
        /// Returns a hash code for a <see cref="ContractTag"/> instance.
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
        public static bool operator ==( ContractTag left, ContractTag right )
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
        /// Compares two <see cref="ContractTag"/> objects to determine if they are different.
        /// </summary>
        /// <param name="left">The first tag to compare.</param>
        /// <param name="right">The second tag to compare.</param>
        /// <returns>True if the two tags are not equal.</returns>
        public static bool operator !=( ContractTag left, ContractTag right )
        {
            return !( left == right );
        }

        // --- ITag implementation ---

        WireType ITag.WireType => WireType.Contract;

        int ITag.ComputeLength()
        {
            int length = 0;

            // Length of our ContractId field.
            length += 4;

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
            // A contract tag's value is handled directly by TlvReader.
        }

        void ITag.WriteValue( byte[] buffer, int position )
        {
            // A contract tag's value is handled directly by TlvWriter.
        }
    }
}
