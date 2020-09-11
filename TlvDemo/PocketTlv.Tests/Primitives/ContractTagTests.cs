using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PocketTlv.Tests.Primitives
{
    [TestClass]
    public class ContractTagTests
    {
        [TestMethod]
        public void When_TagsContainSameData_Equals_ReturnsTrue()
        {
            var contractTag1 = new ContractTag( 1, 100, new IntTag( 2, 3 ) );
            var contractTag2 = new ContractTag( 1, 100, new IntTag( 2, 3 ) );

            Assert.IsTrue( contractTag1 == contractTag2 );
            Assert.IsFalse( contractTag1 != contractTag2 );
            Assert.IsTrue( contractTag1.Equals( contractTag2 ) );
        }

        /// <summary>
        /// Demonstrates that fieldIds are not part of the propery identity or content of a tag.
        /// </summary>
        [TestMethod]
        public void When_TagsContainDifferentFieldIds_Equals_ReturnsTrue()
        {
            var contractTag1 = new ContractTag( 1, 100, new IntTag( 2, 3 ) );
            var contractTag2 = new ContractTag( 2, 100, new IntTag( 2, 3 ) );

            Assert.IsTrue( contractTag1 == contractTag2 );
            Assert.IsFalse( contractTag1 != contractTag2 );
            Assert.IsTrue( contractTag1.Equals( contractTag2 ) );
        }

        [TestMethod]
        public void When_TagsContainDifferentContractIds_Equals_ReturnsFalse()
        {
            var contractTag1 = new ContractTag( 1, 100, new IntTag( 2, 3 ) );
            var contractTag2 = new ContractTag( 1, 101, new IntTag( 2, 3 ) );

            Assert.IsFalse( contractTag1 == contractTag2 );
            Assert.IsTrue( contractTag1 != contractTag2 );
            Assert.IsFalse( contractTag1.Equals( contractTag2 ) );
        }

        [TestMethod]
        public void When_TagsContainDifferentChildrenCounts_Equals_ReturnsFalse()
        {
            var contractTag1 = new ContractTag( 1, 100, new IntTag( 2, 3 ) );
            var contractTag2 = new ContractTag( 1, 100, new IntTag( 2, 3 ), new IntTag( 3, 4 ) );

            Assert.IsFalse( contractTag1 == contractTag2 );
            Assert.IsTrue( contractTag1 != contractTag2 );
            Assert.IsFalse( contractTag1.Equals( contractTag2 ) );
        }

        [TestMethod]
        public void When_TagsContainDifferentChildrenData_Equals_ReturnsFalse()
        {
            var contractTag1 = new ContractTag( 1, 100, new IntTag( 2, 3 ) );
            var contractTag2 = new ContractTag( 1, 100, new IntTag( 2, 4 ) );

            Assert.IsFalse( contractTag1 == contractTag2 );
            Assert.IsTrue( contractTag1 != contractTag2 );
            Assert.IsFalse( contractTag1.Equals( contractTag2 ) );
        }

        [TestMethod]
        public void When_ContractTagSaved_RestoredTag_EqualsReturnsTrue()
        {
            var orig = new ContractTag( 1, 100, new IntTag( 2, 3 ) );

            var copy = RoundTrip( orig );

            Assert.AreEqual( orig, copy );
        }

        private ContractTag RoundTrip( ContractTag orig )
        {
            ITag origAsInterface = (ITag)orig;

            byte[] buffer = new byte[origAsInterface.ComputeLength()];

            origAsInterface.WriteValue( buffer, 0 );

            ContractTag copy = new ContractTag();
            ITag copyAsInterface = (ITag)copy;

            copyAsInterface.ReadValue( buffer, 0, buffer.Length );
            return copy;
        }
    }
}