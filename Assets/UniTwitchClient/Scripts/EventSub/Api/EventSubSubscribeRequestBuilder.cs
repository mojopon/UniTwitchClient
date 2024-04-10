using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub.Api.Models;
using UnityEngine;

namespace UniTwitchClient.EventSub.Api
{
    public class EventSubSubscribeRequestBuilder
    {
        private List<EventSubSubscribeRequest> _requests = new List<EventSubSubscribeRequest>();

        public void CreateAllSubscriptionRequests(string broadcasterUserId, string moderatorUserId) 
        {
            CreateSubscribeChannelFollowRequest(broadcasterUserId, moderatorUserId);
            CreateSubscribeChannelSubscribeRequest(broadcasterUserId);
            CreateSubscribeChannelSubscriptionMessage(broadcasterUserId);
            CreateSubscribeChannelPointsCustomRewardRedemptionAddRequest(broadcasterUserId);
        }

        public void CreateSubscribeChannelFollowRequest(string broadcasterUserId, string moderatorUserId = null)
        {
            if (string.IsNullOrEmpty(moderatorUserId))
            {
                moderatorUserId = broadcasterUserId;
            }

            var condition = CreateCondition(broadcasterUserId, moderatorUserId, "", "", "");
            var subscription = new EventSubSubscribeRequest(SubscriptionType.ChannelFollow, condition);
            _requests.Add(subscription);
        }

        public void CreateSubscribeChannelSubscribeRequest(string broadcasterUserId)
        {
            var condition = CreateCondition(broadcasterUserId, "", "", "", "");
            var subscription = new EventSubSubscribeRequest(SubscriptionType.ChannelSubscribe, condition);
            _requests.Add(subscription);
        }

        public void CreateSubscribeChannelSubscriptionMessage(string broadcasterUserId) 
        {
            var condition = CreateCondition(broadcasterUserId, "", "", "", "");
            var subscription = new EventSubSubscribeRequest(SubscriptionType.ChannelSubscriptionMessage, condition);
            _requests.Add(subscription);
        }

        public void CreateSubscribeChannelPointsCustomRewardRedemptionAddRequest(string broadcasterUserId)
        {
            var condition = CreateCondition(broadcasterUserId, "", "", "", "");
            var subscription = new EventSubSubscribeRequest(SubscriptionType.ChannelPointsCustomRewardRedemptionAdd, condition);
            _requests.Add(subscription);
        }

        public EventSubSubscribeRequest[] GetEventSubSubscribeRequestsWithSessionId(string sessionId)
        {
            foreach (var subscription in _requests)
            {
                subscription.AddSessionId(sessionId);
            }

            return _requests.ToArray();
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