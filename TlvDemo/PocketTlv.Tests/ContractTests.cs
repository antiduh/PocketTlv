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
    public class ContractTests
    {
        [TestMethod]
        public void SimpleContract()
        {
            var contract = new IntContract1( 0 );

            var copy = RoundTrip( contract );

            Assert.AreEqual( contract, copy );
        }

        [TestMethod]
        public void EmbeddedContract_Anonymous()
        {
            var contract = new CarrierRecord( 0, new IntContract1( 0 ) );

            var copy = RoundTrip( contract );

            Assert.IsInstanceOfType( copy, typeof( CarrierRecord ) );
            Assert.AreEqual( copy.Value, contract.Value );
            Assert.IsNotNull( copy.Child );

            var resolvedCopyChild = copy.Child.Resolve<IntContract1>();

            Assert.AreEqual( contract.Child, resolvedCopyChild );
        }

        // Future cases:
        // - We register the child contract so that it can be automatically resolved during the
        //   parsing stage.
        // - We use a carrier that saves and parses a specific child contract, and so, doesn't need pre-registration.

        private static T RoundTrip<T>( T contract ) where T : ITlvContract, new()
        {
            var stream = new MemoryStream();

            TlvStreamWriter writer = new TlvStreamWriter( stream );
            writer.Write( contract );

            stream.Position = 0L;

            TlvStreamReader reader = new TlvStreamReader( stream );
            return reader.ReadContract<T>();
        }

    }
}
