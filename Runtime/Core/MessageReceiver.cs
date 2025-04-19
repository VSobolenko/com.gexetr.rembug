using System;
using UnityEngine;

namespace Core
{
    public class MessageReceiver : MonoBehaviour
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