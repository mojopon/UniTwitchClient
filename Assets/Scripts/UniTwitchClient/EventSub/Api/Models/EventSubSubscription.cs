using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniTwitchClient.EventSub.Api.Models
{
    public class EventSubSubscription
    {
        public string Id { get; private set; }
        public string SessionId { get; private set; }

        public EventSubSubscription(string id, string sessionId) 
        {
            Id = id;
            SessionId = sessionId;
        }
    }
}
