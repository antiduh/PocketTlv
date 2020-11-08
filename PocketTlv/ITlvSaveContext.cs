using System;

namespace PocketTlv
{
    /// <summary>
    /// Specifies the interface for contract types to save their data as TLV tags.
    /// </summary>
    public interface ITlvSaveContext
    {
        /// <summary>
        /// Writes a tag to the output.
        /// </summary>
        /// <param name="fieldId">The field ID to assign to the tag.</param>
        /// <param name="tag">The tag to write.</param>
        void Tag( int fieldId, ITag tag );

        /// <summary>
        /// Writes a sub-contract to the output.
        /// </summary>
        /// <param name="fieldId">The field ID to assign to the sub-contract.</param>
        /// <param name="subContract">The sub-contract to write.</param>
        void Contract( int fieldId, ITlvContract subContract );
    }
}