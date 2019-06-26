using Microsoft.VisualStudio.TestTools.UnitTesting;
using EazySDK;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class TerminationDateCheck
    {
        private EazySDK.Utilities.ContractPostChecks ContractCheck { get; set; }

        [TestInitialize]
        public void TestInit()
        {
            ClientHandler Handler = new ClientHandler();
            ContractCheck = new EazySDK.Utilities.ContractPostChecks();
        }

        [TestMethod]
        public void TestTerminationDateAfterStartDate()
        {
            bool TerminationDate = ContractCheck.CheckTerminationDateIsAfterStartDate("2019-08-15", "2019-07-15");
            Assert.IsTrue(TerminationDate);
        }

        [TestMethod]
        public void TestAcceptsNonISODates()
        {
            bool TerminationDate = ContractCheck.CheckTerminationDateIsAfterStartDate("15/08/2019", "2019-07-15");
            Assert.IsTrue(TerminationDate);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestTerminationDateOnSameDayThrowsError()
        {
             ContractCheck.CheckTerminationDateIsAfterStartDate("2019-07-15", "2019-07-15");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestTerminationDateTooSoonThrowsError()
        {
            ContractCheck.CheckTerminationDateIsAfterStartDate("2019-06-15", "2019-07-15");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestInvalidTerminationDateThrowsError()
        {
            ContractCheck.CheckTerminationDateIsAfterStartDate("2019-15-15", "2019-07-15");
        }
    }
}