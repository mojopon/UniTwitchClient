using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniTwitchClient.EventSub.Api.Models
{
    public class EventSubSubscriptionData
    {
        public List<EventSubSubscription> Subscriptions { get; private set; }

        public EventSubSubscriptionData(List<EventSubSubscription> subscriptions)
        {
            Subscriptions = subscriptions;
        }
    }
}
