using System;
using System.Collections.Generic;
using System.Linq;

namespace PocketTlv
{
    public interface ITlvParseContext
    {
        T Tag<T>( int fieldId ) where T : ITag;

        T Contract<T>( int fieldId ) where T : ITlvContract, new();

        ITlvContract Contract( int fieldId );
    }

    public class TlvParseContext : ITlvParseContext
    {
        private CompositeTag source;
        private readonly bool hideFirst;

        public TlvParseContext( CompositeTag source, bool hideFirst )
        {
            this.source = source;
            this.hideFirst = hideFirst;
        }

        public bool HasField( int fieldId )
        {
            var children = source.Children;

            for( int i = hideFirst ? 1 : 0; i < children.Count; i++ )
            {
                if( children[i].FieldId == fieldId )
                {
                    return true;
                }
            }

            return false;
        }

        public bool TryTag<T>( int fieldId, out T tag )
        {
            var children = source.Children;

            for( int i = hideFirst ? 1 : 0; i < children.Count; i++ )
            {
                if( children[i].FieldId == fieldId )
                {
                    tag = (T)children[i];
                    return true;
                }
            }

            tag = default( T );
            return true;
        }

        public bool TryContract<T>( int fieldId, out T contract ) where T : ITlvContract, new()
        {
            // Technically this method could be implemented by calling `TryContract( int,
            // ITlvContract)` and then calling Resolve() on the returned object. Implementing it the
            // way that we have skips the intermediate allocation+parsing of UnresolvedContract, however.

            CompositeTag contractTag;
            int foundContractId;

            if( GetContractSubTag( fieldId, out contractTag, out foundContractId ) == false )
            {
                contract = default( T );
                return false;
            }

            T result = new T();
            if( result.ContractId != foundContractId )
            {
                throw new InvalidOperationException( "Type mismatch found: contract IDs don't match." );
            }

            var subContext = new TlvParseContext( contractTag, true );
            result.Parse( subContext );

            contract = result;
            return true;
        }

        public bool TryContract( int fieldId, out ITlvContract contract )
        {
            CompositeTag contractTag;
            int foundContractId;

            if( GetContractSubTag( fieldId, out contractTag, out foundContractId ) )
            {
                contract = new UnresolvedContract( contractTag, foundContractId );
                return true;
            }

            contract = null;
            return false;
        }

        public T Tag<T>( int fieldId ) where T : ITag
        {
            if( TryTag<T>( fieldId, out T tag ) )
            {
                return tag;
            }

            throw new KeyNotFoundException( $"No TLV value was found with fieldId = {fieldId}." );
        }

        public T Contract<T>( int fieldId ) where T : ITlvContract, new()
        {
            if( TryContract<T>( fieldId, out T contract ) )
            {
                return contract;
            }

            throw new KeyNotFoundException( $"No TLV value was found with fieldId = {fieldId}." );
        }

        public ITlvContract Contract( int fieldId )
        {
            if( TryContract( fieldId, out ITlvContractcontract ) )
            {
                return contract;
            }

            throw new KeyNotFoundException( $"No TLV value was found with fieldId = {fieldId}." );
        }

        private bool GetContractSubTag( int fieldId, out CompositeTag contractTag, out int foundContractId )
        {
            if( TryTag<CompositeTag>( fieldId, out contractTag ) )
            {
                // See TlvSaveContext.Save. We use value-stuffing to save the contract ID of the
                // serialized contract.
                foundContractId = (ContractIdTag)contractTag.Children.First();
                return true;
            }
            else
            {
                foundContractId = -1;
                return false;
            }
        }
    }
}