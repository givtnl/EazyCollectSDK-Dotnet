using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class UnitTestEmailAddressChecks
    {
        private EazySDK.Utilities.CustomerPostChecks Check { get; set; }

        [TestInitialize]
        public void TestInit()
        {
            Check = new EazySDK.Utilities.CustomerPostChecks();
        }

        [TestMethod]
        public void TestAcceptsValidEmail()
        {
            var result = Check.CheckEmailAddressIsCorrectlyFormatted("test@email.com");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestAcceptsValidEmailWithSpecialCharacters()
        {
            var result = Check.CheckEmailAddressIsCorrectlyFormatted("te_s-t+123@email.com");
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestDoesNotAcceptEmailWithoutAtSymbol()
        {
            var result = Check.CheckEmailAddressIsCorrectlyFormatted("testemail.com");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestDoesNotAcceptEmailWithoutTLDSeperator()
        {
            var result = Check.CheckEmailAddressIsCorrectlyFormatted("tes@temailcom");
        }
    }
}

