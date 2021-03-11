using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProxyApi.Extension
{
    public static class ExtensionStream
    {
        /// <summary>  
        ///Get content from Stream as string
        /// </summary>  
        /// <param name="stream">Stream type 
        public static async Task<string> GetStringAsyncStreamContent(this Stream stream)
        {
            StreamContent content = new StreamContent(stream); //Get body
            return await content.ReadAsStringAsync();
        }

        /// <summary>  
        ///Get content from Stream as string
        /// </summary>  
        /// <param name="stream">Stream type 
        public static async Task<string> GetStringAsync(this Stream stream)
        {
            String body = "";
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                body = await reader.ReadToEndAsync();

            }

            return body;
        }

        /// <summary>  
        ///Set Stream
        /// </summary>  
        /// <param name="result">string to set in stream  
        public static void SetStringStream(this Stream stream, string result)
        {
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Write(result);
            writer.Flush();
        }
    }
}
