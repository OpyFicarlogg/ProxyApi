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
        [Route("/api/test")]
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
        [Route("/api/testget")]
        public void testGet([FromBody] string test){
            
            //pour utiliser cette partie, il faut que la requête soit formaté en json
            _logger.LogInformation($"valeur test: {test}");

            //https://stackoverflow.com/questions/12373738/how-do-i-set-a-cookie-on-httpclients-httprequestmessage
            HttpClient client = new HttpClient();
            //var uriBuilder = new UriBuilder("test.php", "test");
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/WeatherForecast");
            httpRequestMessage.Headers.Add("Host", "test.com");
            httpRequestMessage.Headers.Add("Cookie", "__utmc=266643403; __utmz=266643403.1537352460.3.3.utmccn=(referral)|utmcsr=google.co.uk|utmcct=/|utmcmd=referral; __utma=266643403.817561753.1532012719.1537357162.1537361568.5; __utmb=266643403; __atuvc=0%7C34%2C0%7C35%2C0%7C36%2C0%7C37%2C48%7C38; __atuvs=5ba2469fbb02458f002");
            var httpResponseMessage = client.SendAsync(httpRequestMessage).Result;
            var httpContent = httpResponseMessage.Content;
            string result = httpResponseMessage.Content.ReadAsStringAsync().Result;

            var headers = httpResponseMessage.Content.Headers;
            foreach(var header in headers)
            {
                string valHeader="";
                foreach(var val in header.Value){
                    //TODO:  a tester avec des multiples values pour un header et sans le foreach sinon?
                    valHeader+=val;
                }
                Response.Headers.Add(header.Key,valHeader);
                valHeader="";
            }
            //TODO: Ajouter un cookie dans le ws de test pour voir comment le récupérer 
            IEnumerable<string> cookies = httpResponseMessage.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value;
            Response.StatusCode = (int)httpResponseMessage.StatusCode;
            
            using (StreamWriter writer = new StreamWriter(Response.Body, Encoding.UTF8))
            {
                writer.Write(result);
                writer.Flush();
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


        [Route("api/BodyTypes/ReadBinaryDataManual")]
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
