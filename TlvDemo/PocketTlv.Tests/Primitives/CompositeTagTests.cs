using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PocketTlv.Tests.Primitives
{
    [TestClass]
    public class CompositeTagTests
    {
        [TestMethod]
        public void When_AddNullChild_AddChild_Throws()
        {
            var tag = new CompositeTag();

            Assert.ThrowsException<ArgumentNullException>( () => tag.AddChild( null ) );
            Assert.ThrowsException<ArgumentNullException>( () => tag.AddChild( 0, null ) );
        }

        [TestMethod]
        public void When_BothEmpty_Equals_ReturnsTrue()
        {
            var tag1 = new CompositeTag();
            var tag2 = new CompositeTag();

            Assert.IsTrue( tag1.Equals( tag2 ) );
            Assert.IsTrue( tag1 == tag2 );
            Assert.IsFalse( tag1 != tag2 );
            Assert.IsTrue( tag1.GetHashCode() == tag2.GetHashCode() );
        }

        [TestMethod]
        public void When_TagsContainSameData_Equals_ReturnsTrue()
        {
            var tag1 = new CompositeTag( 0, new IntTag( 0, 0 ) );
            var tag2 = new CompositeTag( 0, new IntTag( 0, 0 ) );

            Assert.IsTrue( tag1.Equals( tag2 ) );
            Assert.IsTrue( tag1 == tag2 );
            Assert.IsFalse( tag1 != tag2 );
            Assert.AreEqual( tag1.GetHashCode(), tag2.GetHashCode() );
        }

        [TestMethod]
        public void When_ChildTagsDifferByValue_Equals_ReturnsFalse()
        {
            var tag1 = new CompositeTag( 0, new IntTag( 0, 0 ) );
            var tag2 = new CompositeTag( 0, new IntTag( 0, 1 ) );

            Assert.IsFalse( tag1.Equals( tag2 ) );
            Assert.IsFalse( tag1 == tag2 );
            Assert.IsTrue( tag1 != tag2 );
        }

        [TestMethod]
        public void When_ChildTagsDifferByFieldId_Equals_RefersFalse()
        {
            var tag1 = new CompositeTag( 0, new IntTag( 0, 0 ) );
            var tag2 = new CompositeTag( 0, new IntTag( 1, 0 ) );

            Assert.IsFalse( tag1.Equals( tag2 ) );
            Assert.IsFalse( tag1 == tag2 );
            Assert.IsTrue( tag1 != tag2 );
        }

        [TestMethod]
        public void When_FieldIdIsNegative_AddChild_Throws()
        {
            var tag = new CompositeTag();

            Assert.ThrowsException<ArgumentOutOfRangeException>( () => tag.AddChild( -1, new IntTag( 0 ) ) );
        }
    }
}
