using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniTwitchClient.EventSub.WebSocket.Models.Raws
{
    public class message
    {
        public string text;
        public List<emote> emotes;
    }
}