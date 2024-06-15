using System;

namespace UniTwitchClient.EventSub.WebSocket.Models.Raws
{
    [Serializable]
    public class notification_raw
    {
        public metadata metadata;
        public payload payload;
    }
}