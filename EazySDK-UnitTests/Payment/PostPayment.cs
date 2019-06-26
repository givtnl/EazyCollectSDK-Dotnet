using Microsoft.VisualStudio.TestTools.UnitTesting;
using EazySDK;
using Microsoft.Extensions.Configuration;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class PostPayment
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
        public void TestPostPaymentUsingAllRequiredFileds()
        {
            var Post = new Post(Settings);
            var Req = Post.Payment("1802e1dd-a657-428c-b8d0-ba162fc76203", "10.00", "2019-07-15", "A comment");
            Assert.IsTrue(Req.Contains("\"Error\":null"));
        }

        [TestMethod]
        public void TestPostPaymentIsCreditAllowedIfSettingTrue()
        {
            Settings.GetSection("payments")["IsCreditAllowed"] = "true";
            var Post = new Post(Settings);
            var Req = Post.Payment("1802e1dd-a657-428c-b8d0-ba162fc76203", "10.00", "2019-07-15", "A comment", true);
            Assert.IsTrue(Req.Contains("Client is not allowed to credit customer"));
        }

        [TestMethod]
        public void TestPostPaymentAutoFixPaymentDate()
        {
            Settings.GetSection("payments")["AutoFixPaymentDate"] = "true";
            var Post = new Post(Settings);
            var Req = Post.Payment("1802e1dd-a657-428c-b8d0-ba162fc76203", "10.00", "2018-07-15", "A comment");
            Assert.IsTrue(Req.Contains("\"Error\":null"));
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.EmptyRequiredParameterException))]
        public void TestPostPaymentACommentIsRequired()
        {
            var Post = new Post(Settings);
            Post.Payment("1802e1dd-a657-428c-b8d0-ba162fc76203", "10.00", "2019-07-15", "");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPostPaymentPaymentAmountCannotBeNegative()
        {
            var Post = new Post(Settings);
            Post.Payment("1802e1dd-a657-428c-b8d0-ba162fc76203", "-10.00", "2019-07-15", "A comment");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.ResourceNotFoundException))]
        public void TestPostPaymentInvalidContractThrowsException()
        {
            var Post = new Post(Settings);
            Post.Payment("1802e1dd-a657-428c-b8d0-b203", "10.00", "2019-07-15", "A comment");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPostPaymentIsCreditNotAllowedIfSettingIsFalse()
        {
            Settings.GetSection("payments")["IsCreditAllowed"] = "false";
            var Post = new Post(Settings);
            Post.Payment("1802e1dd-a657-428c-b8d0-ba162fc76203", "10.00", "2019-07-15", "A comment", true);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidPaymentDateException))]
        public void TestPostPaymentInvalidPaymentDateThrowsException()
        {
            Settings.GetSection("payments")["AutoFixPaymentDate"] = "false";
            var Post = new Post(Settings);
            Post.Payment("1802e1dd-a657-428c-b8d0-ba162fc76203", "10.00", "2018-07-15", "A comment");
        }
    }
}

