using Microsoft.VisualStudio.TestTools.UnitTesting;
using EazySDK;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class UnitTestGetPayment
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
        public void TestGetAllPaymentsFromAContract()
        {
            var Get = new Get(Settings);
            var Req = Get.Payments("2b62a358-9a1a-4c71-9450-e419e393dcd1");
          
            Assert.IsTrue(Req.Contains("be047ad1-e314-4980-9cf6-2bb1d324a41d"));
        }

        [TestMethod]
        public void TestGetAllPaymentsFromAContractNoPaymentsReturnsMessage()
        {
            var Get = new Get(Settings);
            var Req = Get.Payments("a4d77bc5-8480-4148-8838-1afd7f5bab6b");

            Assert.IsTrue(Req.Contains("No payments could be associated with the contract"));
        }

        [TestMethod]
        public void TestGetXNumberOfPaymentsFromAContract()
        {
            var Get = new Get(Settings);
            var Req = Get.Payments("2b62a358-9a1a-4c71-9450-e419e393dcd1", 2);
            JArray ReqAsJson = JArray.Parse(Req);

            Assert.IsTrue(ReqAsJson.Count == 2);
        }

        [TestMethod]
        public void TestGetASinglePayment()
        {
            var Get = new Get(Settings);
            var Req = Get.PaymentsSingle("2b62a358-9a1a-4c71-9450-e419e393dcd1", "6917d51a-be83-424f-b0a6-31fbf9574a79");
            Assert.IsTrue(Req.Contains("\"Amount\":10.0"));
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.ResourceNotFoundException))]
        public void TestGetAllPaymentsInvalidContractThrowsError()
        {
            var Get = new Get(Settings);
            var Req = Get.Payments("2b62a358-9a1a-4c71-9450-e419e393dc");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.ResourceNotFoundException))]
        public void TestGetASinglePaymentInvalidContractThrowsError()
        {
            var Get = new Get(Settings);
            var Req = Get.PaymentsSingle("2b62a358-9a1a-4c71-9450-e41", "6917d51a-be83-424f-b0a6-31fbf9574a79");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.ResourceNotFoundException))]
        public void TestGetASinglePaymentInvalidPaymentThrowsError()
        {
            var Get = new Get(Settings);
            var Req = Get.PaymentsSingle("2b62a358-9a1a-4c71-9450-e419e393dcd1", "6917d51a-be83-424f-b0a6");
        }
    }
}

