using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub.Api.Models;
using UnityEngine;

namespace UniTwitchClient.EventSub.Api
{
    public class SubscriptionBuilder : ISubscriptionManager
    {
        private List<Subscription> _subscriptions = new List<Subscription>();

        public void SubscribeChannelFollow(string broadcasterUserId, string moderatorUserId = null)
        {
            if (string.IsNullOrEmpty(moderatorUserId))
            {
                moderatorUserId = broadcasterUserId;
            }

            var condition = CreateCondition(broadcasterUserId, moderatorUserId, "", "", "");
            var subscription = new Subscription(SubscriptionType.ChannelFollow, condition);
            _subscriptions.Add(subscription);
        }

        public void SubscribeChannelSubscribe(string broadcasterUserId)
        {
            var condition = CreateCondition(broadcasterUserId, "", "", "", "");
            var subscription = new Subscription(SubscriptionType.ChannelSubscribe, condition);
            _subscriptions.Add(subscription);
        }

        public Subscription[] GetSubscriptionsWithSessionId(string sessionId)
        {
            foreach (var subscription in _subscriptions)
            {
                subscription.AddSessionId(sessionId);
            }

            return _subscriptions.ToArray();
        }

        private Condition CreateCondition(string broadcasterUserId, string moderatorUserId, string userId, string fromBroadcasterUserId, string toBroadcasterUserId)
        {
            Condition condition = new Condition()
            {
                BroadcasterUserId = broadcasterUserId,
                ModeratorUserId = moderatorUserId,
                UserId = userId,
                FromBroadcasterUserId = fromBroadcasterUserId,
                ToBroadcasterUserId = toBroadcasterUserId
            };

            return condition;
        }
    }
}