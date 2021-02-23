using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace testApi.Extension
{
    public static class ExtensionResponse
    {
        /// <summary>  
        /// Set body in HttpResponse
        /// </summary>  
        /// <param name="result"> String to write in body  
        public static void WriteBody(this HttpResponse response, string result)
        {
            StreamWriter writer = new StreamWriter(response.Body, Encoding.UTF8);
            writer.Write(result);
            writer.Flush();
        }


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

        /// <summary>  
        /// Create cookie 
        /// </summary>  
        /// <param name="key">key (unique indentifier) of the cookie</param>  
        /// <param name="value">value to store in cookie object</param>  
        /// <param name="expireTime">expiration time</param>  
        /// <returns></returns>
        public static void SetCookie(this HttpResponse response, string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            response.Cookies.Append(key, value, option);
        }

        /// <summary>  
        /// Delete cookie 
        /// </summary>  
        /// <param name="key">key (unique indentifier) of the cookie</param>  
        /// <param name="value">value to store in cookie object</param>  
        /// <param name="expireTime">expiration time</param>  
        public static void Remove(this HttpResponse response, string key)
        {
            response.Cookies.Delete(key);
        }
    }
}
