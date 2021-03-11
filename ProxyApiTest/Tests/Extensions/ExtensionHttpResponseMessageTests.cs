
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;
using ProxyApi.Extension;

namespace ProxyApiTest.Tests.Extensions
{
    [TestClass]
    public class ExtensionHttpResponseMessageTests
    {
        [TestMethod]
        public void GetHeaderTest()
        {
            Mock<HttpResponseMessage> httpResponseMessage = new Mock<HttpResponseMessage>
            {
                CallBase = true
            };

            httpResponseMessage.Object.Content = new StringContent("BodyContent", Encoding.UTF8);
            httpResponseMessage.Object.Headers.Add("keyHeader", "valueHeader");
            httpResponseMessage.Object.Content.Headers.Add("keyContent", "valueContentHeader");

            Dictionary<string, string> dic = httpResponseMessage.Object.GetHeader();

            Assert.AreEqual(dic["keyHeader"], "valueHeader");
            Assert.AreEqual(dic["keyContent"], "valueContentHeader");
        }
    }
}
