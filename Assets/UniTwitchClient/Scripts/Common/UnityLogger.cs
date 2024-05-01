using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
