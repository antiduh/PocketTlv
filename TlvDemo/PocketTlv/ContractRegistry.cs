using System;
using System.Collections.Generic;

namespace PocketTlv
{
    /// <summary>
    /// Registers types that implement the <see cref="ITlvContract"/> interface by their contract
    /// ID, to allow later instantiation by contract ID.
    /// </summary>
    public class ContractRegistry
    {
        private readonly Dictionary<int, IRegistration> registrations;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractRegistry"/> class.
        /// </summary>
        public ContractRegistry()
        {
            this.registrations = new Dictionary<int, IRegistration>();
        }

        /// <summary>
        /// Registers a type implementing the <see cref="ITlvContract"/> interface.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Register<T>() where T : ITlvContract, new()
        {
            // Instantiate a dummy copy just to read out its contract ID.
            T test = new T();

            // Make sure this is the first time we've seen this one.
            if( this.registrations.TryGetValue( test.ContractId, out IRegistration reg ) )
            {
                if( typeof( T ) == reg.TypeInfo() )
                {
                    // The type has already been registered.
                    return;
                }
                else
                {
                    string msg = "Cannot register contract {0} with contract ID {1} - contract ID is already registered as {2}.";
                    msg = string.Format( msg, typeof( T ).Name, test.ContractId, reg.TypeInfo().Name );
                    throw new InvalidOperationException( msg );
                }
            }

            // Store the registration.
            this.registrations.Add( test.ContractId, new Registration<T>() );
        }

        /// <summary>
        /// Returns whether a type has been registered for the given contract ID.
        /// </summary>
        /// <param name="contractId">The contract ID to check.</param>
        /// <returns>
        /// True if a contract implementation has been registered for the given ID, false otherwise.
        /// </returns>
        public bool Contains( int contractId )
        {
            return this.registrations.ContainsKey( contractId );
        }

        /// <summary>
        /// Attempts to create an instance of the registered type that provides the contract implementation for
        /// the given contract ID, returning a value indicating whether the operation was successful.
        /// </summary>
        /// <param name="contractId">The contract ID to obtain an <see cref="ITlvContract"/> instance for.</param>
        /// <param name="contract">Returns a new instance of the contract type if succesful.</param>
        /// <returns>True if a contract could be instantiated for the given contract ID, false otherwise.</returns>
        public bool TryGet( int contractId, out ITlvContract contract )
        {
            if( this.registrations.TryGetValue( contractId, out IRegistration reg ) )
            {
                contract = reg.Create();
                return true;
            }
            else
            {
                contract = null;
                return false;
            }
        }

        /// <summary>
        /// Creates a new instance of the registered type that provides the contract implementation
        /// for the given contract ID.
        /// </summary>
        /// <param name="contractId">
        /// The ID of the contract to obtain an <see cref="ITlvContract"/> instance for.
        /// </param>
        /// <returns>
        /// A newly constructed instance of the <see cref="ITlvContract"/> type registered for the
        /// given contract ID.
        /// </returns>
        /// <exception cref="KeyNotFoundException">Thrown if no contract is currently registered for the given <paramref name="contractId"/>.</exception>
        public ITlvContract Get( int contractId )
        {
            if( TryGet( contractId, out ITlvContract result ) )
            {
                return result;
            }
            else
            {
                throw new KeyNotFoundException( $"No registration found for contract ID {contractId}." );
            }
        }

        private interface IRegistration
        {
            ITlvContract Create();

            Type TypeInfo();
        }

        public class Registration<T> : IRegistration where T : ITlvContract, new()
        {
            public ITlvContract Create()
            {
                return new T();
            }

            public Type TypeInfo()
            {
                return typeof( T );
            }
        }
    }
}