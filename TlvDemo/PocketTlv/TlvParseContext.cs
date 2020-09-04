﻿using System;
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

        public T Tag<T>( int fieldId ) where T : ITag
        {
            var children = source.Children;

            for( int i = hideFirst ? 1 : 0; i < children.Count; i++ )
            {
                if( children[i].FieldId == fieldId )
                {
                    return (T)children[i];
                }
            }

            throw new KeyNotFoundException( $"No TLV value was found with fieldId = {fieldId}." );
        }

        public T Contract<T>( int fieldId ) where T : ITlvContract, new()
        {
            CompositeTag contractTag;
            int foundContractId;

            GetContractSubTag( fieldId, out contractTag, out foundContractId );

            T result = new T();

            if( result.ContractId != foundContractId )
            {
                throw new InvalidOperationException( "Type mismatch found: contract IDs don't match." );
            }

            var subContext = new TlvParseContext( contractTag, true );
            result.Parse( subContext );

            return result;
        }

        public ITlvContract Contract( int fieldId )
        {
            CompositeTag contractTag;
            int foundContractId;

            GetContractSubTag( fieldId, out contractTag, out foundContractId );

            return new UnresolvedContract( contractTag, foundContractId );
        }

        private void GetContractSubTag( int fieldId, out CompositeTag contractTag, out int foundContractId )
        {
            contractTag = Tag<CompositeTag>( fieldId );

            // See TlvSaveContext.Save. We use value-stuffing to save the contract ID of the
            // serialized contract.

            foundContractId = (ContractIdTag)contractTag.Children.First();
        }
    }
}