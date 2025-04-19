using System;
using System.Threading.Tasks;
using UnityEngine;

namespace RemBug
{
public static class LogRemote
{
    private static HttpSender _server;
    private static int _attemptConnect = 0;

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

    private static async Task TrySetupEndPoint()
    {
        if (_server != null || _attemptConnect > 2)
            return;
        _attemptConnect++;

        var conformer = new OverlayIpConformer();
        var ip = await conformer.GetIpAddress();

        if (string.IsNullOrEmpty(ip))
            return;
        _server = new HttpSender($"http://{ip}:8009/", "POST");
    }

    // Feature in progress
    private static void ShowAlertPopup()
    {
        using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.gexetr.inputdialog.InputDialogPlugin"))
        {
            pluginClass.CallStatic(
                "showInputDialog",
                "GameObjectNameToReceiveMessage",
                "OnInputReceived", // method receive message
                "Header",
                "Enter text:"
            );
        }
    }
}
}