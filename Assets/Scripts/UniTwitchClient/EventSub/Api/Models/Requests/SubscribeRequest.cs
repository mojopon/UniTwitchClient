using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub.Api.Models.Raws;
using UnityEngine;

namespace UniTwitchClient.EventSub.Api.Models
{
    public class SubscribeRequest
    {
        public SubscriptionType SubscriptionType { get; set; }
        public string SubscriptionVersion { get; set; } = "1";
        public string ConditionUserId { get; set; }
        public string ConditionBroadCasterUserId { get; set; }
        public string ConditionModeraterUserId { get; set; }
        public string ConditionRewardId { get; set; }
        public string TransportSessionId { get; set; }

        public string ToJson()
        {
            var rawModel = new create_event_sub_request();
            rawModel.type = SubscriptionType.ToName();
            rawModel.version = SubscriptionVersion;
            rawModel.condition = new condition()
            {
                user_id = ConditionUserId,
                broadcaster_user_id = ConditionBroadCasterUserId,
                moderator_user_id = ConditionModeraterUserId,
            };
            rawModel.transport = new transport()
            {
                method = "websocket",
                session_id = TransportSessionId,
            };

            return JsonUtility.ToJson(rawModel);
        }
    }
}
