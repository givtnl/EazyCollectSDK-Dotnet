using Microsoft.VisualStudio.TestTools.UnitTesting;
using EazySDK;
using Microsoft.Extensions.Configuration;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class UnitTestPatchCustomer
    {
        private IConfiguration Settings { get; set; }

        [TestInitialize]
        public void TestInit()
        {
            ClientHandler Handler = new ClientHandler();
            Settings = Handler.Settings();
            Settings.GetSection("currentEnvironment")["Environment"] = "sandbox";
            Settings.GetSection("sandboxClientDetails")["ClientCode"] = "SDKTST";
            Settings.GetSection("sandboxClientDetails")["ApiKey"] = "hkZujzFR2907XAtYe6qkKRsBo";
        }

        [TestMethod]
        public void TestPathCustomerPatchingAllDetailsReturnsCustomer()
        {
            var Patch = new Patch(Settings);
            var Req = Patch.Customer("310a826b-d095-48e7-a55a-19dba82c566f", "test@email.com", "Mr", "1990-01-01", "Test", "Test", "TestCom", "1 Test Road", "Line2", "Line3", "Line4", "GL52 2NF",
            "12345678", "123456", "Mr Test Test", "01242694874", "07398745641", "0145213458", "tt");
            Assert.IsTrue(Req.Contains("The customer 310a826b-d095-48e7-a55a-19dba82c566f has been updated successfully"));
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPatchCustomerPostCodeTooShortThrowsException()
        {
            var Patch = new Patch(Settings);
            Patch.Customer("310a826b-d095-48e7-a55a-19dba82c566f", PostCode: "1");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPatchCustomerPostCodeTooLongThrowsException()
        {
            var Patch = new Patch(Settings);
            Patch.Customer("310a826b-d095-48e7-a55a-19dba82c566f", PostCode: "GL51 ABCDEFG");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPatchCustomerPostCodeMustContainNumbers()
        {
            var Patch = new Patch(Settings);
            Patch.Customer("310a826b-d095-48e7-a55a-19dba82c566f", PostCode: "GLFO NBA");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPatchCustomerPostCodeMustContainLetters()
        {
            var Patch = new Patch(Settings);
            Patch.Customer("310a826b-d095-48e7-a55a-19dba82c566f", PostCode: "1234 123");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPatchCustomerEmailMustBeValidFormat()
        {
            var Patch = new Patch(Settings);
            Patch.Customer("310a826b-d095-48e7-a55a-19dba82c566f", Email: "helpeazycollect.co.uk");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPatchCustomerAccountNumberMustNotContainLetters()
        {
            var Patch = new Patch(Settings);
            Patch.Customer("310a826b-d095-48e7-a55a-19dba82c566f", AccountNumber: "1234567A");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPatchCustomerAccountNumberMustNotBeTooShort()
        {
            var Patch = new Patch(Settings);
            Patch.Customer("310a826b-d095-48e7-a55a-19dba82c566f", AccountNumber: "1234567");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPatchCustomerAccountNumberMustNotBeTooLong()
        {
            var Patch = new Patch(Settings);
            Patch.Customer("310a826b-d095-48e7-a55a-19dba82c566f", AccountNumber: "123456789");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPatchCustomerSortCodeMustNotBeTooShort()
        {
            var Patch = new Patch(Settings);
            Patch.Customer("310a826b-d095-48e7-a55a-19dba82c566f", SortCode: "12345");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPatchCustomerSortCodeMustNotBeTooLong()
        {
            var Patch = new Patch(Settings);
            Patch.Customer("310a826b-d095-48e7-a55a-19dba82c566f", SortCode: "1234567");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPatchCustomerSortCodeMustNotContainLetters()
        {
            var Patch = new Patch(Settings);
            Patch.Customer("310a826b-d095-48e7-a55a-19dba82c566f", SortCode: "12345A");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPatchCustomerAccountHolderNameMustNotSpecialCharacters()
        {
            var Patch = new Patch(Settings);
            Patch.Customer("310a826b-d095-48e7-a55a-19dba82c566f", AccountHolderName: "TEST ! ~ # te");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPatchCustomerAccountHolderNameMustNotBeTooShort()
        {
            var Patch = new Patch(Settings);
            Patch.Customer("310a826b-d095-48e7-a55a-19dba82c566f", AccountHolderName: "TE");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPatchCustomerAccountHolderNameMustNotBeTooLong()
        {
            var Patch = new Patch(Settings);
            Patch.Customer("310a826b-d095-48e7-a55a-19dba82c566f", AccountHolderName: "TEST ACCOUNT HOLDER NAME TEST");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.ResourceNotFoundException))]
        public void TestPatchCustomerInvalidCustomerThrowsException()
        {
            var Patch = new Patch(Settings);
            Patch.Customer("310a826b-d095-48e7-a55a-19db", Email: "test@email.com");
        }
    }
}