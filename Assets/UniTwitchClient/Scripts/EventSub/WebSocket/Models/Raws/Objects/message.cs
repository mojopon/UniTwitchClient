using System.Collections.Generic;

namespace UniTwitchClient.EventSub.WebSocket.Models.Raws
{
    public class message
    {
        public string text;
        public List<emote> emotes;
    }
}