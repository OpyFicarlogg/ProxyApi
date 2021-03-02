using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using testApi.Dao.Interfaces;
using testApi.Extension;

namespace testApi.Services
{
    public class ProxyService
    {
        private IWsService _wsService;

        //Mockit 
        private HttpClient httpClient;
        private readonly HttpRequest request;
        private readonly HttpResponse response;

        public ProxyService(HttpRequest _request, HttpResponse _response)
        {
            this._wsService = new WsService();
            this.request = _request;
            this.response = _response;
            httpClient = new HttpClient();
        }

        /// <summary>
        /// Set httpClient and httpRequestMessage with urlParam, and HttpRequest data
        /// </summary>
        /// <param name="value">Url you want to acces through the proxy </param>  
        public async void Proxy(string value)
        {
            String url = _wsService.GetWs(value);
            if (!String.IsNullOrEmpty(url))
            {
                StreamContent content = new StreamContent(request.Body); //Get body
                var contentString = await content.ReadAsStringAsync();
                // ----------------REQUEST PART-----------------//
                HttpRequestMessage httpReqMessage = new HttpRequestMessage(new HttpMethod(request.Method), url)
                {
                    Content = new StringContent(contentString, Encoding.UTF8) // Need to create Content to add body + Content headers 
                };
                httpReqMessage.SetHeader(request);
                httpReqMessage.SetProperty(request);
                //TODO:  request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userAccountEntity.GetAccessToken());

                try
                {
                    HttpResponseMessage httpRespMessage = httpClient.SendAsync(httpReqMessage).Result;

                    // ----------------RESPONSE PART-----------------//
                    string result = httpRespMessage.Content.ReadAsStringAsync().Result;

                    response.SetResponseHeader(httpRespMessage);
                    response.StatusCode = (int)httpRespMessage.StatusCode;
                    response.WriteBody(result); // must be at the end because it start the response
                }
                catch (Exception e)
                {
                    response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                    response.WriteBody(Regex.Replace(e.Message, "([0-9]{1,3}.){3}[0-9]{1,3}:[0-9]{1,5}", "")); //Hide Ip in return
                }
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
            }

            
        }

        //Allow to mock DAO IWsService
        protected void SetWsService(IWsService wsService)
        {
            this._wsService = wsService;
        }

        //Allow to mock HttpClient
        protected void SetHttpClient(HttpClient client)
        {
            this.httpClient = client;
        }
    }
}
