using System;
using System.Runtime.Serialization;

namespace PocketTlv
{
    /// <summary>
    /// Represents the exception that is thrown when the contract ID read from a data source does
    /// not match the expected contract ID.
    /// </summary>
    public class ContractTypeMismatchException : Exception
    {
        public ContractTypeMismatchException()
        {
        }

        public ContractTypeMismatchException( string message )
            : base( message )
        {
        }

        public ContractTypeMismatchException( string message, Exception innerException )
            : base( message, innerException )
        {
        }

        public ContractTypeMismatchException( int expectedContractId, int foundContractId )
            : this( MakeMessage( expectedContractId, foundContractId ), expectedContractId, foundContractId )
        {
        }

        public ContractTypeMismatchException( string message, int expectedContractId, int foundContractId )
            : base( message )
        {
            this.ExpectedContractId = expectedContractId;
            this.FoundContractId = foundContractId;
        }

        protected ContractTypeMismatchException( SerializationInfo info, StreamingContext context )
            : base( info, context )
        {
            this.ExpectedContractId = info.GetInt32( "ExpectedContractId" );
            this.FoundContractId = info.GetInt32( "FoundContractId" );
        }

        public int ExpectedContractId { get; private set; }

        public int FoundContractId { get; private set; }

        public override void GetObjectData( SerializationInfo info, StreamingContext context )
        {
            base.GetObjectData( info, context );

            info.AddValue( "ExpectedContractId", this.ExpectedContractId );
            info.AddValue( "FoundContractId", this.FoundContractId );
        }

        private static string MakeMessage( int expectedContractId, int foundContractId )
        {
            return
                "The contract ID read from the data source does not match the expected contract ID. " +
                $"Expected: {expectedContractId}, read: {foundContractId}";
        }
    }
}