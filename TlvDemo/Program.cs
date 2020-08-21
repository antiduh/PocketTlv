using System;
using System.IO;
using TlvDemo.TlvApi;

namespace TlvDemo
{
    internal class Program
    {
        private static void Main( string[] args )
        {
            LoadStorePersonRecord();
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

        private static T RoundTrip<T>( T record ) where T : ITlvContract, new()
        {
            var stream = new MemoryStream();

            TlvWriter writer = new TlvWriter( stream );
            writer.Write( record );

            byte[] buffer = stream.GetBuffer();

            stream.Position = 0L;

            TlvReader reader = new TlvReader( stream );
            return reader.Read<T>();
        }
    }
}