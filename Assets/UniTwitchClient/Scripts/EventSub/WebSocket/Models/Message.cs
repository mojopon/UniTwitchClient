using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniTwitchClient.EventSub.WebSocket
{
    public class Message
    {
        public string Text { get; set; }
        public List<Emote> Emotes { get; set; } = new List<Emote>();
    }
}