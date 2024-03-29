using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub.Api.Models;
using UnityEngine;

namespace UniTwitchClient.EventSub.Api.Mocks
{
    public class TwitchEventSubApiClientMock : ITwitchEventSubApiClient
    {
        public UniTask CreateEventSubSubscriptionsAsync(string broadcasterUserId, string sessionId, string moderatorUserId = null)
        {
            throw new System.NotImplementedException();
        }

        public UniTask DeleteEventSubSubscriptionsAsync(EventSubSubscriptionData subscriptions)
        {
            throw new System.NotImplementedException();
        }

        public UniTask<EventSubSubscriptionData> GetEventSubSubscriptionsAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
