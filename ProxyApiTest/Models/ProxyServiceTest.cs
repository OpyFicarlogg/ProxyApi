using Microsoft.AspNetCore.Http;
using ProxyApi.Dao;
using ProxyApi.Dao.Interfaces;
using ProxyApi.Services;
using System.Net.Http;

namespace ProxyApiTest.Models
{
    //WrapperClass for protected method 
    public class ProxyServiceTest : ProxyService
    {
        public ProxyServiceTest(HttpRequest _request, HttpResponse _response, IWsService _serviceDao) : base(_request, _response, _serviceDao)
        {
        }

        public new void SetHttpClient(HttpClient client) {
             base.SetHttpClient(client);
        }
    }
}
