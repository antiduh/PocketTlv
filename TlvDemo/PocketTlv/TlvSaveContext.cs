using System;
using System.Collections.Generic;

namespace PocketTlv
{
    public class TlvSaveContext : ITlvSaveContext
    {
        private List<ITag> children;

        public TlvSaveContext( List<ITag> children )
        {
            this.children = children;
        }

        public void Tag( int fieldId, ITag tag )
        {
            tag.FieldId = fieldId;
            this.children.Add( tag );
        }

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