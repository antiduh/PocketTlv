using System;
using System.Collections.Generic;
using System.Linq;
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
            var dest = new List<ITag>();
            var save = new TlvSaveContext( dest );

            var dataTag = new IntTag( 0, 0 );

            save.Tag( 1, dataTag );

            Assert.AreEqual( 1, dataTag.FieldId );
        }

        [TestMethod]
        public void When_SavingChildTag_DestinationTag_ContainsData()
        {
            var dest = new List<ITag>();
            var save = new TlvSaveContext( dest );

            save.Tag( 1, new IntTag( 0, 1 ) );

            var expected = new List<ITag>()
            {
                new IntTag( 1, 1 )
            };

            CollectionAssert.AreEqual( expected, dest );
        }

        [TestMethod]
        public void When_SavingChildContract_DestinationTag_ContainsData()
        {
            var dest = new List<ITag>();
            var save = new TlvSaveContext( dest );

            var dataContract = new IntContract1() { Value = 1 };

            save.Contract( 1, dataContract );

            var expected = new List<ITag>()
            {
                new ContractTag( IntContract1.Id, IntContract1.Id, new IntTag( 0, 1 ) )
            };

            CollectionAssert.AreEqual( expected, dest );
        }
    }
}