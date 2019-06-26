using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class SchedulesReaderCheck
    {
        private EazySDK.ClientHandler Handler { get; set; }
        private IConfiguration Settings { get; set; }
        private EazySDK.Utilities.SchedulesReader Reader { get; set; }

        [TestInitialize]
        public void TestInit()
        {
            Handler = new EazySDK.ClientHandler();
            Settings = Handler.Settings();
            Reader = new EazySDK.Utilities.SchedulesReader();
            Settings.GetSection("currentEnvironment")["Environment"] = "sandbox";
            Settings.GetSection("sandboxClientDetails")["ApiKey"] = "hkZujzFR2907XAtYe6qkKRsBo";
            Settings.GetSection("sandboxClientDetails")["ClientCode"] = "SDKTST";
        }

        [TestMethod]
        public void TestReaderWillCreateFileIfNeeded()
        {
            if(File.Exists(Directory.GetCurrentDirectory() + @"\Includes\sandboxscheduleslist.json"))
            {
                File.Delete(Directory.GetCurrentDirectory() + @"\Includes\sandboxscheduleslist.json");
            }
            Reader.ReadSchedulesFile(Settings);

            Assert.IsTrue(File.Exists(Directory.GetCurrentDirectory() + @"\Includes\sandboxscheduleslist.json"));
        }

        [TestMethod]
        public void TestReaderWillRebuildFileIfFormattedIncorrectly()
        {
            var x = Reader.ReadSchedulesFile(Settings);
            Assert.IsTrue(x.ContainsKey("LastUpdated"));
        }
    }
}

