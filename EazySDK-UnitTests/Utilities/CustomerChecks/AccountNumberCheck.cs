using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class UnitTestAccountNumberChecks
    {
        private EazySDK.Utilities.CustomerPostChecks Check { get; set; }

        [TestInitialize]
        public void TestInit()
        {
            Check = new EazySDK.Utilities.CustomerPostChecks();
        }

        [TestMethod]
        public void TestAcceptAccountNumberCorrectLength()
        {
            var result = Check.CheckAccountNumberIsFormattedCorrectly("12345678");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestAcceptsInvalidAccountNumberCorrectLength()
        {
            var result = Check.CheckAccountNumberIsFormattedCorrectly("00000000");
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestDoesNotAcceptAlphabeticCharactersInAccountNumber()
        {
            var result = Check.CheckAccountNumberIsFormattedCorrectly("1234567A");
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestDoesNotAcceptSpecialCharactersInAccountNumber()
        {
            var result = Check.CheckAccountNumberIsFormattedCorrectly("12345-78");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestDoesNotAcceptTooSmallAccountNumber()
        {
            var result = Check.CheckAccountNumberIsFormattedCorrectly("123456");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestDoesNotAcceptTooLongAccountNumber()
        {
            var result = Check.CheckAccountNumberIsFormattedCorrectly("1234567890");
        }
    }
}

