using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using ProxyApi.Dao.Interfaces;
using ProxyApi.Extension;
using ProxyApi.Models;
using ProxyApi.Dao;

namespace ProxyApi.Services
{
    public class ProxyService
    {
        private readonly IWsService _serviceDao;

        //Mockit 
        private HttpClient httpClient;
        private readonly HttpRequest request;
        private readonly HttpResponse response;

        public ProxyService(HttpRequest _request, HttpResponse _response, IWsService serviceDao)
        {
            this._serviceDao = serviceDao;
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
            Service service = _serviceDao.GetService(value);
            String url = service is not null ? service.Redirect + value.RemoveValue(service.RequestValue) : null;

            if (!String.IsNullOrEmpty(url))
            {
                var contentString = await request.Body.GetStringAsyncStreamContent();
                //string contentString = await request.GetBodyAsyncStreamContent();
                // ----------------REQUEST PART-----------------//
                HttpRequestMessage httpReqMessage = new HttpRequestMessage(new HttpMethod(request.Method), url)
                {
                    Content = new StringContent(contentString, Encoding.UTF8) // Need to create Content to add body + Content headers 
                };
                httpReqMessage.SetHeader(request);
                httpReqMessage.SetPropertyOption(request);
                //TODO:  request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userAccountEntity.GetAccessToken());

                try
                {
                    HttpResponseMessage httpRespMessage = httpClient.SendAsync(httpReqMessage).Result;

                    // ----------------RESPONSE PART-----------------//
                    string result = httpRespMessage.Content.ReadAsStringAsync().Result;

                    response.SetResponseHeader(httpRespMessage);
                    response.StatusCode = (int)httpRespMessage.StatusCode;
                    response.Body.SetStringStream(result); // must be at the end because it start the response
                }
                catch (Exception e)
                {
                    response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                    response.Body.SetStringStream(Regex.Replace(e.Message, "([0-9]{1,3}.){3}[0-9]{1,3}:[0-9]{1,5}", "")); //Hide Ip in return
                }
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
            }

            
        }

        //Allow to mock HttpClient
        protected void SetHttpClient(HttpClient client)
        {
            this.httpClient = client;
        }
    }
}
