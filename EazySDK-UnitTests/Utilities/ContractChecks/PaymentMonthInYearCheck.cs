using Microsoft.VisualStudio.TestTools.UnitTesting;
using EazySDK;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class PaymentMonthInYearCheck
    {
        private EazySDK.Utilities.ContractPostChecks ContractCheck { get; set; }

        [TestInitialize]
        public void TestInit()
        {
            ClientHandler Handler = new ClientHandler();
            ContractCheck = new EazySDK.Utilities.ContractPostChecks();
        }

        [TestMethod]
        public void TestValidPaymentMonthInYear()
        {
            bool PaymentMonthInYear = ContractCheck.CheckPaymentMonthInYearIsValid("11");
            Assert.IsTrue(PaymentMonthInYear);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestInvalidPaymentMonthInYearThrowsError()
        {
            ContractCheck.CheckPaymentMonthInYearIsValid("13");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestStringNotAcceptable()
        {
            ContractCheck.CheckPaymentMonthInYearIsValid("last month of year");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TesMustBeInt()
        {
            ContractCheck.CheckPaymentMonthInYearIsValid("1.01");
        }
    }
}