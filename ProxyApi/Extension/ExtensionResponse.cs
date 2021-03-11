using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace ProxyApi.Extension
{
    public static class ExtensionResponse
    {

        /// <summary>  
        ///Set header from HttpResponseMessage in HttpResponse 
        /// </summary>  
        /// <param name="httpResponseMessage">HttpResponseMessage 
        public static void SetResponseHeader(this HttpResponse response, HttpResponseMessage httpResponseMessage)
        {
            var headers = httpResponseMessage.Headers.Concat(httpResponseMessage.Content.Headers);

            foreach (var header in headers)
            {
                //Need to escape this because postman crash 
                if (!header.Key.Equals("Transfer-Encoding", StringComparison.CurrentCultureIgnoreCase))
                {
                    response.Headers.Add(header.Key, string.Join(" ", header.Value));
                }
            }
        }
    }
}
