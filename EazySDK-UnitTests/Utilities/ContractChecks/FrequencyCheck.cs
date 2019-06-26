using Microsoft.VisualStudio.TestTools.UnitTesting;
using EazySDK;
using Microsoft.Extensions.Configuration;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class FrequencyCheck
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
        public void TestValidWeeklySchedule()
        {
            int Frequency = ContractCheck.CheckFrequency("weekly_free", Settings);
            Assert.IsTrue(Frequency == 0);
        }

        [TestMethod]
        public void TestValidMonthlySchedule()
        {
            int Frequency = ContractCheck.CheckFrequency("monthly_free", Settings);
            Assert.IsTrue(Frequency == 1);
        }

        [TestMethod]
        public void TestValidAnnualFreeSchedule()
        {
            int Frequency = ContractCheck.CheckFrequency("annual_free", Settings);
            Assert.IsTrue(Frequency == 2);
        }

        [TestMethod]
        public void TestValidNoFrequency()
        {
            int Frequency = ContractCheck.CheckFrequency("adhoc_monthly_free", Settings);
            Assert.IsTrue(Frequency == -1);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidIOConfiguration))]
        public void TestInvalidScheduleName()
        {
            ContractCheck.CheckFrequency("not a schedule name", Settings);
        }
    }
}

