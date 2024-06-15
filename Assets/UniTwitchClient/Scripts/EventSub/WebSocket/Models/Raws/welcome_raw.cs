using System;

namespace UniTwitchClient.EventSub.WebSocket.Models.Raws
{
    [Serializable]
    public class welcome_raw
    {
        public metadata metadata;
        public payload payload;
    }
}