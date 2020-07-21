using Microsoft.VisualStudio.TestTools.UnitTesting;
using EazySDK;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class UnitTestGeneralTest
    {
        [TestMethod]
        public void CreateDefaultSettingsObject()
        {
            SettingsManager.CreateSettings();
        }

        public void CreateCustomSettingsEmptyStringDefaultsToAppSettings()
        {
            SettingsManager.CreateSettings("");
        }

        [TestMethod]
        public void CreateCustomSettingsUsingFile()
        {
            SettingsManager.CreateSettings("testSettings.json");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidSettingsConfigurationException))]
        public void CustomSettingsIncorrectFileException()
        {
            SettingsManager.CreateSettings("test.cs");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidSettingsFileException))]
        public void CustomSettingsFileDoesNotExistException()
        {
            SettingsManager.CreateSettings("DoesNotExist");
        }

        [TestMethod]
        public void CreateCustomSettingsInMemory()
        {
            SettingsManager.CreateSettingsInMemory();
        }
    }
}

