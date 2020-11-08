using System;
using System.Collections.Generic;

namespace PocketTlv
{
    /// <summary>
    /// Saves TLV tags to a tag stream.
    /// </summary>
    public class TlvSaveContext : ITlvSaveContext
    {
        private List<ITag> children;

        /// <summary>
        /// Initializes a new instance of the <see cref="TlvSaveContext"/> type.
        /// </summary>
        /// <param name="children">The list to write saved tags to.</param>
        public TlvSaveContext( List<ITag> children )
        {
            this.children = children;
        }

        /// <summary>
        /// Writes a tag to the tag stream.
        /// </summary>
        /// <param name="fieldId">The field ID to assign to the tag.</param>
        /// <param name="tag">The tag to save.</param>
        public void Tag( int fieldId, ITag tag )
        {
            tag.FieldId = fieldId;
            this.children.Add( tag );
        }

        /// <summary>
        /// Writes a contract to the tag stream. Contracts are represented as <see
        /// cref="ContractTag"/> objects in the tag stream.
        /// </summary>
        /// <param name="fieldId">The field ID to assign to the contract.</param>
        /// <param name="subContract">The contract to save.</param>
        public void Contract( int fieldId, ITlvContract subContract )
        {
            // When saving sub-contracts, we do "value-stuffing":
            // - It's handy to have the contract ID when parsing, for error checking.
            // - It's necessary to have the contract ID when doing deferred parsing for
            //   UnknownContract resolution after the fact.
            // - So we "value-stuff": we put in our own tag in before the contract's tags, and then
            //   hide the value from the real type when they read from the CompositeTag.
            // - The actual value of the fieldId we pass down doesn't matter. It never gets used.

            // A tag to represent the subcontract.
            var subcontractTag = new ContractTag( fieldId, subContract.ContractId );

            // Tell the contract to serialize itself through us by swapping which CompositeTag we're
            // pointing to (effectively doing recursion via the call stack).
            List<ITag> backup = this.children;

            this.children = subcontractTag.Children;
            subContract.Save( this );
            this.children = backup;

            // Save the composite tag representing the subcontract to our parent.
            this.children.Add( subcontractTag );
        }
    }
}