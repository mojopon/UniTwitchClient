using System;

namespace UniTwitchClient.EventSub.WebSocket.Models.Raws
{
    [Serializable]
    public class metadata
    {
        public string message_id;
        public string message_type;
        public string message_timestamp;
        public string subscription_type;
        public string subscription_version;
    }
}