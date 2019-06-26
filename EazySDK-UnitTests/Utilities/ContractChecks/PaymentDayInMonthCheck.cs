using Microsoft.VisualStudio.TestTools.UnitTesting;
using EazySDK;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class PaymentDayInMonthCheck
    {
        private EazySDK.Utilities.ContractPostChecks ContractCheck { get; set; }

        [TestInitialize]
        public void TestInit()
        {
            ClientHandler Handler = new ClientHandler();
            ContractCheck = new EazySDK.Utilities.ContractPostChecks();
        }

        [TestMethod]
        public void TestValidPaymentDayInMonth()
        {
            bool PaymentDayInMonth = ContractCheck.CheckPaymentDayInMonthIsValid("1");
            Assert.IsTrue(PaymentDayInMonth);
        }

        [TestMethod]
        public void TestLastDayOfMonthAcceptable()
        {
            bool PaymentDayInMonth = ContractCheck.CheckPaymentDayInMonthIsValid("last day of month");
            Assert.IsTrue(PaymentDayInMonth);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestBelow1Invalid()
        {
            ContractCheck.CheckPaymentDayInMonthIsValid("0");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestAbove28Invalid()
        {
            ContractCheck.CheckPaymentDayInMonthIsValid("29");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestMustBeInt()
        {
            ContractCheck.CheckPaymentDayInMonthIsValid("1.01");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestMustBeIntOrLastDayOfMonth()
        {
            ContractCheck.CheckPaymentDayInMonthIsValid("first day of month");
        }
    }
}

