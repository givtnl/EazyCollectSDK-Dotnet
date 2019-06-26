using Microsoft.VisualStudio.TestTools.UnitTesting;
using EazySDK;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class CollectionAmountCheck
    {
        private EazySDK.Utilities.PaymentPostChecks PaymentCheck { get; set; }

        [TestInitialize]
        public void TestInit()
        {
            ClientHandler Handler = new ClientHandler();
            PaymentCheck = new EazySDK.Utilities.PaymentPostChecks();
        }

        [TestMethod]
        public void TestValidPaymentAmount()
        {
            bool PaymentAmount = PaymentCheck.CheckPaymentAmount("12.34");
            Assert.IsTrue(PaymentAmount);
        }

        [TestMethod]
        public void TestValidPaymentAmountAsInt()
        {
            bool PaymentAmount = PaymentCheck.CheckPaymentAmount("12");
            Assert.IsTrue(PaymentAmount);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPassing0ThrowsError()
        {
            PaymentCheck.CheckPaymentAmount("0.00");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestNegativeValueThrowsError()
        {
            PaymentCheck.CheckPaymentAmount("-10.00");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestInvalidPaymentAmountThrowsError()
        {
            PaymentCheck.CheckPaymentAmount("1.2.3");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPassingNonNumericStringThrowsError()
        {
            PaymentCheck.CheckPaymentAmount("one");
        }
    }
}

