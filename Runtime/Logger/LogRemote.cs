using System.Threading.Tasks;
using Core;

namespace Game
{
public static class LogRemote
{
    private static HttpSender _server;
    
    public static async void Info(string message)
    {
        await TrySetupEndPoint();
        var result = await _server.SendAsync(message);
    }

    private static async Task TrySetupEndPoint()
    {
        if (_server != null)
            return;
        var conformer = new OverlayIpConformer();
        var ip = await conformer.GetIpAddress();

        _server = new HttpSender($"http://{ip}:8009/", "POST");
    }
}
}