using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProxyApi.Extension;

namespace ProxyApiTest.Tests.Extensions
{
    [TestClass]
    public class ExtensionStringTests
    {

        [DataTestMethod]
        [DataRow("http://192.168.1.55/api/WeatherForecast/1", "http://192.168.1.55/api/WeatherForecast", "/1")]
        [DataRow("ValueBetweeen2Values", "2", "ValueBetweeenValues")]
        [DataRow("ANormalString", "ValueNotIn", "ANormalString")]
        public void RemoveValueTest(string initial,string partToDelete, string result)
        {
            string value = initial.RemoveValue(partToDelete);
            Assert.AreEqual(value, result);
        }
    }
}
