using System;

namespace UniTwitchClient.EventSub.WebSocket
{
    public class Welcome
    {
        public string MessageType { get; set; }
        public string SessionId { get; set; }
        public int KeepAliveTimeoutSeconds { get; set; }
        public DateTime ConnectedAt { get; set; }
    }
}