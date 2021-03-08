
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProxyApi.Services;
using System.Net.Http;

namespace ProxyApi.Controllers
{
    [ApiController]
    [Route("[controller]")] //Replace by [Route("/api/{*value}")]? 
    public class ProxyController : ControllerBase
    {
        private ProxyService _ProxyService;
        private readonly ILogger<ProxyController> _logger;

        public ProxyController(ILogger<ProxyController> logger)
        {
            _logger = logger;
        }

        //TODO: work with swagger  https://docs.microsoft.com/fr-fr/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-5.0&tabs=visual-studio
        /// <summary>
        /// Proxy for Http Verb
        /// </summary>
        /// <param name="value">Url you want to acces through the proxy </param>  
        [HttpGet, HttpPost, HttpPut, HttpDelete, HttpPatch]
        [Route("/api/{*value}")]
        public void Proxy(string value)
        { 
            _ProxyService = new ProxyService(Request, Response);
            _ProxyService.Proxy(value); 
        }        
    }
}
