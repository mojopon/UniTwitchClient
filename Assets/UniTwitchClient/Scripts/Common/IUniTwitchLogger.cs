using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniTwitchClient.Common
{
    public interface IUniTwitchLogger
    {
        void Log(string message);
        void LogError(string message);
    }
}