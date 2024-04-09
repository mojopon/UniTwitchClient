using System;
using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub.WebSocket;
using UnityEngine;

namespace UniTwitchClient.EventSub
{
    public class NotificationConverter : INotificationConverter
    {
        private Dictionary<SubscriptionType, INotificationConverter> converterDictionary = new Dictionary<SubscriptionType, INotificationConverter>();

        public NotificationConverter() 
        {
            converterDictionary.Add(SubscriptionType.ChannelFollow, new ChannelFollowConverter());
            converterDictionary.Add(SubscriptionType.ChannelSubscribe, new ChannelSubscribeConverter());
            converterDictionary.Add(SubscriptionType.ChannelPointsCustomRewardRedemptionAdd, new ChannelPointsCustomRewardRedemptionAddConverter());
        }

        public object Convert(Notification notification)
        {
            if (converterDictionary.ContainsKey(notification.SubscriptionType))
            {
                return converterDictionary[notification.SubscriptionType].Convert(notification);
            }

            return null;
        }
    }
}