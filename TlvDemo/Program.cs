using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
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
            PersonRecord personRecord = new PersonRecord()
            {
                Name = "Kevin Thompson",
                Age = 37,
                Address = new AddressRecord() { LotNumber = 50, StreetName = "50 Hampden Rd" },
            };

            var personCopy = RoundTrip( personRecord );

            MessageRecord messageRecord = new MessageRecord()
            {
                Name = "VorenEcho",
                Message = personRecord
            };

            var messageCopy = RoundTrip( messageRecord );


            PersonRecord personCopy2 = messageCopy.Message.Resolve<PersonRecord>();
            
        }

        private static T RoundTrip<T>( T record ) where T : ITlvContract, new()
        {
            var stream = new MemoryStream();

            TlvWriter writer = new TlvWriter( stream );

            writer.Write( record );

            byte[] buffer = stream.GetBuffer();


            stream.Position = 0L;

            TlvReader reader = new TlvReader( stream );

            var record2 = reader.Read<T>();

            return record2;
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