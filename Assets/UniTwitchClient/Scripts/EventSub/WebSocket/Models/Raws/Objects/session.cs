using System;

namespace UniTwitchClient.EventSub.WebSocket.Models.Raws
{
    [Serializable]
    public class session
    {
        public string id;
        public string status;
        public string connected_at;
        public int keepalive_timeout_seconds;
        public string reconnect_url;
    }
}