using System;
using System.Collections.Generic;
using System.Linq;

namespace PocketTlv
{
    /// <summary>
    /// Specifies the interface for parsing TLV fields from a contract's TLV stream.
    /// </summary>
    public interface ITlvParseContext
    {
        /// <summary>
        /// Determines whether the TLV stream contains the specified field.
        /// </summary>
        /// <param name="fieldId">The ID of the field to locate.</param>
        /// <returns></returns>
        bool HasField( int fieldId );

        /// <summary>
        /// Attempts to parse a <see cref="ITag"/> from the TLV stream. A return value indicates
        /// whether the operation succeeded.
        /// </summary>
        /// <typeparam name="T">The <see cref="ITag"/> type to parse.</typeparam>
        /// <param name="fieldId">The ID of the field to parse.</param>
        /// <param name="tag">If successful, returns the parsed tag.</param>
        /// <returns>True if the field was successfully located, false otherwise.</returns>
        bool TryTag<T>( int fieldId, out T tag ) where T : ITag;

        /// <summary>
        /// Attempts to parse a <see cref="ITlvContract"/> of the given type from the TLV stream. A return value
        /// indicates whether the operation succeeded.
        /// </summary>
        /// <typeparam name="T">The <see cref="ITlvContract"/> to parse.</typeparam>
        /// <param name="fieldId">The ID of the field to parse.</param>
        /// <param name="contract">If successful, returns the parsed contract.</param>
        /// <returns>True if the field was successfully located, falase otherwise.</returns>
        bool TryContract<T>( int fieldId, out T contract ) where T : ITlvContract, new();

        /// <summary>
        /// Attempts to parse a <see cref="ITlvContract"/> from the TLV stream. A return value
        /// indicates whether the operation succeeded.
        /// </summary>
        /// <param name="fieldId">The ID of the field to parse.</param>
        /// <param name="contract">If successful, returns the parsed contract.</param>
        /// <returns>True if the field was successfully located, falase otherwise.</returns>
        bool TryContract( int fieldId, out ITlvContract contract );

        /// <summary>
        /// Parses a <see cref="ITag"/> from the TLV stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldId"></param>
        /// <returns>The parsed tag.</returns>
        /// <exception cref="KeyNotFoundException">
        /// No field with the value <paramref name="fieldId"/> could be found.
        /// </exception>
        T Tag<T>( int fieldId ) where T : ITag;

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
        T Contract<T>( int fieldId ) where T : ITlvContract, new();

        /// <summary>
        /// Parses a <see cref="ITlvContract"/> from the TLV stream.
        /// </summary>
        /// <param name="fieldId">The ID of the field to parse.</param>
        /// <returns>The parsed contract.</returns>
        /// <exception cref="KeyNotFoundException">
        /// No field with the value <paramref name="fieldId"/> could be found.
        /// </exception>
        ITlvContract Contract( int fieldId );
    }
}