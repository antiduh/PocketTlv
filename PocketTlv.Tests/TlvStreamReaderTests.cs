using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PocketTlv.Tests.Inftrastructure.StubContracts;

namespace PocketTlv.Tests
{
    [TestClass]
    public class TlvStreamReaderTests
    {
        [TestMethod]
        public void Constructor_Null_Args()
        {
            Assert.ThrowsException<ArgumentNullException>( 
                () => new TlvStreamReader( null, new ContractRegistry() ) 
            );

            Assert.ThrowsException<ArgumentNullException>(
                () => new TlvStreamReader( new MemoryStream(), null )
            );
        }

        [TestMethod]
        public void Read_EndOfStream_ReturnsNull()
        {
            var stream = new MemoryStream();
            var reader = new TlvStreamReader( stream, new ContractRegistry() );
            var writer = new TlvStreamWriter( stream );

            writer.Write( new IntContract1( 0 ) );
            stream.Position = 0L;

            IntContract1 result;
            
            result = reader.ReadContract<IntContract1>();
            Assert.IsNotNull( result );

            result = reader.ReadContract<IntContract1>();
            Assert.IsNull( result );
        }

        [TestMethod]
        public void Read_WrongContract_Throws_ContractMismatch()
        {
            var stream = new MemoryStream();
            var reader = new TlvStreamReader( stream, new ContractRegistry() );
            var writer = new TlvStreamWriter( stream );

            writer.Write( new IntContract1( 0 ) );
            stream.Position = 0L;

            Assert.ThrowsException<ContractTypeMismatchException>( () => reader.ReadContract<IntContract2>() );
        }
    }
}
