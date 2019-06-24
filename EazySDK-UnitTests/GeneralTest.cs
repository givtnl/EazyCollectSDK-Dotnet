using Microsoft.VisualStudio.TestTools.UnitTesting;
using EazySDK;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.IO;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class UnitTestGeneralTest
    {
        [TestMethod]
        public void Test()
        {
            SettingsManager.CreateSettings();
        }
    }
}

