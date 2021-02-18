using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testApi.Extension
{
    public static class ExtensionCookie
    {

        /// <summary>  
        /// Create cookie 
        /// </summary>  
        /// <param name="key">key (unique indentifier) of the cookie</param>  
        /// <param name="value">value to store in cookie object</param>  
        /// <param name="expireTime">expiration time</param>  
        /// <returns></returns>
        public static void SetCookie(this HttpResponse response,string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            response.Cookies.Append(key, value, option);
        }


        /// <summary>  
        /// Get cookie 
        /// </summary>  
        /// <param name="key">key (unique indentifier) of the cookie</param> 
        /// <returns></returns>  
        public static string GetCookie(this HttpRequest request, string key)
        {

            return request.Cookies[key];
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
