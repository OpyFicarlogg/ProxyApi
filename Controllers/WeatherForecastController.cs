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
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }


        //Test permettant d'écrire dans le response sans return 
        [HttpPost]
        [Route("/apki/test")]
        public void testPost([FromBody] string test){
            //pour utiliser cette partie, il faut que la requête soit formaté en json
            _logger.LogInformation($"valeur test: {test}");

            using (StreamWriter writer = new StreamWriter(Response.Body, Encoding.UTF8))
            {
                writer.Write(test);
                writer.Flush();
            }


        }

        [HttpGet]
        [Route("/api/{*url}")]
        public void testGet([FromBody] string test, string url)
        {
            
            //pour utiliser cette partie, il faut que la requête soit formaté en json
            _logger.LogInformation($"valeur test: {test}");
            _logger.LogInformation($"valeur urltest: {url}");

            if (!String.IsNullOrEmpty(url))
            {
                // ----------------REQUEST PART-----------------//
                HttpClient client = new HttpClient();
                //var uriBuilder = new UriBuilder("test.php", "test");
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"http://192.168.1.55/api/{url}");

                //TODO: Send request header
                //httpRequestMessage.SetHeader(Request);



                // ----------------RESPONSE PART-----------------//
                //TODO: Ajouter une exception si le ws n'est pas joignable 
                var httpResponseMessage = client.SendAsync(httpRequestMessage).Result;
                string result = httpResponseMessage.Content.ReadAsStringAsync().Result;

                //Define header in Response 
                httpResponseMessage.SetResponseHeader(Response);

                //Define return statusCode
                Response.StatusCode = (int)httpResponseMessage.StatusCode;

                //Return body
                using (StreamWriter writer = new StreamWriter(Response.Body, Encoding.UTF8))
                {
                    writer.Write(result);
                    writer.Flush();
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            


        }

        [HttpPost]
        public async Task<string> ReadStringDataManual()
        {
            //https://weblog.west-wind.com/posts/2017/sep/14/accepting-raw-request-body-content-in-aspnet-core-api-controllers
            //https://docs.microsoft.com/fr-fr/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://localhost/api/WeatherForecast");
            /*client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));*/
            var responseString = await client.GetStringAsync("http://localhost/api/WeatherForecast");

            //Création d'un header
            Response.Headers.Add("test","test");
  
            //Création de cookie 
            CookieActions cook = new CookieActions{
                request = Request, 
                response = Response
            };
            cook.SetCookie("sf","sf",60);

            
            //Récupération du header 
            var headers = Request.Headers;
            String headerRet="";
            foreach(var header in headers){
                headerRet+= $"key: {header.Key} value: {header.Value}\n";
            }
            
            //Récupération des querys
            String QueryRet ="";
            var querys = Request.Query;
            foreach(var query in querys){
                QueryRet+= $"Key: {query.Key} value: {query.Value}";
            }
            
            //Read cookie: string cookieValueFromReq = Request.Cookies["Key"]; 
            //Récupération des cookies 
            var cookies = Request.Cookies;
            String cookieRet="";
            foreach (var cookie in cookies){
                cookieRet += $"key: {cookie.Key} value: {cookie.Value}";
            }



            //Récupération du body avec tout type de content 
            var body ="";
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {  
                body = await reader.ReadToEndAsync();
                
            }

            return $"Body: {body} \n header: {headerRet} \n query: {QueryRet} \n cookie: {cookies}";
        }


        [Route("apki/BodyTypes/ReadBinaryDataManual")]
        public async Task<byte[]> RawBinaryDataManual()
        {
            /* lecture de binaire et retour en base 64
            POST http://localhost:5000/api/BodyTypes/ReadBinaryDataManual HTTP/1.1
            Accept-Encoding: gzip,deflate
            User-Agent: West Wind HTTP .NET Client
            Content-Type: application/octet-stream
            Host: localhost:5000
            Content-Length: 40
            Expect: 100-continue
            Connection: Keep-Alive
            */
            using (var ms = new MemoryStream(2048))
            {
                await Request.Body.CopyToAsync(ms);
                return  ms.ToArray();  // returns base64 encoded string JSON result
            }
        }


        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }    
    }
}
