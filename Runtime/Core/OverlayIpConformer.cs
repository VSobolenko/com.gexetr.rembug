using System.Threading.Tasks;
using UnityEngine;

namespace Core
{
    public class OverlayIpConformer
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        [System.Runtime.InteropServices.DllImport("InputDialog")]
        private static extern void ShowInputDialog();
#endif

        private MessageReceiver _receiver;
        private TaskCompletionSource<string> _cts;

        public OverlayIpConformer()
        {
            _receiver = new GameObject().AddComponent<MessageReceiver>();
            _receiver.OnMessageReceived += FinishReceiveMessage;
            _receiver.name = "Gexetr.RemBug.LogRemote";

            Object.DontDestroyOnLoad(_receiver);
        }

        private void FinishReceiveMessage(string message)
        {
            _cts.SetResult(message);
        }

        public async Task<string> GetIpAddress()
        {
            _cts = new TaskCompletionSource<string>();
            ShowDialog();
            return await _cts.Task;
        } 
        
        private void ShowDialog()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var dialogHelper = new AndroidJavaClass("com.gexetr.inputdialoglib.InputDialogHelper"))
            {
                dialogHelper.CallStatic("ShowInputDialog");
            }
#endif
        }
    }
}