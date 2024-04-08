using System;

namespace UniTwitchClient.EventSub.WebSocket
{
    public class KeepAlive
    {
        public string MessageType { get; set; }
        public DateTime ReceivedAt { get; set; }
    }
}