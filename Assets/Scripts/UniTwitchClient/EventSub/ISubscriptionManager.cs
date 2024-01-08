using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniTwitchClient.EventSub
{
    public interface ISubscriptionManager
    {
        void SubscribeChannelFollow(string broadcasterUserId, string moderatorUserId = null);
        void SubscribeChannelSubscribe(string broadcasterUserId);
    }
}