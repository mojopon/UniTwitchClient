using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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