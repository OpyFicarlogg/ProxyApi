using System;
using Microsoft.AspNetCore.Http;


namespace testApi.Services
{
    //https://www.c-sharpcorner.com/article/asp-net-core-working-with-cookie/
    /* Options d'un cookie:
    Domain - The domain you want to associate with cookie
    Path - Cookie Path
    Expires - The expiration date and time of the cookie
    HttpOnly - Gets or sets a value that indicates whether a cookie is accessible by client-side script or not.
    Secure - Transmit the cookie using Secure Sockets Layer (SSL) that is, over HTTPS only.  
    */
    public class CookieActions
    {
        public HttpRequest request {get;set;}
        public HttpResponse response {get;set;}


        /// <summary>  
        /// Create cookie 
        /// </summary>  
        /// <param name="key">key (unique indentifier) of the cookie</param>  
        /// <param name="value">value to store in cookie object</param>  
        /// <param name="expireTime">expiration time</param>  
        /// <returns></returns>
        public void SetCookie( string key, string value, int? expireTime)  
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
        public string GetCookie( string key){

            return request.Cookies[key];
        }

        /// <summary>  
        /// Delete cookie 
        /// </summary>  
        /// <param name="key">key (unique indentifier) of the cookie</param>  
        /// <param name="value">value to store in cookie object</param>  
        /// <param name="expireTime">expiration time</param>  
        public void Remove(string key)  
        {  
            response.Cookies.Delete(key);  
        }  
    }
}