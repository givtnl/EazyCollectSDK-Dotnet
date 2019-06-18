using Microsoft.VisualStudio.TestTools.UnitTesting;
using EazySDK;
using Microsoft.Extensions.Configuration;
using System;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class UnitTestPostCustomer
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
        public void TestPostCustomerUsingOnlyRequiredFields()
        {
            Random rand = new Random();
            var randInt = rand.Next(100000, 999999).ToString();

            var Post = new Post(Settings);
            var Req = Post.Customer(randInt + "@test.com", "Mr", randInt, "John", "Doe", "1 Tebbit Mews", "GL52 2NF", "12345678", "123456", "Mr John Doe");
            Assert.IsTrue(Req.Contains(randInt));
        }

        [TestMethod]
        public void TestPostCustomerUsingAllFields()
        {
            Random rand = new Random();
            var randInt = rand.Next(100000, 999999).ToString();

            var Post = new Post(Settings);
            var Req = Post.Customer(randInt + "@test.com", "Mr", randInt, "John", "Doe", "1 Tebbit Mews", "GL52 2NF", "12345678", "123456", "Mr John Doe", "Eazy Collect", "Cheltenham",
                "Gloucestershire", "Acme Ltd", "1990-01-01", "JD", "01242000000", "07300000000", "01242000000");
            Assert.IsTrue(Req.Contains(randInt));
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.RecordAlreadyExistsException))]
        public void TestCustomerReferenceMustBeUnique()
        {
            Random rand = new Random();
            var randInt = rand.Next(100000, 999999).ToString();

            var Post = new Post(Settings);
            var Req = Post.Customer(randInt + "@test.com", "Mr", "test-000001", "John", "Doe", "1 Tebbit Mews", "GL52 2NF", "12345678", "123456", "Mr John Doe", "Eazy Collect", "Cheltenham",
                "Gloucestershire", "Acme Ltd", "1990-01-01", "JD", "01242000000", "07300000000", "01242000000");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.EmptyRequiredParameterException))]
        public void TestEmailMustBePassed()
        {
            Random rand = new Random();
            var randInt = rand.Next(100000, 999999).ToString();

            var Post = new Post(Settings);
            var Req = Post.Customer("", "Mr", randInt, "John", "Doe", "1 Tebbit Mews", "GL52 2NF", "12345678", "123456", "Mr John Doe");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.EmptyRequiredParameterException))]
        public void TestTitleMustBePassed()
        {
            Random rand = new Random();
            var randInt = rand.Next(100000, 999999).ToString();

            var Post = new Post(Settings);
            var Req = Post.Customer(randInt + "@test.com", "", randInt, "John", "Doe", "1 Tebbit Mews", "GL52 2NF", "12345678", "123456", "Mr John Doe");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.EmptyRequiredParameterException))]
        public void TestCustomerReferenceMustBePassed()
        {
            Random rand = new Random();
            var randInt = rand.Next(100000, 999999).ToString();

            var Post = new Post(Settings);
            var Req = Post.Customer(randInt + "@test.com", "Mr", "", "John", "Doe", "1 Tebbit Mews", "GL52 2NF", "12345678", "123456", "Mr John Doe");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.EmptyRequiredParameterException))]
        public void TestFirstNameMustBePassed()
        {
            Random rand = new Random();
            var randInt = rand.Next(100000, 999999).ToString();

            var Post = new Post(Settings);
            var Req = Post.Customer(randInt + "@test.com", "Mr", randInt, "", "Doe", "1 Tebbit Mews", "GL52 2NF", "12345678", "123456", "Mr John Doe");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.EmptyRequiredParameterException))]
        public void TestSurnameMustBePassed()
        {
            Random rand = new Random();
            var randInt = rand.Next(100000, 999999).ToString();

            var Post = new Post(Settings);
            var Req = Post.Customer(randInt + "@test.com", "Mr", randInt, "John", "", "1 Tebbit Mews", "GL52 2NF", "12345678", "123456", "Mr John Doe");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.EmptyRequiredParameterException))]
        public void TestFirstLineMustBePassed()
        {
            Random rand = new Random();
            var randInt = rand.Next(100000, 999999).ToString();

            var Post = new Post(Settings);
            var Req = Post.Customer(randInt + "@test.com", "Mr", randInt, "John", "Doe", "", "GL52 2NF", "12345678", "123456", "Mr John Doe");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.EmptyRequiredParameterException))]
        public void TestPostCodeMustBePassed()
        {
            Random rand = new Random();
            var randInt = rand.Next(100000, 999999).ToString();

            var Post = new Post(Settings);
            var Req = Post.Customer(randInt + "@test.com", "Mr", randInt, "John", "Doe", "1 Tebbit Mews", "", "12345678", "123456", "Mr John Doe");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.EmptyRequiredParameterException))]
        public void TestAccountNumberMustBePassed()
        {
            Random rand = new Random();
            var randInt = rand.Next(100000, 999999).ToString();

            var Post = new Post(Settings);
            var Req = Post.Customer(randInt + "@test.com", "Mr", randInt, "John", "Doe", "1 Tebbit Mews", "GL52 2NF", "", "123456", "Mr John Doe");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.EmptyRequiredParameterException))]
        public void TestSortCodeMustBePassed()
        {
            Random rand = new Random();
            var randInt = rand.Next(100000, 999999).ToString();

            var Post = new Post(Settings);
            var Req = Post.Customer(randInt + "@test.com", "Mr", randInt, "John", "Doe", "1 Tebbit Mews", "GL52 2NF", "12345678", "", "Mr John Doe");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.EmptyRequiredParameterException))]
        public void TestAccountHolderNameMustBePassed()
        {
            Random rand = new Random();
            var randInt = rand.Next(100000, 999999).ToString();

            var Post = new Post(Settings);
            var Req = Post.Customer(randInt + "@test.com", "Mr", randInt, "John", "Doe", "1 Tebbit Mews", "GL52 2NF", "12345678", "123456", "");
        }

    }
}

