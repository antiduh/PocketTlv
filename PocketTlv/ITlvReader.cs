using System;

namespace PocketTlv
{
    /// <summary>
    /// Specifies the interface for classes that read TLV tags and contracts from data sources.
    /// </summary>
    public interface ITlvReader
    {
        /// <summary>
        /// Reads a <see cref="ITlvContract"/> from the data source.
        /// </summary>
        /// <remarks>
        /// When <see cref="ReadContract"/> determines the type to instantiate to use as the return
        /// value, it examines the contract ID read from the data source and compares it against all
        /// contracts previously registered through the <see cref="RegisterContract{T}"/> method.
        ///
        /// If no registration can be found, <see cref="ReadContract"/> returns an instance of the
        /// <see cref="UnresolvedContract"/> class to represent the read contract. This allows
        /// contracts to be read, stored, and forwarded in cases where the concrete contract type is
        /// not known. This is useful for cases where contract objects are simply being relayed
        /// across multiple systems, where some parts of the relay do not have access to the the
        /// concrete types.
        ///
        /// <see cref="UnresolvedContract"/> instances can be resolved into the concrete type object
        /// after the fact by calling the <see cref="UnresolvedExtensions.Resolve{T}(ITlvContract)"/>
        /// extension method on the <see cref="UnresolvedContract"/> instance.
        ///
        /// If the contract identified by the data stream has been previously registered by calling
        /// <see cref="RegisterContract{T}"/>, then no intermediate <see cref="UnresolvedContract"/>
        /// is created, and instead the registered type is directly instantiated. This modality
        /// should be preferred when possible, since it is more efficient.
        /// </remarks>
        /// <returns></returns>
        ITlvContract ReadContract();

        /// <summary>
        /// Reads a contract of type <typeparamref name="T"/> from the data source. No prior
        /// registration of the contract type through <see cref="RegisterContract{T}"/> is needed.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the <see cref="ITlvContract"/> to expect to read from the data source.
        /// </typeparam>
        /// <returns>
        /// An instance of <typeparamref name="T"/> representing the data read from the data source.
        /// </returns>
        /// <exception cref="ContractTypeMismatchException">
        /// Occurs if the contract ID read from the data source does not match the contract defined
        /// by <typeparamref name="T"/>.
        /// </exception>
        T ReadContract<T>() where T : ITlvContract, new();

        /// <summary>
        /// Reads a single <see cref="ITag"/> from the data source.
        /// </summary>
        /// <returns></returns>
        ITag ReadTag();

        /// <summary>
        /// Reads a single <see cref="ITag"/> of type <typeparamref name="T"/> from the data source.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ITag"/> to read from the stream.</typeparam>
        /// <returns>An instance of <see cref="ITag"/> representing the data read from the data source.</returns>
        T ReadTag<T>() where T : ITag;
    }
}