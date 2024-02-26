using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniTwitchClient.Chat.Models
{
    public enum ConnectionState 
    {
        Idle = 0,
        Connecting,
        Connected,
        Disconnected,
    }
}
