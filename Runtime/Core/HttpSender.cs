using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RemBug
{ 
internal class HttpSender
{
    private readonly string _url;
#if !UNITY_WEBGL
    private readonly HttpMethod _method;
    private static readonly HttpClient Client = new();
#endif

    public HttpSender(string url, string method)
    {
        _url = url;
#if UNITY_WEBGL
#else
        _method = new HttpMethod(method.ToUpper());
#endif
    }

    public async Task<string> SendAsync(string content)
    {
#if UNITY_WEBGL
        var request = new UnityEngine.Networking.UnityWebRequest(_url);
        var bodyRaw = Encoding.UTF8.GetBytes(content);
        request.uploadHandler = new UnityEngine.Networking.UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new UnityEngine.Networking.DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "text/plain");

        var op = request.SendWebRequest();

        while (!op.isDone)
            await Task.Yield();

#if UNITY_2020_1_OR_NEWER
        if (request.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
#else
        if (request.isNetworkError || request.isHttpError)
#endif
        {
            throw new System.Exception(request.error);
        }
        return request.downloadHandler.text;
#else
        var request = new HttpRequestMessage(_method, _url)
        {
            Content = new StringContent(content, Encoding.UTF8, "text/plain")
        };

        var response = await Client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
#endif
    }
}
}