using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub;
using UnityEngine;

namespace UniTwitchClient.EventSub.Api
{
    public interface ITwitchEventSubApiClient : ISubscriptionManager
    {
        UniTask CreateSubscriptionsAsync(string sessionId);
        UniTask<string> GetEventSubSubscriptionsAsync();
    }
}