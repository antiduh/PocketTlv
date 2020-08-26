using System;

namespace PocketTLV.Primitives
{
    /// <summary>
    /// Represents a primitive TLV tag.
    /// </summary>
    public interface ITag
    {
        // The wire format of a TLV in this library is as follows:

        // +-------------------+-------------------+------------------+-------------------------+
        // | FieldId (12 bits) | WireType (4 bits) | Length (32 bits) |  Value (`Length` bytes) |
        // +-------------------+-------------------+------------------+-------------------------+

        // Effectively the FieldId and the WireType are packed into a 2-byte Type field.
        //
        // The fieldID is used by callers to assign ID labels on tags, so that they can be
        // identified later by their ID instead of their position.
        //
        // The WireType is used by the library to identify which primitive type was written to the
        // byte stream.

        /// <summary>
        /// Gets or sets a value that identifies the tag as a field in a larger structure. Field IDs
        /// are provided by callers.
        /// </summary>
        int FieldId { get; set; }

        /// <summary>
        /// Gets a value that identifies the primitive type ID that is represented by the tag.
        /// </summary>
        WireType WireType { get; }

        /// <summary>
        /// Returns the length of the value portion of the tag in bytes.
        /// </summary>
        /// <returns></returns>
        int ComputeLength();

        /// <summary>
        /// Reads the tag's data from the byte stream.
        /// </summary>
        /// <param name="buffer">The byte stream to read from.</param>
        /// <param name="position">The index of the first byte to read from.</param>
        /// <param name="length">The maximum amount of data that can be read.</param>
        void ReadValue( byte[] buffer, int position, int length );

        /// <summary>
        /// Writes the tag's data to the byte stream.
        /// </summary>
        /// <param name="buffer">The byte stream to write to.</param>
        /// <param name="position">The offset in the byte stream to start writing at.</param>
        void WriteValue( byte[] buffer, int position );
    }
}