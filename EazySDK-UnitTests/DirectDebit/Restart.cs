using Microsoft.VisualStudio.TestTools.UnitTesting;
using EazySDK;
using Microsoft.Extensions.Configuration;
using System;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class UnitTestRestart
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
        public void TestRestartContractValidContract()
        {
            var Post = new Post(Settings);
            var Req = Post.RestartContract("a899ced6-a601-4146-92f7-5c8ee40bbf93", "Until further notice", "Switch to further notice", PaymentDayInMonth: "15", PaymentMonthInYear: "6");
            Assert.IsTrue(Req.Contains("Contract restarted"));
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.ResourceNotFoundException))]
        public void TestRestartContractInvalidContractThrowException()
        {
            var Post = new Post(Settings);
            Post.RestartContract("a899ced6-a601-4146-92f7-5c8", "Until further notice", "Switch to further notice", PaymentDayInMonth: "15", PaymentMonthInYear: "6");
        }
    }
}

