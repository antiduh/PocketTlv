using System;
using PocketTlv.ClassLib;

namespace PocketTlv
{
    /// <summary>
    /// Used internally to store TLV contract IDs as a TLV tag.
    /// </summary>
    /// <remarks>
    /// The <see cref="ContractIdTag"/> type is used internally by the library when serializing TLV
    /// contracts to TLV tag streams.
    ///
    /// Contract IDs are defined by contract implementations, and are used to uniquely identify
    /// contracts when parsing. The <see cref="ContractIdTag"/> stores the ID in the tag stream
    /// along side the regular children tags of a contract through a 'value-stuffing' scheme.
    /// Technically, the <see cref="ContractId"/> type stores an integer, same as <see
    /// cref="IntTag"/>, however, treating it as a separate type allows it to be distinguished from
    /// the data provided by the contract itself.
    /// </remarks>
    public class ContractIdTag : ITag
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractIdTag"/> type.
        /// </summary>
        public ContractIdTag()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractIdTag"/> type with the given
        /// contract ID value.
        /// </summary>
        /// <param name="contractId">A contract ID.</param>
        public ContractIdTag( int contractId )
        {
            this.ContractId = contractId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractIdTag"/> type with the given
        /// contract ID value.
        /// </summary>
        /// <param name="fieldId">The TLV field ID to associate to the tag.</param>
        /// <param name="contractId">A contract ID.</param>
        public ContractIdTag( int fieldId, int contractId )
        {
            this.FieldId = fieldId;
            this.ContractId = contractId;
        }

        /// <summary>
        /// Gets or sets the contract ID represented by the tag.
        /// </summary>
        public int ContractId { get; set; }

        /// <summary>
        /// Gets or sets the TLV field ID that the tag represents.
        /// </summary>
        public int FieldId { get; set; }

        /// <summary>
        /// Converts the <see cref="ContractIdTag"/>'s value to a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ContractId.ToString();
        }

        /// <summary>
        /// Returns whether this <see cref="ContractIdTag"/> is equal to the provided object.
        /// </summary>
        /// <param name="other">The object to compare to.</param>
        /// <returns>True if the other object has the same value as this object.</returns>
        public override bool Equals( object other )
        {
            return Equals( other as ContractIdTag );
        }

        /// <summary>
        /// Compares two <see cref="ContractIdTag"/> objects.
        /// </summary>
        /// <param name="other">The tag to compare to.</param>
        /// <returns>True if the tag object has the same value as this tag.</returns>
        public bool Equals( ContractIdTag other )
        {
            return this == other;
        }

        /// <summary>
        /// Returns a hash value for a <see cref="ContractIdTag"/> instance.
        /// </summary>
        /// <returns>An integer hash code.</returns>
        public override int GetHashCode()
        {
            return this.ContractId.GetHashCode();
        }

        /// <summary>
        /// Converts a <see cref="ContractIdTag"/> to a <see cref="int"/>.
        /// </summary>
        /// <param name="tag">The tag to convert.</param>
        public static implicit operator int( ContractIdTag tag )
        {
            return tag.ContractId;
        }

        /// <summary>
        /// Compares two <see cref="ContractIdTag"/> objects to determine if they contain the same value.
        /// </summary>
        /// <param name="left">The first tag to compare.</param>
        /// <param name="right">The second tag to compare.</param>
        /// <returns>True if the two tags contain equal values, false otherwise.</returns>
        public static bool operator ==( ContractIdTag left, ContractIdTag right )
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
                return left.ContractId == right.ContractId;
            }
        }

        /// <summary>
        /// Compares two <see cref="ContractIdTag"/> objects to determine if they contain different values.
        /// </summary>
        /// <param name="left">The first tag to compare.</param>
        /// <param name="right">The second tag to compare.</param>
        /// <returns>True if the two tags do not contain equal values, false otherwise.</returns>
        public static bool operator !=( ContractIdTag left, ContractIdTag right )
        {
            return !( left == right );
        }

        // --- ITag implementation ---

        WireType ITag.WireType => WireType.ContractId;

        int ITag.ComputeLength()
        {
            return 2;
        }

        void ITag.ReadValue( byte[] buffer, int position, int length )
        {
            this.ContractId = DataConverter.ReadShortLE( buffer, position );
        }

        void ITag.WriteValue( byte[] buffer, int position )
        {
            DataConverter.WriteShortLE( (short)this.ContractId, buffer, position );
        }
    }
}