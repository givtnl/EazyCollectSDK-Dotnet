using Microsoft.VisualStudio.TestTools.UnitTesting;
using EazySDK;
using Microsoft.Extensions.Configuration;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class UnitTestPatchPayment
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
        public void TestPatchPaymentValidPatch()
        {
            var Patch = new Patch(Settings);
            var Req = Patch.Payment("2b62a358-9a1a-4c71-9450-e419e393dcd1", "a75f9829-2753-4f67-aafb-bb24aba27dd1", "10.00", "2020-08-15", "test comment");
            Assert.IsTrue(Req.Contains("Payment updated"));
        }
    }
}

