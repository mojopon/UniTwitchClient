using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub;
using UnityEngine;

namespace UniTwitchClient.EventSub.Api
{
    public interface ITwitchEventSubApiClient
    {
        UniTask CreateEventSubSubscriptionsAsync(string broadcasterUserId,string sessionId, string moderatorUserId = null);
        UniTask<List<EventSubSubscription>> GetEventSubSubscriptionsAsync();
        UniTask DeleteEventSubSubscriptionsAsync(string sessionId);
    }
}