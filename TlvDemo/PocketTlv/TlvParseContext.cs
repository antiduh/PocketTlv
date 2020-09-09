﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace PocketTlv
{
    /// <summary>
    /// Parses TLV fields from a TLV tag stream.
    /// </summary>
    public class TlvParseContext : ITlvParseContext
    {
        private List<ITag> children;

        /// <summary>
        /// Initializes a new instance of the <see cref="TlvParseContext"/> class.
        /// </summary>
        /// <param name="children">The list of nodes to parse from.</param>
        public TlvParseContext( List<ITag> children )
        {
            this.children = children;
        }

        /// <summary>
        /// Determines whether the TLV stream contains the specified field.
        /// </summary>
        /// <param name="fieldId">The ID of the field to locate.</param>
        /// <returns></returns>
        public bool HasField( int fieldId )
        {
            for( int i = 0; i < children.Count; i++ )
            {
                if( children[i].WireType != WireType.ContractId && children[i].FieldId == fieldId )
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Attempts to parse a <see cref="ITag"/> from the TLV stream. A return value indicates
        /// whether the operation succeeded.
        /// </summary>
        /// <typeparam name="T">The <see cref="ITag"/> type to parse.</typeparam>
        /// <param name="fieldId">The ID of the field to parse.</param>
        /// <param name="tag">If successful, returns the parsed tag.</param>
        /// <returns>True if the field was successfully located, false otherwise.</returns>
        public bool TryTag<T>( int fieldId, out T tag ) where T : ITag 
        {
            for( int i = 0; i < children.Count; i++ )
            {
                if( children[i].WireType != WireType.ContractId && children[i].FieldId == fieldId )
                {
                    tag = (T)children[i];
                    return true;
                }
            }

            tag = default( T );
            return true;
        }

        /// <summary>
        /// Attempts to parse a <see cref="ITlvContract"/> of the given type from the TLV stream. A return value
        /// indicates whether the operation succeeded.
        /// </summary>
        /// <typeparam name="T">The <see cref="ITlvContract"/> to parse.</typeparam>
        /// <param name="fieldId">The ID of the field to parse.</param>
        /// <param name="contract">If successful, returns the parsed contract.</param>
        /// <returns>True if the field was successfully located, falase otherwise.</returns>
        public bool TryContract<T>( int fieldId, out T contract ) where T : ITlvContract, new()
        {
            // Technically this method could be implemented by calling `TryContract( int,
            // ITlvContract)` and then calling Resolve() on the returned object. Implementing it the
            // way that we have skips the intermediate allocation+parsing of UnresolvedContract, however.

            ContractTag contractTag = GetContractSubTag( fieldId );

            if( contractTag == null )
            {
                contract = default( T );
                return false;
            }

            T result = new T();
            if( result.ContractId != contractTag.ContractId )
            {
                throw new InvalidOperationException( "Type mismatch found: contract IDs don't match." );
            }

            var subContext = new TlvParseContext( contractTag.Children );
            result.Parse( subContext );

            contract = result;
            return true;
        }

        /// <summary>
        /// Attempts to parse a <see cref="ITlvContract"/> from the TLV stream. A return value
        /// indicates whether the operation succeeded.
        /// </summary>
        /// <param name="fieldId">The ID of the field to parse.</param>
        /// <param name="contract">If successful, returns the parsed contract.</param>
        /// <returns>True if the field was successfully located, falase otherwise.</returns>
        public bool TryContract( int fieldId, out ITlvContract contract )
        {
            ContractTag contractTag = GetContractSubTag( fieldId );

            // Since the caller gave us no type information, we have to return them an unresolved contract.
            if( contractTag == null  )
            {
                contract = null;
                return false;
                
            }

            contract = new UnresolvedContract( contractTag, contractTag.ContractId );
            return true;
        }

        /// <summary>
        /// Parses a <see cref="ITag"/> from the TLV stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldId"></param>
        /// <returns>The parsed tag.</returns>
        /// <exception cref="KeyNotFoundException">
        /// No field with the value <paramref name="fieldId"/> could be found.
        /// </exception>
        public T Tag<T>( int fieldId ) where T : ITag
        {
            if( TryTag<T>( fieldId, out T tag ) )
            {
                return tag;
            }

            throw new KeyNotFoundException( $"No TLV value was found with fieldId = {fieldId}." );
        }

        /// <summary>
        /// Parses a <see cref="ITlvContract"/> of the given type from the TLV stream.
        /// </summary>
        /// <typeparam name="T">The <see cref="ITlvContract"/> type to parse.</typeparam>
        /// <param name="fieldId">The ID of the field to parse.</param>
        /// <returns>The parsed contract</returns>
        /// <exception cref="KeyNotFoundException">
        /// No field with the value <paramref name="fieldId"/> could be found.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// The contract with the value <paramref name="fieldId"/> does not match the type
        /// <typeparamref name="T"/>.
        /// </exception>
        public T Contract<T>( int fieldId ) where T : ITlvContract, new()
        {
            if( TryContract<T>( fieldId, out T contract ) )
            {
                return contract;
            }

            throw new KeyNotFoundException( $"No TLV value was found with fieldId = {fieldId}." );
        }

        /// <summary>
        /// Parses a <see cref="ITlvContract"/> from the TLV stream.
        /// </summary>
        /// <param name="fieldId">The ID of the field to parse.</param>
        /// <returns>The parsed contract.</returns>
        /// <exception cref="KeyNotFoundException">
        /// No field with the value <paramref name="fieldId"/> could be found.
        /// </exception>
        public ITlvContract Contract( int fieldId )
        {
            if( TryContract( fieldId, out ITlvContract contract ) )
            {
                return contract;
            }

            throw new KeyNotFoundException( $"No TLV value was found with fieldId = {fieldId}." );
        }

        private ContractTag GetContractSubTag( int fieldId )
        {
            if( TryTag<ContractTag>( fieldId, out ContractTag subContractTag ) )
            {
                // See TlvSaveContext.Save. We use value-stuffing to save the contract ID of the
                // serialized contract.
                return subContractTag;
            }
            else
            {
                return null;
            }
        }
    }
}