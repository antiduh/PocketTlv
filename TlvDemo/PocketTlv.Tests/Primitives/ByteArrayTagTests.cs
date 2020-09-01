using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PocketTlv.Tests.Primitives
{
    [TestClass]
    public class ByteArrayTagTests
    {
        [DataRow( 0, "{ }" )]
        [DataRow( 1, "{ 0x00 }" )]
        [DataRow( 2, "{ 0x00, 0x00 }" )]
        [DataRow( 10, "{ 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }" )]
        [DataRow( 11, "{ 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, ... }" )]
        [DataTestMethod]
        public void When_Empty_ToString_EmptyBraces( int count, string expected )
        {
            var tag = new ByteArrayTag( new byte[count] );

            string text = tag.ToString();

            Assert.AreEqual( expected, text );
        }
    }
}
