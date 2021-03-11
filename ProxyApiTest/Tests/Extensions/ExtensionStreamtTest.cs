using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using ProxyApi.Extension;
using System.Threading.Tasks;
using System.Net.Http;

namespace ProxyApiTest.Tests.Extensions
{
    [TestClass]
    public class ExtensionStreamtTest
    {

        [DataTestMethod]
        [DataRow("ContentStreamExemple")]
        [DataRow("")]
        public async Task GetStringAsyncTest(string streamValue)
        {
            Stream stream = new MemoryStream(Encoding.ASCII.GetBytes(streamValue));
            string result = await stream.GetStringAsync();

            Assert.AreEqual(result, streamValue);
        }

        [TestMethod]
        public async Task GetStringAsyncTest_Empty()
        {
            Stream stream = new MemoryStream();
            string result = await stream.GetStringAsync();

            Assert.AreEqual(result, "");
        }

        [DataTestMethod]
        [DataRow("ContentStreamExemple")]
        [DataRow("")]
        public async Task GetStringAsyncStreamContentTest(string streamValue)
        {
            Stream stream = new MemoryStream(Encoding.ASCII.GetBytes(streamValue));
            string result = await stream.GetStringAsyncStreamContent();

            Assert.AreEqual(result, streamValue);
        }

        [TestMethod]
        public async Task GetStringAsyncStreamContentTest_Empty()
        {
            Stream stream = new MemoryStream();
            string result = await stream.GetStringAsyncStreamContent();

            Assert.AreEqual(result, "");
        }


        [DataTestMethod]
        [DataRow("StringValue")]
        [DataRow("")]
        public async Task SetStringStreamTest(string value)
        {
            Stream stream = new MemoryStream();
            stream.SetStringStream(value);
            stream.Seek(0, SeekOrigin.Begin);

            StreamContent content = new StreamContent(stream); //Get body
            string result =  await content.ReadAsStringAsync();

            Assert.AreEqual(result, value);
        }


    }
}
