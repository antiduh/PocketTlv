using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PocketTlv.Tests.Inftrastructure;

namespace PocketTlv.Tests.Primitives
{
    [TestClass]
    public class VarIntTagTests
    {
        [DataList( long.MinValue, int.MinValue, -2, -1, 0, 1, 2, int.MaxValue, long.MaxValue )]
        [DataTestMethod]
        public void When_SerializingRoundTrip_TagsAreEqual( long value )
        {
            const int maxLength = 8;
            byte[] buffer = new byte[maxLength];

            ITag sourceTag = new VarIntTag( value );
            ITag destTag = new VarIntTag();

            int length = sourceTag.ComputeLength();

            sourceTag.WriteValue( buffer, 0 );
            destTag.ReadValue( buffer, 0, length );

            Assert.AreEqual( sourceTag, destTag );
            Assert.AreEqual( value, ((VarIntTag)destTag).Value );
        }
    }
}
