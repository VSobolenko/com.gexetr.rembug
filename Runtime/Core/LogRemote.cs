using System;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Debugging
{
public class LogRemote : ILogger
{
    private const int InputIPAttempt = 2;
    private static HttpSender _server;
    private static int _attemptConnect;

    public static bool Disable { get; set; }
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatic()
    {
        _server = null;
        _attemptConnect = 0;
        Disable = false;
    }

    public static async void Info(string message)
    {
        if (Disable)
            return;
        
        var setupResult = await TrySetupEndPoint();
        if(setupResult == false)
            return;
        
        try
        {
            if (_server != null)
            {
                message = $"[{DateTime.Now:HH:mm:ss.fff}] {message}";
                var result = await _server.SendAsync(message);
                if (string.IsNullOrEmpty(result))
                    throw new Exception();
            }
        }
        catch (Exception)
        {
            _server = null;
        }
    }

    public static void ResetAttemptCount() => _attemptConnect = 0;

    private static Task<bool> TrySetupEndPoint()
    {
        if (_server != null)
            return Task.FromResult(true);
        
        if (_attemptConnect >= InputIPAttempt)
            return Task.FromResult(false);
        
        return SetupEndPoint();
    }

    public static async Task<bool> SetupEndPoint(string helloMessage = null)
    {
        if (Disable)
            return false;
        
        if (_server != null)
            return false;
        
        _attemptConnect++;
        
        using (var conformer = new OverlayIpConformer())
        {
            var ip = await conformer.GetInputText();
            var url = $"http://{ip}:8009/";
            
            if (string.IsNullOrEmpty(ip))
            {
                Debug.LogWarning("[RemoteDebug] Null input ip not allowed!");
                return false;
            }
            _server = new HttpSender(url, "POST");

            var pingSuccess = await _server.Ping();
            if (pingSuccess == false)
            {
                Debug.LogWarning($"[RemoteDebug] Server not response! {url}");
                _server = null;
            }

            if (pingSuccess && !string.IsNullOrEmpty(helloMessage))
                await _server.SendAsync(helloMessage);

            return pingSuccess;
        }
    }

    #region ILogger

    public void LogFormat(LogType logType, Object context, string format, params object[] args) => Info(format);

    public void LogException(Exception exception, Object context) => Info(exception.Message);

    public bool IsLogTypeAllowed(LogType logType) => true;

    public void Log(LogType logType, object message) => Info(message.ToString());

    public void Log(LogType logType, object message, Object context) => Info(message.ToString());

    public void Log(LogType logType, string tag, object message) => Info(message.ToString());

    public void Log(LogType logType, string tag, object message, Object context) => Info(message.ToString());

    public void Log(object message) => Info(message.ToString());

    public void Log(string tag, object message) => Info(message.ToString());

    public void Log(string tag, object message, Object context) => Info(message.ToString());

    public void LogWarning(string tag, object message) => Info(message.ToString());

    public void LogWarning(string tag, object message, Object context) => Info(message.ToString());

    public void LogError(string tag, object message) => Info(message.ToString());

    public void LogError(string tag, object message, Object context) => Info(message.ToString());

    public void LogFormat(LogType logType, string format, params object[] args) => Info(format.ToString());

    public void LogException(Exception exception) => Info(exception.Message);

    public ILogHandler logHandler { get; set; }
    public bool logEnabled { get; set; } = true;
    public LogType filterLogType { get; set; }

    #endregion
}
}
