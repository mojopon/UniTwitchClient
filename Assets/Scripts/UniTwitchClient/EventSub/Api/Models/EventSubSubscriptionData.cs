using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UniTwitchClient.EventSub.Api.Models
{
    public class EventSubSubscriptionData
    {
        public List<EventSubSubscription> Subscriptions { get; private set; }

        public EventSubSubscriptionData(List<EventSubSubscription> subscriptions)
        {
            Subscriptions = subscriptions;
        }

        public EventSubSubscriptionData GetSubscriptionsBySessionId(string sessionId) 
        {
            return new EventSubSubscriptionData(Subscriptions.Where(x => x.SessionId == sessionId).ToList());
        }
    }
}
