using System;
using System.Collections.Generic;

namespace PocketTlv
{
    public class ContractRegistry
    {
        private readonly Dictionary<int, IRegistration> registrations;

        public ContractRegistry()
        {
            this.registrations = new Dictionary<int, IRegistration>();
        }

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

        public bool Contains( int contractId )
        {
            return this.registrations.ContainsKey( contractId );
        }

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