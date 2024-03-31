using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub.Api.Models.Raws;
using UnityEngine;

namespace UniTwitchClient.EventSub.Api.Models
{
    public class EventSubSubscribeRequest
    {
        public SubscriptionType SubscriptionType { get; private set; }
        public string SessionId { get; private set; }
        public Condition Condition { get; private set; }

        public EventSubSubscribeRequest(SubscriptionType subscriptionType, Condition condition)
        {
            SubscriptionType = subscriptionType;
            Condition = condition;
        }

        public void AddSessionId(string sessionId)
        {
            SessionId = sessionId;
        }

        public string ToJson()
        {
            var rawModel = new request_subscription_json();
            rawModel.type = SubscriptionType.ToName();
            rawModel.version = "1";
            rawModel.transport = new transport()
            {
                method = "websocket",
                session_id = SessionId,
            };
            rawModel.condition = new condition();

            if (!string.IsNullOrEmpty(Condition.BroadcasterUserId)) { rawModel.condition.broadcaster_user_id = Condition.BroadcasterUserId; }
            if (!string.IsNullOrEmpty(Condition.ModeratorUserId)) { rawModel.condition.moderator_user_id = Condition.ModeratorUserId; }
            if (!string.IsNullOrEmpty(Condition.UserId)) { rawModel.condition.user_id = Condition.UserId; }
            if (!string.IsNullOrEmpty(Condition.FromBroadcasterUserId)) { rawModel.condition.from_broadcaster_user_id = Condition.FromBroadcasterUserId; }
            if (!string.IsNullOrEmpty(Condition.ToBroadcasterUserId)) { rawModel.condition.to_broadcaster_user_id = Condition.ToBroadcasterUserId; }

            return JsonWrapper.ConvertToJson(rawModel);
        }
    }
}