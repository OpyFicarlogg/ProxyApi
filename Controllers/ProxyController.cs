using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using testApi.Services;
using testApi.Extension;
using System.Net;

namespace testApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProxyController : ControllerBase
    {
        private readonly ProxyService _ProxyService;
        private readonly ILogger<ProxyController> _logger;

        public ProxyController(ILogger<ProxyController> logger, ProxyService proxyService)
        {
            _ProxyService = proxyService;
            _logger = logger;
        }

        [HttpGet]
        [Route("/api/{*value}")]
        public void ProxyGet( string value)
        {
            _ProxyService.ProxyGet(Request, Response, value); //Exception avec erreur 500



            /*String url = _ProxyService.getWs(value);
            if (!String.IsNullOrEmpty(url))
            {
                // ----------------REQUEST PART-----------------//
                HttpClient client = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                //TODO: Set body 
                //TODO: Send request header
                //httpRequestMessage.SetHeader(Request);
                var httpResponseMessage = client.SendAsync(httpRequestMessage).Result;

                // ----------------RESPONSE PART-----------------//
                //TODO: Ajouter une exception si le ws n'est pas joignable 
                string result = httpResponseMessage.Content.ReadAsStringAsync().Result;

                Response.SetResponseHeader(httpResponseMessage);           
                Response.StatusCode = (int)httpResponseMessage.StatusCode;
                Response.WriteBody(result); // must be at the end because it start the response
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
            }*/
        }


        [HttpPost]
        [Route("/api/{*value}")]
        public void ProxyPost([FromBody] string body, string value)
        {
            _ProxyService.ProxyGet(Request, Response, value);
        }
    }
}
