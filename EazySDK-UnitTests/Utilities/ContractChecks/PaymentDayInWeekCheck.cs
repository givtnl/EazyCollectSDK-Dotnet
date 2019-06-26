using Microsoft.VisualStudio.TestTools.UnitTesting;
using EazySDK;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class PaymentDayInWeekCheck
    {
        private EazySDK.Utilities.ContractPostChecks ContractCheck { get; set; }

        [TestInitialize]
        public void TestInit()
        {
            ClientHandler Handler = new ClientHandler();
            ContractCheck = new EazySDK.Utilities.ContractPostChecks();
        }

        [TestMethod]
        public void TestValidPaymentDayInWeek()
        {
            bool PaymentDayInWeek = ContractCheck.CheckPaymentDayInWeekIsValid("1");
            Assert.IsTrue(PaymentDayInWeek);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestInvalidPaymentDayInWeekThrowsError()
        {
            bool PaymentDayInWeek = ContractCheck.CheckPaymentDayInWeekIsValid("6");
            Assert.IsTrue(PaymentDayInWeek);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestStringNotAcceptable()
        {
            ContractCheck.CheckPaymentDayInWeekIsValid("last day of month");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TesMustBeInt()
        {
            ContractCheck.CheckPaymentDayInWeekIsValid("1.01");
        }
    }
}

