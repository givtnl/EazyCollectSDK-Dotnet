using Microsoft.VisualStudio.TestTools.UnitTesting;
using EazySDK;
using Microsoft.Extensions.Configuration;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class ScheduleNameCheck
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
        public void TestValidScheduleName()
        {
            bool Schedule = ContractCheck.CheckScheduleNameIsAvailable("adhoc_monthly_free", Settings);
            Assert.IsTrue(Schedule);
        }

        [TestMethod]
        public void TestValidScheduleNameFromDifferentService()
        {
            bool Schedule = ContractCheck.CheckScheduleAdHocStatus("Best test", Settings);
            Assert.IsTrue(Schedule);
        }

        [TestMethod]
        public void TestScheduleIsNotCaseSensitive()
        {
            bool Schedule = ContractCheck.CheckScheduleAdHocStatus("Adhoc_monthly_free", Settings);
            Assert.IsTrue(Schedule);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestInvalidScheduleNameThrowsError()
        {
            ContractCheck.CheckScheduleAdHocStatus("not a schedule name", Settings);
        }
    }
}

