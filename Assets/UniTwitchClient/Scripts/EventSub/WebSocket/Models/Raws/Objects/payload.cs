using System;

namespace UniTwitchClient.EventSub.WebSocket.Models.Raws
{
    [Serializable]
    public class payload
    {
        public session session;
        public subscription subscription;
        public @event @event;
    }
}