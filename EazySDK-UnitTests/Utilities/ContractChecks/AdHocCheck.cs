using Microsoft.VisualStudio.TestTools.UnitTesting;
using EazySDK;
using Microsoft.Extensions.Configuration;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class AdHocCheck
    {
        private IConfiguration Settings { get; set; }
        private EazySDK.Utilities.ContractPostChecks ContractCheck { get; set; }

        [TestInitialize]
        public void TestInit()
        {
            ClientHandler Handler = new ClientHandler();
            Settings = Handler.Settings();
            ContractCheck = new EazySDK.Utilities.ContractPostChecks();
            Settings.GetSection("currentEnvironment")["Environment"] = "sandbox";
        }

        [TestMethod]
        public void TestValidAdHocContract()
        {
            bool AdHoc = ContractCheck.CheckScheduleAdHocStatus("adhoc_monthly_free", Settings);
            Assert.IsTrue(AdHoc);
        }

        [TestMethod]
        public void TestValidNonAdHocContract()
        {
            bool AdHoc = ContractCheck.CheckScheduleAdHocStatus("test", Settings);
            Assert.IsFalse(AdHoc);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestInvalidContractThrowsError()
        {
            bool AdHoc = ContractCheck.CheckScheduleAdHocStatus("not a contract", Settings);
        }
    }
}

