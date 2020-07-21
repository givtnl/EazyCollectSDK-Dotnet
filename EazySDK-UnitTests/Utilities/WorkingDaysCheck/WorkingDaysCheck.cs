using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using System;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class WorkingDaysCheck
    {
        private EazySDK.ClientHandler Handler { get; set; }
        private IConfiguration Settings { get; set; }
        private EazySDK.Utilities.CheckWorkingDays WorkingDays { get; set; }

        [TestInitialize]
        public void TestInit()
        {
            WorkingDays = new EazySDK.Utilities.CheckWorkingDays();
        }

        [TestMethod]
        public void TestOneWorkingDayInTheFuture()
        {
            DateTime x = WorkingDays.CheckWorkingDaysInFuture(1);
            DateTime y = new DateTime(2020, 07, 22);
            Assert.IsTrue(x == y);
        }

        [TestMethod]
        public void TestNextMondayInTheFuture()
        {
            DateTime x = WorkingDays.CheckWorkingDaysInFuture(3);
            DateTime y = new DateTime(2020, 07, 24);
            Assert.IsTrue(x == y);
        }

        [TestMethod]
        public void TestDayAfterNextBankHolidayInTheFuture()
        {
            DateTime x = WorkingDays.CheckWorkingDaysInFuture(29);
            DateTime y = new DateTime(2020, 09, 01);
            Assert.IsTrue(x == y);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidSettingsConfigurationException))]
        public void TestDayInThePastThrowsException()
        {
            DateTime x = WorkingDays.CheckWorkingDaysInFuture(-1);
            DateTime y = new DateTime(2019, 06, 25);
        }
    }
}

