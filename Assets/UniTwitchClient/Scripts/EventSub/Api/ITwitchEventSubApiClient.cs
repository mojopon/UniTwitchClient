using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniTwitchClient.Common;
using UniTwitchClient.EventSub;
using UniTwitchClient.EventSub.Api.Models;
using UnityEngine;

namespace UniTwitchClient.EventSub.Api
{
    public interface ITwitchEventSubApiClient
    {
        UniTask CreateEventSubSubscriptionsAsync(string broadcasterUserId, string sessionId, CancellationToken cancellationToken = default);
        UniTask CreateEventSubSubscriptionsAsync(string broadcasterUserId, string sessionId, string moderatorUserId, CancellationToken cancellationToken = default);
        UniTask<EventSubSubscriptionData> GetEventSubSubscriptionsAsync();
        UniTask DeleteEventSubSubscriptionsAsync(EventSubSubscriptionData subscriptions);
        UniTask DeleteEventSubSubscriptionsAsync(string sessionId);

        IUniTwitchLogger Logger { get; set; }
    }
}