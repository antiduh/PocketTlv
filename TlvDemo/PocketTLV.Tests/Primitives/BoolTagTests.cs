using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PocketTLV.Primitives;

namespace PocketTLV.Tests.Primitives
{
    [TestClass]
    public class BoolTagTests
    {
        [TestMethod]
        public void When_True_BoolEquality_Is_True()
        {
            var tag = new BoolTag( true );

            Assert.IsTrue( (bool)tag );
            Assert.IsTrue( tag == true );
            Assert.IsFalse( tag != true );
        }

        [TestMethod]
        public void When_False_BoolEquality_Is_False()
        {
            var tag = new BoolTag( false );

            Assert.IsFalse( (bool)tag );
            Assert.IsTrue( tag == false );
            Assert.IsFalse( tag != false );
        }

        [TestMethod]
        public void When_EqualAsTrue_SelfEquality_Is_True()
        {
            var trueTag = new BoolTag( true );
            var trueTag2 = new BoolTag( true );

            Assert.IsTrue( trueTag == trueTag2 );
            Assert.IsTrue( trueTag2 == trueTag );
            Assert.IsTrue( trueTag.Equals( trueTag2 ) );
            Assert.IsTrue( trueTag2.Equals( trueTag ) );
            Assert.IsTrue( trueTag.GetHashCode() == trueTag2.GetHashCode() );
        }

        [TestMethod]
        public void When_EqualAsFalse_SelfEquality_Is_True()
        {
            var trueTag = new BoolTag( false );
            var trueTag2 = new BoolTag( false );

            Assert.IsTrue( trueTag == trueTag2 );
            Assert.IsTrue( trueTag2 == trueTag );
            Assert.IsTrue( trueTag.Equals( trueTag2 ) );
            Assert.IsTrue( trueTag2.Equals( trueTag ) );
            Assert.IsTrue( trueTag.GetHashCode() == trueTag2.GetHashCode() );
        }

        [TestMethod]
        public void When_NotEqual_SelfEquality_Is_False()
        {
            var falseTag = new BoolTag( false );
            var trueTag = new BoolTag( true );

            Assert.IsFalse( falseTag == trueTag );
            Assert.IsFalse( trueTag == falseTag );
            Assert.IsFalse( falseTag.Equals( trueTag ) );
            Assert.IsFalse( trueTag.Equals( falseTag ) );

            // Note that the following assert is invalid; it's not actually guaranteed (only guaranteed
            // when the objects are equal): 
            // Assert.IsFalse( falseTag.GetHashCode() == trueTag.GetHashCode() );
        }

        [TestMethod]
        public void When_Null_SelfEquality_Is_False()
        {
            BoolTag nullTag = null;
            BoolTag tag = new BoolTag( false );

            Assert.IsTrue( nullTag == null );
            Assert.IsTrue( tag != null );
            Assert.IsFalse( nullTag == tag );
            Assert.IsFalse( tag.Equals( null ) );
            Assert.IsFalse( tag.Equals( nullTag ) );
        }

        [TestMethod]
        public void When_BufferIncorrectSize_Read_Throws()
        {
            byte[] buffer = new byte[2];

            ITag tag = new BoolTag( false );

            Assert.ThrowsException<ArgumentOutOfRangeException>( () => tag.ReadValue( buffer, 0, 2 ) );
        }

        [TestMethod]
        public void When_SerializingRoundTripAsTrue_TagsAreEqual()
        {
            RoundTripTest( true );
        }

        [TestMethod]
        public void When_SerializingRoundTripAsFalse_TagsAreEqual()
        {
            RoundTripTest( false );
        }

        private static void RoundTripTest( bool startValue )
        {
            const int tagLength = 1;
            byte[] buffer = new byte[tagLength];

            ITag sourceTag = new BoolTag( startValue );
            ITag destTag = new BoolTag();

            sourceTag.WriteValue( buffer, 0 );
            destTag.ReadValue( buffer, 0, tagLength );

            Assert.AreEqual( tagLength, sourceTag.ComputeLength() );
            Assert.AreEqual( sourceTag, destTag );
        }

        [TestMethod]
        public void When_WriteMiddleOfBuffer_Write_DoesNotAlterAdjacentBytes()
        {
            byte[] buffer = new byte[3] { 0xFF, 0xFF, 0xFF };
            ITag tag = new BoolTag( false );

            tag.WriteValue( buffer, 1 );

            Assert.AreEqual( 0xFF, buffer[0] );
            Assert.AreNotEqual( 0xFF, buffer[1] );
            Assert.AreEqual( 0xFF, buffer[2] );
        }
    }
}