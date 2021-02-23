using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using testApi.Extension;

namespace testApi.Services
{
    public class ProxyService
    {
        private static readonly Dictionary<string, string> ws = new Dictionary<string, string>(){
            {"WeatherForecast", "http://192.168.1.55/api/WeatherForecast"},
            {"USA", "Chicago, New York, Washington"},
            {"India", "Mumbai, New Delhi, Pune"}
        };

        public async void ProxyGet(HttpRequest request, HttpResponse response, string value)
        {
            String url = this.getWs(value);
            if (!String.IsNullOrEmpty(url))
            {
                StreamContent content = new StreamContent(request.Body); //Get body
                var contentString = await content.ReadAsStringAsync();
                // ----------------REQUEST PART-----------------//
                HttpClient client = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage(new HttpMethod(request.Method), url)
                {
                    Content = new StringContent(contentString, Encoding.UTF8) // Need to create Content to add body + Content headers 
                };
                httpRequestMessage.SetHeader(request);
                httpRequestMessage.SetProperty(request);
                //TODO:  request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userAccountEntity.GetAccessToken());

                try
                {
                    var httpResponseMessage = client.SendAsync(httpRequestMessage).Result;

                    // ----------------RESPONSE PART-----------------//
                    string result = httpResponseMessage.Content.ReadAsStringAsync().Result;

                    response.SetResponseHeader(httpResponseMessage);
                    response.StatusCode = (int)httpResponseMessage.StatusCode;
                    response.WriteBody(result); // must be at the end because it start the response
                }
                catch(Exception e)
                {
                    response.StatusCode= (int)HttpStatusCode.ServiceUnavailable;
                    response.WriteBody(Regex.Replace(e.Message, "([0-9]{1,3}.){3}[0-9]{1,3}:[0-9]{1,5}", "")); //Hide Ip in return
                }
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
            }
        }

        // vas surement sortir et passer dans une DAO 
        public String getWs(string requestUrl)
        {
            if (!String.IsNullOrEmpty(requestUrl))
            {
                //Dois faire un appel à la database 
                //return ws.ContainsKey(RequestUrl) ? ws.GetValueOrDefault(RequestUrl) : null;
                //return ws.GetValueOrDefault(requestUrl);
                var query = ws.Where(dic => requestUrl.Contains(dic.Key));
                return query.SingleOrDefault().Value + requestUrl.RemoveValue(query.SingleOrDefault().Key);
            }
            else
            {
                return null;
                //TODO: Ajouter un log 
            }
            
        }

        //Surement à sortir dans une autre class 
        public Boolean Authentification(String test)
        {
            test = "ok";

            return test.Equals("ok");
        }       
    }
}
