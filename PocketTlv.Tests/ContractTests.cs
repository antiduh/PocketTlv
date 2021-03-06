﻿using System;
using System.IO;
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
            var contract = new IntContract1( 1 );

            var copy = RoundTrip( contract );

            Assert.AreEqual( contract, copy );
        }

        [TestMethod]
        public void EmbeddedContract_AnonymousUnregistered()
        {
            var contract = new CarrierRecord( 1, new IntContract1( 1 ) );

            var copy = RoundTrip( contract );

            Assert.IsInstanceOfType( copy, typeof( CarrierRecord ) );
            Assert.AreEqual( copy.Value, contract.Value );
            Assert.IsNotNull( copy.Child );

            var resolvedCopyChild = copy.Child.Resolve<IntContract1>();

            Assert.AreEqual( contract.Child, resolvedCopyChild );
        }

        [TestMethod]
        public void EmbeddedContract_AnonymousRegistered()
        {
            var contract = new CarrierRecord( 1, new IntContract1( 1 ) );

            var copy = RoundTrip<CarrierRecord, IntContract1>( contract );

            Assert.AreEqual( contract, copy );
        }

        [TestMethod]
        public void EmbeddedContract_SelfResolved()
        {
            var parent = new ParentContract( new IntContract1( 1 ) );

            var copy = RoundTrip( parent );

            Assert.AreEqual( parent, copy );
        }

        private static T RoundTrip<T>( T contract ) where T : ITlvContract, new()
        {
            var stream = new MemoryStream();

            TlvStreamWriter writer = new TlvStreamWriter( stream );
            writer.Write( contract );

            stream.Position = 0L;

            var contractReg = new ContractRegistry();
            var reader = new TlvStreamReader( stream, contractReg );

            return reader.ReadContract<T>();
        }

        private static TParent RoundTrip<TParent, TChild>( TParent contract )
            where TParent : ITlvContract, new()
            where TChild : ITlvContract, new()
        {
            var stream = new MemoryStream();

            TlvStreamWriter writer = new TlvStreamWriter( stream );
            writer.Write( contract );

            stream.Position = 0L;

            var contractReg = new ContractRegistry();
            contractReg.Register<TChild>();

            var reader = new TlvStreamReader( stream, contractReg );
            
            return reader.ReadContract<TParent>();
        }
    }
}