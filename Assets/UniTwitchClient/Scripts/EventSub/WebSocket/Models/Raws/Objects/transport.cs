using System;

namespace UniTwitchClient.EventSub.WebSocket.Models.Raws
{
    [Serializable]
    public class transport
    {
        public string method;
        public string session_id;
    }
}
