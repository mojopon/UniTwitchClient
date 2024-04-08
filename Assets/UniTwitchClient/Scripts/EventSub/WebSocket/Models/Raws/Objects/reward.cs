using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniTwitchClient.EventSub.WebSocket.Models.Raws
{
    [Serializable]
    public class reward
    {
        public string id;
        public string title;
        public int cost;
        public string prompt;
    }
}