using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class UnitTestSortCodeChecks
    {
        private EazySDK.Utilities.CustomerPostChecks Check { get; set; }

        [TestInitialize]
        public void TestInit()
        {
            Check = new EazySDK.Utilities.CustomerPostChecks();
        }

        [TestMethod]
        public void TestAcceptSortCodeCorrectLength()
        {
            var result = Check.CheckSortCodeIsFormattedCorrectly("123456");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestAcceptsInvalidSortCodeCorrectLength()
        {
            var result = Check.CheckSortCodeIsFormattedCorrectly("000000");
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestDoesNotAcceptAlphabeticCharactersInSortCode()
        {
            var result = Check.CheckSortCodeIsFormattedCorrectly("12345A");
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestDoesNotAcceptHyphensInSortCode()
        {
            var result = Check.CheckSortCodeIsFormattedCorrectly("12-34-56");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestDoesNotAcceptTooSmallSortCode()
        {
            var result = Check.CheckSortCodeIsFormattedCorrectly("1234");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestDoesNotAcceptTooLongSortCode()
        {
            var result = Check.CheckSortCodeIsFormattedCorrectly("12345678");
        }
    }
}
