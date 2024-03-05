using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniTwitchClient.EventSub.Api.Mocks
{
    public class TwitchEventSubApiClientMock : ITwitchEventSubApiClient
    {
        public UniTask CreateSubscriptionsAsync(string sessionId)
        {
            throw new System.NotImplementedException();
        }

        public void SubscribeChannelFollow(string broadcasterUserId, string moderatorUserId = null)
        {
            throw new System.NotImplementedException();
        }

        public void SubscribeChannelPointsCustomRewardRedemptionAdd(string broadcasterUserId)
        {
            throw new System.NotImplementedException();
        }

        public void SubscribeChannelSubscribe(string broadcasterUserId)
        {
            throw new System.NotImplementedException();
        }
    }
}
