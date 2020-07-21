using Microsoft.VisualStudio.TestTools.UnitTesting;
using EazySDK;
using Microsoft.Extensions.Configuration;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class CollectionDateChecks
    {
        private EazySDK.Utilities.PaymentPostChecks PaymentCheck { get; set; }
        private IConfiguration Settings { get; set; }

        [TestInitialize]
        public void TestInit()
        {
            ClientHandler Handler = new ClientHandler();
            PaymentCheck = new EazySDK.Utilities.PaymentPostChecks();
            Settings = Handler.Settings();
            Settings.GetSection("currentEnvironment")["Environment"] = "sandbox";
        }

        [TestMethod]
        public void TestValidPaymentDate()
        {
            Settings.GetSection("directDebitProcessingDays")["OngoingProcessingDays"] = "5";
            string PaymentAmount = PaymentCheck.CheckPaymentDate("2020-08-15", Settings);
            Assert.IsTrue(PaymentAmount == "2020-08-15");
        }

        [TestMethod]
        public void TestValidPaymentDateAfterBankHoliday()
        {
            Settings.GetSection("directDebitProcessingDays")["OngoingProcessingDays"] = "29";
            string PaymentAmount = PaymentCheck.CheckPaymentDate("2020-09-01", Settings);
            Assert.IsTrue(PaymentAmount == "2020-09-01");
        }

        [TestMethod]
        public void TestPaymentDateAcceptsNonISODates()
        {
            Settings.GetSection("directDebitProcessingDays")["OngoingProcessingDays"] = "5";
            string PaymentAmount = PaymentCheck.CheckPaymentDate("15/08/2020", Settings);
            Assert.IsTrue(PaymentAmount == "2020-08-15");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidSettingsConfigurationException))]
        public void TestInvalidOngoingProcessingDaysThrowsError()
        {
            Settings.GetSection("directDebitProcessingDays")["OngoingProcessingDays"] = "five";
            PaymentCheck.CheckPaymentDate("15/07/2019", Settings);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidPaymentDateException))]
        public void TestPaymentDateInvalidDateThrowsError()
        {
            PaymentCheck.CheckPaymentDate("2019-13-13", Settings);
        }
    }
}

