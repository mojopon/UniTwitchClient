using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UniTwitchClient.EventSub.Api.Models;
using UnityEngine;

namespace UniTwitchClient.Tests.EventSub.Api
{
    public class EventSubSubscriptionDataTest
    {
        [Test]
        public void GetSubscriptionsWithSessionIdTest() 
        {
            List<EventSubSubscription> eventSubSubscriptions = new List<EventSubSubscription>();

            EventSubSubscription subs1 = new EventSubSubscription(SubscriptionType.ChannelFollow, "abc1", "abcsession1", "enabled");
            EventSubSubscription subs2 = new EventSubSubscription(SubscriptionType.ChannelCheer, "abc2", "abcsession2", "enabled");
            EventSubSubscription subs3 = new EventSubSubscription(SubscriptionType.ChannelPointsCustomRewardRedemptionAdd, "abc3", "abcsession1", "enabled");
            EventSubSubscription subs4 = new EventSubSubscription(SubscriptionType.ChannelSubscribe, "abc4", "abcsession2", "enabled");


            eventSubSubscriptions.Add(subs1);
            eventSubSubscriptions.Add(subs2);
            eventSubSubscriptions.Add(subs3);
            eventSubSubscriptions.Add(subs4);

            EventSubSubscriptionData eventSubSubscriptionData = new EventSubSubscriptionData(eventSubSubscriptions);

            var dataWithSessionId = eventSubSubscriptionData.GetSubscriptionsBySessionId("abcsession2");

            Assert.IsNotNull(dataWithSessionId);
            Assert.AreEqual(2, dataWithSessionId.Subscriptions.Count);
            Assert.AreEqual("abc2", dataWithSessionId.Subscriptions[0].Id);
            Assert.AreEqual("abcsession2", dataWithSessionId.Subscriptions[0].SessionId);
            Assert.AreEqual("abc4", dataWithSessionId.Subscriptions[1].Id);
            Assert.AreEqual("abcsession2", dataWithSessionId.Subscriptions[1].SessionId);
        }

        [Test]
        public void GetEnabledSubscriptionsTest() 
        {
            List<EventSubSubscription> eventSubSubscriptions = new List<EventSubSubscription>();

            EventSubSubscription subs1 = new EventSubSubscription(SubscriptionType.ChannelFollow, "abc1", "abcsession1", "enabled");
            EventSubSubscription subs2 = new EventSubSubscription(SubscriptionType.ChannelCheer, "abc2", "abcsession2", "enabled");
            EventSubSubscription subs3 = new EventSubSubscription(SubscriptionType.ChannelPointsCustomRewardRedemptionAdd, "abc3", "abcsession1", "websocket_disconnected");
            EventSubSubscription subs4 = new EventSubSubscription(SubscriptionType.ChannelSubscribe, "abc4", "abcsession2", "enabled");

            eventSubSubscriptions.Add(subs1);
            eventSubSubscriptions.Add(subs2);
            eventSubSubscriptions.Add(subs3);
            eventSubSubscriptions.Add(subs4);

            EventSubSubscriptionData eventSubSubscriptionData = new EventSubSubscriptionData(eventSubSubscriptions);

            var enabledSubs = eventSubSubscriptionData.GetEnabledSubscriptions();

            Assert.IsNotNull(enabledSubs);
            Assert.AreEqual("abc1", enabledSubs.Subscriptions[0].Id);
            Assert.AreEqual("abc2", enabledSubs.Subscriptions[1].Id);
            Assert.AreEqual("abc4", enabledSubs.Subscriptions[2].Id);
        }
    }
}