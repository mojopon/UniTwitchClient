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

            EventSubSubscription subs1 = new EventSubSubscription("abc1", "abcsession1");
            EventSubSubscription subs2 = new EventSubSubscription("abc2", "abcsession2");
            EventSubSubscription subs3 = new EventSubSubscription("abc3", "abcsession1");
            EventSubSubscription subs4 = new EventSubSubscription("abc4", "abcsession2");


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
    }
}