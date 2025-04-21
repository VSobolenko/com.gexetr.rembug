using System;
using System.Threading.Tasks;
using UnityEngine;

namespace RemBug
{
public static class LogRemote
{
    private static HttpSender _server;
    private static int _attemptConnect;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatic()
    {
        _server = null;
        _attemptConnect = 0;
    }

    public static async void Info(string message)
    {
        await TrySetupEndPoint();
        try
        {
            if (_server != null)
            {
                var result = await _server.SendAsync(message);
                if (string.IsNullOrEmpty(result))
                    throw new Exception();
            }
            else Debug.LogWarning("Send message to unknown server!");
        }
        catch (Exception)
        {
            _server = null;
        }
    }

    public static void ResetAttemptCount() => _attemptConnect = 0;

    public static async Task SetupEndPoint()
    {
        using (var conformer = new OverlayIpConformer())
        {
            var ip = await conformer.GetInputText();

            if (string.IsNullOrEmpty(ip))
                return;
            _server = new HttpSender($"http://{ip}:8009/", "POST");
        }
    }
    
    private static async Task TrySetupEndPoint()
    {
        if (_server != null || _attemptConnect > 1)
            return;
        _attemptConnect++;
        await SetupEndPoint();
    }
}
}