using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class BankHolidaysReaderCheck
    {
        private EazySDK.ClientHandler Handler { get; set; }
        private IConfiguration Settings { get; set; }
        private EazySDK.Utilities.WorkingDaysReader Reader { get; set; }

        [TestInitialize]
        public void TestInit()
        {
            Handler = new EazySDK.ClientHandler();
            Settings = Handler.Settings();
            Reader = new EazySDK.Utilities.WorkingDaysReader();
            Settings.GetSection("currentEnvironment")["Environment"] = "sandbox";
            Settings.GetSection("sandboxClientDetails")["ApiKey"] = "hkZujzFR2907XAtYe6qkKRsBo";
            Settings.GetSection("sandboxClientDetails")["ClientCode"] = "SDKTST";
        }

        [TestMethod]
        public void TestReaderWillCreateFileIfNeeded()
        {
            if (File.Exists(Directory.GetCurrentDirectory() + @"\Includes\bankholidays.json"))
            {
                File.Delete(Directory.GetCurrentDirectory() + @"\Includes\bankholidays.json");
            }
            Reader.ReadWorkingDaysFile();

            Assert.IsTrue(File.Exists(Directory.GetCurrentDirectory() + @"\Includes\bankholidays.json"));
        }
    }
}

