using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using ProxyApi.Dao.Interfaces;
using ProxyApiTest.Models;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ProxyApi.Extension;

namespace ProxyApiTest.Tests.Services
{
    [TestClass] // Herit from ProxyService to acces protected methods https://stackoverflow.com/questions/13416223/unit-testing-c-sharp-protected-methods
    public class ProxyTest
    {
        private Mock<HttpRequest> request;
        private Mock<HttpResponse> response;
        private HttpClient httpClient;
        private Mock<IWsService> WsService;

        [TestMethod]
        public async Task ProxyMethodTest()
        {
            //Mock Response
            this.SetResponse();
            //Mock Request
            this.SetRequest();
            //Mock WsService
            this.setWsService("http://test.com");
            //Mock HttpClient
            this.setHttpClient(200, "BodyContent");

            ProxyServiceTest prox = new ProxyServiceTest(request.Object, response.Object);
            prox.SetHttpClient(httpClient);
            prox.SetWsService(WsService.Object);
            prox.Proxy("test");

            response.Object.Body.Seek(0, SeekOrigin.Begin); //Because of StreamMemory
            string body = await response.Object.Body.GetStringAsyncStreamContent();

            string qryVal = response.Object.Headers["header1"];
            
            Assert.AreEqual(response.Object.StatusCode, 200);
            Assert.AreEqual("BodyContent",body);
            Assert.AreEqual("valHeader1",qryVal);

            WsService.Verify(); //https://stackoverflow.com/questions/980554/what-is-the-purpose-of-verifiable-in-moq
            response.Verify();
            //mock.Verify(v => v.Baz(It.IsAny<int>())); // Verifies that Baz was called
        }


        public void SetResponse()
        {
            response = new Mock<HttpResponse>
            {
                CallBase = true
            };

            var headers = new HeaderDictionary(new Dictionary<string, StringValues>()) as IHeaderDictionary;

            Stream stream = new MemoryStream();

            //https://stackoverflow.com/questions/4130087/difference-between-setupset-and-setupproperty-in-moq //setupGet when no setter
            response.SetupGet(x => x.Headers).Returns(headers);
            response.SetupProperty(x => x.StatusCode, 0);
            response.SetupProperty(x => x.Body, stream);


            //TODO: Use mole to mock extension   https://stackoverflow.com/questions/2295960/mocking-extension-methods-with-moq
            //response.Setup(x => x.SetResponseHeader(It.IsAny<HttpResponseMessage>())).Verifiable();
            //response.Setup(x => x.WriteBody(It.IsAny<string>())).Verifiable();
        }

        public void SetRequest()
        {
            request = new Mock<HttpRequest>();

            //https://stackoverflow.com/questions/52855484/iheadersdictionary-returning-null-after-mocking/52855890
            var headers = new HeaderDictionary(new Dictionary<string, StringValues>
            {
                { "Key", "Value"}
            }) as IHeaderDictionary;
            
            //https://csharp.hotexamples.com/fr/examples/-/QueryCollection/-/php-querycollection-class-examples.html
            var query = new QueryCollection(new Dictionary<string, StringValues>()
            {
                { "name2", "value2" }
            }) as IQueryCollection;
               
            Stream stream = new MemoryStream(Encoding.ASCII.GetBytes("BodyContent"));
           
            request.SetupProperty(x => x.Body, stream);
            request.SetupGet(x => x.Headers).Returns(headers);
            request.SetupProperty(x => x.Query, query);
            request.SetupProperty(x => x.Method, "GET");
        }


        public void setHttpClient(int status, string bodyContent)
        {
            Mock<HttpResponseMessage> httpResponseMessage = new Mock<HttpResponseMessage>
            {
                CallBase = true
            };
            httpResponseMessage.Object.Content = new StringContent(bodyContent, Encoding.UTF8);
            httpResponseMessage.Object.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpResponseMessage.Object.StatusCode = (HttpStatusCode)status;
            httpResponseMessage.Object.Headers.Add("header1", "valHeader1");

            //https://stackoverflow.com/questions/36425008/mocking-httpclient-in-unit-tests httpClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>())).ReturnsAsync(httpResponseMessage.Object);
            //Autre possibilité https://stackoverflow.com/questions/36425008/mocking-httpclient-in-unit-tests
            //.protected() https://stackoverflow.com/questions/7715654/unit-test-protected-method-in-c-sharp-using-moq
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage.Object);
            httpClient = new HttpClient(mockHttpMessageHandler.Object);
        }

        public void setWsService(string url)
        {
            //Mock WsService
            WsService = new Mock<IWsService>();
            WsService.Setup(x => x.GetWs(It.IsAny<string>())).Returns(url).Verifiable();
        }
    }
}
