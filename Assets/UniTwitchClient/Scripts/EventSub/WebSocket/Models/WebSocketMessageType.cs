using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniTwitchClient.EventSub.WebSocket
{
    public enum WebSocketMessageType
    {
        None = 0,
        SessionWelcome,
        SessionKeepAlive,
        Ping,
        Notification,
        Reconnect,
        Revocation,
        Close,
    }
}