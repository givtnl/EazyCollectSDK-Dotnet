using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class UnitTestAccountHolderNameChecks
    {
        private EazySDK.Utilities.CustomerPostChecks Check { get; set; }

        [TestInitialize]
        public void TestInit()
        {
            Check = new EazySDK.Utilities.CustomerPostChecks();
        }

        [TestMethod]
        public void TestAcceptValidAccountHolerName()
        {
            var result = Check.CheckAccountHolderNameIsFormattedCorrectly("Mr John Doe");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestAcceptsAccountHolderName18Digits()
        {
            var result = Check.CheckAccountHolderNameIsFormattedCorrectly("John Doe Test Test");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestAcceptsAccountHolderName3Digits()
        {
            var result = Check.CheckAccountHolderNameIsFormattedCorrectly("Joh");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestAceptsSpecialChractersAccountHolderName()
        {
            var result = Check.CheckAccountHolderNameIsFormattedCorrectly("John & Doe - A / B");
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestDoesNotAcceptTooSmallAccountHodlerName()
        {
            var result = Check.CheckAccountHolderNameIsFormattedCorrectly("Jo");
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestDoesNotAcceptTooLongAccountHolderName()
        {
            var result = Check.CheckAccountHolderNameIsFormattedCorrectly("Too long account holder name");
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestDoesNotAcceptNumberrsAccountHolderName()
        {
            var result = Check.CheckAccountHolderNameIsFormattedCorrectly("Account 123");
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestDoesNotAcceptSpecialCharactersAccountHolderName()
        {
            var result = Check.CheckAccountHolderNameIsFormattedCorrectly("Account!;@");
            Assert.IsTrue(result);
        }
    }
}
