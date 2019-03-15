using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lux.Helpers
{
    public abstract class HttpUtil
    {
        public static HttpResponseMessage Get(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                return client.GetAsync(url).Result;
            }
        }

        public static async Task<HttpResponseMessage> GetAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(10);
                //client.MaxResponseContentBufferSize = System.Int64.MaxValue;
                return await client.GetAsync(url);
            }
        }

        public static HttpResponseMessage Post(string url, object content = null)
        {
            using (HttpClient client = new HttpClient())
            {
                string jsonString = JsonConvert.SerializeObject(content);

                using (HttpContent httpContent = new StringContent(jsonString))
                {
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    return client.PostAsync(url, httpContent).Result;
                }
            }
        }

        public static async Task<HttpResponseMessage> PostAsync(string url, object content = null)
        {
            using (HttpClient client = new HttpClient())
            {
                string jsonString = JsonConvert.SerializeObject(content);

                using (HttpContent httpContent = new StringContent(jsonString))
                {
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    return await client.PostAsync(url, httpContent);
                }
            }
        }

        public static HttpResponseMessage Put(string url, object content = null)
        {
            using (HttpClient client = new HttpClient())
            {
                string jsonString = JsonConvert.SerializeObject(content);

                using (HttpContent httpContent = new StringContent(jsonString))
                {
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    return client.PutAsync(url, httpContent).Result;
                }
            }
        }

        public static async Task<HttpResponseMessage> PutAsync(string url, object content = null)
        {
            using (HttpClient client = new HttpClient())
            {
                string jsonString = JsonConvert.SerializeObject(content);

                using (HttpContent httpContent = new StringContent(jsonString))
                {
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    return await client.PutAsync(url, httpContent);
                }
            }
        }

        public static HttpResponseMessage Delete(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                return client.DeleteAsync(url).Result;
            }
        }

        public static async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                return await client.DeleteAsync(url);
            }
        }
    }
}
