using Microsoft.VisualStudio.TestTools.UnitTesting;
using EazySDK;
using Microsoft.Extensions.Configuration;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class UnitTestReactivate
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
        public void TestReactivateDirectDebit()
        {
            var Post = new Post(Settings);
            var Req = Post.ReactivateDirectDebit("2b62a358-9a1a-4c71-9450-e419e393dcd1");
            Assert.IsTrue(Req.Contains("Contract reactivated"));
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.ResourceNotFoundException))]
        public void TestRestartContractInvalidContractThrowException()
        {
            var Post = new Post(Settings);
            Post.ReactivateDirectDebit("2b62a358-9450-e419e393dcd1");
        }
    }
}

