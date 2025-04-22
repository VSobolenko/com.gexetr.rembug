using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RemBug
{
public class OverlayIpConformer : IDisposable
{
#if UNITY_WEBGL
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void ShowTextInput(string gameObject, string positiveMethod, string negativeMethod);
#endif
    
    private readonly string _gameObjectReceiverName;
    private readonly ExternMessageReceiver _receiver;
    private TaskCompletionSource<string> _cts;

    public OverlayIpConformer()
    {
        _gameObjectReceiverName = Guid.NewGuid().ToString();
        _receiver = new GameObject().AddComponent<ExternMessageReceiver>();
        _receiver.OnMessageReceived += FinishReceiveMessage;
        _receiver.name = _gameObjectReceiverName;

        Object.DontDestroyOnLoad(_receiver);
    }

    private void FinishReceiveMessage(string message)
    {
        message = message == "cancel" ? string.Empty : message;
        _cts.SetResult(message);
    }

    public Task<string> GetInputText(CancellationToken cancellationToken = default)
    {
        _cts = new TaskCompletionSource<string>();
        using (cancellationToken.Register(() =>
        {
            if (_cts.Task?.IsCompleted == false)
                _cts.TrySetCanceled();
        }))
        {
            ShowOverlayInputDialog();
            return _cts.Task;
        }
    }

    private void ShowOverlayInputDialog()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPaused = true;
#endif
        new Thread(() =>
        {
            var result = InputDialogLibWindows.TextInput.ShowInputWindow();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += () =>
            {
                UnityEditor.EditorApplication.isPaused = false;
            };
#endif
            _cts.SetResult(result);
        })
        {
            IsBackground = true,
            Name = "InputThread"
        }.Start();
#elif UNITY_WEBGL
        ShowTextInput(_gameObjectReceiverName,
            nameof(ExternMessageReceiver.ReceivePositiveMessage),
            nameof(ExternMessageReceiver.ReceiveNegativeMessage));
#elif UNITY_ANDROID
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
        using (var launcher = new AndroidJavaClass("com.gexetr.inputdialoglib.InputOverlayLauncher"))
        {
            launcher.CallStatic("ShowInputOverlay", 
                _gameObjectReceiverName,            
                nameof(ExternMessageReceiver.ReceivePositiveMessage),
                nameof(ExternMessageReceiver.ReceiveNegativeMessage));
        }
#else
        Debug.LogWarning("IP Unsupported platform");
#endif
    }

    // Feature in progress
    private void ShowPopupInputDialog(bool minimizeUnityApp = false)
    {
        if (Application.isEditor)
        {
            FinishReceiveMessage("cancel");
            return;
        }
        
#if UNITY_ANDROID
        using (var popup = new AndroidJavaClass("com.gexetr.inputdialog.PopupInputDialog"))
        {
            popup.CallStatic(
                "show",
                "Header",
                "Enter text:",
                _gameObjectReceiverName,
                nameof(ExternMessageReceiver.ReceivePositiveMessage),
                nameof(ExternMessageReceiver.ReceiveNegativeMessage),
                minimizeUnityApp
            );
        }
#endif
    }
    
    public void Dispose()
    {
        _receiver.OnMessageReceived -= FinishReceiveMessage;
        Object.Destroy(_receiver.gameObject);
    }
}
}