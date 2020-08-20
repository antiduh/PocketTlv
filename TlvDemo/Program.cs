﻿using System;
using System.IO;
using TlvDemo.Messages;
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


            // Method 1:
            PersonRecord personCopy1 = messageCopy.Message.Resolve<PersonRecord>();
            ProcessPersonRecord( personCopy1 );

            // Method 2:
            if( messageCopy.Message.ContractId == PersonRecord.ContractId )
            {
                PersonRecord personCopy2 = messageCopy.Message.Resolve<PersonRecord>();
                ProcessPersonRecord( personCopy2 );
            }

            // Method 3:
            if( messageCopy.Message.TryResolve<PersonRecord>( out PersonRecord personCopy3 ) )
            {
                ProcessPersonRecord( personCopy3 );
            }
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

            stream.Position = 0L;

            TlvReader reader = new TlvReader( stream );
            return reader.Read<T>();
        }

#if false
        private static void LoadStoreTest()
        {
            var top = new CompositeTag( 1,
                new StringTag( 2, "Hello" ),
                new IntTag( 3, 42 ),
                new CompositeTag( 4, 
                    new CompositeTag( 5,
                        new IntTag( 6, 111 ),
                        new IntTag( 7, 112 ),
                        new IntTag( 8, 113 )
                    )
                ),
                new IntTag( 9, 1011 )
            );
            

            var stream = new MemoryStream();
            TlvWriter writer = new TlvWriter( stream );

            writer.Write( top );

            byte[] buffer = stream.GetBuffer();

            // -- Read --

            ITag rebuiltTop;

            stream.Position = 0L;
            var reader = new TlvReader( stream );

            rebuiltTop = reader.Read();
        }

#endif

        private static void ConstructTest()
        {
            var frame = new ClientFrame()
            {
                Exchange = "voren-core",
                RoutingKey = "voren.Echo",
                MessageName = "EchoRequest",
                Guid = "123456",
                //    Message = new EchoRequest() { Text = "Hello world." }
            };
        }
    }
}