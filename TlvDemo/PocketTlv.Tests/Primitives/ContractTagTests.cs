using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PocketTlv.Tests.Primitives
{
    [TestClass]
    public class ContractTagTests
    {
        [TestMethod]
        public void When_CompositeTagsContainSameData_Equals_ReturnsTrue()
        {
            var contractTag1 = new ContractTag( 1, new IntTag( 2, 3 ) )
            {
                ContractId = 100,
            };

            var contractTag2 = new ContractTag( 1, new IntTag( 2, 3 ) )
            {
                ContractId = 100
            };

            Assert.IsTrue( contractTag1 == contractTag2 );
            Assert.IsTrue( contractTag1.Equals( contractTag2 ) );
        }
    }
}
