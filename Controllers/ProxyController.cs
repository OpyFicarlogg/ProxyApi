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
    [Route("[controller]")] //Replace by [Route("/api/{*value}")]? 
    public class ProxyController : ControllerBase
    {
        private readonly ProxyService _ProxyService;
        private readonly ILogger<ProxyController> _logger;

        public ProxyController(ILogger<ProxyController> logger, ProxyService proxyService)
        {
            _ProxyService = proxyService;
            _logger = logger;
        }

        //TODO: work with swagger  https://docs.microsoft.com/fr-fr/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-5.0&tabs=visual-studio
        /// <summary>
        /// Proxy for Get Http Verb
        /// </summary>
        /// <param name="value">Url you want to acces through the proxy </param>  
        [HttpGet]
        [Route("/api/{*value}")]
        public void ProxyGet(string value)
        {
            _ProxyService.ProxyGet(Request, Response, value); 
            //TODO: add try catch
        }

        /// <summary>
        /// Proxy for Post Http Verb
        /// </summary>
        /// <param name="value">Url you want to acces through the proxy </param>  
        [HttpPost]
        [Route("/api/{*value}")]
        public void ProxyPost(string value)
        {
            _ProxyService.ProxyGet(Request, Response, value);
        }

        /// <summary>
        /// Proxy for Put Http Verb
        /// </summary>
        /// <param name="value">Url you want to acces through the proxy </param>  
        [HttpPut]
        [Route("/api/{*value}")]
        public void ProxyPut(string value)
        {
            _ProxyService.ProxyGet(Request, Response, value);
        }

        /// <summary>
        /// Proxy for Delete Http Verb
        /// </summary>
        /// <param name="value">Url you want to acces through the proxy </param>  
        [HttpDelete]
        [Route("/api/{*value}")]
        public void ProxyDelete(string value)
        {
            _ProxyService.ProxyGet(Request, Response, value);
        }

        
    }
}
