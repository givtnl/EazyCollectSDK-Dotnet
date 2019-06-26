using Microsoft.VisualStudio.TestTools.UnitTesting;
using EazySDK;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class AtTheEndCheck
    {
        private EazySDK.Utilities.ContractPostChecks ContractCheck { get; set; }

        [TestInitialize]
        public void TestInit()
        {
            ClientHandler Handler = new ClientHandler();
            ContractCheck = new EazySDK.Utilities.ContractPostChecks();
        }

        [TestMethod]
        public void TestAtTheEndExpireReturns0()
        {
            int AtTheEnd = ContractCheck.CheckAtTheEndIsValid("Expire");
            Assert.IsTrue(AtTheEnd == 0);
        }

        [TestMethod]
        public void TestAtTheEndSwitchToFurtherNoticeReturns1()
        {
            int AtTheEnd = ContractCheck.CheckAtTheEndIsValid("Switch to further notice");
            Assert.IsTrue(AtTheEnd == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestInvalidAtTheEndThrowsError()
        {
            ContractCheck.CheckAtTheEndIsValid("not an at the end");
        }
    }
}

