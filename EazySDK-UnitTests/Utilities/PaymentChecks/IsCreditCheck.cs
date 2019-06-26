using Microsoft.VisualStudio.TestTools.UnitTesting;
using EazySDK;
using Microsoft.Extensions.Configuration;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class IsCreditCheck
    {
        private EazySDK.Utilities.PaymentPostChecks PaymentCheck { get; set; }
        private IConfiguration Settings { get; set; }

        [TestInitialize]
        public void TestInit()
        {
            ClientHandler Handler = new ClientHandler();
            PaymentCheck = new EazySDK.Utilities.PaymentPostChecks();
            Settings = Handler.Settings();
        }

        [TestMethod]
        public void TestValidIsCreditReturnsTrue()
        {
            Settings.GetSection("payments")["IsCreditAllowed"] = "true";
            bool IsCredit = PaymentCheck.CheckIfCreditIsAllowed(Settings);
            Assert.IsTrue(IsCredit);
        }

        [TestMethod]
        public void TestValidNotCreditReturnsFalse()
        {
            Settings.GetSection("payments")["IsCreditAllowed"] = "false";
            bool IsCredit = PaymentCheck.CheckIfCreditIsAllowed(Settings);
            Assert.IsFalse(IsCredit);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidSettingsConfigurationException))]
        public void TestInvalidIsCreditThrowsError()
        {
            Settings.GetSection("payments")["IsCreditAllowed"] = "is not credit";
            PaymentCheck.CheckIfCreditIsAllowed(Settings);
        }
    }
}

