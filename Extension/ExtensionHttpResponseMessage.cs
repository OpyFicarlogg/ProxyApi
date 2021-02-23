using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace testApi.Extension
{
    public static class ExtensionHttpResponseMessage
    {
        /// <summary>  
        /// Return a dictionary of String,String 
        /// </summary>  
        /// 
        /// <returns>Dictionary<string, string></returns>
        public static Dictionary<string, string> GetHeader(this HttpResponseMessage httpResponseMessage)
        {
            var headers = httpResponseMessage.Headers.Concat(httpResponseMessage.Content.Headers);

            //IEnumerable<(String,String)> allHeaders = Enumerable.Empty<(String,String)>();

            Dictionary<string, string> allHeaders = new Dictionary<string, string>();

            foreach (var header in headers)
            {
                string val = string.Join(" ", header.Value);
                //Need to escape this because postman crash 
                if (!header.Key.Equals("Transfer-Encoding", StringComparison.CurrentCultureIgnoreCase))
                {
                    allHeaders.Add(header.Key, val);
                }
                /*string headerValue = "";
                header.Value.Select(v => headerValue += $" {v}"); // supprimer l'espace avant après */
            }

            return allHeaders;
        }
    }
}
