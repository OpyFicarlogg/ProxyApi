using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProxyApi.Extension;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProxyApiTest.Tests.Extensions
{
    [TestClass]
    public class ExtensionResponseTest
    {
        [TestMethod]
        public void SetResponseHeaderTest()
        {
            Mock<HttpResponse> response = new Mock<HttpResponse>();

            var headers = new HeaderDictionary(new Dictionary<string, StringValues>
            {
                { "Key", "Value"}
            }) as IHeaderDictionary;

            response.SetupGet(x => x.Headers).Returns(headers);

            Mock<HttpResponseMessage> httpResponseMessage = new Mock<HttpResponseMessage>
            {
                CallBase = true
            };

            httpResponseMessage.Object.Content = new StringContent("BodyContent", Encoding.UTF8);
            httpResponseMessage.Object.Headers.Add("keyHeader", "valueHeader");
            httpResponseMessage.Object.Content.Headers.Add("keyContent", "valueContentHeader");

            response.Object.SetResponseHeader(httpResponseMessage.Object);

            Assert.IsTrue(response.Object.Headers.ContainsKey("keyHeader"));
            Assert.IsTrue(response.Object.Headers.ContainsKey("keyContent"));

        }
    }
}
