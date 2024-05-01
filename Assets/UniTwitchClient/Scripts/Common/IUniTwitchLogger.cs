using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUniTwitchLogger 
{
    void Log(string message);
    void LogError(string message);
}
