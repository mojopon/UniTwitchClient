using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub.Api.Models;
using UnityEngine;

namespace UniTwitchClient.EventSub.Api
{
    public class EventSubSubscribeRequestBuilder : ISubscriptionManager
    {
        private List<EventSubSubscribeRequest> _subscriptions = new List<EventSubSubscribeRequest>();

        public void SubscribeChannelFollow(string broadcasterUserId, string moderatorUserId = null)
        {
            if (string.IsNullOrEmpty(moderatorUserId))
            {
                moderatorUserId = broadcasterUserId;
            }

            var condition = CreateCondition(broadcasterUserId, moderatorUserId, "", "", "");
            var subscription = new EventSubSubscribeRequest(SubscriptionType.ChannelFollow, condition);
            _subscriptions.Add(subscription);
        }

        public void SubscribeChannelSubscribe(string broadcasterUserId)
        {
            var condition = CreateCondition(broadcasterUserId, "", "", "", "");
            var subscription = new EventSubSubscribeRequest(SubscriptionType.ChannelSubscribe, condition);
            _subscriptions.Add(subscription);
        }

        public void SubscribeChannelPointsCustomRewardRedemptionAdd(string broadcasterUserId)
        {
            var condition = CreateCondition(broadcasterUserId, "", "", "", "");
            var subscription = new EventSubSubscribeRequest(SubscriptionType.ChannelPointsCustomRewardRedemptionAdd, condition);
            _subscriptions.Add(subscription);
        }

        public EventSubSubscribeRequest[] GetEventSubSubscribeRequestsWithSessionId(string sessionId)
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