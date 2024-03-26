using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UniTwitchClient.EventSub.Api.Models;
using UniTwitchClient.EventSub.Api.Models.Raws;
using UnityEngine;

namespace UniTwitchClient.EventSub.Api
{
    public static class RawToModelConverter
    {
        public static EventSubSubscription ConvertRawToModel(this subscription data) 
        {
            return new EventSubSubscription(data.id);
        }

        public static EventSubSubscriptionData ConvertRawToModel(this subscription_data data) 
        {
            var subscriptions = new List<EventSubSubscription>();

            if (data.data != null)
            {
                foreach (var subscription in data.data)
                {
                    subscriptions.Add(subscription.ConvertRawToModel());
                }
            }

            return new EventSubSubscriptionData(subscriptions);
        }
    }
}