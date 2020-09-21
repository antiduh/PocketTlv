using System;

namespace PocketTlv
{
    /// <summary>
    /// Represents a contract object has been parsed from a tag stream but has not been resolved to
    /// a concrete type.
    ///
    /// <see cref="UnresolvedContract"/> objects allow the original TLV tag stream to be
    /// reconstituted as if the <see cref="UnresolvedContract"/> never existed. This makes it easy
    /// to build systems that read and forward contracts without access to the concrete types that
    /// define the contracts.
    ///
    /// <see cref="UnresolvedContract"/> can later be resolved to concrete types by calling the <see
    /// cref="UnknownExtensions.Resolve{T}(ITlvContract)"/> methods.
    /// </summary>
    public class UnresolvedContract : ITlvContract
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnresolvedContract"/> class.
        /// </summary>
        /// <param name="tag">A <see cref="ContractTag"/> storing the contract's data.</param>
        public UnresolvedContract( ContractTag tag )
        {
            this.Tag = tag;
        }

        /// <summary>
        /// Gets the <see cref="ContractTag"/> that stores the contract's data.
        /// </summary>
        public ContractTag Tag { get; private set; }

        /// <summary>
        /// Gets the contract ID of the represented contract.
        /// </summary>
        public int ContractId => this.Tag.ContractId;

        void ITlvContract.Parse( ITlvParseContext parse )
        {
            // Empty on purpose. The data is provided through the constructor.
        }

        void ITlvContract.Save( ITlvSaveContext save )
        {
            ITag child;
            for( int i = 0; i < this.Tag.Children.Count; i++ )
            {
                child = this.Tag.Children[i];
                save.Tag( child.FieldId, child );
            }
        }
    }

    /// <summary>
    /// Provides extension methods for resolving <see cref="UnresolvedContract"/> objects to concrete contract types.
    /// </summary>
    public static class UnknownExtensions
    {
        /// <summary>
        /// Resolves the <see cref="UnresolvedContract"/> instance to an instance of type
        /// <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the concrete contract to resolve to.</typeparam>
        /// <param name="unknown">
        /// A reference to the <see cref="UnresolvedContract"/> object representing the contract to resolve.
        /// </param>
        /// <returns></returns>
        public static T Resolve<T>( this ITlvContract unknown ) where T : ITlvContract, new()
        {
            if( unknown.TryResolve( out T contract ) )
            {
                return contract;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Attempts to resolves the <see cref="UnresolvedContract"/> instance to an instance of type
        /// <typeparamref name="T"/>, returning a value to indicate whether the operation was successful.
        /// </summary>
        /// <typeparam name="T">The type of the concrete contract to resolve to.</typeparam>
        /// <param name="unknown">
        /// A reference to the <see cref="UnresolvedContract"/> object representing the contract to resolve.
        /// </param>
        /// <param name="contract">Returns the resolved contract if the operation is successful.</param>
        /// <returns>True if the resolution was successful, false otherwise.</returns>
        public static bool TryResolve<T>( this ITlvContract unknown, out T contract ) where T : ITlvContract, new()
        {
            if( unknown is T known )
            {
                contract = known;
                return true;
            }
            else if( unknown is UnresolvedContract internalUnknown )
            {
                TlvParseContext parser = new TlvParseContext( internalUnknown.Tag.Children );

                T bound = new T();

                if( bound.ContractId != internalUnknown.ContractId )
                {
                    contract = default;
                    return false;
                }

                bound.Parse( parser );

                contract = bound;
                return true;
            }
            else
            {
                contract = default;
                return false;
            }
        }
    }
}