using System;
using System.Diagnostics;
using System.IO;
using TlvDemo.TlvApi;

namespace TlvDemo
{
    internal class Program
    {
        private static void Main( string[] args )
        {
            PrimitivesTest();
            SimpleContractTest();
            StaticContractCompositionTest();
        }

        /// <summary>
        /// Demonstrates how to directly write out TLV tags by hand.
        /// </summary>
        private static void PrimitivesTest()
        {
            // This leaves the fieldIds all set to zero. If you were to parse this, you'd need to
            // assume the order and structure below never changes. You can init the fieldIds if you
            // want, i just don't bother.
            var top = new CompositeTag(
                new StringTag( "Hello world" ),
                new IntTag( 42 ),
                new CompositeTag(
                    new StringTag( "Kevin Thompson" ),
                    new IntTag( 37 ),
                    new StringTag( "50 Hampden Rd" )
                )
            );

            var stream = new MemoryStream();
            var writer = new TlvWriter( stream );
            writer.Write( top );

            var copy = RoundTrip( top );
            Debug.Assert( copy.Equals( top ) );
        }

        /// <summary>
        /// Demonstrates how to write out a simple contract that has statically resolved members.
        /// </summary>
        private static void SimpleContractTest()
        {
            var record = new AddressRecord()
            {
                LotNumber = 50,
                StreetName = "Hampden Rd",
            };

            var copy = RoundTrip( record );
            Debug.Assert( copy.Equals( record ) );
        }

        /// <summary>
        /// Demonstrates that contracts can themselves contain contracts. In this demo, PersonRecord
        /// has a concrete property typed on AddressRecord.
        /// </summary>
        private static void StaticContractCompositionTest()
        {
            var record = new PersonRecord()
            {
                Name = "Kevin Thompson",
                Age = 37,
                Address = new AddressRecord() { LotNumber = 50, StreetName = "50 Hampden Rd" },
            };

            var copy = RoundTrip( record );
            Debug.Assert( copy.Equals( record ) );
        }

        private static void LoadStorePersonRecord()
        {
            // --- Contracts can be statically composed ---
            PersonRecord personRecordOrig = new PersonRecord()
            {
                Name = "Kevin Thompson",
                Age = 37,
                Address = new AddressRecord() { LotNumber = 50, StreetName = "50 Hampden Rd" },
            };

            // --- Contracts can by dynamically composed ---
            MessageRecord messageRecord = new MessageRecord()
            {
                Name = "VorenEcho",
                Message = personRecordOrig
            };

            var messageCopy = RoundTrip( messageRecord );

            PersonRecord personCopy;

            // Method 1:

            personCopy = messageCopy.Message.Resolve<PersonRecord>();
            ProcessPersonRecord( personCopy );

            // Method 2:
            if( messageCopy.Message.ContractId == PersonRecord.ContractId )
            {
                personCopy = messageCopy.Message.Resolve<PersonRecord>();
                ProcessPersonRecord( personCopy );
            }

            // Method 3:
            if( messageCopy.Message.TryResolve<PersonRecord>( out personCopy ) )
            {
                ProcessPersonRecord( personCopy );
            }

            // --- Incomplete constructions can be relayed in their incomplete form ---
            messageCopy = RoundTrip( messageRecord );
            var messageCopy2 = RoundTrip( messageCopy );

            personCopy = messageCopy2.Message.Resolve<PersonRecord>();
            ProcessPersonRecord( personCopy );
        }

        private static void ProcessPersonRecord( PersonRecord record )
        {
            Console.WriteLine( record.Name );
        }

        private static ITag RoundTrip( ITag tag )
        {
            var stream = new MemoryStream();

            TlvWriter writer = new TlvWriter( stream );
            writer.Write( tag );

            // This so i can look at the byte stream in the debugger. Demo only.
            byte[] buffer = stream.GetBuffer();

            stream.Position = 0L;

            TlvReader reader = new TlvReader( stream );
            return reader.ReadTag();
        }

        private static T RoundTrip<T>( T contract ) where T : ITlvContract, new()
        {
            var stream = new MemoryStream();

            TlvWriter writer = new TlvWriter( stream );
            writer.Write( contract );

            // This so i can look at the byte stream in the debugger. Demo only.
            byte[] buffer = stream.GetBuffer();

            stream.Position = 0L;

            TlvReader reader = new TlvReader( stream );
            return reader.Read<T>();
        }
    }
}