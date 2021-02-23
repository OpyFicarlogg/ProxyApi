using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testApi.Extension
{
    public static class ExtensionRequest
    {
        /// <summary>  
        /// Get cookie 
        /// </summary>  
        /// <param name="key">key (unique indentifier) of the cookie</param> 
        /// <returns></returns>  
        public static string GetCookie(this HttpRequest request, string key)
        {

            return request.Cookies[key];
        }

        public static async Task<string> GetBodyAsync(this HttpRequest request)
        {
            String body = "";
            using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8))
            {
                body = await reader.ReadToEndAsync();

            }

            return body;
        }
    }
}
