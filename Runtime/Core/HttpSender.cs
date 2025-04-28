using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using UnityEngine;
using Ping = System.Net.NetworkInformation.Ping;

namespace Game.Debugging
{ 
internal class HttpSender
{
    private readonly string _url;
#if !UNITY_WEBGL
    private readonly HttpMethod _method;
    private static readonly HttpClient Client = new()
    {
        Timeout = TimeSpan.FromSeconds(10),
    };
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

        try
        {
            var response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception e)
        {
            Debug.Log($"[RemoteDebug] Error to {_url} with message: " + e.Message);
            return string.Empty;
        }
#endif
    }
    
    public async Task<bool> Ping(int timeoutMilliseconds = 3000)
    {
        try
        {
            var host = ExtractHost(_url);
            if (IPAddress.TryParse(host, out var ipAddress) == false)
                return false;

            if (!(ipAddress.AddressFamily == AddressFamily.InterNetwork ||
                  ipAddress.AddressFamily == AddressFamily.InterNetworkV6))
                return false;
            
            using (var ping = new Ping())
            {
                var reply = await ping.SendPingAsync(ipAddress, timeoutMilliseconds);
                return reply.Status == IPStatus.Success;
            }
        }
        catch
        {
            return false;
        }
    }
    

    private static string ExtractHost(string hostOrUrl)
    {
        if (hostOrUrl.StartsWith("http://") || hostOrUrl.StartsWith("https://"))
        {
            var uri = new Uri(hostOrUrl);
            return uri.Host;
        }

        return hostOrUrl;
    }
}
}