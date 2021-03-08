using Microsoft.AspNetCore.Http;
using ProxyApi.Dao.Interfaces;
using ProxyApi.Services;
using System.Net.Http;

namespace ProxyApiTest.Models
{
    //WrapperClass for protected method 
    public class ProxyServiceTest : ProxyService
    {
        public ProxyServiceTest(HttpRequest _request, HttpResponse _response) : base(_request, _response)
        {
        }

        public new void SetHttpClient(HttpClient client) {
             base.SetHttpClient(client);
        }

        public new void SetWsService(IWsService WsSevice)
        {
            base.SetWsService(WsSevice);
        }

    }
}
