using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
            TagTest();
        }

        private static void TagTest()
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