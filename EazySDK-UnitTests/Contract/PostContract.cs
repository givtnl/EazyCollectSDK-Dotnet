using Microsoft.VisualStudio.TestTools.UnitTesting;
using EazySDK;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class UnitTestPostContract
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
        public void TestPostAdHocContract()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "adhoc_monthly_free", "2019-07-15", false, "Until further notice", "Switch to further notice");

            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        public void TestPostAdHocContractAutoFixStartDate()
        {
            Settings.GetSection("contracts")["AutoFixStartDate"] = "true";
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "adhoc_monthly_free", "1990-07-15", false, "Until further notice", "Switch to further notice");

            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        public void TestPostAdHocContractAutoFixTerminationType()
        {
            Settings.GetSection("contracts")["AutoFixTerminationTypeAdHoc"] = "true";
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "adhoc_monthly_free", "2019-07-15", false, "End on exact date", "Switch to further notice");

            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        public void TestPostAdHocContractAutoFixAtTheEnd()
        {
            Settings.GetSection("contracts")["AutoFixAtTheEndAdHoc"] = "true";
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "adhoc_monthly_free", "2019-07-15", false, "Until further notice", "Expire");

            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        public void TestPostAdHocContractGiftAidTrue()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "adhoc_monthly_free", "2019-07-15", true, "Until further notice", "Switch to further notice");

            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        public void TestPostAdHocContractAllowAddiitonalRef()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "adhoc_monthly_free", "2019-07-15", false, "Until further notice", "Switch to further notice", AdditionalReference: "Test");

            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.ResourceNotFoundException))]
        public void TestPostAdHocContractInvalidCustomerException()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba8", "adhoc_monthly_free", "2019-07-15", false, "Until further notice", "Switch to further notice");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidStartDateException))]
        public void TestPostAdHocContractInvalidStartDateException()
        {
            Settings.GetSection("contracts")["AutoFixStartDate"] = "false";
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "adhoc_monthly_free", "1990-07-15", false, "Until further notice", "Switch to further notice");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPostAdHocContractInvalidTerminationTypeException()
        {
            Settings.GetSection("contracts")["AutoFixTerminationTypeAdHoc"] = "false";
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "adhoc_monthly_free", "2019-07-15", false, "Take certain number of debits", "Switch to further notice");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPostAdHocContractInvalidAtTheEndException()
        {
            Settings.GetSection("contracts")["AutoFixAtTheEndAdHoc"] = "false";
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "adhoc_monthly_free", "2019-07-15", false, "Until further notice", "Expire");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.ParameterNotAllowedException))]
        public void TestPostAdHocContractInitialAmountException()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "adhoc_monthly_free", "2019-07-15", false, "Until further notice", "Switch to further notice", InitialAmount: "10.00");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.ParameterNotAllowedException))]
        public void TestPostAdHocContractExtraInitialAmountException()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "adhoc_monthly_free", "2019-07-15", false, "Until further notice", "Switch to further notice", ExtraInitialAmount: "10.00");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.ParameterNotAllowedException))]
        public void TestPostAdHocContractFinalAmountException()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "adhoc_monthly_free", "2019-07-15", false, "Until further notice", "Switch to further notice", FinalAmount: "10.00");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPostContractInvalidScheduleNameException()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "not a schedule", "2019-07-15", false, "Until further notice", "Switch to further notice");
        }

        [TestMethod]
        public void TestPostMonthlyContractUntilFurtherNotice()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "monthly_free", "2019-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "15");
            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        public void TestPostMonthlyContractTakeCertainNumberOfDebits()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "monthly_free", "2019-07-15", false, "Take certain number of debits", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "15", NumberofDebits: "10");
            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        public void TestPostMonthlyContractEndOnExactDate()
        {

            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "monthly_free", "2019-07-15", false, "End On Exact Date", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "15", TerminationDate: "2019-10-10");
            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        public void TestPostMonthlyContractAutoFixStartDate()
        {
            Settings.GetSection("contracts")["AutoFixStartDate"] = "true";
            Settings.GetSection("contracts")["AutoFixPaymentDayInMonth"] = "true";
            Settings.GetSection("contracts")["AutoFixPaymentMonthInYear"] = "true";
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "monthly_free", "1990-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "1");
            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        public void TestPostMonthlyContractAutoFixPaymentDayInMonth()
        {
            Settings.GetSection("contracts")["AutoFixPaymentDayInMonth"] = "true";
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "monthly_free", "2019-08-01", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "15");
            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        public void TestPostMonthlyContractInitialAmount()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "monthly_free", "2019-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "15", InitialAmount: "10.00");
            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        public void TestPostMonthlyContractExtraInitialAmount()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "monthly_free", "2019-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "15", ExtraInitialAmount: "10.00");
            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        public void TestPostMonthlyContractFinalAmount()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "monthly_free", "2019-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "15", FinalAmount: "10.00");
            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        public void TestPostMonthlyContractAdditionalReference()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "monthly_free", "2019-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "15", AdditionalReference: "A Reference");
            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.ResourceNotFoundException))]
        public void TestPostMonthlyContractInvalidCustomerException()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19db", "monthly_free", "2019-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "15");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPostMonthlyContractInvalidScheduleException()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "not a schedule", "2019-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "15");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidStartDateException))]
        public void TestPostMonthlyContractInvalidStartDateException()
        {
            Settings.GetSection("contracts")["AutoFixStartDate"] = "false";
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "monthly_free", "1990-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "15");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPostMonthlyContractInvalidPaymentDayInMonthException()
        {
            Settings.GetSection("contracts")["AutoFixPaymentDayInMonth"] = "false";
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "monthly_free", "2019-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "1");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.EmptyRequiredParameterException))]
        public void TestPostMonthlyContractFrequencyMustBePassedException()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "monthly_free", "2019-07-15", false, "Until further notice", "Switch to further notice", PaymentAmount: "10.00", PaymentDayInMonth: "15");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.EmptyRequiredParameterException))]
        public void TestPostMonthlyContractPaymentAmountMustBePassedException()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "monthly_free", "2019-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentDayInMonth: "15");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.EmptyRequiredParameterException))]
        public void TestPostMonthlyContractIfCertainNumberOfDebitsNumberOfDebitsMustBePassedException()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "monthly_free", "2019-07-15", false, "Take certain number of debits", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "15");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.EmptyRequiredParameterException))]
        public void TestPostMonthlyContractIfEndOnExactDateTerminationDateMustBePassedException()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "monthly_free", "2019-07-15", false, "End On Exact Date", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "15");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPostMonthlyContractIfEndOnExactDateTerminationDateMustNotBeSoonerThanStartDateException()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "monthly_free", "2019-07-15", false, "End On Exact Date", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "15", TerminationDate: "1990-01-01");
        }

        [TestMethod]
        public void TestPostAnnualContractUntilFurtherNotice()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "annual_free", "2019-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "15", PaymentMonthInYear: "7");
            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        public void TestPostAnnualContractTakeCertainNumberOfDebits()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "annual_free", "2019-07-15", false, "Take certain number of debits", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "15", PaymentMonthInYear: "7", NumberofDebits: "10");
            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        public void TestPostAnnualContractEndOnExactDate()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "annual_free", "2019-07-15", false, "End on exact date", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "15", PaymentMonthInYear: "7", TerminationDate: "2020-01-01");
            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        public void TestPostAnnualContractAutoFixStartDate()
        {
            Settings.GetSection("contracts")["AutoFixStartDate"] = "true";
            Settings.GetSection("contracts")["AutoFixPaymentDayInMonth"] = "true";
            Settings.GetSection("contracts")["AutoFixPaymentMonthInYear"] = "true";
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "annual_free", "1990-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "15", PaymentMonthInYear: "7");
            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        public void TestPostAnnualContractAutoFixPaymentDayInMonth()
        {
            Settings.GetSection("contracts")["AutoFixPaymentDayInMonth"] = "true";
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "annual_free", "2019-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "1", PaymentMonthInYear: "7");
            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        public void TestPostAnnualContractAutoFixPaymentMonthInYear()
        {
            Settings.GetSection("contracts")["AutoFixPaymentMonthInYear"] = "true";
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "annual_free", "2019-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "15", PaymentMonthInYear: "1");
            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        public void TestPostAnnualContractInitialAmount()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "annual_free", "2019-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "15", PaymentMonthInYear: "7", InitialAmount: "10.00");
            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        public void TestPostAnnualContractExtraInitialAmount()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "annual_free", "2019-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "15", PaymentMonthInYear: "7", ExtraInitialAmount: "10.00");
            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        public void TestPostAnnualContractFinalAmount()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "annual_free", "2019-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "15", PaymentMonthInYear: "7", FinalAmount: "10.00");
            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        public void TestPostAnnualContractAdditionalReference()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "annual_free", "2019-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "15", PaymentMonthInYear: "7", AdditionalReference: "A Reference");
            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.ResourceNotFoundException))]
        public void TestPostAnnualContractInvalidCustomerException()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dbaf", "annual_free", "2019-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "15", PaymentMonthInYear: "7");
            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidStartDateException))]
        public void TestPostAnnualContractInvalidStartDateException()
        {
            Settings.GetSection("Contracts")["AutoFixStartDate"] = "false";
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "annual_free", "1990-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "15", PaymentMonthInYear: "7");
            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPostAnnualContractInvalidPaymentMonthInYearException()
        {
            Settings.GetSection("Contracts")["AutoFixPaymentDayInYear"] = "false";
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "annual_free", "2019-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "15", PaymentMonthInYear: "1");
            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.EmptyRequiredParameterException))]
        public void TestPostAnnualContractPaymentAmountMustBePassedException()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "annual_free", "2019-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentDayInMonth: "15", PaymentMonthInYear: "7");
            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.EmptyRequiredParameterException))]
        public void TestPostAnnualContractFrequencyMustBePassedException()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "annual_free", "2019-07-15", false, "Until further notice", "Switch to further notice", PaymentAmount: "10.00", PaymentDayInMonth: "15", PaymentMonthInYear: "7");
            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.EmptyRequiredParameterException))]
        public void TestPostAnnualContractIfEndOnExactDateTerminationDateMustBePassed()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "annual_free", "2019-07-15", false, "End on exact date", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "15", PaymentMonthInYear: "7");
            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPostAnnualContractIfEndOnExactDateTerminationDateMustNotBeSoonerThanStartDate()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "annual_free", "2019-07-15", false, "End on exact date", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInMonth: "15", PaymentMonthInYear: "7", TerminationDate: "1990-01-01");
            Assert.IsTrue(Req.Contains("DirectDebitRef"));
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPostWeeklyContractInvalidCustomerException()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba8", "weekly_free", "2019-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInWeek: "Monday", AdditionalReference: "A Reference");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidStartDateException))]
        public void TestPostWeeklyContractInvalidStartDateException()
        {
            Settings.GetSection("contracts")["AutoFixStartDate"] = "false";
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "weekly_free", "1990-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInWeek: "Monday", AdditionalReference: "A Reference");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPostWeeklyContractInvalidPaymentDayInWeekException()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "weekly_free", "2019-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInWeek: "Sunday", AdditionalReference: "A Reference");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.EmptyRequiredParameterException))]
        public void TestPostWeeklyContractFrequencyMustBePassedException()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "weekly_free", "2019-07-15", false, "Until further notice", "Switch to further notice", PaymentAmount: "10.00", PaymentDayInWeek: "Monday", AdditionalReference: "A Reference");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.EmptyRequiredParameterException))]
        public void TestPostWeeklyContractPaymentAmountMustBePassedException()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "weekly_free", "2019-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentDayInWeek: "Monday", AdditionalReference: "A Reference");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPostWeeklyContractIfCertainNumberOfDebitsNumberOfDebitsMustBePassedException()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "weekly_free", "2019-07-15", false, "Take certain number of debits", "Switch to further notice", PaymentAmount: "10.00", Frequency: "1", PaymentDayInWeek: "Monday", AdditionalReference: "A Reference");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPostWeeklyContractIfEndOnExactDateTerminationDateMustBePassedException()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "weekly_free", "2019-07-15", false, "End on exact date", "Switch to further notice", PaymentAmount: "10.00", Frequency: "1", PaymentDayInWeek: "Monday", AdditionalReference: "A Reference");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidParameterException))]
        public void TestPostWeeklyContractIfEndOnExactDateTerminationDateMustBeLaterThanStartDateException()
        {
            var Post = new Post(Settings);
            var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "weekly_free", "2019-07-15", false, "End on exact date", "Switch to further notice", TerminationDate: "1990-01-01", PaymentAmount: "10.00", Frequency: "1", PaymentDayInWeek: "Monday", AdditionalReference: "A Reference");
        }

        ///<summary>It is a known issue that non-adhoc weekly contracts are not functioning correctly.</summary>
        //[TestMethod]
        //public void TestPostWeeklyContractTakeCertainNumberOfDebits()
        //{
        //    var Post = new Post(Settings);
        //    var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "weekly_free", "2019-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInWeek: "Monday", NumberofDebits: "10");

        //    Assert.IsTrue(Req.Contains("DirectDebitRef"));
        //}

        //[TestMethod]
        //public void TestPostWeeklyContractEndOnExactDate()
        //{
        //    var Post = new Post(Settings);
        //    var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "weekly_free", "2019-07-15", false, "End on exact date", "Expire", Frequency: "1", PaymentAmount: "10.00", PaymentDayInWeek: "Monday", TerminationDate: "2020-01-01");

        //    Assert.IsTrue(Req.Contains("DirectDebitRef"));
        //}

        //[TestMethod]
        //public void TestPostWeeklyContractAutoFixStartDate()
        //{
        //    Settings.GetSection("contracts")["AutoFixStartDate"] = "true";
        //    var Post = new Post(Settings);
        //    var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "weekly_free", "1990-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInWeek: "Monday");

        //    Assert.IsTrue(Req.Contains("DirectDebitRef"));
        //}

        //[TestMethod]
        //public void TestPostWeeklyContractExtraInitialAmount()
        //{
        //    var Post = new Post(Settings);
        //    var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "weekly_free", "2019-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInWeek: "Monday", ExtraInitialAmount: "10.00");

        //    Assert.IsTrue(Req.Contains("DirectDebitRef"));
        //}

        //[TestMethod]
        //public void TestPostWeeklyContractFinalAmount()
        //{
        //    var Post = new Post(Settings);
        //    var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "weekly_free", "2019-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInWeek: "Monday", FinalAmount: "10.00");

        //    Assert.IsTrue(Req.Contains("DirectDebitRef"));
        //}

        //[TestMethod]
        //public void TestPostWeeklyContractAdditionalReference()
        //{
        //    var Post = new Post(Settings);
        //    var Req = Post.Contract("310a826b-d095-48e7-a55a-19dba82c566f", "weekly_free", "2019-07-15", false, "Until further notice", "Switch to further notice", Frequency: "1", PaymentAmount: "10.00", PaymentDayInWeek: "1", AdditionalReference: "A Reference");
        //    Assert.IsTrue(Req.Contains("DirectDebitRef"));
        //}
    }
}
