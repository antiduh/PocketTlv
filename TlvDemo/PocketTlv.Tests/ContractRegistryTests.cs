using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PocketTlv.Tests.Inftrastructure.StubContracts;

namespace PocketTlv.Tests
{
    [TestClass]
    public class ContractRegistryTests
    {
        [TestMethod]
        public void When_Empty_DoesNotContain_Contract()
        {
            var reg = new ContractRegistry();

            Assert.IsFalse( reg.Contains( IntContract1.Id ) );
        }

        [TestMethod]
        public void When_Empty_GetUnknownContract_Throws()
        {
            var reg = new ContractRegistry();

            Assert.ThrowsException<KeyNotFoundException>( () => reg.Get( IntContract1.Id ) );
        }

        [TestMethod]
        public void When_ContractNotRegistered_GetUnknownContract_Throws()
        {
            var reg = new ContractRegistry();

            reg.Register<IntContract1>();

            Assert.ThrowsException<KeyNotFoundException>( () => reg.Get( IntContract2.Id ) );
        }

        [TestMethod]
        public void When_ContainsContract_Can_RetreiveContract()
        {
            var reg = new ContractRegistry();

            reg.Register<IntContract1>();

            Assert.IsTrue( reg.Contains( IntContract1.Id ) );
            Assert.IsInstanceOfType( reg.Get( IntContract1.Id ), typeof( IntContract1 ) );
        }

        [TestMethod]
        public void When_AlreadyRegistered_Register_AcceptsDuplicates()
        {
            var reg = new ContractRegistry();

            reg.Register<IntContract1>();
            reg.Register<IntContract1>();

            Assert.IsTrue( reg.Contains( IntContract1.Id ) );
        }

        [TestMethod]
        public void When_AlreadyRegistered_Register_RefusesConflicts()
        {
            var reg = new ContractRegistry();

            reg.Register<IntContract1>();

            Assert.ThrowsException<InvalidOperationException>( () => reg.Register<IntContractDup1>() );
        }
    }
}