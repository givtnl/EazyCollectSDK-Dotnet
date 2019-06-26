using Microsoft.VisualStudio.TestTools.UnitTesting;
using EazySDK;
using Microsoft.Extensions.Configuration;
using System;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class UnitTestPatchContract
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
        public void TestPatchContractPatchCOntractAmount()
        {
            Random rand = new Random();
            var randInt = rand.Next(1, 100).ToString();

            var Patch = new Patch(Settings);
            var Req = Patch.ContractAmount("1802e1dd-a657-428c-b8d0-ba162fc76203", randInt, "Change contract amount");

            Assert.IsTrue(Req.Contains(randInt));
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPatchContractAmountPaymentAmountMustContainOnlyNumbers()
        {
            var Patch = new Patch(Settings);
            Patch.ContractAmount("1802e1dd-a657-428c-b8d0-ba162fc76203", "ten", "Change contract amount");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPatchContractAmountPaymentCannotBeNegative()
        {
            var Patch = new Patch(Settings);
            Patch.ContractAmount("1802e1dd-a657-428c-b8d0-ba162fc76203", "-10", "Change contract amount");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.EmptyRequiredParameterException))]
        public void TestPatchContractAmountCommentCannotBeEmpty()
        {
            var Patch = new Patch(Settings);
            Patch.ContractAmount("1802e1dd-a657-428c-b8d0-ba162fc76203", "10", "");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.ResourceNotFoundException))]
        public void TestPatchContractAmountInvalidContractThrowsException()
        {
            var Patch = new Patch(Settings);
            Patch.ContractAmount("1802e1dd-a657-428c-b8d0", "10", "Change contract amount");
        }

        [TestMethod]
        public void TestPatchCollectionDateMonthlyContractsValidCollectionDate()
        {
            Random rand = new Random();
            var randInt = rand.Next(1, 28).ToString();

            var Patch = new Patch(Settings);
            var Req = Patch.ContractDayMonthly("6dfb8179-2f7f-46cb-bc05-fe7f2d36bf36", randInt, "Change contract amount", false);

            Assert.IsTrue(Req.Contains(randInt));
        }

        [TestMethod]
        public void TestPatchCollectionDateMonthlyContractsPatchNextPaymentAmount()
        {
            Random rand = new Random();
            var randInt = rand.Next(1, 28).ToString();

            var Patch = new Patch(Settings);
            var Req = Patch.ContractDayMonthly("6dfb8179-2f7f-46cb-bc05-fe7f2d36bf36", randInt, "Change contract amount", true, "10.50");
            Assert.IsTrue(Req.Contains(randInt));
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPatchCollectionDateMonthlyContractsInvalidCollectionDateThrowsException()
        {

            var Patch = new Patch(Settings);
            Patch.ContractDayMonthly("6dfb8179-2f7f-46cb-bc05-fe7f2d36bf36", "30", "Change contract amount", false);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.EmptyRequiredParameterException))]
        public void TestPatchCollectionDateMonthlyContractsCommentCannotBeEmpty()
        {

            var Patch = new Patch(Settings);
            Patch.ContractDayMonthly("6dfb8179-2f7f-46cb-bc05-fe7f2d36bf36", "28", "" , false);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.ParameterNotAllowedException))]
        public void TestPatchCollectionDateMonthlyIfNotAmendNextPaymentNextPaymentAmountMustNotBeCalled()
        {

            var Patch = new Patch(Settings);
            Patch.ContractDayMonthly("6dfb8179-2f7f-46cb-bc05-fe7f2d36bf36", "28", "A comment", false, "10.50");
        }


        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.EmptyRequiredParameterException))]
        public void TestPatchCollectionDateMonthlyContractsIfAmendNextPaymentPaymentAmountMustBeCalled()
        {

            var Patch = new Patch(Settings);
            Patch.ContractDayMonthly("6dfb8179-2f7f-46cb-bc05-fe7f2d36bf36", "28", "", true);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.ResourceNotFoundException))]
        public void TestPatchCollectionDateMonthlyInvalidContractThrowsException()
        {
            var Patch = new Patch(Settings);
            Patch.ContractDayMonthly("6dfb8179-2f7f-46cb-bc05", "28", "Change contract amount", false);
        }

        [TestMethod]
        public void TestPatchCollectionDateAnnualContractsValidCollectionDate()
        {
            Random rand = new Random();
            var m = rand.Next(1, 12).ToString();
            var d = rand.Next(1, 28).ToString();

            var Patch = new Patch(Settings);
            var Req = Patch.ContractDayAnnually("8998eab6-f4fe-43b8-b718-78b4520e5529", d, m, "Change contract amount", false);

            Assert.IsTrue(Req.Contains(m));
        }

        [TestMethod]
        public void TestPatchCollectionDateMAnnualContractsPatchNextPaymentAmount()
        {
            Random rand = new Random();
            var m = rand.Next(1, 12).ToString();
            var d = rand.Next(1, 28).ToString();

            var Patch = new Patch(Settings);
            var Req = Patch.ContractDayAnnually("8998eab6-f4fe-43b8-b718-78b4520e5529", d, m, "Change contract amount", true, "10.50");

            Assert.IsTrue(Req.Contains(m));
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPatchCollectionDateAnnualContractsInvalidCollectionDayThrowsException()
        {

            var Patch = new Patch(Settings);
            Patch.ContractDayAnnually("8998eab6-f4fe-43b8-b718-78b4520e5529", "30", "6", "Change contract amount", false);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPatchCollectionDateAnnualContractsInvalidCollectionMonthThrowsException()
        {
            var Patch = new Patch(Settings);
            Patch.ContractDayAnnually("8998eab6-f4fe-43b8-b718-78b4520e5529", "15", "15", "Change contract amount", false);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.EmptyRequiredParameterException))]
        public void TestPatchCollectionDateAnnualContractsCommentCannotBeEmpty()
        {

            var Patch = new Patch(Settings);
            Patch.ContractDayAnnually("8998eab6-f4fe-43b8-b718-78b4520e5529", "15", "7", "", false);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.ParameterNotAllowedException))]
        public void TestPatchCollectionDateAnnualCfNotAmendNextPaymentNextPaymentAmountMustNotBeCalled()
        {

            var Patch = new Patch(Settings);
            Patch.ContractDayAnnually("8998eab6-f4fe-43b8-b718-78b4520e5529", "15", "7", "A comment", false, "10.50");
        }


        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.EmptyRequiredParameterException))]
        public void TestPatchCollectionDateAnnualContractsIfAmendNextPaymentPaymentAmountMustBeCalled()
        {

            var Patch = new Patch(Settings);
            Patch.ContractDayAnnually("8998eab6-f4fe-43b8-b718-78b4520e5529", "15", "7", "A comment", true);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.ResourceNotFoundException))]
        public void TestPatchCollectionDateAnnualInvalidContractThrowsException()
        {
            var Patch = new Patch(Settings);
            Patch.ContractDayAnnually("8998eab6-f4fe-43b8-b718", "15", "7", "A comment", false);
        }
    }
}
