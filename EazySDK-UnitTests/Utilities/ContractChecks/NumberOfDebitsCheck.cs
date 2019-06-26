using Microsoft.VisualStudio.TestTools.UnitTesting;
using EazySDK;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class NumberOfDebitsCheck
    {
        private EazySDK.Utilities.ContractPostChecks ContractCheck { get; set; }

        [TestInitialize]
        public void TestInit()
        {
            ClientHandler Handler = new ClientHandler();
            ContractCheck = new EazySDK.Utilities.ContractPostChecks();
        }

        [TestMethod]
        public void TestValidNumberOfDebits()
        {
            bool NumberOfDebits = ContractCheck.CheckNumberOfDebitsIsValid("49");
            Assert.IsTrue(NumberOfDebits);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestNumberOfDebitsCannotBeBelow1()
        {
            ContractCheck.CheckNumberOfDebitsIsValid("0");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestNumberOfDebitsCannotBeAbove99()
        {
            ContractCheck.CheckNumberOfDebitsIsValid("100");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestNumberOfDebitsMustBeANumber()
        {
            ContractCheck.CheckNumberOfDebitsIsValid("test");
        }
    }
}

