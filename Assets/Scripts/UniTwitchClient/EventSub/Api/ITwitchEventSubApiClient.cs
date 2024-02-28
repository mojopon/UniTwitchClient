using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub;
using UnityEngine;

public interface ITwitchEventSubApiClient : ISubscriptionManager
{
    UniTask CreateSubscriptionsAsync(string sessionId);
}
