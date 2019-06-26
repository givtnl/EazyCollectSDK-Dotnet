using Microsoft.VisualStudio.TestTools.UnitTesting;
using EazySDK;
using Microsoft.Extensions.Configuration;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class UnitTestDeletePayment
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
        public void TestDeletePaymentDeleteValidPayment()
        {
            var Delete = new Delete(Settings);
            var Req = Delete.Payment("2b62a358-9a1a-4c71-9450-e419e393dcd1", "750f142f-1608-464a-8e34-4b322e703c2c", "A comment");

            Assert.IsTrue(Req.Contains("Payment 750f142f-1608-464a-8e34-4b322e703c2c deleted"));
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.EmptyRequiredParameterException))]
        public void TestDeletePaymentCommentRequired()
        {
            var Delete = new Delete(Settings);
            Delete.Payment("2b62a358-9a1a-4c71-9450-e419e393dcd1", "750f142f-1608-464a-8e34-4b322e703c2c", "");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.ResourceNotFoundException))]
        public void TestDeletePaymentInvalidContractThrowsException()
        {
            var Delete = new Delete(Settings);
            Delete.Payment("2b62a358-9a1a-4c71-9450-e4", "750f142f-1608-464a-8e34-4b322e703c2c", "A comment");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.ResourceNotFoundException))]
        public void TestDeletePaymentInvalidPaymentThrowsException()
        {
            var Delete = new Delete(Settings);
            Delete.Payment("2b62a358-9a1a-4c71-9450-e419e393dcd1", "750f142f-1608-464a-8e34-4b32c", "A comment");
        }
    }
}

