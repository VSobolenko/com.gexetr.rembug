using System;
using UnityEngine;

namespace RemBug
{
internal class ExternMessageReceiver : MonoBehaviour
{
    public event Action<string> OnMessageReceived;

    public void ReceivePositiveMessage(string message)
    {
        OnMessageReceived?.Invoke(message);
    }
    
    public void ReceiveNegativeMessage(string message)
    {
        OnMessageReceived?.Invoke(message);
    }
}
}