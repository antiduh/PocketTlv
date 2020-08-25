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
            AnonymousContractCompositionTest();
            AnonymousContractRetransmitTest();
            ReadDirectAnonymousContractTest();
        }

        /// <summary>
        /// Demonstrates how to directly write out TLV tags by hand.
        /// </summary>
        private static void PrimitivesTest()
        {
            var top = new CompositeTag( 4242,
                new StringTag( 1, "Hello world" ),
                new IntTag( 2, 42 ),
                new CompositeTag( 3,
                    new StringTag( 1, "Kevin Thompson" ),
                    new IntTag( 2, 37 ),
                    new StringTag( 3, "50 Hampden Rd" ),
                    new DoubleTag( 4, 1.6180339 ),
                    new ByteArrayTag( 5, new byte[] { 1, 2, 3 } )
                )
            );

            var stream = new MemoryStream();
            var writer = new TlvWriter( stream );
            writer.Write( top );

            var copy = RoundTrip( top );
            Debug.Assert( copy.Equals( top ) );
        }

        /// <summary>
        /// Demonstrates how to write out a simple contract that has statically defined and resolved members.
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

        /// <summary>
        /// Demonstrates that contracts can store other contracts anonymously.
        /// </summary>
        private static void AnonymousContractCompositionTest()
        {
            var personRecord = new PersonRecord()
            {
                Name = "Kevin Thompson",
                Age = 37,
                Address = new AddressRecord() { LotNumber = 50, StreetName = "50 Hampden Rd" },
            };

            // MessageRecord can transmit arbitrary contracts as its Message property.
            var personMsgBusRequest = new MessageRecord()
            {
                Name = "PersonRequest",
                Message = personRecord
            };

            // Note that upon deserialization, the Message field is filled with a place holder.
            var copy = RoundTrip( personMsgBusRequest );
            Debug.Assert( copy.Message is UnresolvedContract );

            PersonRecord personRecordCopy;

            // Method 1
            personRecordCopy = copy.Message.Resolve<PersonRecord>();
            Debug.Assert( personRecordCopy.Equals( personRecord ) );

            // Method 2:
            if( copy.Message.ContractId == PersonRecord.ContractId )
            {
                personRecordCopy = copy.Message.Resolve<PersonRecord>();
                Debug.Assert( personRecordCopy.Equals( personRecord ) );
            }
            else
            {
                Debug.Assert( false );
            }

            // Method 3:
            if( copy.Message.TryResolve<PersonRecord>( out personRecordCopy ) )
            {
                Debug.Assert( personRecordCopy.Equals( personRecord ) );
            }
            else
            {
                Debug.Assert( false );
            }
        }

        /// <summary>
        /// Demonstrates that contracts can store other contracts anonymously.
        /// </summary>
        private static void AnonymousContractRetransmitTest()
        {
            var personRecord = new PersonRecord()
            {
                Name = "Kevin Thompson",
                Age = 37,
                Address = new AddressRecord() { LotNumber = 50, StreetName = "50 Hampden Rd" },
            };

            // MessageRecord can transmit arbitrary contracts as its Message property.
            var personMsgBusRequest = new MessageRecord()
            {
                Name = "PersonRequest",
                Message = personRecord
            };

            // Note that upon deserialization, the Message field is filled with a place holder.
            var copy = RoundTrip( personMsgBusRequest );
            Debug.Assert( copy.Message is UnresolvedContract );

            // This demonstrates we can reserialize a live instance that's holding placeholders.
            var secondCopy = RoundTrip( copy );

            var personRecordCopy = secondCopy.Message.Resolve<PersonRecord>();
            Debug.Assert( personRecordCopy.Equals( personRecord ) );
        }

        private static void ReadDirectAnonymousContractTest()
        {
            var record = new AddressRecord()
            {
                LotNumber = 50,
                StreetName = "Hampden Rd",
            };

            var stream = new MemoryStream();
            var writer = new TlvWriter( stream );
            var reader = new TlvReader( stream );

            reader.RegisterContract<AddressRecord>();

            writer.Write( record );

            stream.Position = 0L;

            ITlvContract anonymousContract = reader.ReadContract();

            Debug.Assert( anonymousContract is AddressRecord );
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
            return reader.ReadContract<T>();
        }
    }
}