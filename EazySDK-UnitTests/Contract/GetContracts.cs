using Microsoft.VisualStudio.TestTools.UnitTesting;
using EazySDK;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class UnitTestGetContract
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
        public void TestSearchingForACustomerWithContractsReturnsContracts()
        {
            var Get = new Get(Settings);
            var Req = Get.Contracts("310a826b-d095-48e7-a55a-19dba82c566f");

            JArray ReqAsJson = JArray.Parse(Req);

            foreach (var x in ReqAsJson)
            {
                JToken contractId = x["Id"];
                if (contractId.ToString() == "null")
                {
                    Assert.Fail(string.Format("The customer does not own any contracts"));
                }
            }

            Assert.IsTrue(Req.Contains("Id"));
        }


        [TestMethod]
        public void TestSearchingForACustomerWithNoContractsReturnsNoContracts()
        {
            var Get = new Get(Settings);
            var Req = Get.Contracts("7c1a60c5-af12-4477-a10b-a61770e312a5");
            
            Assert.IsTrue(Req.Contains("No contracts could be associated with the customer"));
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.ResourceNotFoundException))]
        public void TestSearchingForAnInvalidCustomerReturnsAnError()
        {
            var Get = new Get(Settings);
            var Req = Get.Contracts("7c1a60c5-af12-4477-a10b-a617");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.ResourceNotFoundException))]
        public void TestSearchingForABlankCustomerReturnsAnError()
        {
            var Get = new Get(Settings);
            var Req = Get.Contracts("");
        }
    }
}

