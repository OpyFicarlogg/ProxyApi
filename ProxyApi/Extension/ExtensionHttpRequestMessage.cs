using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ProxyApi.Extension
{
    public static class ExtensionHttpRequestMessage
    {
        /// <summary>  
        ///Set header from HttpRequest in HttpRequestMessage
        /// </summary>  
        /// <param name="request">HttpRequest 
        public static void SetHeader(this HttpRequestMessage httpRequestMessage, HttpRequest request)
        {
            var headers = request.Headers;
            foreach (var header in headers)
            {
                if (header.Key.IndexOf("Content", StringComparison.CurrentCultureIgnoreCase) != -1)
                {
                    switch (header.Key)
                    {
                        case "Content-Type":
                            httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(header.Value); //ToString was deleted;
                            break;
                        case "Content-Length":
                            httpRequestMessage.Content.Headers.ContentLength = Convert.ToInt64(header.Value);
                            break;
                        case "Content-Location":
                            httpRequestMessage.Content.Headers.ContentLocation = new Uri(header.Value);
                            break;
                        default:
                            httpRequestMessage.Content.Headers.Add(header.Key, header.Value.ToString());
                            //TODO:  voir les autres content 
                            break;
                    }
                }
                else
                {
                    httpRequestMessage.Headers.Add(header.Key, header.Value.ToString());
                }
            }
        }

        public static void SetProperty(this HttpRequestMessage httpRequestMessage, HttpRequest request)
        {
            foreach (var query in request.Query)
            {
                //TODO: see httpRequestMessage.Options? 
                httpRequestMessage.Properties.Add(query.Key, query.Value);
            }
        }
    }
}
