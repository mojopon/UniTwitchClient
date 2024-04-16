using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniTwitchClient.EventSub.WebSocket
{
    public class Emote
    {
        public int Begin { get; set; }
        public int End { get; set; }
        public string Id { get; set; }
    }
}