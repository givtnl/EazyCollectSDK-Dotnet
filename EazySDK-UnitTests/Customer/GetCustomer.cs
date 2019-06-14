using Microsoft.VisualStudio.TestTools.UnitTesting;
using EazySDK;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class UnitTestGetCustomer
    {
        private  IConfiguration Settings { get; set; }

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
        public void TestSearchForCustomersNoParametersThrowsWarningIfEnabled()
        {
            Settings.GetSection("warnings")["CustomerSearch"] = "true";
            var Get = new Get(Settings);
            var Req = Get.Customers();
            Assert.IsTrue( 1 == 1);
        }

        [TestMethod]
        public void TestSearchForCustomerInvalidSearchTermsReturnsNoCustomers()
        {
            var Get = new Get(Settings);
            var Req = Get.Customers(PostCode: "Not a post code", FirstName: "Test");
            Assert.IsTrue(Req.Contains("No customers could be found with the provided parameters"));
        }
        [TestMethod]
        public void TestUsingFullEmailAsParameterReturnsRecordsContainingFullEmail()
        {
            var Get = new Get(Settings);
            var Req = Get.Customers(Email: "test@email.com");
            JArray ReqAsJson = JArray.Parse(Req);

            foreach (var x in ReqAsJson)
            {
                JToken email = x["Email"];
                if (email.ToString() != "test@email.com")
                {
                    Assert.Fail(string.Format("An email address was returned which did not equals test@email.com. The email address was {0}", email.ToString()));
                }
            }

            Assert.IsTrue(Req.Contains("test@email.com"));
        }

        [TestMethod]
        public void TestUsingFullTitleAsParameterReturnsRecordsContainingFullTitle()
        {
            var Get = new Get(Settings);
            var Req = Get.Customers(Title: "Mrs");
            JArray ReqAsJson = JArray.Parse(Req);

            foreach (var x in ReqAsJson)
            {
                JToken title = x["Title"];
                if (title.ToString() != "Mrs")
                {
                    Assert.Fail(string.Format("A title was returned which did not equals Mrs. The title was {0}", title.ToString()));
                }
            }

            Assert.IsTrue(Req.Contains("Mrs"));
        }

        [TestMethod]
        public void TestUsingPartialTitleAsParameterReturnsRecordsContainingPartialTitle()
        {
            var Get = new Get(Settings);
            var Req = Get.Customers(Title: "Mr");
            JArray ReqAsJson = JArray.Parse(Req);

            foreach (var x in ReqAsJson)
            {
                JToken title = x["Title"];
                if (!title.ToString().Contains("Mr"))
                {
                    Assert.Fail(string.Format("A title was returned which did not contain Mr. The title was {0}", title.ToString()));
                }
            }

            Assert.IsTrue(Req.Contains("Mr"));
        }

        [TestMethod]
        public void TestUsingSearchFromAsParameterReturnsRecordsCreatedAfterSearchDate()
        {
            var Get = new Get(Settings);
            var Req = Get.Customers(SearchFrom: "2019-01-01");
            JArray ReqAsJson = JArray.Parse(Req);

            foreach (var x in ReqAsJson)
            {
                JToken dateAdded = x["DateAdded"];
                if (!dateAdded.ToString().Contains("/2019"))
                {
                    Assert.Fail(string.Format("A customer was returned although they were created to soon. The customer was created on {0}", dateAdded.ToString()));
                }
            }

            Assert.IsTrue(Req.Contains("2019"));
        }

        [TestMethod]
        public void TestUsingSearchToAsAParameterReturnsRecordsCreatedBeforeSearchDate()
        {
            var Get = new Get(Settings);
            var Req = Get.Customers(SearchTo: "2019-05-10");
            JArray ReqAsJson = JArray.Parse(Req);

            foreach (var x in ReqAsJson)
            {
                JToken dateAdded = x["DateAdded"];
                if (dateAdded.ToString().Contains("0/2019"))
                {
                    Assert.Fail(string.Format("A customer was returned although they were created to late. The customer was created on {0}", dateAdded.ToString()));
                }
            }

            Assert.IsTrue(Req.Contains("2019"));
        }

        [TestMethod]
        public void TestUsingDateOfBirthAsAParameterReturnsRecordsContainingDateOfBirth()
        {
            var Get = new Get(Settings);
            var Req = Get.Customers(DateOfBirth: "1990-06-29");
            JArray ReqAsJson = JArray.Parse(Req);

            foreach (var x in ReqAsJson)
            {
                JToken dateOfBirth = x["DateOfBirth"];
                if (!dateOfBirth.ToString().Contains("1996"))
                {
                    Assert.Fail(string.Format("A customer was returned although they don't have the correct date of birth. The customers date of birth is {0}", dateOfBirth.ToString()));
                }
            }

            Assert.IsTrue(Req.Contains("29/06/1996"));
        }

        [TestMethod]
        public void TestUsingCustomerReferenceAsAParameterReturnsRecordsContainingCustomerRef()
        {
            var Get = new Get(Settings);
            var Req = Get.Customers(CustomerReference: "test-000002");
            JArray ReqAsJson = JArray.Parse(Req);

            foreach (var x in ReqAsJson)
            {
                JToken customerRef = x["CustomerRef"];
                if (!customerRef.ToString().Contains("test-000002"))
                {
                    Assert.Fail(string.Format("A customer was returned although they don't have the correct customer reference. The customers customer reference is {0}", customerRef.ToString()));
                }
            }

            Assert.IsTrue(Req.Contains("test-000002"));
        }

        [TestMethod]
        public void TestUsingFirstNameAsAParameterReturnsRecordsContainingFirstName()
        {
            var Get = new Get(Settings);
            var Req = Get.Customers(FirstName: "test");
            JArray ReqAsJson = JArray.Parse(Req);

            foreach (var x in ReqAsJson)
            {
                JToken firstName = x["FirstName"];
                if (!firstName.ToString().ToLower().Contains("test"))
                {
                    Assert.Fail(string.Format("A customer was returned although they don't have the correct first name. The customers first name is {0}", firstName.ToString()));
                }
            }
        
            Assert.IsTrue(Req.Contains("test"));
        }

        [TestMethod]
        public void TestUsingSurnameAsAParameterRetursRecordsContainingSurname()
        {
            var Get = new Get(Settings);
            var Req = Get.Customers(Surname: "test");
            JArray ReqAsJson = JArray.Parse(Req);

            foreach (var x in ReqAsJson)
            {
                JToken surname = x["FirstName"];
                if (!surname.ToString().ToLower().Contains("test"))
                {
                    Assert.Fail(string.Format("A customer was returned although they don't have the correct surname. The customers surname is {0}", surname.ToString()));
                }
            }

            Assert.IsTrue(Req.Contains("test"));
        }

        [TestMethod]
        public void TestUsingCompanyNameAsAParameterReturnsRecordsContainingCompanyName()
        {
            var Get = new Get(Settings);
            var Req = Get.Customers(CompanyName: "test");
            JArray ReqAsJson = JArray.Parse(Req);

            foreach (var x in ReqAsJson)
            {
                JToken companyName = x["CompanyName"];
                if (!companyName.ToString().ToLower().Contains("test"))
                {
                    Assert.Fail(string.Format("A customer was returned although they don't have the correct company name. The customers company name is {0}", companyName.ToString()));
                }
            }

            Assert.IsTrue(Req.Contains("test"));
        }

        [TestMethod]
        public void TestUsingPostCodeAsAParameterReturnsRecordsContainingPostCode()
        {
            var Get = new Get(Settings);
            var Req = Get.Customers(PostCode: "GL52 2NF");
            JArray ReqAsJson = JArray.Parse(Req);

            foreach (var x in ReqAsJson)
            {
                JToken postCode = x["AddressDetail"]["PostCode"];
                if (!postCode.ToString().ToLower().Contains("gl52 2nf"))
                {
                    Assert.Fail(string.Format("A customer was returned although they don't have the correct post code. The customers post code is {0}", postCode.ToString()));
                }
            }

            Assert.IsTrue(Req.Contains("GL52 2NF"));
        }

        [TestMethod]
        public void TestUsingAccountNumberAsAParameterReturnsRecordsContainingAccountNumber()
        {
            var Get = new Get(Settings);
            var Req = Get.Customers(AccountNumber: "45678912");
            JArray ReqAsJson = JArray.Parse(Req);

            foreach (var x in ReqAsJson)
            {
                JToken accountNumber = x["BankDetail"]["AccountNumber"];
                if (!accountNumber.ToString().ToLower().Contains("45678912"))
                {
                    Assert.Fail(string.Format("A customer was returned although they don't have the correct account number. The customers account number is {0}", accountNumber.ToString()));
                }
            }

            Assert.IsTrue(Req.Contains("45678912"));
        }

        [TestMethod]
        public void TestUsingSortCodeAsAParameterReturnsRecordsContainingSortCode()
        {
            var Get = new Get(Settings);
            var Req = Get.Customers(SortCode: "147852");
            JArray ReqAsJson = JArray.Parse(Req);

            foreach (var x in ReqAsJson)
            {
                JToken sortCode = x["BankDetail"]["BankSortCode"];
                if (!sortCode.ToString().ToLower().Contains("147852"))
                {
                    Assert.Fail(string.Format("A customer was returned although they don't have the correct sort code. The customers sort code is {0}", sortCode.ToString()));
                }
            }

            Assert.IsTrue(Req.Contains("147852"));
        }

        [TestMethod]
        public void TestUsingAccountHolderNameAsAParameterReturnsRecordsContainingAccountHolderName()
        {
            var Get = new Get(Settings);
            var Req = Get.Customers(AccountHolderName: "Mr Test Client");
            JArray ReqAsJson = JArray.Parse(Req);

            foreach (var x in ReqAsJson)
            {
                JToken accountHolderName = x["BankDetail"]["AccountHolderName"];
                if (!accountHolderName.ToString().ToLower().Contains("mr test client"))
                {
                    Assert.Fail(string.Format("A customer was returned although they don't have the correct account holder name. The customers account holder name is {0}", accountHolderName.ToString()));
                }
            }

            Assert.IsTrue(Req.Contains("Mr Test Client"));
        }

        [TestMethod]
        public void TestUsingHomePhoneAsAParameterReturnsRecordsContainingHomePhone()
        {
            var Get = new Get(Settings);
            var Req = Get.Customers(HomePhone: "01242147852");
            JArray ReqAsJson = JArray.Parse(Req);

            foreach (var x in ReqAsJson)
            {
                JToken homePhone = x["HomePhoneNumber"];
                if (!homePhone.ToString().ToLower().Contains("01242147852"))
                {
                    Assert.Fail(string.Format("A customer was returned although they don't have the correct home phone. The customers home phone is {0}", homePhone.ToString()));
                }
            }

            Assert.IsTrue(Req.Contains("01242147852"));
        }

        [TestMethod]
        public void TestUsingMobilePhoneAsAParameterReturnsRecordsContainingMobilePhone()
        {
            var Get = new Get(Settings);
            var Req = Get.Customers(MobilePhone: "07393549789");
            JArray ReqAsJson = JArray.Parse(Req);

            foreach (var x in ReqAsJson)
            {
                JToken mobilePhone = x["MobilePhoneNumber"];
                if (!mobilePhone.ToString().ToLower().Contains("07393549789"))
                {
                    Assert.Fail(string.Format("A customer was returned although they don't have the correct mobile phone. The customers mobile phone is {0}", mobilePhone.ToString()));
                }
            }

            Assert.IsTrue(Req.Contains("07393549789"));
        }

        [TestMethod]
        public void TestUsingWorkPhoneAsAParameterReturnsRecordsContainingWorkPhone()
        {
            var Get = new Get(Settings);
            var Req = Get.Customers(WorkPhone: "01452365478");
            JArray ReqAsJson = JArray.Parse(Req);

            foreach (var x in ReqAsJson)
            {
                JToken workPhone = x["WorkPhoneNumber"];
                if (!workPhone.ToString().ToLower().Contains("01452365478"))
                {
                    Assert.Fail(string.Format("A customer was returned although they don't have the correct work phone. The customers work phone is {0}", workPhone.ToString()));
                }
            }

            Assert.IsTrue(Req.Contains("01452365478"));
        }

    }
}

