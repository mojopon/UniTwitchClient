using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniTwitchClient.EventSub.Api.Models
{
    public class EventSubSubscription
    {
        public SubscriptionType SubscriptionType { get; private set; }
        public string Id { get; private set; }
        public string SessionId { get; private set; }
        public string Status { get; private set; }

        public EventSubSubscription(SubscriptionType subscriptionType, string id, string sessionId, string status) 
        {
            SubscriptionType = subscriptionType;
            Id = id;
            SessionId = sessionId;
            Status = status;
        }
    }
}
