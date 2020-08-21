using System;

namespace TlvDemo.TlvApi
{
    public interface ITlvSaveContext
    {
        void Save( int fieldId, ITag tag );

        void Save( int fieldId, ITlvContract subContract );
    }

    public class TlvSaveContext : ITlvSaveContext
    {
        private CompositeTag contractTag;

        public TlvSaveContext( CompositeTag contractTag )
        {
            this.contractTag = contractTag;
        }

        public void Save( int fieldId, ITag tag )
        {
            tag.FieldId = fieldId;
            this.contractTag.Children.Add( tag );
        }

        public void Save( int fieldId, ITlvContract subContract )
        {
            CompositeTag subcontractTag = new CompositeTag();

            var subSaveContext = new TlvSaveContext( subcontractTag );

            // When saving sub-contracts, we do "value-stuffing":
            // - It's handy to have the contract ID when parsing, for error checking.
            // - It's necessary to have the contract ID when doing deferred parsing for
            //   UnknownContract resolution after the fact.
            // - So we "value-stuff": we put in our own tag in before the contract's tags, and then
            //   hide the value from the real type when they read from the CompositeTag.
            // - The actual value of the fieldId we pass down doesn't matter. It never gets used.

            subSaveContext.Save( 0xABC, new ContractIdTag( subContract.ContractId ) );

            // Tell the contract to serialize itself.
            subContract.Save( subSaveContext );

            // Save the composite tag representing the contract to our parent.
            Save( fieldId, subcontractTag );
        }
    }
}