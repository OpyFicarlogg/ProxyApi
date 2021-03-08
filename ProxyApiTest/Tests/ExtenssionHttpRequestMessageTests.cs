using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProxyApi.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace ProxyApiTest
{
    [TestClass]
    public class ExtenssionHttpRequestMessageTests
    {
        [TestMethod]
        public void SetHeaderTest()
        {
            //Set httpRequestMessage
            Mock<HttpRequestMessage> httpRequestMessage = new Mock<HttpRequestMessage>
            {
                CallBase = true
            };

            httpRequestMessage.Object.Content = new StringContent("BodyContent", Encoding.UTF8);

            //set HttpRequest
            Mock<HttpRequest> request = new Mock<HttpRequest>();
            var headers = new HeaderDictionary(new Dictionary<string, StringValues>
            {
                { "Key", "Value"},
                { "Content-Type", "application/json"}
            }) as IHeaderDictionary;

            request.SetupGet(x => x.Headers).Returns(headers);

            httpRequestMessage.Object.SetHeader(request.Object);

            Assert.AreEqual(string.Join(" ", httpRequestMessage.Object.Content.Headers.ContentType), "application/json");
            Assert.AreEqual(string.Join(" ", httpRequestMessage.Object.Headers.GetValues("Key")), "Value");
            Assert.ThrowsException<InvalidOperationException>(() => string.Join(" ", httpRequestMessage.Object.Content.Headers.GetValues("Key")));
        }
    }
}
