using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PocketTlv.Tests.Inftrastructure.StubContracts;

namespace PocketTlv.Tests
{
    [TestClass]
    public class ResolutionTests
    {
        [TestMethod]
        public void EmbeddedUnresolvedContract_DoubleParse()
        {
            var carrier = new CarrierRecord()
            {
                Value = 0,
                Child = new IntContract1() { Value = 1 }
            };

            ITlvContract copy = RoundTrip_Anonymous( carrier );

            var resolvedCopy = copy.Resolve<CarrierRecord>();
            var resolvedcopyChild = resolvedCopy.Child.Resolve<IntContract1>();

            Assert.AreEqual( carrier.Value, resolvedCopy.Value );
            Assert.AreEqual( ( (IntContract1)carrier.Child ).Value, resolvedcopyChild.Value );
        }

        private static ITlvContract RoundTrip_Anonymous( CarrierRecord carrier )
        {
            MemoryStream stream = new MemoryStream();
            TlvStreamWriter writer = new TlvStreamWriter( stream );
            TlvStreamReader reader = new TlvStreamReader( stream );

            writer.Write( carrier );
            stream.Position = 0L;
            return reader.ReadContract();
        }
    }
}