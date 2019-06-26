using Microsoft.VisualStudio.TestTools.UnitTesting;
using EazySDK;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class TerminationTypeCheck
    {
        private EazySDK.Utilities.ContractPostChecks ContractCheck { get; set; }

        [TestInitialize]
        public void TestInit()
        {
            ClientHandler Handler = new ClientHandler();
            ContractCheck = new EazySDK.Utilities.ContractPostChecks();
        }

        [TestMethod]
        public void TestTerminationTypeTakeCertainNumberOfDebitsReturns0()
        {
            int AtTheEnd = ContractCheck.CheckTerminationTypeIsValid("take certain number of debits");
            Assert.IsTrue(AtTheEnd == 0);
        }

        [TestMethod]
        public void TestTerminationTypeUntilFurtherNoticeReturns1()
        {
            int AtTheEnd = ContractCheck.CheckTerminationTypeIsValid("until further notice");
            Assert.IsTrue(AtTheEnd == 1);
        }

        [TestMethod]
        public void TestTerminationTypeEndOnExactDateReturns2()
        {
            int AtTheEnd = ContractCheck.CheckTerminationTypeIsValid("end on exact date");
            Assert.IsTrue(AtTheEnd == 2);
        }

        [TestMethod]
        public void TestTerminationTypeIsNotCaseSensitive()
        {
            int AtTheEnd = ContractCheck.CheckTerminationTypeIsValid("Until further notice");
            Assert.IsTrue(AtTheEnd == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestTerminationTypeInvalidTypeThrowsError()
        {
            ContractCheck.CheckTerminationTypeIsValid("not a termination type");
        }
    }
}

