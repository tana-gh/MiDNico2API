using System.Net;
using System.Net.Http;

namespace MiDNicoAPI.Core.Network
{
    public sealed class Nico2Signal
    {
        public static HttpResponseMessage Get (
            in string url, 
            in CookieContainer cookie
        )
        {
            using (var handler = new HttpClientHandler() { UseCookies = true, CookieContainer = cookie })
            using (var client  = new HttpClient(handler))
            {
                return client.GetAsync(url).Result;
            }
        }

        public static HttpResponseMessage Post (
            in string          url, 
            in CookieContainer cookie = null,
            in HttpContent     param = null
        )
        {
            using (var handler = new HttpClientHandler() { UseCookies = true, CookieContainer = cookie ?? new CookieContainer() })
            using (var client  = new HttpClient(handler))
            {
                return client.PostAsync(url, param).Result;
            }
        }

        public static HttpResponseMessage Send (
            in string             url,
            in CookieContainer    cookie,
            in HttpRequestMessage param = null
        )
        {
            using (var handler = new HttpClientHandler() { UseCookies = true, CookieContainer = cookie })
            using (var client  = new HttpClient(handler))
            {
                return client.SendAsync(param).Result;
            }
        }

        public static HttpResponseMessage Put (
            in string          url, 
            in CookieContainer cookie, 
            in HttpContent     param = null
        )
        {
            using (var handler = new HttpClientHandler() { UseCookies = true, CookieContainer = cookie })
            using (var client = new HttpClient(handler))
            {
                return client.PutAsync(url, param).Result;
            }
        }

        public static HttpResponseMessage Delete (
            in string          url,
            in CookieContainer cookie
        )
        {
            using (var handler = new HttpClientHandler() { CookieContainer = cookie })
            using (var client  = new HttpClient(handler))
            {
                return client.DeleteAsync(url).Result;
            }
        }

        public static CookieContainer TakeCookie (
            in string      url, 
            in HttpContent param
        )
        {
            using (var handler = new HttpClientHandler() { UseCookies = true, CookieContainer = new CookieContainer() })
            using (var client  = new HttpClient(handler))
            {
                var response = client.PostAsync(url, param).Result;
                return handler.CookieContainer;
            }
        }       
    }
}
