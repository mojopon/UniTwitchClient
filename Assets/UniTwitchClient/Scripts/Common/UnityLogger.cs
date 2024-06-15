using UnityEngine;

namespace UniTwitchClient.Common
{
    public class UnityLogger : IUniTwitchLogger
    {
        public void Log(string message)
        {
            Debug.Log(message);
        }

        public void LogError(string message)
        {
            Debug.LogError(message);
        }
    }
}