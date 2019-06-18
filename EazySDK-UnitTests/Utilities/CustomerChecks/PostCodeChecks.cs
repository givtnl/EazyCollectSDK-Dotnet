using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class UnitTestPostCodeChecks
    {
        private EazySDK.Utilities.CustomerPostChecks Check { get; set; }

        [TestInitialize]
        public void TestInit()
        {
            Check = new EazySDK.Utilities.CustomerPostChecks();
        }

        [TestMethod]
        public void TestAcceptsValidPostCode()
        {
            var result = Check.CheckPostCodeIsCorectlyFormatted("GL52 2NF");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestAcceptsInvalidPostCodeUsingCorrectFormat()
        {
            var result = Check.CheckPostCodeIsCorectlyFormatted("GL51 6NC");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestAcceptsBFPOPostCodes()
        {
            var result = Check.CheckPostCodeIsCorectlyFormatted("BF1 3AA");
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestDoesNotAcceptInvalidUKPostCode()
        {
            var result = Check.CheckPostCodeIsCorectlyFormatted("ZZ11 3ZZ");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestDoesNotAcceptNonUKPostCode()
        {
            var result = Check.CheckPostCodeIsCorectlyFormatted("20500");
        }
    }
}

