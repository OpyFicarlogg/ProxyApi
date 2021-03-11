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

namespace ProxyApiTest.Tests.Extensions
{
    [TestClass]
    public class ExtensionHttpRequestMessageTests
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
            Assert.ThrowsException<InvalidOperationException>(() => string.Join(" ", httpRequestMessage.Object.Headers.GetValues("Content-Type")));
        }

        [TestMethod]
        public void SetPropertyTest()
        {
            //Set httpRequestMessage
            Mock<HttpRequestMessage> httpRequestMessage = new Mock<HttpRequestMessage>
            {
                CallBase = true
            };

            httpRequestMessage.Object.Content = new StringContent("BodyContent", Encoding.UTF8);

            //set HttpRequest
            Mock<HttpRequest> request = new Mock<HttpRequest>();
            var query = new QueryCollection(new Dictionary<string, StringValues>()
            {
                { "prop1", "value1" },
                { "prop2", "value2" }
            }) as IQueryCollection;

            request.SetupProperty(x => x.Query, query);
            httpRequestMessage.Object.SetPropertyOption(request.Object);
            //httpRequestMessage.Object.SetProperty(request.Object);

            string qryVal = httpRequestMessage.Object.Options.Where(opt => "prop1".Contains(opt.Key)).SingleOrDefault().Value.ToString();

            Assert.AreEqual(qryVal, "value1");
        }
    }
}
