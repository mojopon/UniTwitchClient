using System.Collections.Generic;
using UniTwitchClient.EventSub.Api.Models;

namespace UniTwitchClient.EventSub.Api
{
    public class EventSubSubscribeRequestBuilder
    {
        private List<EventSubSubscribeRequest> _requests = new List<EventSubSubscribeRequest>();

        public void CreateAllRequests(string broadcasterUserId, string moderatorUserId)
        {
            CreateChannelFollowRequest(broadcasterUserId, moderatorUserId);
            CreateChannelSubscribeRequest(broadcasterUserId);
            CreateChannelSubscriptionMessage(broadcasterUserId);
            CreateChannelCheerRequest(broadcasterUserId);
            CreateChannelPointsCustomRewardRedemptionAddRequest(broadcasterUserId);
        }

        public void CreateChannelFollowRequest(string broadcasterUserId, string moderatorUserId = null)
        {
            if (string.IsNullOrEmpty(moderatorUserId))
            {
                moderatorUserId = broadcasterUserId;
            }

            var condition = CreateCondition(broadcasterUserId, moderatorUserId, "", "", "");
            var subscription = new EventSubSubscribeRequest(SubscriptionType.ChannelFollow, condition);
            _requests.Add(subscription);
        }

        public void CreateChannelSubscribeRequest(string broadcasterUserId)
        {
            var condition = CreateCondition(broadcasterUserId, "", "", "", "");
            var subscription = new EventSubSubscribeRequest(SubscriptionType.ChannelSubscribe, condition);
            _requests.Add(subscription);
        }

        public void CreateChannelSubscriptionMessage(string broadcasterUserId)
        {
            var condition = CreateCondition(broadcasterUserId, "", "", "", "");
            var subscription = new EventSubSubscribeRequest(SubscriptionType.ChannelSubscriptionMessage, condition);
            _requests.Add(subscription);
        }

        public void CreateChannelCheerRequest(string broadcasterUserId)
        {
            var condition = CreateCondition(broadcasterUserId, "", "", "", "");
            var subscription = new EventSubSubscribeRequest(SubscriptionType.ChannelCheer, condition);
            _requests.Add(subscription);
        }

        public void CreateChannelPointsCustomRewardRedemptionAddRequest(string broadcasterUserId)
        {
            var condition = CreateCondition(broadcasterUserId, "", "", "", "");
            var subscription = new EventSubSubscribeRequest(SubscriptionType.ChannelPointsCustomRewardRedemptionAdd, condition);
            _requests.Add(subscription);
        }

        public EventSubSubscribeRequest[] BuildRequestsWithSessionId(string sessionId)
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