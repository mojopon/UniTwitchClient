using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub;
using UniTwitchClient.EventSub.Api.Models;
using UnityEngine;

namespace UniTwitchClient.EventSub.Api
{
    public interface ITwitchEventSubApiClient
    {
        UniTask CreateEventSubSubscriptionsAsync(string broadcasterUserId,string sessionId, string moderatorUserId = null);
        UniTask<EventSubSubscriptionData> GetEventSubSubscriptionsAsync();
        UniTask DeleteEventSubSubscriptionsAsync(EventSubSubscriptionData subscriptions);
        UniTask DeleteEventSubSubscriptionsAsync(string sessionId);
    }
}