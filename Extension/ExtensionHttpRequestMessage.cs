using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace testApi.Extension
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
                            httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(header.Value.ToString());
                            break;
                        case "Content-Length":
                            httpRequestMessage.Content.Headers.ContentLength = Convert.ToInt64(header.Value.ToString());
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
                

                /*TODO: 
                 * System.InvalidOperationException : 'Misused header name, 'Content-Length'. 
                 * Make sure request headers are used with HttpRequestMessage, response headers with HttpResponseMessage, 
                 * and content headers with HttpContent objects.'
                 */

                /* 
                 * System.InvalidOperationException : 'Misused header name, 'Content-Type'. 
                 * Make sure request headers are used with HttpRequestMessage, 
                 * response headers with HttpResponseMessage, and content headers with HttpContent objects.'
                 */
            }

            //httpRequestMessage.Headers.Add("Host", "test.com");
            //httpRequestMessage.Headers.Add("Cookie", "__utmc=266643403; __utmz=266643403.1537352460.3.3.utmccn=(referral)|utmcsr=google.co.uk|utmcct=/|utmcmd=referral; __utma=266643403.817561753.1532012719.1537357162.1537361568.5; __utmb=266643403; __atuvc=0%7C34%2C0%7C35%2C0%7C36%2C0%7C37%2C48%7C38; __atuvs=5ba2469fbb02458f002");
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
