using System;

namespace PocketTlv
{
    /// <summary>
    /// Specifies the interface for complete messages composed of various TLV parts.
    /// </summary>
    public interface ITlvContract
    {
        /// <summary>
        /// Gets a value that uniquely identifies this contract from all other contracts. The
        /// ContractID is written to the byte stream in order to reconstitute the contract during
        /// the deserialization process.
        /// </summary>
        int ContractId { get; }

        /// <summary>
        /// Reads the contract's values from TLV tags.
        /// </summary>
        /// <param name="parse"></param>
        void Parse( ITlvParseContext parse );

        /// <summary>
        /// Writes the contract's values as TLV tags.
        /// </summary>
        /// <param name="save"></param>
        void Save( ITlvSaveContext save );
    }
}