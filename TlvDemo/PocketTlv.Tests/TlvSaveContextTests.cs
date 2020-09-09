using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PocketTlv.Tests.Inftrastructure.StubContracts;

namespace PocketTlv.Tests
{
    [TestClass]
    public class TlvSaveContextTests
    {
        [TestMethod]
        public void When_SavingTagWithFieldId_TagFieldId_IsOverwritten()
        {
            var destTag = new CompositeTag();
            var save = new TlvSaveContext( destTag.Children );

            var dataTag = new IntTag( 0, 0 );

            save.Tag( 1, dataTag );

            Assert.AreEqual( 1, dataTag.FieldId );
        }

        [TestMethod]
        public void When_SavingChildTag_DestinationTag_ContainsData()
        {
            var destTag = new CompositeTag();
            var save = new TlvSaveContext( destTag.Children );

            var dataTag = new IntTag( 0, 1 );

            save.Tag( 1, dataTag );

            Assert.IsInstanceOfType( destTag.Children[0], typeof( IntTag ) );
            Assert.IsTrue( ( (IntTag)destTag.Children[0] ).Value == 1 );
        }

        [TestMethod]
        public void When_SavingChildContract_DestinationTag_ContainsData()
        {
            var destTag = new CompositeTag();
            var save = new TlvSaveContext( destTag.Children );

            var dataContract = new IntContract1() { Value = 1 };

            save.Contract( 1, dataContract );

            var contractTag = destTag.Children[0] as CompositeTag;
            Assert.IsInstanceOfType( contractTag, typeof( CompositeTag ) );

            var contractIdChildTag = contractTag.Children[0] as ContractIdTag;
            Assert.IsInstanceOfType( contractIdChildTag, typeof( ContractIdTag ) );
            Assert.AreEqual( dataContract.ContractId, contractIdChildTag.ContractId );

            var intChildTag = contractTag.Children[1] as IntTag;
            Assert.IsInstanceOfType( intChildTag, typeof( IntTag ) );
            Assert.AreEqual( dataContract.Value, intChildTag.Value );
        }
    }
}