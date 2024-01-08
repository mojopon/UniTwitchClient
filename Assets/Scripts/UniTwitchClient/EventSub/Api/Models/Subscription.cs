using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub.Api.Models.Raws;
using UnityEngine;

namespace UniTwitchClient.EventSub.Api.Models
{
    public class Subscription
    {
        public SubscriptionType SubscriptionType { get; private set; }
        public string SessionId { get; private set; }
        public Condition Condition { get; private set; }

        public Subscription(SubscriptionType subscriptionType, Condition condition)
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
            rawModel.condition = new condition()
            {
                broadcaster_user_id = Condition.BroadcasterUserId,
                moderator_user_id = Condition.ModeratorUserId,
                user_id = Condition.UserId,
                from_broadcaster_user_id = Condition.FromBroadcasterUserId,
                to_broadcaster_user_id = Condition.ToBroadcasterUserId
            };

            return JsonConverter.ConvertToJson(rawModel);
        }
    }
}