using System;

namespace PocketTlv
{
    /// <summary>
    /// Specifies the interface to writes TLV tags to a data source, such as a stream.
    /// </summary>
    public interface ITlvWriter
    {
        /// <summary>
        /// Writes a TLV tag to the data source.
        /// </summary>
        /// <param name="tag">The tag to write.</param>
        void Write( ITag tag );

        /// <summary>
        /// Writes a contract to the data source.
        /// </summary>
        /// <param name="contract">The contract to write.</param>
        void Write( ITlvContract contract );
    }
}