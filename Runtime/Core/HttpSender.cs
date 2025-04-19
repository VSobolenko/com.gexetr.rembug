using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Core
{
    public class HttpSender
    {
        private readonly string _url;
        private readonly HttpMethod _method;
        private static readonly HttpClient Client = new HttpClient();

        public HttpSender(string url, string method)
        {
            _url = url;
            _method = new HttpMethod(method.ToUpper());
        }

        public async Task<string> SendAsync(string content)
        {
            var request = new HttpRequestMessage(_method, _url)
            {
                Content = new StringContent(content, Encoding.UTF8, "text/plain")
            };

            var response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}